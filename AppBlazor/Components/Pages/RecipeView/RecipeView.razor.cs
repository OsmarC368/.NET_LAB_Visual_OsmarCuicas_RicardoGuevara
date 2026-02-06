using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AppBlazor.Data.Models;
using AppBlazor.Data.Services;
using Core.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Core.Interfaces.Services;
using Microsoft.Extensions.Localization;
using Core.Entities;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using System.IO.Compression;
namespace AppBlazor.Components.Pages.RecipeView
{
    public partial class RecipeView
    {
        [Parameter]
        public int RecipeId { get; set; }
        [Inject]
        public RecipesService? recipeService { get; set; } = default!;
        [Inject]
        public IngredientService? ingredientService { get; set; } = default!;
        [Inject]
        public MeasureService? measureService { get; set; } = default!;
        [Inject]
        public IngredientPerRecipeService? ingredientPerRecipeService { get; set; } = default!;
        [Inject]
        public StepService? stepService { get; set; } = default!;
        [Inject]
        public StepUserService? stepUserService { get; set; } = default!;
        [Inject] private IStringLocalizer<SharedResources> L { get; set; }
        public Recipe? recipe { get; set; } = new Recipe();
        public string RecipeName { get; set; } = string.Empty;
        public List<Step> steps { get; set; } = new List<Step>();
        public List<IngredientPerRecipeDTO> recipeIngredients { get; set; } = new List<IngredientPerRecipeDTO>();
        [Inject]
        public AuthenticationStateProvider? AuthStateProvider { get; set; }
        
        private int currentStepIndex = 0;
        private string currentStepNote = "";
        private float recServings = 0;
        private List<StepUser> stepUserList = new();
        private string play_pause = "▶";
 

        protected async override Task OnInitializedAsync()
        {
            await LoadRecipe();
            await LoadIngredients();
            await verifyStepUser();
        }

        private void resetServings()
        {
            resetAmount();
            recServings = 0;
        }

        private void resetAmount()
        {
            foreach(var ingre in recipeIngredients)
            {
                ingre.amount = ingre.amountOrg;
            }
        }

        private void recalculateServings()
        {
            if (recServings != 0 && recServings > 0)
            {
                resetAmount();
                float value = ((recServings * 100) / recipe.Servings) / 100;
                foreach(var ingre in recipeIngredients)
                {
                    ingre.amount = (float.Parse(ingre.amount) * value).ToString();
                }
            }
            
        }

        private void startPause(int stepID)
        {
            if (play_pause == "▶")
            {
                play_pause = "⏸";
                start(stepID);
            }
            else
            {
                play_pause = "▶";
                pause(stepID);
            }
        }

        private void pause(int stepID)
        {
            var step = steps.FirstOrDefault(x => x.Id == stepID);
            step.stepTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void start(int stepID)
        {
            var step = steps.FirstOrDefault(x => x.Id == stepID);
            step.stepTimer.Change(0, 1000);
        }

        private void reset(int stepID)
        {
            var step = steps.FirstOrDefault(x => x.Id == stepID);
            step.currentTimer = step.Duration * 60;
            var minutes = Math.Floor(step.currentTimer / 60);
            var seconds = step.currentTimer % 60;
            step.timerValue = minutes + ":" + seconds;
            pause(stepID);
            play_pause = "▶";
            InvokeAsync(StateHasChanged);

        }

        private void timer(object? state, int stepID)
        {
            var step = steps.FirstOrDefault(x => x.Id == stepID);
            if(step.currentTimer > 0)
            {
                step.currentTimer -= 1;
                var minutes = Math.Floor(step.currentTimer / 60);
                var seconds = step.currentTimer % 60;
                step.timerValue = minutes + ":" + seconds;
            }
            else
            {
                reset(stepID);
            }
            InvokeAsync(StateHasChanged);
        }

        public async Task LoadRecipe()
        {
            var response = await recipeService.GetRecipeById(RecipeId);
            if (response.Ok && response.Data != null)
            {
                recipe = response.Data.Datos;
                RecipeName = recipe.Name;
                await LoadSteps();
            }
        }

        private async Task LoadSteps()
        {
            var response = await stepService.GetAllAsync();
            if (response.Ok && response.Datos != null)
            {
                var temp = response.Datos.ToList();
                foreach (var step in temp)
                {
                    if(step.RecipeIdS == RecipeId)
                    {
                        step.currentTimer = step.Duration*60;
                        var minutes = Math.Floor(step.currentTimer / 60);
                        var seconds = step.currentTimer % 60;
                        step.timerValue = minutes + ":" + seconds;
                        step.stepTimer = new Timer(state => timer(state, step.Id), null, Timeout.Infinite, Timeout.Infinite);
                        steps.Add(step);
                    }
                }
            }
        }

        private async Task LoadIngredients()
        {
            var response = await ingredientPerRecipeService.GetAllAsync();
            if (response.Ok && response.Datos != null)
            {
                var temp = response.Datos.Where(i => i.RecipeId == RecipeId).ToList();
                foreach (var ingredient in temp)
                {
                    if (ingredient.RecipeId == RecipeId)
                    {
                        var newIngredient = new IngredientPerRecipeDTO();
                        var measure = await measureService.GetByIdAsync(ingredient.measureIdIPR);
                        newIngredient.measure = measure.Datos.symbol;
                        var ingre = await ingredientService.GetByIdAsync(ingredient.IngredientIdIPR);
                        newIngredient.ingredient = ingre.Datos.name;
                        newIngredient.amount = ingredient.amount.ToString();
                        newIngredient.amountOrg = ingredient.amount.ToString();
                        recipeIngredients.Add(newIngredient);
                    }
                }
            }
        }

        private void NextStep()
        {
            if (currentStepIndex < steps.Count - 1)
                currentStepIndex++;
        }

        private void PreviousStep()
        {
            if (currentStepIndex > 0)
                currentStepIndex--;
        }

        private async Task MarkStep(int stepID, bool state)
        {
            StepUser stepUserToUpdate = new();
            foreach(var stepUser in stepUserList)
            {
                if(stepUser.stepSURID == stepID)
                {
                    stepUserToUpdate = stepUser;
                    stepUserToUpdate.completed = state;
                }
            }
            await stepUserService.Update(stepUserToUpdate.id, stepUserToUpdate);
        }



        private async Task saveComment(int stepID)
        {
            StepUser stepUserToUpdate = new();
            var step = steps.FirstOrDefault(x => x.Id == stepID);
            foreach(var stepUser in stepUserList)
            {
                if(stepUser.stepSURID == stepID)
                {
                    stepUserToUpdate = stepUser;
                    stepUserToUpdate.comment = step.note;
                }
            }
            await stepUserService.Update(stepUserToUpdate.id, stepUserToUpdate);
        }


        private async Task verifyStepUser()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userID = int.Parse(user.FindFirst("id")?.Value ?? "0");
            bool verified = false;
            var response = await stepUserService.GetAllAsync();
            if (response.Ok)
            {
                foreach(var step in steps)
                {
                    verified = false;
                    foreach(var x in response.Datos)
                    {
                        if(x.stepSURID == step.Id && x.userSURID == userID)
                        {
                            verified = true;
                            step.note = x.comment;
                            stepUserList.Add(x);
                        }
                    }
                    if (!verified)
                    {
                        var newStepUser = await createStepUser(userID, step.Id);
                        step.note = newStepUser.comment;
                        stepUserList.Add(newStepUser);
                    }
                }
            }

        }




        private async Task<StepUser> createStepUser(int userID, int stepID)
        {
            var stepUser = new StepUser();
            stepUser.comment = "";
            stepUser.completed = false;
            stepUser.stepSURID = stepID;
            stepUser.userSURID = userID;
            await stepUserService.Create(stepUser);
            return stepUser;
        }

        private bool verifyCompletedStep(int stepID)
        {
            bool verified = false; 
            foreach(var stepUser in stepUserList)
            {
                if(stepUser.stepSURID == stepID)
                {
                    verified = stepUser.completed;
                }
            }
            return verified; 
        }


        private void GoBack()
        {
            Navigation.NavigateTo("/dashboard");
        }
    }
}
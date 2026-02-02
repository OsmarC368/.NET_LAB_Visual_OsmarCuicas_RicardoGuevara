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
        [Inject] private IStringLocalizer<SharedResources> L { get; set; }
        public Recipe? recipe { get; set; } = new Recipe();
        public string RecipeName { get; set; } = string.Empty;
        public List<Step> steps { get; set; } = new List<Step>();
        public List<IngredientPerRecipeDTO> recipeIngredients { get; set; } = new List<IngredientPerRecipeDTO>();

        protected async override Task OnInitializedAsync()
        {
            await LoadRecipe();
            await LoadIngredients();
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
                        steps.Add(step);
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
                        recipeIngredients.Add(newIngredient);
                    }
                }
            }
        }

        private int currentStepIndex = 0;
        private Dictionary<int, string> stepNotes = new();
        private HashSet<int> completedSteps = new();

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

        private void MarkStepCompleted()
        {
            completedSteps.Add(currentStepIndex);
        }

        private string CurrentStepNote
        {
            get => stepNotes.ContainsKey(currentStepIndex)
                ? stepNotes[currentStepIndex]
                : string.Empty;

            set => stepNotes[currentStepIndex] = value;
        }
    }
}
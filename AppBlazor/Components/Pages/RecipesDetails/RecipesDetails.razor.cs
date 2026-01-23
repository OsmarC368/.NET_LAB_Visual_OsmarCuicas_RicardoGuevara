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

namespace AppBlazor.Components.Pages.RecipesDetails
{
    public partial class RecipesDetails
    {
        private int? selectedIngredient;
        private int? selectedMeasure;
        public StepDTO step = new();
        public string RecipeName = string.Empty;
        public List<Core.Entities.Step> steps = new();
        public string loading = string.Empty;
        public string loadingIngredient = string.Empty;
        public string message = string.Empty;
        public bool updatingStep = false;
        public bool updatingIngredient = false;
        public List<Ingredient> ingredientsList = new();
        public List<Measure> measureList = new();
        public string messageClass = string.Empty;
        public IngredientPerRecipeDTO ingredientPerRecipe = new IngredientPerRecipeDTO();
        public List<IngredientPerRecipeDTO> ingredientsPerRecipeList = new List<IngredientPerRecipeDTO>();
        [Inject]
        public StepService? stepService { get; set; }
        [Inject]
        public RecipesService? recipesService { get; set; }
        [Inject] private IIngredientService ingredientService { get; set; } = default!;
        [Inject]
        public IMeasureService measureService { get; set; }
        [Inject]
        public IngredientPerRecipeService? ingredientPerRecipeService { get; set; }
        [Inject]
        public AuthenticationStateProvider? AuthStateProvider { get; set; }
        [Inject]
        public NavigationManager? NavigationManager { get; set; }
        [Inject] private IStringLocalizer<SharedResources> L { get; set; }

        [Parameter]
        public int RecipeId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            loading = @L["Loading"];
            StateHasChanged();
            var recipeFound = await recipesService.GetRecipeById(RecipeId);
            if (recipeFound.Data != null)
            {
                RecipeName = recipeFound.Data.Datos?.Name ?? string.Empty;
            }

            steps = (await GetAllSteps())?.ToList() ?? new List<Core.Entities.Step>();
            loading = string.Empty;

            var ingredientsResponse = await GetAllIngredients();
            if (ingredientsResponse != null)
            {
                ingredientsList = ingredientsResponse.ToList();
            }

            var measureResponse = await GetAllMeasures();
            if (measureResponse != null)
            {
                measureList = measureResponse.ToList();
            }

            await GetIngredientsPerRecipe();

            StateHasChanged();
        }


        public async Task CreateStep()
        {
            loading = @L["Loading"];
            StateHasChanged();
            if(step.ImageFile == null)
            {
                var authState = await AuthStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                step.RecipeID = RecipeId;
                step.RecipeIdS = RecipeId;
                var response = await stepService.CreateStep(step);
                if (response.Ok)
                {
                message = response.Data?.Mensaje ?? "Step created successfully";
                messageClass = "alert alert-success";
                }
                else
                {
                    message = "Error, No se Pudo Crear El Paso";
                    messageClass = "alert alert-danger";
                }
                ClearStep();
                steps = (await GetAllSteps())?.ToList() ?? new List<Core.Entities.Step>();
                StateHasChanged();
                
            }
            else
            {
                CreateStepImage();
            }
            
        }

        public async void CreateStepImage()
        {
            loading = @L["Loading"];
            StateHasChanged();
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            step.RecipeID = RecipeId;
            step.RecipeIdS = RecipeId;
            var response = await stepService.CreateStepImage(step);
            if (response.Ok)
            {
                message = response.Data?.Mensaje ?? "Step created successfully";
                messageClass = "alert alert-success";
            }
            else
            {
                message = "Error, No se Pudo Crear la Receta";
                messageClass = "alert alert-danger";
            }
            ClearStep();
            steps = (await GetAllSteps())?.ToList() ?? new List<Core.Entities.Step>();
            StateHasChanged();
        }


        public async Task<IEnumerable<Core.Entities.Step>?> GetAllSteps()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var temp = new List<Core.Entities.Step>();
            var response = await stepService.GetAllAsync();
            if (response.Datos != null)
            {
                foreach (var step in response.Datos)
                {
                    if (step.RecipeIdS == RecipeId)
                    {
                        temp.Add(step);
                    }
                }
            }
            return temp;
        }


        public async Task DeleteStep(int id)
        {
            var response = await stepService.DeleteStep(id);
            if (response.Ok)
            {
                var stepToDelete = steps.FirstOrDefault(s => s.Id == id);
                if (stepToDelete != null)
                {
                    steps.Remove(stepToDelete);
                }
                StateHasChanged();
            }
            else
            {
                message = response.message;
                messageClass = "alert alert-danger";
            }
        }


        public void FillStep(int stepId)
        {
            var stepToUpdate = steps.FirstOrDefault(s => s.Id == stepId);
            updatingStep = true;
            step.name = stepToUpdate?.Name;
            step.description = stepToUpdate?.Description;
            step.Duration = stepToUpdate?.Duration.ToString() ?? string.Empty;
        }


        public async Task UpdateStep()
        {
            loading = @L["Loading"];
            StateHasChanged();
            var response = await stepService.UpdateStep(step);
            if (response.Ok)
            {
                message = response.Data?.Mensaje ?? "Step updated successfully";
                messageClass = "alert alert-success";
            }
            else
            {
                message = response.message;
                messageClass = "alert alert-danger";
            }
            CancelUpdateStep();
            steps = (await GetAllSteps())?.ToList() ?? new List<Core.Entities.Step>();
        }


        public void CancelUpdateStep()
        {
            updatingStep = false;
            ClearStep();
        }


        public void OnImageSelected(InputFileChangeEventArgs e)
        {
            step.ImageFile = e.File;
        }


        public void ClearStep()
        {
            step.name = string.Empty;
            step.description = string.Empty;
            step.Duration = string.Empty;
            step.ImageFile = null;
            loading = string.Empty;
        }

        public void ClearIngredient()
        {
            ingredientPerRecipe.amount = string.Empty;
            selectedIngredient = null;
            selectedMeasure = null;
            loadingIngredient = string.Empty;
        }

        public void CancelUpdateIngredient()
        {
            updatingIngredient = false;
            ClearIngredient();
        }

        public async Task<IEnumerable<Core.Entities.Measure>?> GetAllMeasures()
        {
            var response = await measureService.GetAllAsync();
            return response.Datos;
        }

        public async Task<IEnumerable<Core.Entities.Ingredient>?> GetAllIngredients()
        {
            var response = await ingredientService.GetAllAsync();
            return response.Datos;
        }

        public async Task CreatIngredientPerRecipe()
        {
            if (!selectedIngredient.HasValue || !selectedMeasure.HasValue)
            {
                message = "Por favor, selecciona un ingrediente y una medida.";
                messageClass = "alert alert-warning";
                StateHasChanged();
                return;
            }

            loadingIngredient = @L["Loading"];
            StateHasChanged();

            ingredientPerRecipe.recipeID = RecipeId;

            ingredientPerRecipe.ingredientIdIPR = selectedIngredient.Value;
            ingredientPerRecipe.measureIdIPR = selectedMeasure.Value;

            var response = await ingredientPerRecipeService.Create(ingredientPerRecipe);

            if (response.Ok)
            {
                message = response.Data?.Mensaje ?? "Ingrediente añadido exitosamente";
                messageClass = "alert alert-success";
            }
            else
            {
                message = "Error, No se pudo añadir el ingrediente";
                messageClass = "alert alert-danger";
            }

            ClearIngredient();
            await GetIngredientsPerRecipe();
            loadingIngredient = string.Empty;
            StateHasChanged();
        }

        public async Task DeleteIngredientPerRecipe(int id)
        {
            var response = await ingredientPerRecipeService.Delete(id);
            if (response.Ok)
            {
                var ingredientToDelete = ingredientsPerRecipeList.FirstOrDefault(s => s.id == id);
                if (ingredientToDelete != null)
                {
                    ingredientsPerRecipeList.Remove(ingredientToDelete);
                }
                await GetIngredientsPerRecipe();
                StateHasChanged();
            }
            else
            {
                message = response.message;
                messageClass = "alert alert-danger";
            }
        }

        public async Task GetIngredientsPerRecipe()
        {
            ingredientsPerRecipeList.Clear(); 

            var allIngredientsPerRecipeResponse = await ingredientPerRecipeService.GetAllAsync();

            if (allIngredientsPerRecipeResponse.Datos == null)
                return;

            foreach (var ipr in allIngredientsPerRecipeResponse.Datos)
            {
                if (ipr.RecipeId != RecipeId)
                    continue;

                var ingredientResponse = await ingredientService.GetByIdAsync(ipr.IngredientIdIPR);
                var measureResponse = await measureService.GetByIdAsync(ipr.measureIdIPR);

                IngredientPerRecipeDTO dto = new IngredientPerRecipeDTO
                {
                    id = ipr.id,
                    recipeID = ipr.RecipeId,
                    ingredientIdIPR = ipr.IngredientIdIPR,
                    measureIdIPR = ipr.measureIdIPR,
                    amount = ipr.amount.ToString(),

                    ingredient = ingredientResponse?.Datos?.name ?? "—",
                    measure = measureResponse?.Datos?.name ?? "—"
                };

                ingredientsPerRecipeList.Add(dto);
            }
        }
    }
}
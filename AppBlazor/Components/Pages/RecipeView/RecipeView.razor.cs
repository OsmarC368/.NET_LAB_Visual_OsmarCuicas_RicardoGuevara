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
        public IngredientPerRecipeService? ingredientPerRecipeService { get; set; } = default!;
        [Inject]
        public StepService? stepService { get; set; } = default!;
        public Recipe? recipe { get; set; } = new Recipe();
        public string RecipeName { get; set; } = string.Empty;
        public List<Step> steps { get; set; } = new List<Step>();
        public List<IngredientPerRecipeDTO> recipeIngredients { get; set; } = new List<IngredientPerRecipeDTO>();

        protected async override Task OnInitializedAsync()
        {
            await LoadRecipe();
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

        /*private async void LoadIngredients()
        {
            var response = await ingredientPerRecipeService.GetAllAsync();
            if (response.Ok && response.Data != null)
            {
                var temp = response.Data.Where(i => i.RecipeId == RecipeId).ToList();
                foreach (var ingredient in temp)
                {
                    if (ingredient.RecipeId == RecipeId)
                    {
                        var newIngredient = new IngredientPerRecipeDTO();
                        newIngredient.meaure = ingredient.Measure;
                        recipeIngredients.Add(newIngredient);
                    }
                }
            }
        }*/
    }
}
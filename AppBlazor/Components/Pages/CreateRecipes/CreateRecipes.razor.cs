using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBlazor.Data.Models;
using AppBlazor.Data.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AppBlazor.Components.Pages.CreateRecipes
{
    public partial class CreateRecipes
    {
        public RecipeDTO recipe = new RecipeDTO();
        public List<Core.Entities.Recipe> recipes = new ();
        public string loading = string.Empty;
        public string message = string.Empty;
        public bool updating = false;
        public string messageClass = string.Empty;
        [Inject]
        public RecipesService? recipesService { get; set; }
        [Inject]
        public AuthenticationStateProvider? AuthStateProvider { get; set; }
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            loading = "loading...";
            StateHasChanged();
            recipes = (await GetAllRecipes())?.ToList() ?? new List<Core.Entities.Recipe>();
            loading = string.Empty;
            StateHasChanged();
        }

        public async Task CreateRecipe()
        {
            loading = "loading...";
            StateHasChanged();
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            var user = authState.User;
            recipe.userIDR = int.Parse(user.FindFirst("id")?.Value ?? "0");
            recipe.userRID = int.Parse(user.FindFirst("id")?.Value ?? "0");
            var response = await recipesService.CreateRecipe(recipe);
            if (response.Ok)
            {
                message = response.Data?.Mensaje ?? "Recipe created successfully";
                messageClass = "alert alert-success";
            }
            else
            {
                message = response.message;
                messageClass = "alert alert-danger";
            }
            Clear();
            recipes = (await GetAllRecipes())?.ToList() ?? new List<Core.Entities.Recipe>();
            StateHasChanged();

        }

        public async Task<IEnumerable<Core.Entities.Recipe>?> GetAllRecipes()
        {
            var response = await recipesService.GetAllRecipes();
            return response.Datos;
        }

        public async Task DeleteRecipe(int id)
        { 
            var response = await recipesService.DeleteRecipe(id);
            if (response.Ok)
            {
                var recipeToDelete = recipes.FirstOrDefault(r => r.Id == id);
                if (recipeToDelete != null)
                {
                    recipes.Remove(recipeToDelete);
                }
                StateHasChanged();
            }
            else
            {
                message = response.message;
                messageClass = "alert alert-danger";
            }
        }

        public void FillRecipe(int recipeId)
        {
            var recipeToUpdate = recipes.FirstOrDefault(r => r.Id == recipeId);
            updating = true;
            recipe.name = recipeToUpdate?.Name;
            recipe.description = recipeToUpdate?.Description;
            recipe.difficultyLevel = recipeToUpdate?.DifficultyLevel.ToString() ?? string.Empty;
            recipe.type = recipeToUpdate?.Type;
            recipe.visibility = recipeToUpdate?.Visibility ?? 1;
            recipe.Id = recipeToUpdate?.Id ?? 0;
        }

        public async Task UpdateRecipe()
        {
            loading = "loading...";
            StateHasChanged();
            var response = await recipesService.UpdateRecipe(recipe);
            if (response.Ok)
            {
                message = response.Data?.Mensaje ?? "Recipe updated successfully";
                messageClass = "alert alert-success";
            }
            else
            {
                message = response.message;
                messageClass = "alert alert-danger";
            }
            CancelUpdate();
            recipes = (await GetAllRecipes())?.ToList() ?? new List<Core.Entities.Recipe>();


        }

        public void CancelUpdate()
        {
            updating = false;
            Clear();
        }

        public void Clear()
        {
            recipe.name = string.Empty;
            recipe.description = string.Empty;
            recipe.difficultyLevel = string.Empty;
            recipe.type = string.Empty;
            loading = string.Empty;
        }

        public void DetallesReceta(int recipeId)
        {
            NavigationManager.NavigateTo($"/recipedetails/{recipeId}");
        }

    }
}
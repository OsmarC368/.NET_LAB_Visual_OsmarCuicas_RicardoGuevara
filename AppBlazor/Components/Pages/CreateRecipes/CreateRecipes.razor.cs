using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBlazor.Data.Models;
using AppBlazor.Data.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Core.Entities;

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
        public ThemeContainer? themeContainer { get; set; }
        [Inject]
        public NavigationManager? NavigationManager { get; set; }
        [Inject] private IStringLocalizer<SharedResources> L { get; set; }
        private string filterName = string.Empty;
        private string filterType = string.Empty;
        private string theme = string.Empty;
        private int? filterDifficulty = null;

        private IEnumerable<Recipe> FilteredRecipes => recipes
            .Where(r =>
                (string.IsNullOrWhiteSpace(filterName) || r.Name.Contains(filterName, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(filterType) || r.Type.Contains(filterType, StringComparison.OrdinalIgnoreCase)) &&
                (!filterDifficulty.HasValue || r.DifficultyLevel == filterDifficulty.Value)
            );

        private string GetDifficultyText(float level)
        {
            if (level == 1) return L["Easy"];
            if (level == 2) return L["Medium"];
            return L["Hard"];
        }

        private string GetVisibilityText(float level)
        {
            if (level == 1) return L["Public"];
            return L["Private"];
        }

        private string GetDifficultyClass(float level)
        {
            if (level == 1) return "bg-success";
            if (level == 2) return "bg-warning text-dark";
            return "bg-danger";
        }

        protected override async Task OnInitializedAsync()
        {
            theme = themeContainer.theme;
            loading = "loading...";
            StateHasChanged();
            recipes = (await GetAllRecipes())?.ToList() ?? new List<Core.Entities.Recipe>();
            loading = string.Empty;
            StateHasChanged();
        }

        public async void CreateWithImage()
        {
            if (recipe.ImageFile == null)
            {
                loading = string.Empty;
                message = "Debe Ingresar una Imagen para la Receta";
                messageClass = "alert alert-danger";
                StateHasChanged();
                return;
            }
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            var user = authState.User;
            recipe.userIDR = int.Parse(user.FindFirst("id")?.Value ?? "0");
            recipe.userRID = int.Parse(user.FindFirst("id")?.Value ?? "0");
            var response = await recipesService.CreateRecipeImage(recipe);
            if (response.Ok)
            {
                message = response.Data?.Mensaje ?? "Recipe created successfully";
                messageClass = "alert alert-success";
            }
            else
            {
                message = "Error, No se Pudo Crear la Receta";
                messageClass = "alert alert-danger";
            }
            Clear();
            recipes = (await GetAllRecipes())?.ToList() ?? new List<Core.Entities.Recipe>();
            StateHasChanged();
        }

        public async Task CreateRecipe()
        {
            loading = "loading...";
            StateHasChanged();
            if(recipe.ImageFile == null)
            {
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
                    message = "Error, No se Pudo Crear la Receta";
                    messageClass = "alert alert-danger";
                }
                Clear();
                recipes = (await GetAllRecipes())?.ToList() ?? new List<Core.Entities.Recipe>();
                StateHasChanged();
            }
            else
            {
                CreateWithImage();
            }

        }

        public async Task<IEnumerable<Core.Entities.Recipe>?> GetAllRecipes()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var temp = new List<Core.Entities.Recipe>();
            var response = await recipesService.GetAllRecipes();
            if (response.Datos != null)
            {
                foreach (var recipe in response.Datos)
                {
                    if (recipe.UserIdR ==  int.Parse(user.FindFirst("id")?.Value ?? "0"))
                    {
                        temp.Add(recipe);
                    }
                }
            }
            return temp;
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
            recipe.servings = recipeToUpdate?.Servings.ToString();
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

        public void OnImageSelected(InputFileChangeEventArgs e)
        {
            recipe.ImageFile = e.File;
        }

        public void Clear()
        {
            recipe.name = string.Empty;
            recipe.description = string.Empty;
            recipe.difficultyLevel = string.Empty;
            recipe.type = string.Empty;
            recipe.ImageFile = null;
            loading = string.Empty;
        }

        public void DetallesReceta(int recipeId)
        {
            NavigationManager.NavigateTo($"/recipe-details/{recipeId}");
        }

        private void GoBack()
        {
            Navigation.NavigateTo("/dashboard");
        }
    }
}
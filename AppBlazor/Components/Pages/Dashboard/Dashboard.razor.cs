using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.Extensions.Localization;
using AppBlazor.Data.Services;
using Core.Interfaces.Services;
using AppBlazor.Data.Models;

namespace AppBlazor.Components.Pages.Dashboard
{
    public partial class Dashboard : ComponentBase
    {
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
        [Inject] private AuthService authService { get; set; } = default!;
        [Inject] private IStringLocalizer<SharedResources> L { get; set; } = default!;
        [Inject] private IRecipeService RecipeService { get; set; } = default!;
        [Inject] private IIngredientService IngredientService { get; set; } = default!;
        [Inject] private IngredientPerRecipeService IngredientPerRecipeService { get; set; } = default!;

        private bool isAuthenticated = false;
        private bool isLoaded = false;
        private int userType = 0;

        private List<RecipeDTO> recipes = new();
        private List<IngredientPerRecipeDTO> ingredientsPerRecipe = new();
        private List<Ingredient> ingredients = new();

        private string filterName = string.Empty;
        private string filterType = string.Empty;
        private int? filterDifficulty = null;
        private string filterByAuthor = string.Empty;
        private void GoToMeasures() => Navigation.NavigateTo("/measure");
        private void GoToIngredients() => Navigation.NavigateTo("/ingredient");
        private void GoToRecipes() => Navigation.NavigateTo("/create-recipes");
        private void GoToRecipeDetails(int recipeId) => Navigation.NavigateTo($"/recipe-view/{recipeId}");
        private string filterIngredientsText = string.Empty;

        private IEnumerable<RecipeDTO> FilteredRecipes =>
            recipes
                .Select(r => new
                {
                    Recipe = r,
                    MatchCount = GetIngredientMatchCount(r)
                })
                .Where(x =>
                    x.Recipe.visibility == 1 &&
                    (string.IsNullOrWhiteSpace(filterName) ||
                        x.Recipe.name.Contains(filterName, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrWhiteSpace(filterType) ||
                        x.Recipe.type.Contains(filterType, StringComparison.OrdinalIgnoreCase)) &&
                    (!filterDifficulty.HasValue ||
                        x.Recipe.difficultyLevelFloat == filterDifficulty.Value) &&
                    (string.IsNullOrWhiteSpace(filterByAuthor) ||
                        (x.Recipe.author != null &&
                         x.Recipe.author.Contains(filterByAuthor, StringComparison.OrdinalIgnoreCase))) &&
                    (string.IsNullOrWhiteSpace(filterIngredientsText) || x.MatchCount > 0)
                )
                .OrderByDescending(x => x.MatchCount) 
                .Select(x => x.Recipe);

        protected override async Task OnInitializedAsync()
        {
            isLoaded = false;

            recipes = new();
            ingredientsPerRecipe = new();
            ingredients = new();

            var ingredientsPerRecipeResp = await IngredientPerRecipeService.GetAllAsync();
            if (ingredientsPerRecipeResp?.Ok == true && ingredientsPerRecipeResp.Datos != null)
            {
                ingredientsPerRecipe = ingredientsPerRecipeResp.Datos.Select(ipr => new IngredientPerRecipeDTO
                {
                    id = ipr.id,
                    recipeID = ipr.RecipeId,
                    ingredientIdIPR = ipr.IngredientIdIPR,
                    measureIdIPR = ipr.measureIdIPR,
                    amount = ipr.amount.ToString(),
                    ingredient = ipr.IngredientIPR?.name ?? string.Empty,
                    measure = string.Empty
                }).ToList();
            }

            var ingredientsResp = await IngredientService.GetAllAsync();
            if (ingredientsResp?.Ok == true && ingredientsResp.Datos != null)
                ingredients = ingredientsResp.Datos.ToList();

            var recipesResp = await RecipeService.GetAllAsync();
            if (recipesResp?.Ok == true && recipesResp.Datos != null)
            {
                foreach (var recipe in recipesResp.Datos)
                {
                    var dto = new RecipeDTO();

                    var userResp = await authService.GetByIdAsync(recipe.UserIdR);
                    dto.author = $"{userResp.Data.Datos.Name} {userResp.Data.Datos.Lastname}";

                    dto.Id = recipe.Id;
                    dto.name = recipe.Name;
                    dto.type = recipe.Type;
                    dto.imageUrl = recipe.imageUrl;
                    dto.difficultyLevelFloat = recipe.DifficultyLevel;
                    dto.visibility = recipe.Visibility;

                    recipes.Add(dto);
                }
            }

            isLoaded = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) return;

            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            isAuthenticated = user.Identity?.IsAuthenticated ?? false;

            if (isAuthenticated)
            {
                var claim = user.FindFirst("userType");
                if (claim != null && int.TryParse(claim.Value, out var parsed))
                    userType = parsed;
            }

            StateHasChanged();
        }

        private List<int> GetSelectedIngredientIds()
        {
            if (string.IsNullOrWhiteSpace(filterIngredientsText))
                return new();

            var searchTerms = filterIngredientsText
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(s => s.ToLower())
                .ToList();

            return ingredients
                .Where(i => searchTerms.Any(t => i.name.ToLower().Contains(t)))
                .Select(i => i.id)
                .ToList();
        }

        private int GetIngredientMatchCount(RecipeDTO recipe)
        {
            var selectedIds = GetSelectedIngredientIds();
            if (!selectedIds.Any())
                return 0;

            var recipeIngredientIds = ingredientsPerRecipe
                .Where(ipr => ipr.recipeID == recipe.Id)
                .Select(ipr => ipr.ingredientIdIPR)
                .ToList();

            return selectedIds.Count(id => recipeIngredientIds.Contains(id));
        }

        private string GetDifficultyText(float level)
        {
            if (level == 1) return L["Easy"];
            if (level == 2) return L["Medium"];
            return L["Hard"];
        }

        private string GetDifficultyClass(float level)
        {
            if (level == 1) return "bg-success";
            if (level == 2) return "bg-warning text-dark";
            return "bg-danger";
        }
    }
}

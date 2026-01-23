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
        [Inject] private NavigationManager Navigation { get; set; }
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] private AuthService authService { get; set; }
        [Inject] private IStringLocalizer<SharedResources> L { get; set; }
        [Inject] private IRecipeService RecipeService { get; set; }
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
        private int? selectedIngredientId;

        private IEnumerable<RecipeDTO> FilteredRecipes => recipes
            .Where(r =>
                r.visibility == 1 &&
                (string.IsNullOrWhiteSpace(filterName) ||
                    r.name.Contains(filterName, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(filterType) ||
                    r.type.Contains(filterType, StringComparison.OrdinalIgnoreCase)) &&
                (!filterDifficulty.HasValue ||
                    r.difficultyLevelFloat == filterDifficulty.Value) &&
                (string.IsNullOrWhiteSpace(filterByAuthor) ||
                    (r.author != null &&
                     r.author.Contains(filterByAuthor, StringComparison.OrdinalIgnoreCase))) &&
                (!selectedIngredientId.HasValue ||
                ingredientsPerRecipe.Any(ipr => ipr.recipeID == r.Id && ipr.ingredientIdIPR == selectedIngredientId.Value))
            );

        protected override async Task OnInitializedAsync()
        {
            isLoaded = false;

            recipes = new List<RecipeDTO>();
            ingredientsPerRecipe = new List<IngredientPerRecipeDTO>();
            ingredients = new List<Ingredient>();

            var usersDict = new Dictionary<int, string>();

            var recipesResponse = await RecipeService.GetAllAsync();

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

            var allIngredientsResponse = await IngredientService.GetAllAsync();
            if (allIngredientsResponse?.Ok == true && allIngredientsResponse.Datos != null)
                ingredients = allIngredientsResponse.Datos.ToList();

            if (recipesResponse?.Ok == true && recipesResponse.Datos != null)
            {
                foreach (var recipe in recipesResponse.Datos)
                {
                    if (!usersDict.ContainsKey(recipe.UserIdR))
                    {
                        var userResp = await authService.GetByIdAsync(recipe.UserIdR);
                        usersDict[recipe.UserIdR] = (userResp?.Ok == true && userResp.Datos != null)
                            ? $"{userResp.Datos.Name} {userResp.Datos.Lastname}"
                            : "Unknown";
                    }

                    recipes.Add(new RecipeDTO
                    {
                        Id = recipe.Id,
                        name = recipe.Name,
                        description = recipe.Description,
                        type = recipe.Type,
                        visibility = recipe.Visibility,
                        difficultyLevelFloat = recipe.DifficultyLevel,
                        imageUrl = recipe.imageUrl,
                        userRID = recipe.UserIdR,
                        author = usersDict[recipe.UserIdR]
                    });
                }
            }

            isLoaded = true;
            StateHasChanged();
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
                {
                    userType = parsed;
                }
            }

            isLoaded = true;
            StateHasChanged();
        }

        private void GoToMeasures() => Navigation.NavigateTo("/measure");
        private void GoToIngredients() => Navigation.NavigateTo("/ingredient");
        private void GoToRecipes() => Navigation.NavigateTo("/create-recipes");
        private void GoToRecipeDetails(int recipeId) => Navigation.NavigateTo($"/recipe-view/{recipeId}");

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

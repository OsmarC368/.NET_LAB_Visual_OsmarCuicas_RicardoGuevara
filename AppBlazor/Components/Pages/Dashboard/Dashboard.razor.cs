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


namespace AppBlazor.Components.Pages.Dashboard
{
    public partial class Dashboard : ComponentBase
    {
        [Inject] private NavigationManager Navigation { get; set; }
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] private IStringLocalizer<SharedResources> L { get; set; }
        [Inject] private IRecipeService RecipeService { get; set; }
        [Inject] private IIngredientService IngredientService { get; set; } = default!;

        private bool isAuthenticated = false;
        private bool isLoaded = false;
        private int userType = 0;
        private List<Recipe> recipes = new();
        private string filterName = string.Empty;
        private string filterType = string.Empty;
        private int? filterDifficulty = null;
        private string filterByAuthor = string.Empty;
        private int? selectedIngredientId = null;
        private List<Ingredient> ingredients = new();

        private IEnumerable<Recipe> FilteredRecipes => recipes
            .Where(r =>
                (string.IsNullOrWhiteSpace(filterName) || r.Name.Contains(filterName, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(filterType) || r.Type.Contains(filterType, StringComparison.OrdinalIgnoreCase)) &&
                (!filterDifficulty.HasValue || r.DifficultyLevel == filterDifficulty.Value) &&
                (!selectedIngredientId.HasValue || 
                r.IngredientsR.Any(ipr => ipr.IngredientIdIPR == selectedIngredientId.Value))
            );

        protected override async Task OnInitializedAsync()
        {
            var recipesResponse = await RecipeService.GetAllAsync();
            if (recipesResponse != null && recipesResponse.Ok && recipesResponse.Datos != null)
            {
                recipes = recipesResponse.Datos.ToList();
            }
            else
            {
                recipes = new List<Recipe>();
            }

            var ingredientsResponse = await IngredientService.GetAllAsync();
            if (ingredientsResponse != null && ingredientsResponse.Ok && ingredientsResponse.Datos != null)
            {
                ingredients = ingredientsResponse.Datos.ToList();
            }
            else
            {
                ingredients = new List<Ingredient>();
            }
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

        private void GoToRecipeDetails(int recipeId)
        {
            Navigation.NavigateTo("");
        }
    }
}

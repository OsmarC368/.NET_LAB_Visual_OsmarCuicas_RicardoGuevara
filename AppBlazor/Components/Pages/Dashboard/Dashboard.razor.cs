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

        private bool isAuthenticated = false;
        private bool isLoaded = false;
        private int userType = 0;
        private List<Recipe> recipes = new();

        protected override async Task OnInitializedAsync()
        {
            var response = await RecipeService.GetAllAsync();

            if (response != null && response.Ok && response.Datos != null)
            {
                recipes = response.Datos.ToList();
            }
            else
            {
                recipes = new List<Recipe>();
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
                var claim = user.FindFirst("UserTypeID");
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
            if (level <= 2) return L["Easy"];
            if (level <= 4) return L["Medium"];
            return L["Hard"];
        }

        private string GetDifficultyClass(float level)
        {
            if (level <= 2) return "bg-success";
            if (level <= 4) return "bg-warning text-dark";
            return "bg-danger";
        }
    }
}

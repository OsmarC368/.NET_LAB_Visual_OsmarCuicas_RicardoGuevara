using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBlazor.Data.Models;
using AppBlazor.Data.Services;
using Microsoft.Extensions.Localization;


namespace AppBlazor.Components.Pages.Profile
{
    public partial class Profile : ComponentBase
    {
        [Inject] private AuthService authService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private RecipeService RecipeService { get; set; } = default!;
        [Inject] private IStringLocalizer<SharedResources> L { get; set; } = default!;
        [Inject] private ThemeContainer themeContainer { get; set; } = default!;
        [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; } = default!;

        private bool isAuthenticated = false;
        private bool isLoaded = false;
        private int UserId;

        private string UserName = string.Empty;
        private string theme = string.Empty;
        private string UserLastname = string.Empty;
        private string UserEmail = string.Empty;

        private List<RecipeDTO> UserRecipes = new();

        protected override async Task OnInitializedAsync()
        {
            theme = themeContainer.theme;
            isLoaded = false;

            var authState = await AuthState;
            var user = authState.User;

            isAuthenticated = user.Identity?.IsAuthenticated ?? false;
            if (!isAuthenticated)
            {
                isLoaded = true;
                return;
            }

            var idClaim = user.FindFirst("id");
            if (idClaim == null || !int.TryParse(idClaim.Value, out var id))
            {
                isLoaded = true;
                return;
            }

            UserId = id;

            var userResponse = await authService.GetByIdAsync(UserId);

            if (userResponse?.Ok == true && userResponse.Data?.Ok == true && userResponse.Data.Datos != null)
            {
                var u = userResponse.Data.Datos; 
                UserName = u.Name;
                UserLastname = u.Lastname;
                UserEmail = u.Email;
            }

            var recipesResp = await RecipeService.GetAllAsync();

            if (recipesResp?.Ok == true && recipesResp.Datos != null)
            {
                UserRecipes = recipesResp.Datos
                    .Where(r => r.UserIdR == UserId)
                    .Select(r => new RecipeDTO
                    {
                        Id = r.Id,
                        name = r.Name,
                        type = r.Type,
                        imageUrl = r.imageUrl,
                        difficultyLevelFloat = r.DifficultyLevel,
                        visibility = r.Visibility
                    }).ToList();
            }

            isLoaded = true;
        }

        private string GetDifficultyText(float level)
        {
            return level switch
            {
                1 => L["Easy"],
                2 => L["Medium"],
                _ => L["Hard"]
            };
        }

        private string GetDifficultyClass(float level)
        {
            return level switch
            {
                1 => "bg-success",
                2 => "bg-warning text-dark",
                _ => "bg-danger"
            };
        }
    }
}

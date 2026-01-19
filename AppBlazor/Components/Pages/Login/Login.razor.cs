using AppBlazor.Data;
using AppBlazor.Data.Auth;
using AppBlazor.Data.Models;
using AppBlazor.Data.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AppBlazor.Components.Pages.Login
{
    public partial class Login
    {
        public UserDTO user = new UserDTO();
        public string loading = string.Empty;

        [Inject]
        public AuthService? authService { get; set; }
        [Inject]
        public NavigationManager? NavigationManager { get; set; }
        [Inject]
        public TokenContainer? tokenContainer { get; set; }
        [Inject] private IStringLocalizer<SharedResources> L { get; set; }
        [Inject]
        public Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

        public string message = string.Empty;
        public string messageClass = string.Empty;

        public async Task LoginUser()
        {
            if (user.email.Length < 2 || user.password.Length < 2)
            {
                message = "Email and Password are required";
                messageClass = "alert alert-danger";
                return;
            }

            loading = L["Loading"];
            StateHasChanged();

            var response = await authService!.Login(user);

            if (!response.Ok || response.Data == null || response.Data.Datos == null)
            {
                message = response.message ?? "Login failed";
                messageClass = "alert alert-danger";
                loading = string.Empty;
                StateHasChanged();
                return;
            }

            var jwt = response.Data.Datos.jwt;

            if (string.IsNullOrEmpty(jwt))
            {
                message = "Invalid token received";
                messageClass = "alert alert-danger";
                loading = string.Empty;
                StateHasChanged();
                return;
            }

            tokenContainer!.asingToken(jwt);

            ((CustomAuthorizationStateProvider)AuthenticationStateProvider!)
                .AuthenticateUser(jwt);

            NavigationManager!.NavigateTo("/dashboard");
        }


        public void Clear()
        {
            user.email = string.Empty;
            user.password = string.Empty;
            loading = string.Empty;
        }
    }
}
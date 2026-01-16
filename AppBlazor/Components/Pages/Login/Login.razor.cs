using AppBlazor.Data;
using AppBlazor.Data.Auth;
using AppBlazor.Data.Models;
using AppBlazor.Data.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
        [Inject]
        public Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

        public string message = string.Empty;
        public string messageClass = string.Empty;

        public async void LoginUser()
        {
            if(user.email.Length < 2 || user.password.Length < 2)
            {
                message = "Email and Password are required";
                messageClass = "alert alert-danger";
                return;
            }
            loading = "Loading...";
            StateHasChanged();
            var response = await authService!.Login(user);

            if(response.Ok)
            {
                tokenContainer.asingToken(response.Data.Datos.jwt);
                ((CustomAuthorizationStateProvider)AuthenticationStateProvider).AuthenticateUser(response.message);
                NavigationManager.NavigateTo("/dashboard");
            }
            else
            {
                message = response.message;
                messageClass = "alert alert-danger";
            }
            Clear();
            StateHasChanged();
        }

        public void Clear()
        {
            user.email = string.Empty;
            user.password = string.Empty;
            loading = string.Empty;
        }
    }
}
using AppBlazor.Data.Models;
using AppBlazor.Data.Services;
using Core.Entities;
using Microsoft.AspNetCore.Components;

namespace AppBlazor.Components.Pages.Registration
{
    public partial class Register
    {
        public UserDTO user = new UserDTO();

        [Inject]
        public AuthService? authService { get; set; }

        public string message = string.Empty;
        public string messageClass = string.Empty;

        public async void RegisterUser()
        {
            if(user.email.Length < 2 || user.password.Length < 2)
            {
                message = "Email and Password are required";
                messageClass = "alert alert-danger";
                return;
            }

            var response = await authService!.Register(user);

            if(response.Ok)
            {
                message = "Registration Successful!";
                messageClass = "alert alert-success";
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
            user.name = string.Empty;
            user.lastname = string.Empty;
        }
    }
}
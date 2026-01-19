using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AppBlazor.Data.Models;
using AppBlazor.Data.Services;
using Core.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Core.Interfaces.Services;
using Microsoft.Extensions.Localization;
using Core.Entities;
namespace AppBlazor.Components.Pages.RecipeView
{
    public partial class RecipeView
    {
        [Parameter]
        public int RecipeId { get; set; }
        [Inject]
        public RecipeService? RecipeService { get; set; } = default!;
        public Recipe? recipe { get; set; } = new Recipe();

        protected async override Task OnInitializedAsync()
        {
            await LoadRecipe();
        }

        public async Task LoadRecipe()
        {
            var response = await RecipeService.GetByIdAsync(RecipeId);
            if (response.Ok && response.Datos != null)
            {
                recipe = response.Datos;
            }
        }


    }
}
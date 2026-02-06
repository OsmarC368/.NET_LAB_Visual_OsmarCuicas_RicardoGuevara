using Microsoft.AspNetCore.Components;
using AppBlazor.Data.Services;
using AppBlazor.Data.Models.Core;
using AppBlazor.Data.Models.Core.Responses;

namespace AppBlazor.Components.Pages.Ingredients
{
    public partial class Ingredients
    {
        [Inject] private IngredientService IngredientService { get; set; } = default!;
        [Inject] private ThemeContainer themeContainer { get; set; } = default!;

        protected List<Ingredient> ingredients = new();
        protected Ingredient currentIngredient = new();
        protected bool isEditMode = false;
        protected string message = "";
        protected string theme = "";
        protected bool success = false;

        protected override async Task OnInitializedAsync()
        {
            theme = themeContainer.theme;
            await LoadIngredients();
        }

        private async Task LoadIngredients()
        {
            var response = await IngredientService.GetAllAsync();
            if (response.Ok && response.Datos != null)
            {
                ingredients = response.Datos.ToList();
            }
        }

        protected async Task SaveIngredient()
        {
            bool exists = ingredients.Any(i =>
                i.name.Trim().Equals(currentIngredient.name.Trim(), StringComparison.OrdinalIgnoreCase)
                && (!isEditMode || i.id != currentIngredient.id)
            );

            if (exists)
            {
                message = "Ya existe un ingrediente con ese nombre.";
                success = false;
                return;
            }

            Response<Ingredient> result;

            if (isEditMode)
                result = await IngredientService.Update(currentIngredient.id, currentIngredient);
            else
                result = await IngredientService.Create(currentIngredient);

            if (result.Ok)
            {
                message = isEditMode ? "Ingrediente actualizado correctamente" : "Ingrediente creado correctamente";
                success = true;
                currentIngredient = new Ingredient();
                isEditMode = false;
                await LoadIngredients();
            }
            else
            {
                message = "Error: " + (string.IsNullOrEmpty(result.Mensaje) ? "Sin detalles de la API" : result.Mensaje);
                success = false;
            }
        }

        protected void EditIngredient(Ingredient i)
        {

            bool exists = ingredients.Any(i =>
                i.name.Trim().Equals(currentIngredient.name.Trim(), StringComparison.OrdinalIgnoreCase)
                && (!isEditMode || i.id != currentIngredient.id)
            );

            if (exists)
            {
                message = "Ya existe un ingrediente con ese nombre.";
                success = false;
                return;
            }

            currentIngredient = new Ingredient
            {
                id = i.id,
                name = i.name,
                type = i.type
            };
            isEditMode = true;
            message = "";
        }

        protected async Task DeleteIngredient(Ingredient i)
        {
            var result = await IngredientService.Remove(i.id);
            if (result.Ok)
            {
                message = "Ingrediente eliminado correctamente";
                success = true;
                await LoadIngredients();
            }
            else
            {
                message = "Error al eliminar: " + result.Mensaje;
                success = false;
            }
        }

        protected void CancelEdit()
        {
            currentIngredient = new Ingredient();
            isEditMode = false;
            message = "";
        }

        private void GoBack()
        {
            Navigation.NavigateTo("/dashboard");
        }
    }
}

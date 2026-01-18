using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Components;
using Core.Responses;          
using AppBlazor.Data.Services;

namespace AppBlazor.Components.Pages.Ingredients
{
    public partial class Ingredients
    {
        [Inject] private IIngredientService IngredientService { get; set; } = default!;

        protected List<Ingredient> ingredients = new();
        protected Ingredient currentIngredient = new();
        protected bool isEditMode = false;
        protected string message = "";
        protected bool success = false;

        protected override async Task OnInitializedAsync()
        {
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
                i.Name.Trim().Equals(currentIngredient.Name.Trim(), StringComparison.OrdinalIgnoreCase)
                && (!isEditMode || i.Id != currentIngredient.Id)
            );

            if (exists)
            {
                message = "Ya existe un ingrediente con ese nombre.";
                success = false;
                return;
            }

            Response<Ingredient> result;

            if (isEditMode)
                result = await IngredientService.Update(currentIngredient.Id, currentIngredient);
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
                i.Name.Trim().Equals(currentIngredient.Name.Trim(), StringComparison.OrdinalIgnoreCase)
                && (!isEditMode || i.Id != currentIngredient.Id)
            );

            if (exists)
            {
                message = "Ya existe un ingrediente con ese nombre.";
                success = false;
                return;
            }

            currentIngredient = new Ingredient
            {
                Id = i.Id,
                Name = i.Name,
                Type = i.Type
            };
            isEditMode = true;
            message = "";
        }

        protected async Task DeleteIngredient(Ingredient i)
        {
            var result = await IngredientService.Remove(i.Id);
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

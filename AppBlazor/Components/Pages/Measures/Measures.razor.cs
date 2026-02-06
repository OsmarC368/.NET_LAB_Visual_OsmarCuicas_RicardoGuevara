using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Components;
using Core.Responses;          
using AppBlazor.Data.Services;

namespace AppBlazor.Components.Pages.Measures
{
    public partial class Measures
    {
        [Inject] private IMeasureService MeasureService { get; set; } = default!;
        [Inject] private ThemeContainer themeContainer { get; set; } = default!;

        protected List<Measure> measures = new();
        protected Measure currentMeasure = new();
        protected bool isEditMode = false;
        protected string message = "";
        protected string theme = "";
        protected bool success = false;

        protected override async Task OnInitializedAsync()
        {
            theme = themeContainer.theme;
            await LoadMeasures();
        }

        private async Task LoadMeasures()
        {
            var response = await MeasureService.GetAllAsync();
            if (response.Ok && response.Datos != null)
            {
                measures = response.Datos.ToList();
            }
        }

        protected async Task SaveMeasure()
        {

            bool exists = measures.Any(m =>
                m.name.Trim().Equals(currentMeasure.name.Trim(), StringComparison.OrdinalIgnoreCase)
                && (!isEditMode || m.id != currentMeasure.id)
            );

            if (exists)
            {
                message = @L["Ya existe una Measure con ese nombre."];
                success = false;
                return;
            }
            Response<Measure> result;

            if (isEditMode)
                result = await MeasureService.Update(currentMeasure.id, currentMeasure);
            else
                result = await MeasureService.Create(currentMeasure);

            if (result.Ok)
            {
                message = isEditMode ? "Medida actualizada correctamente" : "Medida creada correctamente";
                success = true;
                currentMeasure = new Measure();
                isEditMode = false;
                await LoadMeasures();
            }
            else
            {
                message = "Error: " + (string.IsNullOrEmpty(result.Mensaje) ? "Sin detalles de la API" : result.Mensaje);
                success = false;
            }
        }

        protected void EditMeasure(Measure m)
        {
            currentMeasure = new Measure
            {
                id = m.id,
                name = m.name,
                symbol = m.symbol
            };
            isEditMode = true;
            message = "";
        }

        protected async Task DeleteMeasure(Measure m)
        {

            bool exists = measures.Any(m =>
                m.name.Trim().Equals(currentMeasure.name.Trim(), StringComparison.OrdinalIgnoreCase)
                && (!isEditMode || m.id != currentMeasure.id)
            );

            if (exists)
            {
                message = "Ya existe una medida con ese nombre.";
                success = false;
                return;
            }

            var result = await MeasureService.Remove(m.id);
            if (result.Ok)
            {
                message = "Medida eliminada correctamente";
                success = true;
                await LoadMeasures();
            }
            else
            {
                message = "Error al eliminar: " + result.Mensaje;
                success = false;
            }
        }

        protected void CancelEdit()
        {
            currentMeasure = new Measure();
            isEditMode = false;
            message = "";
        }

        private void GoBack()
        {
            Navigation.NavigateTo("/dashboard");
        }
    }
}

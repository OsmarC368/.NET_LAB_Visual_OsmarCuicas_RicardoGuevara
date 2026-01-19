using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace AppBlazor.Data.Models
{
    public class StepDTO
    {
        [Required(ErrorMessage = "El Nombre es Requerido!")]
        [StringLength(225, ErrorMessage = "El Nombre es Demasiado Corto", MinimumLength =2)]
        public string name {get; set;} = string.Empty;

        [Required(ErrorMessage = "La Descripción es Requerida!")]
        [StringLength(225, ErrorMessage = "La Descripción es Debe Contener al Menos 5 Caracteres", MinimumLength =5)]
        public string description {get; set;} = string.Empty;

        [Required(ErrorMessage = "La Duración es Requerido!")]
        [Range(1, 50000000, ErrorMessage = "La Duración Debe Ser Mayor a 0")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Solo se Permiten Numeros")]
        public string Duration {get; set;} = string.Empty;

        [Required(ErrorMessage = "El Apellido es Requerido!")]
        [StringLength(225, ErrorMessage = "El Apellido es Demasiado Corto", MinimumLength =2)]
        public string imageURL {get; set;} = string.Empty;
        public int RecipeID {get; set;} = 0;
        public int RecipeIdS {get; set;} = 0;
        public int id {get; set;} = 0;
        // Nuevo campo para la imagen
        public IBrowserFile? ImageFile { get; set; }
    }
}
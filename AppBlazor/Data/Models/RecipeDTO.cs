using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace AppBlazor.Data.Models
{
    public class RecipeDTO
    {
        [Required(ErrorMessage = "El Nombre es Requerido!")]
        [StringLength(225, ErrorMessage = "El Nombre es Demasiado Corto", MinimumLength =2)]
        public string name {get; set;} = string.Empty;

        [Required(ErrorMessage = "La Descripción es Requerida!")]
        [StringLength(225, ErrorMessage = "La Descripción es Demasiado Corta", MinimumLength =5)]
        public string description {get; set;} = string.Empty;

        [Required(ErrorMessage = "El Tipo de Receta es Requerido!")]
        [StringLength(225, ErrorMessage = "El Tipo es Demasiado Corto", MinimumLength =2)]
        public string type {get; set;} = string.Empty;

        [Required(ErrorMessage = "El Nivel de Dificultad es Requerido!")]
        [Range(1, 5, ErrorMessage = "El Nivel de Dificultad Debe Estar Entre 1 y 5")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Solo se Permiten Numeros")]
        public float difficultyLevelFloat {get; set;}
        [Required(ErrorMessage = "El Numero de Platos es Requerido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Solo se permiten números enteros positivos")]
        [Range(1, int.MaxValue, ErrorMessage = "La Cantidad de Platos debe ser de al menos 1")]
        public string servings { get; set;}
        public string difficultyLevel {get; set;} = string.Empty;
        public int visibility {get; set;} = 1;
        public int userIDR {get; set;} = 0;
        public int userRID {get; set;} = 0;
        public int Id {get; set;} = 0;
        public string imageUrl { get; set; } = "";

        // Nuevo campo para la imagen
        public IBrowserFile? ImageFile { get; set; }

        public string? author { get; set; }

        public List<string> ingridients { get; set; } = new();
    }
}
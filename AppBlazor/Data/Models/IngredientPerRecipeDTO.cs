using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppBlazor.Data.Models
{
    public class IngredientPerRecipeDTO
    {

        [Required(ErrorMessage = "La Cantidad es Requerida!")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Solo se Permiten Numeros")]
        public string amount {get; set;} = string.Empty;
        public int ingredientIdIPR {get; set;} = 0;
        public int measureIdIPR {get; set;} = 0;
        public int recipeID {get; set;} = 0;
        public int id {get; set;} = 0;

        public string measure { get; set; } = string.Empty;
        public string ingredient { get; set; } = string.Empty;
    }
}
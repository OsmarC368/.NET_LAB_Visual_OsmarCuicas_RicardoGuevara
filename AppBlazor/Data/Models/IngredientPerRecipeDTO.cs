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
        public float amount {get; set;} = 0;
        public int ingredientID {get; set;} = 0;
        public int recipeID {get; set;} = 0;
        public int Id {get; set;} = 0;
    }
}
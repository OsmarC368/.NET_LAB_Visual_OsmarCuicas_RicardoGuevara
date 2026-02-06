using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBlazor.Data.Models.Core
{
    public class IngredientsPerRecipe
    {
        public int id { get; set; }
        public int RecipeId { get; set; }
        public int IngredientIdIPR { get; set; }
        public int measureIdIPR { get; set; }
        public float amount { get; set; }

        public virtual Recipe? RecipeIPR { get; set; }
        public virtual Ingredient? IngredientIPR { get; set; }
    }
}
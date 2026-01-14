using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces.Entities;

namespace Core.Entities
{
    public class Recipe : IRecipe
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Type { get; set; } = "";
        public virtual List<IngredientsPerRecipe> IngredientsR { get; set; } = new();
        public virtual List<Step> StepsR { get; set; } = new();
        public float DifficultyLevel { get; set; }
        public int Visibility { get; set; }
        public int UserIdR { get; set; }
        public virtual User? UserR { get; set; }
    }
}
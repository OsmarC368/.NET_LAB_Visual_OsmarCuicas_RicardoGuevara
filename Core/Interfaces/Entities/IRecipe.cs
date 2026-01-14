using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Entities
{
    public interface IRecipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public List<IngredientsPerRecipe> IngredientsR { get; set; }
        public List<Step> StepsR { get; set; }
        public float DifficultyLevel { get; set; }
        public int Visibility { get; set; }
        public int UserIdR { get; set; }

    }
}
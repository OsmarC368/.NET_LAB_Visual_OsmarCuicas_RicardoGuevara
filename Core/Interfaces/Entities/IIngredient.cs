using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Entities
{
    public interface IIngredient
    {
        public int Id { get; set; }
		public string Name { get; set; }
		public string Type { get; set;}

    }
}
using Core.Interfaces.Entities;

namespace Core.Entities
{
	public class Ingredient : IIngredient
	{
		public int Id { get; set; }
		public string Name { get; set; } = "";
		public string Type { get; set;} = "";

	}
}
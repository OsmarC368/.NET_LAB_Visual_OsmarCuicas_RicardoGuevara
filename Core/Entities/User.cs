using Core.Interfaces.Entities;

namespace Core.Entities
{
	public class User : IUser
	{
		public int Id { get; set; }
		public string Name { get; set; } = "";
		public string Lastname { get; set;} = "";
		public string Email { get; set; } = "";
		public string Password { get; set; } = "";
		public int UserTypeID { get; set; }
		public virtual List<Recipe> Recipes { get; set; } = new();
		public virtual UserType? UserType { get; set;}
	}
}

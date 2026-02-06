namespace AppBlazor.Data.Models.Core
{
	public class UserCopy
	{
		public int id { get; set; }
		public string name { get; set; } = "";
		public string lastname { get; set;} = "";
		public string email { get; set; } = "";
		public string password { get; set; } = "";
		public int userTypeID { get; set; }
		public virtual List<Recipe> recipes { get; set; } = new();
		public virtual UserType? userType { get; set;}
	}
}

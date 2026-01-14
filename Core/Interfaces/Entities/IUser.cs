using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Entities
{
    public interface IUser
    {
        public int Id { get; set; }
		public string Name { get; set; }
		public string Lastname { get; set;}
		public string Email { get; set; }
		public string Password { get; set; }
		public int UserTypeID { get; set; }
    }
}
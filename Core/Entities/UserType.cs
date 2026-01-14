using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces.Entities;

namespace Core.Entities
{
    public class UserType : IUserType
    {
        public int id { get; set; }
        public string name { get; set; } = "";
        public string description { get; set; } = "";

    }
}
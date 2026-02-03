using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class StepUser
    {
        public int id { get; set; }
        public bool completed { get ; set; }
        public string comment { get; set; } = string.Empty;

        public int stepSURID { get; set; }
        public int userSURID { get; set; }

        public virtual Step? StepSUR { get; set; }
        public virtual User? UserSUR { get; set; }
    }
}
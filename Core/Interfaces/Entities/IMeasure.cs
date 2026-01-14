using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Entities
{
    public interface IMeasure
    {
        public int id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
    }
}
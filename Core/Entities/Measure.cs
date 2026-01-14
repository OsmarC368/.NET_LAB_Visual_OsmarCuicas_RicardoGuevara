using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces.Entities;

namespace Core.Entities
{
    public class Measure : IMeasure
    {
        public int id { get; set; }
        public string name { get; set; } = "";
        public string symbol { get; set; } = "";
    }
}
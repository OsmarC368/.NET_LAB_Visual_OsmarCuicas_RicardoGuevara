using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBlazor.Data.Models.Core
{
    public class Step
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string imageURL { get; set; } = "";
        public string videoURL { get; set; } = "";
        public float Duration { get; set; }
        public float currentTimer { get; set; }
        public int RecipeIdS { get; set; }
        public string note { get; set; } = "";
        public virtual Recipe? RecipeS { get; set; }
        public Timer stepTimer { get; set; }
        public string timerValue { get; set; }
    }
}
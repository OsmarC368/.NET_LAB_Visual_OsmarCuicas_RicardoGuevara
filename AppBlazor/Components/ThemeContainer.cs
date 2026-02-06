using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBlazor.Components.Pages
{
    public class ThemeContainer
    {
        public string theme { get; set; } = string.Empty;

        public void changeTheme()
        {
            if (theme == string.Empty)
                theme = "dark-mode";
            else
                theme = string.Empty;
            NotifyStateChanged();
        }

        public event Action? Onchange;

        private void NotifyStateChanged() => Onchange?.Invoke();

    }
}
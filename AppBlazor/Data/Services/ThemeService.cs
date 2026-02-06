using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace AppBlazor.Data.Services
{
    public class ThemeService
    {
        public bool IsDarkModeActive { get; private set; }
        private readonly IJSRuntime _js;
        public ThemeService(IJSRuntime js) => _js = js;

        public async Task ToggleTheme()
        {
            IsDarkModeActive = !IsDarkModeActive;
            string theme = IsDarkModeActive ? "dark-mode" : "light-mode";

            await _js.InvokeVoidAsync("eval", $@"
                document.querySelector('.page').className = 'page {theme}';
                document.cookie = 'user-theme={theme}; path=/; max-age=31536000';
            ");
        }


    }
}
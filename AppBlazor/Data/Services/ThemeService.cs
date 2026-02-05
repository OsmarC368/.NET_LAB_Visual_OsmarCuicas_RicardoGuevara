using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace AppBlazor.Data.Services
{
    public class ThemeService
    {
        public bool IsDarkModeActive { get; set; }
        private readonly IJSRuntime _js;
        public ThemeService(IJSRuntime js) => _js = js;

        public async Task ToggleTheme()
        {
            IsDarkModeActive = !IsDarkModeActive;
            await _js.InvokeVoidAsync("applyGlobalTheme", IsDarkModeActive);
        }

    }
}
using AppBlazor.Components;
using AppBlazor.Data;
using AppBlazor.Data.Auth;
using AppBlazor.Data.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Core.Interfaces.Services;
using Blazored.SessionStorage;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => options.DetailedErrors = true);
builder.Services.AddAuthorizationCore();

builder.Services.AddBlazoredSessionStorage(); 

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Custom Services
builder.Services.AddSingleton<TokenContainer>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<Consumer>();
builder.Services.AddSingleton<RecipesService>();
builder.Services.AddSingleton<IMeasureService, MeasureService>();
builder.Services.AddSingleton<IIngredientService, IngredientService>();
builder.Services.AddSingleton<IRecipeService, RecipeService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthorizationStateProvider>();

var app = builder.Build();

var supportedCultures = new[]
{
    new CultureInfo("es"),
    new CultureInfo("en")
};

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("es"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

localizationOptions.RequestCultureProviders.Clear();
localizationOptions.RequestCultureProviders.Add(
    new CookieRequestCultureProvider()
);

app.UseRequestLocalization(localizationOptions);

app.MapGet("/set-culture", (string culture, string redirectUri, HttpContext context) =>
{
    context.Response.Cookies.Append(
        CookieRequestCultureProvider.DefaultCookieName,
        CookieRequestCultureProvider.MakeCookieValue(
            new RequestCulture(culture)
        ),
        new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            IsEssential = true,
            Path = "/" 
        }
    );

    var absoluteUrl =
        $"{context.Request.Scheme}://{context.Request.Host}{redirectUri}";

    context.Response.Redirect(absoluteUrl);
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

using AppBlazor.Components;
using AppBlazor.Data;
using AppBlazor.Data.Auth;
using AppBlazor.Data.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Core.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => options.DetailedErrors = true);
builder.Services.AddAuthorizationCore();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

//Custom Services
//builder.Services.AddSingleton<StateContainer>();
builder.Services.AddSingleton<TokenContainer>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<RecipesService>();
builder.Services.AddSingleton<StepService>();
builder.Services.AddSingleton<IngredientPerRecipeService>();
builder.Services.AddSingleton<Consumer>();
builder.Services.AddSingleton<IMeasureService, MeasureService>();
builder.Services.AddSingleton<MeasureService>();
builder.Services.AddSingleton<IIngredientService, IngredientService>();
builder.Services.AddSingleton<IngredientService>();
builder.Services.AddSingleton<IRecipeService, RecipeService>();
builder.Services.AddSingleton<StepUserService>();


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthorizationStateProvider>();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddHttpContextAccessor();

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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

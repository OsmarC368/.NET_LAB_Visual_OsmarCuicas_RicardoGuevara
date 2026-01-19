using AppBlazor.Components;
using AppBlazor.Data;
using AppBlazor.Data.Auth;
using AppBlazor.Data.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => options.DetailedErrors = true);
builder.Services.AddAuthorizationCore();

//Custom Services
//builder.Services.AddSingleton<StateContainer>();
builder.Services.AddSingleton<TokenContainer>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<RecipesService>();
builder.Services.AddSingleton<StepService>();
builder.Services.AddSingleton<IngredientPerRecipeService>();
builder.Services.AddSingleton<IngredientService>();
builder.Services.AddSingleton<MeasureService>();
builder.Services.AddSingleton<Consumer>();


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthorizationStateProvider>();

var app = builder.Build();


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

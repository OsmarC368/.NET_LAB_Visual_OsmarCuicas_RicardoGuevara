using AppBlazor.Components;
using AppBlazor.Data;
using AppBlazor.Data.Auth;
using AppBlazor.Data.Services;
using Microsoft.AspNetCore.Components.Authorization;
using AppBlazor.Data.Services;
using Core.Interfaces.Services;
using Blazored.SessionStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => options.DetailedErrors = true);
builder.Services.AddAuthorizationCore();

builder.Services.AddBlazoredSessionStorage(); 

// Custom Services
builder.Services.AddSingleton<TokenContainer>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<Consumer>();
builder.Services.AddSingleton<IMeasureService, MeasureService>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthorizationStateProvider>();

var app = builder.Build();

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

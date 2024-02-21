using Microsoft.Extensions.Configuration;
using TalkAzureIAVision.BlazorApp.Code;
using TalkAzureIAVision.BlazorApp.Components;

var builder = WebApplication.CreateBuilder(args);

var config = new TalkAzureIAVisionConfig();
builder.Configuration.GetSection(TalkAzureIAVisionConfig.SECTION_CONFIG_NAME).Bind(config);

// Add services to the container.
builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

builder.Services.AddSingleton(config);
builder.Services.AddScoped<ImageAnalysisHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

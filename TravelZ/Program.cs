using TravelZ.Core.Infrastructure;
using System.Text.Json;
using TravelZ.Models;
using TravelZ.Core.Models;
using TravelZ.Core.Services;
using TravelZ.Core.Interfaces;

void ConfigureApiClients(IServiceCollection services, IConfiguration configuration)
{
    var apiClientSection = configuration.GetSection("ApiClient");
    var client = apiClientSection.Get<ApiClientConfig>();
    if (client != null)
    {
        services.AddHttpClient(client.Name)
            .AddHttpMessageHandler<ApiHttpMessageHandler>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(client.BaseUrl);
            });
        
        services.AddScoped<IApiClientService, ApiClientService>(provider =>
            new ApiClientService(
                provider.GetRequiredService<IHttpClientFactory>(),
                provider.GetRequiredService<IHttpContextAccessor>(),
                provider.GetRequiredService<ILogger<ApiClientService>>(),
                client.Name)
            );
    }
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ApiHttpMessageHandler>();

ConfigureApiClients(builder.Services, builder.Configuration);

var menuJson = File.ReadAllText("menu.json");
var menuItems = JsonSerializer.Deserialize<List<MenuItem>>(menuJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
    ?? new List<MenuItem>();
builder.Services.AddSingleton(menuItems);

var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

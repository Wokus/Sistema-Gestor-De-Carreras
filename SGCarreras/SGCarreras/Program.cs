using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SGCarreras.Controllers;
using SGCarreras.Data;
using System;
using System.Globalization;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SGCarrerasContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SGCarrerasContext") ?? throw new InvalidOperationException("Connection string 'SGCarrerasContext' not found.")));

// Agregar servicios MVC y Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

// Agregar autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

// Configurar culturas soportadas y cultura predeterminada
var supportedCultures = new[] { new CultureInfo("es-ES") };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("es-ES");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SGCarrerasContext>();
    dbContext.Database.Migrate(); // Aplicar migraciones pendientes
}

app.UseRequestLocalization(app.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<RequestLocalizationOptions>>().Value);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SGCarrerasContext>();
    var clientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();

    var controlador = new SGCarreras.Controllers.CarrerasController(context, clientFactory);
    await controlador.InicializarCarrerasActivasAsync(clientFactory);
}


app.Run();

/* using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var clientFactory = services.GetRequiredService<IHttpClientFactory>();
    var carrerasController = new SGCarreras.Controllers.CarrerasController(
        services.GetRequiredService<SGCarreras.Data.SGCarrerasContext>()
    );

    await carrerasController.InicializarCarrerasActivasAsync(clientFactory);
}
*/


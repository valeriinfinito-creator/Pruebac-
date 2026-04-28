using DeportivoApp.Data;
using DeportivoApp.Helpers;
using DeportivoApp.Services.Implementations;
using DeportivoApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Conexión a MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Falta ConnectionStrings:DefaultConnection en appsettings.json");
}

builder.Services.AddDbContext<MySqlDBContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 36))
    )
);

// 2️⃣ SMTP (MailKit)
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);

builder.Services.AddScoped<EmailHelper>();

// 3️⃣ SERVICIOS (Dependency Injection)
builder.Services.AddScoped<IEspacioService, EspacioService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();
builder.Services.AddScoped<IReporteService, ReporteService>();

// 4️⃣ MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 5️⃣ Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 6️⃣ Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
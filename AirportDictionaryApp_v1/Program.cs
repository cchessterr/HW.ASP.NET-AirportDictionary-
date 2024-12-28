using AirportDictionaryApp_v1.Model;
using AirportDictionaryApp_v1.Service;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// добавление сервисов приложения
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddTransient<CountryService>();
builder.Services.AddTransient<AirportService>();
builder.Services.AddTransient<CompanyService>();

var app = builder.Build();

// конфигурирование приложения
app.MapControllers();

app.Run();


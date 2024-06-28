using StocksApp.ServiceContract;
using StocksApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IFinnhubService,FinnhubService>();


var app = builder.Build();



app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

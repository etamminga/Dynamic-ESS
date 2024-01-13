using Dynamic_ESS.SolarForecast;
using Dynamic_ESS.EnergyPrices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<SolarForecastOptions>(
    builder.Configuration.GetSection(SolarForecastOptions.SolarForecast));
builder.Services.AddTransient<SolarForecastClient>();

builder.Services.Configure<EntsoEClientOptions>(
    builder.Configuration.GetSection(EntsoEClientOptions.EntsoE));
builder.Services.AddTransient<EntsoEClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();

app.Run();

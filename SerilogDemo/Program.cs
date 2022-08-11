//https://www.youtube.com/watch?v=oXNslgIXIbQ&ab_channel=IAmTimCorey
using Microsoft.Extensions.Logging.Configuration;
using Serilog;
using SerilogDemo.Data;

#region Serilog
var configuraiton = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json")
  .Build();

Log.Logger = new LoggerConfiguration()
  .ReadFrom.Configuration(configuraiton)
  .CreateLogger();

Log.Information("================================");
Log.Information("App starting up at {datetime} UTC", DateTime.UtcNow);
#endregion

try
{
  var builder = WebApplication.CreateBuilder(args);

  #region Serilog
  builder.Host.UseSerilog();
  #endregion

  #region Default Logging Stuff
  //builder.Host.ConfigureLogging((context, logging) =>
  //{
  //  logging.ClearProviders();
  //  logging.AddConfiguration(context.Configuration.GetSection("Logging"));
  //  //logging.AddDebug();
  //  logging.AddConsole();
  //  //EventSource, EventLog, TraceSource, AzureAppServicesFile, AzureAppServicesBlob, ApplicationInsights
  //});
  #endregion

  // Add services to the container.
  builder.Services.AddRazorPages();
  builder.Services.AddServerSideBlazor();
  builder.Services.AddSingleton<WeatherForecastService>();

  var app = builder.Build();

  #region Default Logging Stuff
  //var logger = app.Services.GetRequiredService<ILogger<Program>>();
  //logger.LogInformation("App started {datetime} UTC", DateTime.UtcNow);
  #endregion

  // Configure the HTTP request pipeline.
  if (!app.Environment.IsDevelopment())
  {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
  }

  app.UseHttpsRedirection();

  app.UseStaticFiles();

  app.UseSerilogRequestLogging();

  app.UseRouting();

  app.MapBlazorHub();
  app.MapFallbackToPage("/_Host");

  app.Run();
}
catch (Exception ex)
{
  Log.Fatal(ex, "The app failed to start");
}
finally
{
  Log.CloseAndFlush();
}

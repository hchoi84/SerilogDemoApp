//https://www.youtube.com/watch?v=oXNslgIXIbQ&ab_channel=IAmTimCorey
using SerilogDemo.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogging((context, logging) =>
{
  logging.ClearProviders();
  logging.AddConfiguration(context.Configuration.GetSection("Logging"));
  //logging.AddDebug();
  logging.AddConsole();
  //EventSource, EventLog, TraceSource, AzureAppServicesFile, AzureAppServicesBlob, ApplicationInsights
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("App started {datetime} UTC", DateTime.UtcNow);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

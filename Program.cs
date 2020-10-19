using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;

namespace ArqNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
        string DD_API_KEY = Environment.GetEnvironmentVariable("DD_API_KEY");
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            //.WriteTo.Console() // Output Console
            //.WriteTo.File(new RenderedCompactJsonFormatter(), "../logs/log-medical-supplies.ndjson") // Output to log file with json structure
            //.WriteTo.Seq("http://localhost:5341") // Output to Seq
            .WriteTo.DatadogLogs(DD_API_KEY)
            .CreateLogger();
        try
        {
            Log.Information("Starting up");
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application start-up failed");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

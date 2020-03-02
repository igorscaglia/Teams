using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Teams.API
{
    public class Program
    {
        // TODO Instalar swagger, configurar https
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

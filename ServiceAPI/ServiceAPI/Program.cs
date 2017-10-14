using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;
using ServiceAPI.Dal;
namespace ServiceAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new StudentsDbContext()) {
                context.Database.EnsureCreated();
            }

                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseStartup<Startup>()
                    //.UseUrls("http://localhost:6789") serve a modificare la porta di ascolto del web service che di default è 5000
                    .Build();
            host.Run();
            /*Task restService = host.RunAsync();

            //System.Diagnostics.Process.Start("chrome.exe", "http://localhost/netcoreapp2.0/corsoing/");
            System.Diagnostics.Process.Start("cmd", "/C start http://localhost/netcoreapp2.0/corsoing/");
            restService.Wait();*/
        }
    }
}

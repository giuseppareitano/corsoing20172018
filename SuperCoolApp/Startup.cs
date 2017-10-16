using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SuperCoolApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles(); //permette di servire file statici

            app.UseMvc(routes => // tutta questa e una lambda expression: presento una variabile e dico cosa bisogna fare con tale variabile
            {
                routes.MapRoute( //sta istanziando il controller principale della nostra applicazione,
                                 //che e' un oggetto che mi ritorna sempre e solo una pagina
                                 //in quanto si tratta di una single page application
                                 //quindi tutta la parte logica viene gestita da Angular
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute( //route che permette di fare il pass to
                    name: "api",
                    template: "api/{*url}");

                routes.MapSpaFallbackRoute( //tutte le route che io chiedo che non impattano quella principale devono essere gestite da Angular
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}

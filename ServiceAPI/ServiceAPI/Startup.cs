using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceAPI
{
    public class Startup
    {
        //la classe startup deve avere obbligatoriamente un metodo, ovvero il Configure
        //questo viene chiamato dal web host builder per configurare la parte di gestione delle richieste
        //in questo caso noi utilizziamo quella di default AspNet Mvc

            /*
             * Ogni volta che chiamiamo un metodo che inizia con Use noi aggiungiamo un middleware,
             * ovvero un componente che si interpone tra l'host e chi sta intercettando le chiamate
             */
        public void ConfigureServices(IServiceCollection services)
        {
            /*
             questo metodo viene chiamato dopo il metodo Configure e serve per aggiungere alcune funzionalità
             all'hosting della nostra api.
             In particolare serve ad aggiungere servizi, come per esempio il servizio di accesso ai dati (data layer),
             che poi vengono risolti tramite dependency injection.
             */
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvcWithDefaultRoute();

            /*Quando viene chiamato il metodo Configure, il motore di ASPNET MVC va a leggere tramite reflection
             quali sono le classi nella mia applicazione che implementano altrove.
             Con questo meccanismo le classi che implementano Controller vengono istanziate in automatico senza che 
             lo sviluppatore faccia nulla. 
             */

            //reflection: capacita d un programma di venire a conoscenza di come esso stesso e' costruito
            //posso conoscere per esempio le classi definite in un'applicazione a run time

        }
    }
}
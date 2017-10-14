using Microsoft.AspNetCore.Mvc;
using ServiceAPI.Dal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
//utilizziamo questo context per accedere al database.
//in realta per accedere al database nelle applicazioni vere non si utilizza un'istanza del livello di accesso
//ai dati ma si utilizza la dependency injection, si utilizza l'interfaccia per accedere al livello di accesso ai dati
//e' sbagliato creare un'istanza del livello di accesso ai dati
//questo in genere ci viene passato dall'esterno


namespace ServiceAPI
{
    [Route("api")] //attributo: annotazione: modo per aggiungere metadati a una classe
    //Il prefisso "api" e' il prefisso utilizzato nella uri, per l'accesso alle risorse
    public class ServiceApiController : Controller 

        /*Controller è una classe base che implementa alcune funzionalita che poi vengono utilizzate da Aspnet Mvc
         per il routing delle nostre risorse
         */
    {
        static readonly object setupLock = new object();
        static readonly SemaphoreSlim parallelism = new SemaphoreSlim(2);

        //notiamo che questa classe espone dei metodi tutti sulla stessa risorsa ovvero students
        //quello che cambia e' che cosa andiamo a fare su questa risorsa

        [HttpGet("setup")]
        public IActionResult SetupDatabase()
        {
            lock (setupLock)
            {
                using (var context = new StudentsDbContext()) //in questo modo utilizziamo la dependency injection
                    //dal'esterno ci viene passata un'istanza del data access layer
                {
                    // Create database
                    context.Database.EnsureCreated();
                }
                return Ok("database created"); //genera una risposta http
            }
        }


        [HttpGet("students")] //ritorna al client la lista degli studenti
        public async Task<IActionResult> GetStudents()
        {
            try
            {
                await parallelism.WaitAsync();

                using (var context = new StudentsDbContext())
                {
                    return Ok(await context.Students.ToListAsync());

                    //Ok e' un metodo della classe base Controller che genera una response http
                    //con status 200 e con body la lista studenti che gli sto passando
                    //ci sono altri metodi in grado di generare risposte http con status diversi
                }
            }
            finally
            {
                parallelism.Release();
            }
        }

        [HttpGet("student")] //get student per id
        public async Task<IActionResult> GetStudent([FromQuery]int id) // ?id= nell'url
        {
            using (var context = new StudentsDbContext())
            {
                //return Ok(await context.Students.FirstOrDefaultAsync(x => x.Id == id));
                //qui potremmo implementare un altro tipo di response nel caso in cui non troviamo lo studente
                var student = await context.Students.FirstOrDefaultAsync(x => x.Id == id);
                if (student == null)
                    return NotFound();
                return Ok(student);
                //nel body della response http verra restituita un'istanza di student in formato JSON
            }
        }

        [HttpPut("students")] //creazione di un nuovo studente
        public async Task<IActionResult> CreateStudent([FromBody]Student student)
            //attributo FromBody indica che la rappresentazione dell'istanza studente deve essere
            //specificata nel body della richiesta http, mediante rappresentazione JSON
        {
            using (var context = new StudentsDbContext())
            {
                context.Students.Add(student);

                await context.SaveChangesAsync();

                return Ok(); //response http 200: tutto apposto!
            }
        }

        [HttpPost("students")] //Update di un'istanza di student
        public async Task<IActionResult> UpdateStudent([FromBody]Student student)
        {
            using (var context = new StudentsDbContext())
            {
                context.Students.Update(student);
                await context.SaveChangesAsync();
                return Ok();
            }
        }


        [HttpDelete("students")]
        public async Task<IActionResult> DeleteStudent([FromQuery]int id)
        {
            //Attributo FromQuery: il parametro non deve essere all'interno del body della richiesta http
            //ma direttamente nella uri ?=
            using (var context = new StudentsDbContext())
            {
                var student = await context.Students.FirstOrDefaultAsync(x => x.Id == id);
                context.Students.Remove(student);
                await context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}

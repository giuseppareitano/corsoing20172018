using Microsoft.AspNetCore.Mvc;
using ServiceAPI.Dal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace ServiceAPI
{
    [Route("api")]
    public class ServiceApiController : Controller
    {
        static readonly object setupLock = new object();
        static readonly SemaphoreSlim parallelism = new SemaphoreSlim(2);

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


        [HttpGet("students")]
        public async Task<IActionResult> GetStudents()
        {
            try
            {
                await parallelism.WaitAsync();

                using (var context = new StudentsDbContext())
                {
                    return Ok(await context.Students.ToListAsync());
                }
            }
            finally
            {
                parallelism.Release();
            }
        }

        [HttpGet("student")]
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

        [HttpPut("students")]
        public async Task<IActionResult> CreateStudent([FromBody]Student student)
        {
            using (var context = new StudentsDbContext())
            {
                context.Students.Add(student);

                await context.SaveChangesAsync();

                return Ok(); //response http 200: tutto apposto!
            }
        }

        [HttpPost("students")]
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

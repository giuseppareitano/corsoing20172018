using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceAPI.Dal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceAPI
{
    [Route("api")]
    public class CourseController: Controller
    {
        static readonly object setupLock = new object();
        static readonly SemaphoreSlim parallelism = new SemaphoreSlim(2);

        [HttpGet("setup")]
        public IActionResult SetupDatabase()
        {
            lock (setupLock)
            {
                using (var context = new StudentsDbContext()) 
                {
                    // Create database
                    context.Database.EnsureCreated();
                }
                return Ok("database created");
            }
        }

        [HttpGet("courses")]
        public async Task<IActionResult> GetCourses()
        {
            try
            {
                await parallelism.WaitAsync();

                using (var context = new StudentsDbContext())
                {
                    return Ok(await context.Courses.ToListAsync());
                }
            }
            finally
            {
                parallelism.Release();
            }
        }

        [HttpGet("course")]
        public async Task<IActionResult> GetCourse([FromQuery]int id)
        {
            using (var context = new StudentsDbContext())
            {
                var course = await context.Courses.FirstOrDefaultAsync(x => x.Id == id);
                if (course == null)
                    return NotFound();
                return Ok(course);
            }
        }

        [HttpPut("courses")]
        public async Task<IActionResult> CreateCourse([FromBody]Course course)
        {
            using (var context = new StudentsDbContext())
            {
                context.Courses.Add(course);
                await context.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpPost("courses")]
        public async Task<IActionResult> UpdateCourse([FromBody]Course course)
        {
            using (var context = new StudentsDbContext())
            {
                context.Courses.Update(course);
                await context.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpDelete("courses")]
        public async Task<IActionResult> DeleteCourse([FromQuery]int id)
        {
            using (var context = new StudentsDbContext())
            {
                var course = await context.Courses.FirstOrDefaultAsync(x => x.Id == id);
                context.Courses.Remove(course);
                await context.SaveChangesAsync();
                return Ok();
            }
        }

    }
}

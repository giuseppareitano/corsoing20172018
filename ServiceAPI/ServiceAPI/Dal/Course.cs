using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceAPI.Dal
{
    public class Course
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int Hours { get; set; } //numero di ore del corso
    }
}

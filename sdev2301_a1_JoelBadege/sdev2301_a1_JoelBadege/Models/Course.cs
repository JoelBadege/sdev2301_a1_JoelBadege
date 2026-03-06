using System;
using System.Collections.Generic;
using System.Text;

namespace sdev2301_a1_JoelBadege.Models
{
    public class Course
    {
        public int Id { get; private set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public int Credits { get; set; }

        public List<Enrollment> Enrollments { get; set; } = new();
    }
}

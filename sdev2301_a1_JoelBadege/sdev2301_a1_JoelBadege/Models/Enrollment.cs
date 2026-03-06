using System;
using System.Collections.Generic;
using System.Text;

namespace sdev2301_a1_JoelBadege.Models
{
    public class Enrollment
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}

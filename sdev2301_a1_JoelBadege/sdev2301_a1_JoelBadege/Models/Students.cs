using System;
using System.Collections.Generic;
using System.Text;

namespace sdev2301_a1_JoelBadege.Models
{
    public class Students
    {
       public int Id { get; private set; }
        public string FullName { get; set; } = "";
        public DateTime EnrollmentDate { get; set; }

        public List<Enrollment> Enrollments { get; set; } = new();
    }
}

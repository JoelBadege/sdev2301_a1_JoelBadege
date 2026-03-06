using Microsoft.EntityFrameworkCore;
using sdev2301_a1_JoelBadege.Data;
using sdev2301_a1_JoelBadege.Models;


namespace sdev2301_a1_JoelBadege.Services
{
    public class EnrollmentService
    {
        private readonly AppDbContext _context;

        public EnrollmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task EnrollAsync(int studentId, int courseId)
        {
            bool studentExists = await _context.Students.AnyAsync(s => s.Id == studentId);
            if (!studentExists)
                throw new KeyNotFoundException("Student not found.");

            bool courseExists = await _context.Courses.AnyAsync(c => c.Id == courseId);
            if (!courseExists)
                throw new KeyNotFoundException("Course not found.");

            bool alreadyEnrolled = await _context.Enrollments
                .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (alreadyEnrolled)
                throw new ArgumentException("This student is already enrolled in that course.");

            _context.Enrollments.Add(new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId
            });

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DropAsync(int studentId, int courseId)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (enrollment == null)
                return false;

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Student>> GetStudentsInCourseAsync(int courseId)
        {
            return await _context.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.Student)
                .OrderBy(s => s.FullName)
                .ToListAsync();
        }

        public async Task<List<(string Code, string Name, int Count)>> GetCourseEnrollmentCountsAsync()
        {
            var data = await _context.Courses
                .OrderBy(c => c.Code)
                .Select(c => new
                {
                    c.Code,
                    c.Name,
                    Count = c.Enrollments.Count
                })
                .ToListAsync();

            return data
                .Select(x => (x.Code, x.Name, x.Count))
                .ToList();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using sdev2301_a1_JoelBadege.Models;
using sdev2301_a1_JoelBadege.Data;


namespace sdev2301_a1_JoelBadege.Services
{
    public class CourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Course course)
        {
            ArgumentNullException.ThrowIfNull(course);

            if (string.IsNullOrWhiteSpace(course.Code))
                throw new ArgumentNullException(nameof(course.Code), "Course code is required.");

            if (string.IsNullOrWhiteSpace(course.Name))
                throw new ArgumentNullException(nameof(course.Name), "Course name is required.");

            if (course.Credits <= 0)
                throw new ArgumentException("Credits must be greater than 0.", nameof(course.Credits));

            course.Code = course.Code.Trim().ToUpper();
            course.Name = course.Name.Trim();

            bool exists = await _context.Courses.AnyAsync(c => c.Code == course.Code);
            if (exists)
                throw new ArgumentException($"Course code '{course.Code}' already exists.");

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return course.Id;
        }

        public async Task<List<Course>> GetAllAsync()
        {
            return await _context.Courses
                .OrderBy(c => c.Code)
                .ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
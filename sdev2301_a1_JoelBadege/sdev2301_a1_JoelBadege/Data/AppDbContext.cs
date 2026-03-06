using Microsoft.EntityFrameworkCore;
using sdev2301_a1_JoelBadege.Models;
using sdev2301_a1_JoelBadege.Data;


namespace sdev2301_a1_JoelBadege.Services
{
    public class StudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Student student)
        {
            ArgumentNullException.ThrowIfNull(student);

            if (string.IsNullOrWhiteSpace(student.FullName))
                throw new ArgumentNullException(nameof(student.FullName), "Full name is required.");

            student.FullName = student.FullName.Trim();

            if (student.EnrollmentDate == default)
                student.EnrollmentDate = DateTime.Today;

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return student.Id;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students
                .OrderBy(s => s.FullName)
                .ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
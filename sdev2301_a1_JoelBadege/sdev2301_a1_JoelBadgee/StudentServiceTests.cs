using sdev2301_a1_JoelBadege.Data;
using sdev2301_a1_JoelBadege.Models;
using sdev2301_a1_JoelBadege.Services;


namespace sdev2301_a1_JoelBadege.Tests
{
    public class StudentServiceTests
    {
        private static async Task<int> SeedStudentAsync(AppDbContext db, string fullName, DateTime enrollmentDate)
        {
            var student = new Student
            {
                FullName = fullName,
                EnrollmentDate = enrollmentDate
            };

            db.Students.Add(student);
            await db.SaveChangesAsync();
            return student.Id;
        }

        [Fact]
        public async Task AddAsync_GivenValidStudent_PersistsAndReturnsStudent()
        {
            using var conn = DbTestHelper.CreateOpenInMemoryConnection();
            var options = DbTestHelper.CreateOptions(conn);

            int id;
            using (var db1 = DbTestHelper.CreateContext(options, ensureCreated: true))
            {
                var service = new StudentService(db1);

                var student = new Student
                {
                    FullName = "Joel Badgee",
                    EnrollmentDate = new DateTime(2026, 3, 20)
                };

                id = await service.AddAsync(student);

                Assert.True(id > 0);
            }

            using (var db2 = DbTestHelper.CreateContext(options))
            {
                var saved = await db2.Students.FindAsync(id);

                Assert.NotNull(saved);
                Assert.Equal("Joel Badgee", saved!.FullName);
                Assert.Equal(new DateTime(2026, 3, 20), saved.EnrollmentDate);
            }
        }

        [Fact]
        public async Task GetAllAsync_WhenStudentsExist_ReturnsOrderedByFullNameAscending()
        {
            using var conn = DbTestHelper.CreateOpenInMemoryConnection();
            var options = DbTestHelper.CreateOptions(conn);

            using (var db1 = DbTestHelper.CreateContext(options, ensureCreated: true))
            {
                await SeedStudentAsync(db1, "Zara Khan", new DateTime(2026, 1, 10));
                await SeedStudentAsync(db1, "Adam Smith", new DateTime(2026, 1, 11));
                await SeedStudentAsync(db1, "Mary Jones", new DateTime(2026, 1, 12));
            }

            using (var db2 = DbTestHelper.CreateContext(options))
            {
                var service = new StudentService(db2);

                var students = await service.GetAllAsync();

                Assert.Equal(3, students.Count);
                Assert.Equal(new[] { "Adam Smith", "Mary Jones", "Zara Khan" },
                    students.Select(s => s.FullName).ToArray());
            }
        }
    }
}
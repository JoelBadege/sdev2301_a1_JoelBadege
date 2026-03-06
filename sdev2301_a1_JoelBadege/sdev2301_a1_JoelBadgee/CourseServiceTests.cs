using sdev2301_a1_JoelBadege.Models;
using sdev2301_a1_JoelBadege.Services;


namespace sdev2301_a1_JoelBadege.Tests
{
    public class CourseServiceTests
    {
        [Fact]
        public async Task AddAsync_GivenValidCourse_PersistsAndReturnsCourse()
        {
            using var conn = DbTestHelper.CreateOpenInMemoryConnection();
            var options = DbTestHelper.CreateOptions(conn);

            int id;
            using (var db1 = DbTestHelper.CreateContext(options, ensureCreated: true))
            {
                var service = new CourseService(db1);

                var course = new Course
                {
                    Code = "SDEV2301",
                    Name = "Enterprise Application Development",
                    Credits = 3
                };

                id = await service.AddAsync(course);

                Assert.True(id > 0);
            }

            using (var db2 = DbTestHelper.CreateContext(options))
            {
                var saved = await db2.Courses.FindAsync(id);

                Assert.NotNull(saved);
                Assert.Equal("SDEV2301", saved!.Code);
                Assert.Equal("Enterprise Application Development", saved.Name);
                Assert.Equal(3, saved.Credits);
            }
        }
    }
}

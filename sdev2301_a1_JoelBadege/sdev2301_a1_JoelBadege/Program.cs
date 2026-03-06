using Microsoft.EntityFrameworkCore;
using sdev2301_a1_JoelBadege.Data;
using sdev2301_a1_JoelBadege.Models;
using sdev2301_a1_JoelBadege.Services;


var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite("Data Source=student_enrollment.db")
    .Options;

using var context = new AppDbContext(options);


context.Database.EnsureCreated();

var studentService = new StudentService(context);
var courseService = new CourseService(context);
var enrollmentService = new EnrollmentService(context);

bool exit = false;

while (!exit)
{
    Console.WriteLine("\n=== Student Enrollment System ===");
    Console.WriteLine("1. Add Student");
    Console.WriteLine("2. List Students");
    Console.WriteLine("3. Add Course");
    Console.WriteLine("4. List Courses");
    Console.WriteLine("5. Enroll Student in Course");
    Console.WriteLine("6. Drop Student from Course");
    Console.WriteLine("7. Show Students in a Course");
    Console.WriteLine("8. Show Course Enrollment Counts");
    Console.WriteLine("0. Exit");
    Console.Write("Choose an option: ");

    var choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                await AddStudentAsync();
                break;

            case "2":
                await ListStudentsAsync();
                break;

            case "3":
                await AddCourseAsync();
                break;

            case "4":
                await ListCoursesAsync();
                break;

            case "5":
                await EnrollStudentAsync();
                break;

            case "6":
                await DropStudentAsync();
                break;

            case "7":
                await ShowStudentsInCourseAsync();
                break;

            case "8":
                await ShowEnrollmentCountsAsync();
                break;

            case "0":
                exit = true;
                break;

            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

async Task AddStudentAsync()
{
    Console.Write("Enter full name: ");
    string? name = Console.ReadLine();

    var student = new Student
    {
        FullName = name ?? "",
        EnrollmentDate = DateTime.Today
    };

    int id = await studentService.AddAsync(student);
    Console.WriteLine($"Student added with Id {id}.");
}

async Task ListStudentsAsync()
{
    var students = await studentService.GetAllAsync();

    if (!students.Any())
    {
        Console.WriteLine("No students found.");
        return;
    }

    Console.WriteLine("\nStudents:");
    foreach (var student in students)
    {
        Console.WriteLine($"{student.Id} - {student.FullName} - {student.EnrollmentDate:d}");
    }
}

async Task AddCourseAsync()
{
    Console.Write("Enter course code: ");
    string? code = Console.ReadLine();

    Console.Write("Enter course name: ");
    string? name = Console.ReadLine();

    Console.Write("Enter credits: ");
    bool validCredits = int.TryParse(Console.ReadLine(), out int credits);

    if (!validCredits)
    {
        Console.WriteLine("Credits must be a whole number.");
        return;
    }

    var course = new Course
    {
        Code = code ?? "",
        Name = name ?? "",
        Credits = credits
    };

    int id = await courseService.AddAsync(course);
    Console.WriteLine($"Course added with Id {id}.");
}

async Task ListCoursesAsync()
{
    var courses = await courseService.GetAllAsync();

    if (!courses.Any())
    {
        Console.WriteLine("No courses found.");
        return;
    }

    Console.WriteLine("\nCourses:");
    foreach (var course in courses)
    {
        Console.WriteLine($"{course.Id} - {course.Code} - {course.Name} - Credits: {course.Credits}");
    }
}

async Task EnrollStudentAsync()
{
    await ListStudentsAsync();
    await ListCoursesAsync();

    Console.Write("Enter student Id: ");
    if (!int.TryParse(Console.ReadLine(), out int studentId))
    {
        Console.WriteLine("Invalid student Id.");
        return;
    }

    Console.Write("Enter course Id: ");
    if (!int.TryParse(Console.ReadLine(), out int courseId))
    {
        Console.WriteLine("Invalid course Id.");
        return;
    }

    await enrollmentService.EnrollAsync(studentId, courseId);
    Console.WriteLine("Enrollment saved.");
}

async Task DropStudentAsync()
{
    await ListStudentsAsync();
    await ListCoursesAsync();

    Console.Write("Enter student Id: ");
    if (!int.TryParse(Console.ReadLine(), out int studentId))
    {
        Console.WriteLine("Invalid student Id.");
        return;
    }

    Console.Write("Enter course Id: ");
    if (!int.TryParse(Console.ReadLine(), out int courseId))
    {
        Console.WriteLine("Invalid course Id.");
        return;
    }

    bool dropped = await enrollmentService.DropAsync(studentId, courseId);

    if (dropped)
        Console.WriteLine("Enrollment removed.");
    else
        Console.WriteLine("That enrollment was not found.");
}

async Task ShowStudentsInCourseAsync()
{
    await ListCoursesAsync();

    Console.Write("Enter course Id: ");
    if (!int.TryParse(Console.ReadLine(), out int courseId))
    {
        Console.WriteLine("Invalid course Id.");
        return;
    }

    var students = await enrollmentService.GetStudentsInCourseAsync(courseId);

    if (!students.Any())
    {
        Console.WriteLine("No students enrolled in that course.");
        return;
    }

    Console.WriteLine("\nStudents in course:");
    foreach (var student in students)
    {
        Console.WriteLine($"{student.Id} - {student.FullName}");
    }
}

async Task ShowEnrollmentCountsAsync()
{
    var counts = await enrollmentService.GetCourseEnrollmentCountsAsync();

    if (!counts.Any())
    {
        Console.WriteLine("No courses found.");
        return;
    }

    Console.WriteLine("\nCourse enrollment counts:");
    foreach (var item in counts)
    {
        Console.WriteLine($"{item.Code} - {item.Name}: {item.Count}");
    }
}
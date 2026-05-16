using System;
using System.Collections.Generic;
using System.Linq;

// ═══════════════════════════════════════════════════════════════
//  CONCEPT 1 — OOP : Class, Properties, Encapsulation
// ═══════════════════════════════════════════════════════════════
class Student
{
    public int      Id         { get; set; }
    public string   Name       { get; set; }
    public string   Course     { get; set; }
    public double   CGPA       { get; set; }
    public DateTime EnrolledOn { get; set; }   // CONCEPT 6 — Date & Time

    // CONCEPT 1 — OOP : Constructor
    public Student(int id, string name, string course, double cgpa)
    {
        Id         = id;
        Name       = name.Trim().ToUpper();    // CONCEPT 4 — String Handling
        Course     = course.Trim();
        CGPA       = cgpa;
        EnrolledOn = DateTime.Now;             // CONCEPT 6 — Date & Time
    }

    public override string ToString() =>
        $"[{Id}] {Name,-20} | {Course,-10} | CGPA: {CGPA:F2} | Enrolled: {EnrolledOn:dd-MM-yyyy}";
}

// ═══════════════════════════════════════════════════════════════
//  CONCEPT 1 — OOP : Abstract Base Class (Abstraction)
// ═══════════════════════════════════════════════════════════════
abstract class BaseRepository<T>
{
    protected List<T> _records = new List<T>(); // CONCEPT 5 — Collections (Generic List)
    public abstract void Add(T item);
    public abstract void Delete(int id);
    public abstract T Find(int id);
    public List<T> GetAll() => _records;
}

// ═══════════════════════════════════════════════════════════════
//  CONCEPT 1 — OOP : Inheritance + CRUD
// ═══════════════════════════════════════════════════════════════
class StudentRepository : BaseRepository<Student>
{
    private int _nextId = 1;

    // CREATE
    public override void Add(Student student)
    {
        student.Id = _nextId++;
        _records.Add(student);
        Console.WriteLine("\n Student added successfully.");
    }

    // READ — Find by ID
    public override Student Find(int id) =>
        _records.Find(s => s.Id == id);

    // READ — Search by Name                     // CONCEPT 4 — String Handling
    public List<Student> SearchByName(string keyword)
    {
        keyword = keyword.Trim().ToUpper();
        return _records.FindAll(s => s.Name.Contains(keyword));
    }

    // UPDATE
    public void Update(int id, string name, string course, double cgpa)
    {
        Student s = Find(id);
        if (s == null) { Console.WriteLine("Student not found."); return; }
        s.Name   = name.Trim().ToUpper();
        s.Course = course.Trim();
        s.CGPA   = cgpa;
        Console.WriteLine("\n Record updated successfully.");
    }

    // DELETE
    public override void Delete(int id)
    {
        Student s = Find(id);
        if (s == null) { Console.WriteLine("Student not found."); return; }
        _records.Remove(s);
        Console.WriteLine("\n Record deleted successfully.");
    }

    // SORT by CGPA Descending                   // CONCEPT 7 — Sorting
    public List<Student> SortByCGPA() =>
        _records.OrderByDescending(s => s.CGPA).ToList();

    // SORT by Name Alphabetical                 // CONCEPT 7 — Sorting
    public List<Student> SortByName() =>
        _records.OrderBy(s => s.Name).ToList();

    // SEARCH using Array                        // CONCEPT 8 — Searching + CONCEPT 5 Arrays
    public Student SearchById(int id)
    {
        Student[] arr = _records.ToArray();      // CONCEPT 5 — Arrays
        foreach (Student s in arr)               // CONCEPT 3 — Control Flow (foreach)
            if (s.Id == id) return s;
        return null;
    }
}

// ═══════════════════════════════════════════════════════════════
//  CONCEPT 1 — OOP : Interface (Polymorphism)
// ═══════════════════════════════════════════════════════════════
interface IPrintable { void Print(); }

class ReportPrinter : IPrintable
{
    private readonly List<Student> _students;
    public ReportPrinter(List<Student> students) => _students = students;

    public void Print()
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine($"  {"ID",-5} {"NAME",-20} {"COURSE",-12} {"CGPA",-8} ENROLLED");
        Console.WriteLine(new string('-', 70));
        foreach (var s in _students)             // CONCEPT 3 — Control Flow (foreach)
            Console.WriteLine($"  {s.Id,-5} {s.Name,-20} {s.Course,-12} {s.CGPA,-8:F2} {s.EnrolledOn:dd-MM-yyyy}");
        Console.WriteLine(new string('=', 70));
    }
}

// ═══════════════════════════════════════════════════════════════
//  MAIN PROGRAM
// ═══════════════════════════════════════════════════════════════
class Program
{
    static readonly StudentRepository repo = new StudentRepository();

    static void Main()
    {
        // CONCEPT 5 — Arrays : Seed data
        string[] seedNames   = { "Pawan Kumar", "Ravi Sharma", "Anita Das", "Suresh Nair" };
        string[] seedCourses = { "CSE", "ECE", "IT", "CSE" };
        double[] seedCGPAs   = { 8.10, 7.50, 9.00, 6.80 };

        for (int i = 0; i < seedNames.Length; i++)  // CONCEPT 3 — Control Flow (for)
            repo.Add(new Student(0, seedNames[i], seedCourses[i], seedCGPAs[i]));

        Console.WriteLine("\n Student Record Manager v1.0");

        bool running = true;
        while (running)                              // CONCEPT 3 — Control Flow (while)
        {
            Console.WriteLine("\n--- MENU ---");
            Console.WriteLine(" 1. Add Student");
            Console.WriteLine(" 2. View All");
            Console.WriteLine(" 3. Search by ID");
            Console.WriteLine(" 4. Search by Name");
            Console.WriteLine(" 5. Update Student");
            Console.WriteLine(" 6. Delete Student");
            Console.WriteLine(" 7. Sort by CGPA");
            Console.WriteLine(" 8. Sort by Name");
            Console.WriteLine(" 9. Exit");
            Console.Write("Choose: ");

            switch (Console.ReadLine())              // CONCEPT 3 — Control Flow (switch)
            {
                case "1": AddStudent();    break;
                case "2": ViewAll();       break;
                case "3": SearchById();    break;
                case "4": SearchByName();  break;
                case "5": UpdateStudent(); break;
                case "6": DeleteStudent(); break;
                case "7": SortByCGPA();    break;
                case "8": SortByName();    break;
                case "9": running = false; break;
                default: Console.WriteLine("Invalid option."); break;
            }
        }
        Console.WriteLine("Goodbye!");
    }

    static void AddStudent()
    {
        try                                          // CONCEPT 2 — Exception Handling
        {
            Console.Write("Name   : "); string name   = Console.ReadLine();
            Console.Write("Course : "); string course = Console.ReadLine();
            Console.Write("CGPA   : "); double cgpa   = double.Parse(Console.ReadLine());

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(course))
                throw new ArgumentException("Name and Course cannot be empty.");

            if (cgpa < 0 || cgpa > 10)              // CONCEPT 3 — Control Flow (if-else)
                throw new ArgumentOutOfRangeException("CGPA must be between 0 and 10.");

            repo.Add(new Student(0, name, course, cgpa));
        }
        catch (FormatException)                      // CONCEPT 2 — Multiple catch blocks
        { Console.WriteLine(" Invalid input. CGPA must be a number."); }
        catch (ArgumentOutOfRangeException ex)
        { Console.WriteLine($" {ex.Message}"); }
        catch (ArgumentException ex)
        { Console.WriteLine($" {ex.ParamName}"); }
    }

    static void ViewAll()
    {
        List<Student> all = repo.GetAll();
        if (all.Count == 0) { Console.WriteLine("No records found."); return; }
        new ReportPrinter(all).Print();              // CONCEPT 1 — Polymorphism via Interface
        Console.WriteLine($"  Total: {all.Count} students");
    }

    static void SearchById()
    {
        try
        {
            Console.Write("Enter ID: ");
            int id = int.Parse(Console.ReadLine()); // CONCEPT 2 — Exception Handling
            Student s = repo.SearchById(id);
            Console.WriteLine(s != null ? "\n" + s : "Not found.");
        }
        catch (FormatException) { Console.WriteLine(" ID must be a number."); }
    }

    static void SearchByName()
    {
        Console.Write("Enter name keyword: ");
        string kw = Console.ReadLine();             // CONCEPT 4 — String Handling
        List<Student> results = repo.SearchByName(kw);
        if (results.Count == 0) { Console.WriteLine("No match found."); return; }
        new ReportPrinter(results).Print();
    }

    static void UpdateStudent()
    {
        try
        {
            Console.Write("ID to update : "); int id     = int.Parse(Console.ReadLine());
            Console.Write("New Name     : "); string name   = Console.ReadLine();
            Console.Write("New Course   : "); string course = Console.ReadLine();
            Console.Write("New CGPA     : "); double cgpa   = double.Parse(Console.ReadLine());

            if (cgpa < 0 || cgpa > 10)
                throw new ArgumentOutOfRangeException("CGPA must be between 0 and 10.");

            repo.Update(id, name, course, cgpa);
        }
        catch (FormatException)        { Console.WriteLine(" Invalid input format."); }
        catch (ArgumentOutOfRangeException ex) { Console.WriteLine($" {ex.ParamName}"); }
    }

    static void DeleteStudent()
    {
        try
        {
            Console.Write("ID to delete: ");
            repo.Delete(int.Parse(Console.ReadLine()));
        }
        catch (FormatException) { Console.WriteLine(" ID must be a number."); }
    }

    static void SortByCGPA()
    {
        Console.WriteLine("\n  Sorted by CGPA (High to Low):");
        new ReportPrinter(repo.SortByCGPA()).Print(); // CONCEPT 7 — Sorting
    }

    static void SortByName()
    {
        Console.WriteLine("\n  Sorted by Name (A to Z):");
        new ReportPrinter(repo.SortByName()).Print(); // CONCEPT 7 — Sorting
    }
}

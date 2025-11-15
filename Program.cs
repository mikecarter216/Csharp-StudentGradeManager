using System;
using System.Collections.Generic;
using System.IO;

// Struct to hold course-level information
struct CourseInfo
{
    public string CourseName;
    public int MaxScore;
    public CourseInfo(string courseName, int maxScore)
    {
        CourseName = courseName;
        MaxScore = maxScore;
    }
}

// Abstract base class for a person
abstract class Person
{
    public string Name { get; set; }
    public Person(string name)
    {
        Name = name;
    }

    // Abstract method to display role
    public abstract void PrintRole();
}

// Student class inherits from Person
class Student : Person
{
    public double Score { get; set; }

    public Student(string name, double score) : base(name)
    {
        Score = score;
    }

    // Implement abstract method
    public override void PrintRole()
    {
        Console.WriteLine($"{Name} is a student.");
    }

    public string GetGrade()
    {
        if (Score >= 90) return "A";
        if (Score >= 80) return "B";
        if (Score >= 70) return "C";
        if (Score >= 60) return "D";
        return "F";
    }

    public override string ToString() => $"{Name} - {Score} ({GetGrade()})";
}

class Program
{
    static List<Student> students = new List<Student>();
    static string filePath = "students.txt";
    static CourseInfo course = new CourseInfo("Programming Fundamentals", 100);

    static void Main()
    {
        LoadDataFromFile();
        int choice = 0;
        do
        {
            ShowMenu();
            Console.Write("Enter choice: ");
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Enter a number.");
                continue;
            }
            switch (choice)
            {
                case 1: AddStudent(); break;
                case 2: ListStudents(); break;
                case 3: SaveDataToFile(); break;
                case 4: LoadDataFromFile(); break;
                case 5: Console.WriteLine("Exiting program..."); break;
                default: Console.WriteLine("Invalid choice."); break;
            }
        } while (choice != 5);
    }

    static void ShowMenu()
    {
        Console.WriteLine("\n====== Student Grade Manager ======");
        Console.WriteLine($"Course: {course.CourseName}");
        Console.WriteLine("1. Add Student");
        Console.WriteLine("2. List Students");
        Console.WriteLine("3. Save to File");
        Console.WriteLine("4. Load from File");
        Console.WriteLine("5. Exit");
        Console.WriteLine("===================================");
    }

    static void AddStudent()
    {
        Console.Write("Enter student name: ");
        string name = Console.ReadLine();
        Console.Write($"Enter score (0-{course.MaxScore}): ");
        if (!double.TryParse(Console.ReadLine(), out double score) || score < 0 || score > course.MaxScore)
        {
            Console.WriteLine("Invalid score. Try again.");
            return;
        }
        var student = new Student(name, score);
        student.PrintRole(); // Demonstrates the abstract method
        students.Add(student);
        Console.WriteLine("Student added!");
    }

    static void ListStudents()
    {
        Console.WriteLine("\n----- Student List -----");
        if (students.Count == 0) { Console.WriteLine("No students found."); return; }
        students.ForEach(s => Console.WriteLine(s));
    }

    static void SaveDataToFile()
    {
        using StreamWriter writer = new StreamWriter(filePath);
        foreach (var s in students) writer.WriteLine($"{s.Name},{s.Score}");
        Console.WriteLine("Data saved!");
    }

    static void LoadDataFromFile()
    {
        if (!File.Exists(filePath)) { Console.WriteLine("No file found."); return; }
        students.Clear();
        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split(',');
            if (parts.Length == 2 && double.TryParse(parts[1], out double score))
                students.Add(new Student(parts[0], score));
        }
        Console.WriteLine("Data loaded!");
    }
}

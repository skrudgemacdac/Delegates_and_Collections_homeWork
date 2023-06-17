using System;
using System.Collections.Generic;
using System.IO;

class Student
{
    public string lastName;
    public string firstName;
    public string university;
    public string faculty;
    public int course;
    public string department;
    public int group;
    public string city;
    public int age;

    // Создаем конструктор
    public Student(string firstName, string lastName, string university, string faculty, string department, int course, int age, int group, string city)
    {
        this.lastName = lastName;
        this.firstName = firstName;
        this.university = university;
        this.faculty = faculty;
        this.department = department;
        this.course = course;
        this.age = age;
        this.group = group;
        this.city = city;
    }
}

class Program
{
    static int MyDelegate(Student st1, Student st2)
    {
        return String.Compare(st1.firstName, st2.firstName);
    }

    static void Main(string[] args)
    {
        int bakalavr = 0;
        int magistr = 0;
        List<Student> list = new List<Student>();

        DateTime dt = DateTime.Now;
        StreamReader sr = new StreamReader("students.csv");

        while (!sr.EndOfStream)
        {
            try
            {
                string[] s = sr.ReadLine().Split(';');
                list.Add(new Student(s[0], s[1], s[2], s[3], s[4], int.Parse(s[5]), int.Parse(s[6]), int.Parse(s[7]), s[8]));

                // Количество бакалавров и магистров
                if (int.Parse(s[5]) < 5)
                    bakalavr++;
                else
                    magistr++;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Ошибка! Нажмите ESC для прекращения выполнения программы.");

                if (Console.ReadKey().Key == ConsoleKey.Escape)
                    return;
            }
        }

        sr.Close();

        // а) Количество студентов, учащихся на 5 и 6 курсах
        int students5And6 = bakalavr + magistr;
        Console.WriteLine("Количество студентов на 5 и 6 курсах: " + students5And6);

        // б)  Студенты в возрасте от 18 до 20 лет на каждом курсе (частотный массив)
        int[] ageFrequency = new int[7]; 
        foreach (Student student in list)
        {
            if (student.age >= 18 && student.age <= 20)
            {
                ageFrequency[student.course]++;
            }
        }

        // в) Список по возрасту студента
        list.Sort((st1, st2) => st1.age.CompareTo(st2.age));

        // г) Список по курсу и возрасту студента
        list.Sort((st1, st2) =>
        {
            int courseComparison = st1.course.CompareTo(st2.course);
            if (courseComparison != 0)
            {
                return courseComparison;
            }
            else
            {
                return st1.age.CompareTo(st2.age);
            }
        });

        Console.WriteLine("Список студентов, отсортированный по курсу и возрасту:");
        foreach (var student in list)
        {
            Console.WriteLine(student.firstName + " " + student.lastName + " - Возраст: " + student.course + ", Курс: " + student.age);
        }

        Console.WriteLine(DateTime.Now - dt);
        Console.ReadKey();
    }
}
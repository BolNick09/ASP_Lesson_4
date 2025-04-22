using System;
using System.ComponentModel.DataAnnotations;

namespace ASP_Lesson_4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            //app.MapGet("/", () => "Hello World!");
            ////�������� 100 �� ������ https://localhost:7287/square/10
            //app.MapPost("/square/{num}", (int num) => num * num);

            //app.MappGet ("/", (HttpContext context))


            var people = new List<Person>();

            //Task4
            app.MapGet("/people", () => Results.Ok(people));

            //Task5
            app.MapGet("/people/{id}", (int id) =>
            {
                if (id >= 0 && id < people.Count)
                {
                    return Results.Ok(people[id]);
                }
                return Results.NotFound("������������ �� ������");
            });

            
            app.MapPost("/people", (Person person) =>
            {
                people.Add(person);
                return Results.Created($"/people/{people.Count - 1}", person);
            })
            .WithParameterValidation(); //Task3

            app.Run();
        }
        //Task1
        public class Person
        {
            //Task2
            [Required(ErrorMessage = "��� �����������")]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "��� ������ ���� �� 2 �� 50 ��������")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "������� �����������")]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "������� ������ ���� �� 2 �� 50 ��������")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "���� �������� �����������")]
            [DataType(DataType.Date)]
            [Range(typeof(DateTime), "1900-01-01", "2023-01-01", ErrorMessage = "���� �������� ������ ���� ����� 1900 � 2023 �����")]
            public DateTime BirthDate { get; set; }

            [StringLength(500, ErrorMessage = "�������������� ���������� �� ������ ��������� 500 ��������")]
            public string AdditionalInfo { get; set; }

            public void PrintToConsole()
            {
                Console.WriteLine(this.ToString());
            }

            public async Task PrintToFileAsync(string filePath)
            {
                await File.WriteAllTextAsync(filePath, this.ToString());
            }

            public override string ToString()
            {
                return $"���: {FirstName} {LastName}\n" +
                       $"���� ��������: {BirthDate.ToShortDateString()}\n" +
                       $"���. ����������: {AdditionalInfo}\n" +
                       $"�������: {CalculateAge()} ���";
            }

            private int CalculateAge()
            {
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Year;
                if (BirthDate.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
    }
}

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
            ////Получить 100 по адресу https://localhost:7287/square/10
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
                return Results.NotFound("Пользователь не найден");
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
            [Required(ErrorMessage = "Имя обязательно")]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 50 символов")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Фамилия обязательна")]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "Фамилия должна быть от 2 до 50 символов")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Дата рождения обязательна")]
            [DataType(DataType.Date)]
            [Range(typeof(DateTime), "1900-01-01", "2023-01-01", ErrorMessage = "Дата рождения должна быть между 1900 и 2023 годом")]
            public DateTime BirthDate { get; set; }

            [StringLength(500, ErrorMessage = "Дополнительная информация не должна превышать 500 символов")]
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
                return $"Имя: {FirstName} {LastName}\n" +
                       $"Дата рождения: {BirthDate.ToShortDateString()}\n" +
                       $"Доп. информация: {AdditionalInfo}\n" +
                       $"Возраст: {CalculateAge()} лет";
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

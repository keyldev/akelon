﻿namespace VacationTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Employee> employees = new List<Employee>()
            {
                new Employee()
                {
                    Name = "Иванов Иван",
                    VacationDays = new List<DateTime>(),

                },
                new Employee()
                {
                    Name = "Сидоров Сидор",
                    VacationDays = new List<DateTime>(),
                    RemainingVacationDays = 32 // у Сидра случились переработки
                },
                new Employee()
                {
                    Name = "Петров Петр",
                    VacationDays = new List<DateTime>(),

                }
            };

            var vacations = DistributeVacations(employees);
            foreach (var employee in vacations)
            {
                Console.WriteLine($"Сотрудник: {employee.Name} имееет такие дни отпуска: {string.Join(",", employee.VacationDays)}");
            }

        }
        static List<Employee> DistributeVacations(List<Employee> employees)
        {
            var vacations = new List<Employee>();



            return vacations;
        }

        static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }
    }
    class Employee
    {
        public string Name { get; set; }
        public List<DateTime> VacationDays { get; set; }
        public int RemainingVacationDays { get; set; } = 28;
    }
}
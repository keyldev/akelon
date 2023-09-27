namespace VacationTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Employee> employees = new List<Employee>();



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
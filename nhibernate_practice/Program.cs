using NH.Repo.Extensions;
using NH.Service.Extensions;
using NH.Service.Services.Abstract;

namespace nhibernate_practice
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add all services through the extension method
            builder.Services.LoadRepositoryLayerExtensions();
            builder.Services.AddNHibernateServices();
            var app = builder.Build();

            // Initialize database with sample data
            using (var scope = app.Services.CreateScope())
            {
                var dataInitService = scope.ServiceProvider.GetRequiredService<IDataInitializationService>();
                dataInitService.InitializeDatabase();

                // Query and display data using services
                var employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();
                var departmentService = scope.ServiceProvider.GetRequiredService<IDepartmentService>();

                // Display all employees
                Console.WriteLine("All Employees:");
                var allEmployees = employeeService.GetAllEmployees();
                foreach (var emp in allEmployees)
                {
                    Console.WriteLine($"- {emp.FirstName} {emp.LastName} ({emp.GetType().Name})");
                }

                // Display full-time employees
                Console.WriteLine("\nFull-Time Employees:");
                var fullTimeEmployees = employeeService.GetFullTimeEmployees();
                foreach (var emp in fullTimeEmployees)
                {
                    Console.WriteLine($"- {emp.FirstName} {emp.LastName}, Vacation Days: {emp.VacationDays}");
                }

                // Display contract employees
                Console.WriteLine("\nContract Employees:");
                var contractEmployees = employeeService.GetContractEmployees();
                foreach (var emp in contractEmployees)
                {
                    Console.WriteLine($"- {emp.FirstName} {emp.LastName}, Hourly Rate: {emp.HourlyRate:C}");
                }
            }

            app.Run();
        }
    }
}

using System;
using NHibernate.Linq;
using System.Linq;
using NH.Entity.Entities;
using NHibernate.Transform;
using Microsoft.Extensions.DependencyInjection;
using NH.Entity.Interfaces;
using NH.Entity.Services;
using NHibernate;
using NH.Entity.Repositories;

namespace nhibernate_practice
{
    class Program
    {
        static void TruncateAllTables(NHibernate.ISession session)
        {
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    // Disable foreign key checks
                    session.CreateSQLQuery("SET FOREIGN_KEY_CHECKS = 0").ExecuteUpdate();

                    // Truncate tables
                    session.CreateSQLQuery("TRUNCATE TABLE Employee").ExecuteUpdate();
                    session.CreateSQLQuery("TRUNCATE TABLE Department").ExecuteUpdate();

                    // Re-enable foreign key checks
                    session.CreateSQLQuery("SET FOREIGN_KEY_CHECKS = 1").ExecuteUpdate();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddScoped<NHibernate.ISession>(provider => 
            {
                return NHibernateHelper.OpenSession();
            });

            // Register repositories
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Register services
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();

            var app = builder.Build();

            // Example of using the services
            using (var scope = app.Services.CreateScope())
            {
                var employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();
                var departmentService = scope.ServiceProvider.GetRequiredService<IDepartmentService>();

                // Create sample data
                using (var session = NHibernateHelper.OpenSession())
                {
                    // Truncate all tables before inserting new data
                    TruncateAllTables(session);

                    using (var transaction = session.BeginTransaction())
                    {
                        var itDepartment = new Department { Name = "IT" };
                        var hrDepartment = new Department { Name = "HR" };

                        var fullTimeEmployee = new FullTimeEmployee
                        {
                            FirstName = "John",
                            LastName = "Doe",
                            Salary = 50000,
                            Department = itDepartment,
                            VacationDays = 20,
                            InsuranceNumber = "INS123"
                        };

                        var contractEmployee = new ContractEmployee
                        {
                            FirstName = "Jane",
                            LastName = "Smith",
                            Salary = 45000,
                            Department = hrDepartment,
                            ContractEndDate = DateTime.Now.AddMonths(6),
                            HourlyRate = 25.5m
                        };

                        session.Save(itDepartment);
                        session.Save(hrDepartment);
                        session.Save(fullTimeEmployee);
                        session.Save(contractEmployee);

                        transaction.Commit();
                    }
                }

                // Query data
                using (var session = NHibernateHelper.OpenSession())
                {
                    // Query all employees
                    Console.WriteLine("All Employees:");
                    var allEmployees = session.Query<Employee>().ToList();
                    foreach (var emp in allEmployees)
                    {
                        Console.WriteLine($"- {emp.FirstName} {emp.LastName} ({emp.GetType().Name})");
                    }

                    // Query only full-time employees
                    Console.WriteLine("\nFull-Time Employees:");
                    var fullTimeEmployees = session.Query<FullTimeEmployee>().ToList();
                    foreach (var emp in fullTimeEmployees)
                    {
                        Console.WriteLine($"- {emp.FirstName} {emp.LastName}, Vacation Days: {emp.VacationDays}");
                    }

                    // Query only contract employees
                    Console.WriteLine("\nContract Employees:");
                    var contractEmployees = session.Query<ContractEmployee>().ToList();
                    foreach (var emp in contractEmployees)
                    {
                        Console.WriteLine($"- {emp.FirstName} {emp.LastName}, Hourly Rate: {emp.HourlyRate:C}");
                    }
                }
            }

            app.Run();
        }
    }
}

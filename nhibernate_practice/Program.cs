using System;
using NH.Entity;
using NHibernate.Linq;
using System.Linq;
using NH.Entity.Entities;

namespace nhibernate_practice
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create sample data
            using (var session = NHibernateHelper.OpenSession())
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
    }
}

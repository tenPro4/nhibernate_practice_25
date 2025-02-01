using NH.Service.Services.Abstract;
using NH.Entity.Entities;
using NHibernate;
using Microsoft.Extensions.Configuration;

namespace NH.Service.Services.Concrete
{
    public class DataInitializationService : IDataInitializationService
    {
        private readonly ISession _session;
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IConfiguration _configuration;

        public DataInitializationService(
            ISession session,
            IEmployeeService employeeService,
            IDepartmentService departmentService,
            IConfiguration configuration)
        {
            _session = session;
            _employeeService = employeeService;
            _departmentService = departmentService;
            _configuration = configuration;
        }

        public void InitializeDatabase()
        {
            TruncateAllTables();
            CreateSampleData();
        }

        public void TruncateAllTables()
        {
            var dbType = _configuration.GetConnectionString("Type")?.ToLower() ?? "mysql";

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    if (dbType == "mysql")
                    {
                        _session.CreateSQLQuery("SET FOREIGN_KEY_CHECKS = 0").ExecuteUpdate();
                        _session.CreateSQLQuery("TRUNCATE TABLE Employee").ExecuteUpdate();
                        _session.CreateSQLQuery("TRUNCATE TABLE Department").ExecuteUpdate();
                        _session.CreateSQLQuery("SET FOREIGN_KEY_CHECKS = 1").ExecuteUpdate();
                    }
                    else if (dbType == "sqlite")
                    {
                        _session.CreateSQLQuery("DELETE FROM Employee").ExecuteUpdate();
                        _session.CreateSQLQuery("DELETE FROM Department").ExecuteUpdate();
                        // Reset SQLite auto-increment counters
                        _session.CreateSQLQuery("DELETE FROM sqlite_sequence WHERE name IN ('Employee', 'Department')").ExecuteUpdate();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void CreateSampleData()
        {
            var itDepartment = new Department { Name = "IT" };
            var hrDepartment = new Department { Name = "HR" };

            _departmentService.AddDepartment(itDepartment);
            _departmentService.AddDepartment(hrDepartment);

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

            _employeeService.AddEmployee(fullTimeEmployee);
            _employeeService.AddEmployee(contractEmployee);
        }
    }
} 
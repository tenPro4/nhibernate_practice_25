using NH.Entity.Entities;
using NH.Entity.Interfaces;
using NHibernate;

namespace NH.Entity.Services
{
    public class DataInitializationService : IDataInitializationService
    {
        private readonly ISession _session;
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        public DataInitializationService(
            ISession session,
            IEmployeeService employeeService,
            IDepartmentService departmentService)
        {
            _session = session;
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        public void InitializeDatabase()
        {
            TruncateAllTables();
            CreateSampleData();
        }

        public void TruncateAllTables()
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    _session.CreateSQLQuery("SET FOREIGN_KEY_CHECKS = 0").ExecuteUpdate();
                    _session.CreateSQLQuery("TRUNCATE TABLE Employee").ExecuteUpdate();
                    _session.CreateSQLQuery("TRUNCATE TABLE Department").ExecuteUpdate();
                    _session.CreateSQLQuery("SET FOREIGN_KEY_CHECKS = 1").ExecuteUpdate();
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
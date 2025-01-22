using NH.Entity.Entities;
using NH.Entity.Interfaces;
using NHibernate;

namespace NH.Entity.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<FullTimeEmployee> _fullTimeEmployeeRepository;
        private readonly IRepository<ContractEmployee> _contractEmployeeRepository;

        public EmployeeService(
            IRepository<Employee> employeeRepository,
            IRepository<FullTimeEmployee> fullTimeEmployeeRepository,
            IRepository<ContractEmployee> contractEmployeeRepository)
        {
            _employeeRepository = employeeRepository;
            _fullTimeEmployeeRepository = fullTimeEmployeeRepository;
            _contractEmployeeRepository = contractEmployeeRepository;
        }

        public IList<Employee> GetAllEmployees()
        {
            return _employeeRepository.GetAll().ToList();
        }

        public IList<FullTimeEmployee> GetFullTimeEmployees()
        {
            return _fullTimeEmployeeRepository.GetAll().ToList();
        }

        public IList<ContractEmployee> GetContractEmployees()
        {
            return _contractEmployeeRepository.GetAll().ToList();
        }

        public void AddEmployee(Employee employee)
        {
            _employeeRepository.Add(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            _employeeRepository.Update(employee);
        }

        public void DeleteEmployee(int id)
        {
            _employeeRepository.DeleteById(id);
        }

        public Employee GetEmployeeById(int id)
        {
            return _employeeRepository.GetById(id);
        }
    }
} 
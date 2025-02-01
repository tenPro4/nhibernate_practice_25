using NH.Entity.Entities;

namespace NH.Service.Services.Abstract
{
    public interface IEmployeeService
    {
        IList<Employee> GetAllEmployees();
        IList<FullTimeEmployee> GetFullTimeEmployees();
        IList<ContractEmployee> GetContractEmployees();
        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(int id);
        Employee GetEmployeeById(int id);
    }
} 
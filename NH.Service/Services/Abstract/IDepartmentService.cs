using NH.Entity.Entities;

namespace NH.Service.Services.Abstract
{
    public interface IDepartmentService
    {
        IList<Department> GetAllDepartments();
        Department GetDepartmentById(int id);
        void AddDepartment(Department department);
        void UpdateDepartment(Department department);
        void DeleteDepartment(int id);
        IList<Employee> GetEmployeesByDepartment(int departmentId);
    }
} 
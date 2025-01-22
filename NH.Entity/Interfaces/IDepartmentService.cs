using NH.Entity.Entities;

namespace NH.Entity.Interfaces
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
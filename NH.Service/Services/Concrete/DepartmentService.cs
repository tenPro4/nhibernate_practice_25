using NH.Service.Services.Abstract;
using NH.Entity.Entities;
using NH.Repo.Repositories;

namespace NH.Service.Services.Concrete
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<Employee> _employeeRepository;

        public DepartmentService(IRepository<Department> departmentRepository, IRepository<Employee> employeeRepository)
        {
            _departmentRepository = departmentRepository;
            _employeeRepository = employeeRepository;
        }

        public IList<Department> GetAllDepartments()
        {
            return _departmentRepository.GetAll().ToList();
        }

        public Department GetDepartmentById(int id)
        {
            return _departmentRepository.GetById(id);
        }

        public void AddDepartment(Department department)
        {
            _departmentRepository.Add(department);
        }

        public void UpdateDepartment(Department department)
        {
            _departmentRepository.Update(department);
        }

        public void DeleteDepartment(int id)
        {
            _departmentRepository.DeleteById(id);
        }

        public IList<Employee> GetEmployeesByDepartment(int departmentId)
        {
            return _employeeRepository.Find(e => e.Department.Id == departmentId).ToList();
        }
    }
} 
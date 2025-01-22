namespace NH.Entity.Entities
{
    public class FullTimeEmployee : Employee
    {
        public virtual int VacationDays { get; set; }
        public virtual string InsuranceNumber { get; set; }
    }
} 
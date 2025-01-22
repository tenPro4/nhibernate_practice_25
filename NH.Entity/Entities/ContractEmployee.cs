namespace NH.Entity.Entities
{
    public class ContractEmployee : Employee
    {
        public virtual DateTime ContractEndDate { get; set; }
        public virtual decimal HourlyRate { get; set; }
    }
} 
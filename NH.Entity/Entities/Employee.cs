namespace NH.Entity.Entities
{
    public abstract class Employee
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual decimal Salary { get; set; }
        public virtual Department Department { get; set; }
    }
}
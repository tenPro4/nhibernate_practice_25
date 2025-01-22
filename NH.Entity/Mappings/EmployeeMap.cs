using FluentNHibernate.Mapping;
using NH.Entity.Entities;

namespace NH.Entity.Mappings
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.FirstName).Length(50).Not.Nullable();
            Map(x => x.LastName).Length(50).Not.Nullable();
            Map(x => x.Salary).Not.Nullable();
            References(x => x.Department)
                .Column("DepartmentId")
                .Not.Nullable();

            // Add discriminator column
            DiscriminateSubClassesOnColumn("EmployeeType")
                .Length(50);
        }
    }

    public class FullTimeEmployeeMap : SubclassMap<FullTimeEmployee>
    {
        public FullTimeEmployeeMap()
        {
            DiscriminatorValue("FullTime");
            Map(x => x.VacationDays);
            Map(x => x.InsuranceNumber).Length(50);
        }
    }

    public class ContractEmployeeMap : SubclassMap<ContractEmployee>
    {
        public ContractEmployeeMap()
        {
            DiscriminatorValue("Contract");
            Map(x => x.ContractEndDate);
            Map(x => x.HourlyRate);
        }
    }
} 
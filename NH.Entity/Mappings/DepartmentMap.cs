using FluentNHibernate.Mapping;
using NH.Entity.Entities;

namespace NH.Entity.Mappings
{
    public class DepartmentMap : ClassMap<Department>
    {
        public DepartmentMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name).Length(100).Not.Nullable();
            HasMany(x => x.Employees)
                .Inverse()
                .Cascade.All()
                .KeyColumn("DepartmentId");
        }
    }
} 
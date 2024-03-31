using EmployeeMgt.Domain.Enums;

namespace EmployeeMgt.Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Sex Sex { get; set; }
        public Guid? UserId { get; set; }
    }
}

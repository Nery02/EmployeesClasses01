using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesClasses01.Models
{
    public class Employee : BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double Salary { get; set; }
        public string ProfilePic { get; set; }

        [ForeignKey("EmployeeType")]
        public int EmployeeTypeId { get; set; }
        public EmployeeType EmployeeType { get; set; }

    }
}

using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public int? ApplicationId { get; set; }
        public int JobId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime? HireDate { get; set; }
        public decimal Salary { get; set; }
        public string Status { get; set; } = null!;

        public virtual JobApplication? Application { get; set; }
        public virtual Job Job { get; set; } = null!;
    }
}

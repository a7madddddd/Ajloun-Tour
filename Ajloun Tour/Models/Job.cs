using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class Job
    {
        public Job()
        {
            Employees = new HashSet<Employee>();
            JobApplications = new HashSet<JobApplication>();
            JobImages = new HashSet<JobImage>();
        }

        public int JobId { get; set; }
        public string? Title { get; set; }
        public string? JobType { get; set; }
        public string? Salary { get; set; }
        public int? Vacancies { get; set; }
        public string? Description { get; set; }
        public string? Requirements { get; set; }
        public string? Overview { get; set; }
        public string? Experinces { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<JobApplication> JobApplications { get; set; }
        public virtual ICollection<JobImage> JobImages { get; set; }
    }
}

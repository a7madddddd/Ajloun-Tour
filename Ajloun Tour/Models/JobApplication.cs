using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class JobApplication
    {
        public int ApplicationId { get; set; }
        public int? JobId { get; set; }
        public string? ApplicantName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Cvpath { get; set; }
        public string? Message { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string? Status { get; set; }

        public virtual Job? Job { get; set; }

    }
}

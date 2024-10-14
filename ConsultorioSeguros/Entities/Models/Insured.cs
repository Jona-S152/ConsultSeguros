using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Insured
    {
        public Insured()
        {
            InsuranceInsureds = new HashSet<InsuranceInsured>();
        }

        public int Id { get; set; }
        public string? Identification { get; set; }
        public string? InsuredName { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Age { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<InsuranceInsured> InsuranceInsureds { get; set; }
    }
}

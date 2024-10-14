using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Insurance
    {
        public Insurance()
        {
            InsuranceInsureds = new HashSet<InsuranceInsured>();
        }

        public int Id { get; set; }
        public string? InsuranceName { get; set; }
        public string? InsuranceCode { get; set; }
        public decimal? InsuredAmount { get; set; }
        public decimal? Prima { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<InsuranceInsured> InsuranceInsureds { get; set; }
    }
}

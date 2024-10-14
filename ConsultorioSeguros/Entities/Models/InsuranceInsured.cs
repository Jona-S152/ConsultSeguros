using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class InsuranceInsured
    {
        public int Id { get; set; }
        public int? IdInsurance { get; set; }
        public int? IdInsured { get; set; }
        public bool? Status { get; set; }

        public virtual Insurance? IdInsuranceNavigation { get; set; }
        public virtual Insured? IdInsuredNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class InsuranceDTO
    {
        public int? Id { get; set; }
        public string? InsuranceName { get; set; }
        public string? InsuranceCode { get; set; }
        public decimal? InsuranceAmount { get; set; }
        public decimal? Prima { get; set; }
    }
}

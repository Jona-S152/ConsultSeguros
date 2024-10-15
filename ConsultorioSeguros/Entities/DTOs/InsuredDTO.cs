using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class InsuredDTO
    {
        public int Id { get; set; }
        public string Identification { get; set; }
        public string InsuredName { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
    }
}

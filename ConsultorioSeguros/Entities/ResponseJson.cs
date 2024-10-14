using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ResponseJson
    {
        public string Message { get; set; }
        public object Data { get; set; }
        public bool Error { get; set; }
    }
}

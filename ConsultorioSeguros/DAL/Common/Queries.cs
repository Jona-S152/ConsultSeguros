using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Common
{
    public class Queries
    {
        public const string GetInsuredId = "SELECT TOP 1 Id FROM Insured WHERE Status = 1 ORDER BY Id DESC";
    }
}

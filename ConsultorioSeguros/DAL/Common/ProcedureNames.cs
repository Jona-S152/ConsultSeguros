using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Common
{
    public class ProcedureNames
    {
        public const string InsertInsured = "InsertInsured";
        public const string UpdateInsured = "UpdateInsured";
        public const string GetInsuredByIdentification = "GetInsuredByIdentification";
        public const string GetInsuredById = "GetInsuredById";
        public const string GetAllInsureds = "GetAllInsureds";
        public const string DeleteInsured = "DeleteInsured";
        public const string GetInsurancesByInsured = "GetInsurancesByInsured";
    }
}

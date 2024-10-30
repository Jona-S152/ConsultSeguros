using Entities.DTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Insured
{
    public interface IInsuredRepository
    {
        public Task<bool> AddInsuredAsync(InsuredDTO insuredDTO);
        public Task<bool> UpdateInsuredAsync(int id, InsuredDTO insuredDTO);
        public Task<bool> DeleteInsuredAsync(int id);
        public Task<Dictionary<bool, List<InsuredDTOGet>?>> GetAllInsuredAsync();
        public Task<Dictionary<bool, InsuredDTOGet>> GetInsuredAsync(int id);
        public Task<Dictionary<bool, InsuredDTOGet>> GetInsuredByIdentificationAsync(string identification);
        public Task<Dictionary<bool, List<InsuranceDTO>>> GetAllInsuranceByInsuredAsync(string identification);
        public Task<bool> UploadInsuredsAsync(DataTable? insureds);
        public Task<int> GetInsuredIdAsync(SqlConnection conn, SqlTransaction tran);
        public Task<bool> AssignInsuanceToInsuredAsync(int id, InsuredDTO insuredDTO, DataTable insurances, SqlConnection conn, SqlTransaction tran);
        public Task<bool> UpdateInsuredWithInsurances(int id, InsuredDTO insured, DataTable insurances);
        public Task<bool> AssignInsuanceToInsured(int id, InsuredDTO insuredDTO, DataTable insurances, SqlConnection conn, SqlTransaction tran);
    }
}

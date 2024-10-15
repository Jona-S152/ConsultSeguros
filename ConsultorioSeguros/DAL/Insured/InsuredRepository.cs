using DAL.Common;
using Entities.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Insured
{
    public class InsuredRepository : IInsuredRepository
    {
        private readonly string _connectionString;
        public InsuredRepository(IOptions<ConnectionStrings> connectionString)
        {
            _connectionString = connectionString.Value.DB_Seguros;
        }
        public async Task<bool> AddInsuredAsync(InsuredDTO insuredDTO)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string spName = ProcedureNames.InsertInsured;
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    conn.Open();

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(SPParameters.Identification, insuredDTO.Identification);
                    cmd.Parameters.AddWithValue(SPParameters.InsuredName, insuredDTO.InsuredName);
                    cmd.Parameters.AddWithValue(SPParameters.PhoneNumber, insuredDTO.PhoneNumber);
                    cmd.Parameters.AddWithValue(SPParameters.Age, insuredDTO.Age);

                    SqlParameter outputParameter = new SqlParameter(SPParameters.Result, System.Data.SqlDbType.Bit)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };

                    cmd.Parameters.Add(outputParameter);

                    await cmd.ExecuteNonQueryAsync();

                    bool result = (bool)cmd.Parameters[SPParameters.Result].Value;

                    conn.Close();

                    return result;
                }
            }
        }

        public Task<bool> DeleteInsuredAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<InsuredDTO>> GetAllInsuredAsync()
        {
            throw new NotImplementedException();
        }

        public Task<InsuredDTO> GetInsuredAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<InsuredDTO> GetInsuredByIdentificationAsync(string identification)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateInsuredAsync(int id, InsuredDTO insuredDTO)
        {
            throw new NotImplementedException();
        }
    }
}

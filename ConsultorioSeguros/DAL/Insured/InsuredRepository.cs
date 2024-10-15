using DAL.Common;
using Entities.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
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

                    cmd.Parameters.Add(new SqlParameter() { ParameterName = SPParameters.Result, SqlDbType = System.Data.SqlDbType.Bit, Direction = System.Data.ParameterDirection.Output });

                    await cmd.ExecuteNonQueryAsync();

                    bool result = (bool)cmd.Parameters[SPParameters.Result].Value;

                    conn.Close();

                    return result;
                }
            }
        }

        public async Task<bool> DeleteInsuredAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string spName = ProcedureNames.DeleteInsured;
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    conn.Open();

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(SPParameters.Id, id);

                    cmd.Parameters.Add(new SqlParameter() { ParameterName = SPParameters.Result, SqlDbType = System.Data.SqlDbType.Bit, Direction = System.Data.ParameterDirection.Output });

                    await cmd.ExecuteNonQueryAsync();

                    bool result = (bool)cmd.Parameters[SPParameters.Result].Value;

                    conn.Close();

                    return result;
                }
            }
        }

        public async Task<Dictionary<bool, List<InsuredDTO>?>> GetAllInsuredAsync()
        {
            Dictionary<bool, List<InsuredDTO>?> result = new Dictionary<bool, List<InsuredDTO>?>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string spName = ProcedureNames.GetAllInsureds;

                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    conn.Open();

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            List<InsuredDTO> insuredList = new List<InsuredDTO>();
                            while (reader.Read())
                            {
                                InsuredDTO insured = new InsuredDTO();
                                insured.Id = reader.GetInt32(0);
                                insured.Identification = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                                insured.InsuredName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                                insured.PhoneNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                                insured.Age = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);

                                insuredList.Add(insured);
                            }

                            result.Add(true, insuredList);
                        }
                        else
                        {
                            result.Add(false, null);
                        }

                        conn.Close();

                        return result;
                    }
                }
            }
        }

        public async Task<Dictionary<bool, InsuredDTO?>> GetInsuredAsync(int id)
        {
            Dictionary<bool, InsuredDTO> result = new Dictionary<bool, InsuredDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string spName = ProcedureNames.GetInsuredById;

                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    conn.Open();

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(SPParameters.Id, id);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            InsuredDTO insured = new InsuredDTO();
                            insured.Id = reader.GetInt32(0);
                            insured.Identification = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            insured.InsuredName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                            insured.PhoneNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                            insured.Age = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);

                            result.Add(true, insured);
                        }
                        else
                        {
                            result.Add(false, null);
                        }

                        conn.Close();

                        return result;
                    }
                }
            }
        }

        public async Task<Dictionary<bool, InsuredDTO?>> GetInsuredByIdentificationAsync(string identification)
        {
            Dictionary<bool, InsuredDTO> result = new Dictionary<bool, InsuredDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string spName = ProcedureNames.GetInsuredByIdentification;

                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    conn.Open();

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(SPParameters.Identification, identification);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            InsuredDTO insured = new InsuredDTO();
                            insured.Id = reader.GetInt32(0);
                            insured.Identification = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            insured.InsuredName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                            insured.PhoneNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                            insured.Age = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);

                            result.Add(true, insured);
                        }
                        else
                        {
                            result.Add(false, null);
                        }

                        conn.Close();

                        return result;
                    }
                }
            }
        }

        public async Task<Dictionary<bool, InsuredDTO?>> UpdateInsuredAsync(int id, InsuredDTO insuredDTO)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string spName = ProcedureNames.UpdateInsured;
                Dictionary<bool, InsuredDTO> dictionaryResult = new Dictionary<bool, InsuredDTO>();

                conn.Open();

                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(SPParameters.Id, id);
                    cmd.Parameters.AddWithValue(SPParameters.Identification, insuredDTO.Identification);
                    cmd.Parameters.AddWithValue(SPParameters.InsuredName, insuredDTO.InsuredName);
                    cmd.Parameters.AddWithValue(SPParameters.PhoneNumber, insuredDTO.PhoneNumber);
                    cmd.Parameters.AddWithValue(SPParameters.Age, insuredDTO.Age);

                    cmd.Parameters.Add(new SqlParameter() { ParameterName = SPParameters.Result, SqlDbType = System.Data.SqlDbType.Bit, Direction = System.Data.ParameterDirection.Output });

                    await cmd.ExecuteNonQueryAsync();

                    bool result = (bool)cmd.Parameters[SPParameters.Result].Value;
                    
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (result)
                        {
                            if (reader.Read())
                            {
                                InsuredDTO insured = new InsuredDTO();
                                insured.Id = reader.GetInt32(0);
                                insured.Identification = reader.GetString(1);
                                insured.InsuredName = reader.GetString(2);
                                insured.PhoneNumber = reader.GetString(3);
                                insured.Age = reader.GetInt32(4);

                                dictionaryResult.Add(result, insured);
                            } 
                        }
                        else
                        {
                            dictionaryResult.Add(result, null);
                        }

                        conn.Close();

                        return dictionaryResult;
                    }

                }
            }
        }
    }
}

using DAL.Common;
using Entities.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<Dictionary<bool, List<InsuranceDTO>>> GetAllInsuranceByInsuredAsync(string identification)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string spName = ProcedureNames.GetInsurancesByInsured;

                Dictionary<bool, List<InsuranceDTO>> dictionaryResult = new Dictionary<bool, List<InsuranceDTO>>();

                List<InsuranceDTO> insurances = new List<InsuranceDTO>();

                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(SPParameters.Identification, identification);
                    cmd.Parameters.Add(new SqlParameter() { ParameterName = SPParameters.Result, SqlDbType = System.Data.SqlDbType.Bit, Direction = System.Data.ParameterDirection.Output });


                    await cmd.ExecuteNonQueryAsync();

                    bool result = (bool)cmd.Parameters[SPParameters.Result].Value;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows)
                        {
                            dictionaryResult.Add(false, null);

                            return dictionaryResult;
                        }

                        while (reader.Read())
                        {
                            InsuranceDTO insurance = new InsuranceDTO();
                            insurance.InsuranceName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            insurance.InsuranceCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                            insurance.InsuranceAmount = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3);
                            insurance.Prima = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4);

                            insurances.Add(insurance);
                        }

                        dictionaryResult.Add(true, insurances);

                        conn.Close();

                        return dictionaryResult;
                    }
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
                        if (reader.HasRows)
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
                        if (reader.HasRows && reader.Read())
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
                        if (reader.HasRows && reader.Read())
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
                            if (reader.HasRows)
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

        public async Task<bool> UploadInsuredsAsync(DataTable? insureds)
        {
            if (insureds == null) return false;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    conn.Open();
                    bulkCopy.DestinationTableName = insureds.TableName;

                    bulkCopy.ColumnMappings.Add(ColumnNamesInsured.Identification, ColumnNamesInsured.Identification);
                    bulkCopy.ColumnMappings.Add(ColumnNamesInsured.Name, ColumnNamesInsured.Name);
                    bulkCopy.ColumnMappings.Add(ColumnNamesInsured.PhoneNumber, ColumnNamesInsured.PhoneNumber);
                    bulkCopy.ColumnMappings.Add(ColumnNamesInsured.Age, ColumnNamesInsured.Age);
                    bulkCopy.ColumnMappings.Add(ColumnNamesInsured.Status, ColumnNamesInsured.Status);

                    await bulkCopy.WriteToServerAsync(insureds);

                    conn.Close();
                }
            }

            return true;
        }
    }
}

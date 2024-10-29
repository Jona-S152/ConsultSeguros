using DAL.Common;
using Entities.DTOs;
using Entities.Models;
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
        public async Task<Dictionary<bool, Dictionary<SqlConnection, SqlTransaction>>> AddInsuredAsync(InsuredDTO insuredDTO)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {
                            string spName = ProcedureNames.InsertInsured;
                            using (SqlCommand cmd = new SqlCommand(spName, conn))
                            {
                                cmd.Transaction = tran;

                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue(SPParameters.Identification, insuredDTO.Identification);
                                cmd.Parameters.AddWithValue(SPParameters.InsuredName, insuredDTO.InsuredName);
                                cmd.Parameters.AddWithValue(SPParameters.PhoneNumber, insuredDTO.PhoneNumber);
                                cmd.Parameters.AddWithValue(SPParameters.Age, insuredDTO.Age);

                                cmd.Parameters.Add(new SqlParameter() { ParameterName = SPParameters.Result, SqlDbType = System.Data.SqlDbType.Bit, Direction = System.Data.ParameterDirection.Output });

                                await cmd.ExecuteNonQueryAsync();

                                bool result = (bool)cmd.Parameters[SPParameters.Result].Value;

                                Dictionary<bool, Dictionary<SqlConnection, SqlTransaction>> resultInsert = new Dictionary<bool, Dictionary<SqlConnection, SqlTransaction>>();

                                Dictionary<SqlConnection, SqlTransaction> keyValuePairs = new Dictionary<SqlConnection, SqlTransaction>();

                                keyValuePairs.Add(conn, tran);

                                resultInsert.Add(result, keyValuePairs);

                                return resultInsert;
                            }
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetInsuredIdAsync(SqlConnection conn, SqlTransaction tran)
        {
            string query = Queries.GetInsuredId;

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = tran;

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }

        public async Task<bool> AssignInsuanceToInsuredAsync(int id, InsuredDTO insuredDTO, DataTable insurances, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                if (insurances == null) return false;

                string spName = ProcedureNames.IU_InsuranceInsured;

                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = tran;

                    cmd.Parameters.AddWithValue(SPParameters.Id_Insured, id);
                    cmd.Parameters.AddWithValue(SPParameters.Insurances, insurances);

                    await cmd.ExecuteNonQueryAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteInsuredAsync(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {
                            string spName = ProcedureNames.DeleteInsured;
                            using (SqlCommand cmd = new SqlCommand(spName, conn))
                            {

                                cmd.Transaction = tran;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue(SPParameters.Id, id);

                                cmd.Parameters.Add(new SqlParameter() { ParameterName = SPParameters.Result, SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });

                                await cmd.ExecuteNonQueryAsync();

                                bool result = (bool)cmd.Parameters[SPParameters.Result].Value;

                                tran.Commit();

                                return result;
                            }
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new Exception(ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                            insurance.Id = reader.GetInt32(0);
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

        public async Task<Dictionary<bool, List<InsuredDTOGet>?>> GetAllInsuredAsync()
        {
            Dictionary<bool, List<InsuredDTOGet>?> result = new Dictionary<bool, List<InsuredDTOGet>?>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string spName = ProcedureNames.GetAllInsureds;

                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    conn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            List<InsuredDTOGet> insuredList = new List<InsuredDTOGet>();
                            while (reader.Read())
                            {
                                InsuredDTOGet insured = new InsuredDTOGet();
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

        public async Task<Dictionary<bool, InsuredDTOGet?>> GetInsuredAsync(int id)
        {
            Dictionary<bool, InsuredDTOGet> result = new Dictionary<bool, InsuredDTOGet>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string spName = ProcedureNames.GetInsuredById;

                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    conn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(SPParameters.Id, id);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && reader.Read())
                        {
                            InsuredDTOGet insured = new InsuredDTOGet();
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

        public async Task<Dictionary<bool, InsuredDTOGet?>> GetInsuredByIdentificationAsync(string identification)
        {
            Dictionary<bool, InsuredDTOGet> result = new Dictionary<bool, InsuredDTOGet>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string spName = ProcedureNames.GetInsuredByIdentification;

                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    conn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(SPParameters.Identification, identification);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && reader.Read())
                        {
                            InsuredDTOGet insured = new InsuredDTOGet();
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

        public async Task<bool> UpdateInsuredAsync(int id, InsuredDTO insuredDTO, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                string spName = ProcedureNames.UpdateInsured;

                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(SPParameters.Id, id);
                    cmd.Parameters.AddWithValue(SPParameters.Identification, insuredDTO.Identification);
                    cmd.Parameters.AddWithValue(SPParameters.InsuredName, insuredDTO.InsuredName);
                    cmd.Parameters.AddWithValue(SPParameters.PhoneNumber, insuredDTO.PhoneNumber);
                    cmd.Parameters.AddWithValue(SPParameters.Age, insuredDTO.Age);

                    cmd.Parameters.Add(new SqlParameter() { ParameterName = SPParameters.Result, SqlDbType = System.Data.SqlDbType.Bit, Direction = System.Data.ParameterDirection.Output });

                    await cmd.ExecuteNonQueryAsync();

                    bool result = (bool)cmd.Parameters[SPParameters.Result].Value;

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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

        public async Task<bool> UpdateInsuredWithInsurances(int id, InsuredDTO insured, DataTable insurances)
        {
            try
            {
                bool result = false;

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {
                            bool statusUpdate = await UpdateInsuredAsync(id, insured, conn, tran);

                            if (!statusUpdate)
                            {
                                tran.Rollback();
                                throw new Exception(MessageResponse.InsuredNotFound);
                            }

                            bool statusInsurances = await AssignInsuanceToInsuredAsync(id, insured, insurances, conn, tran);

                            if (!statusInsurances)
                            {
                                tran.Rollback();
                                throw new Exception(MessageResponse.EmptyFields);
                            }

                            tran.Commit();
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                    conn.Close();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AssignInsuanceToInsured(int id, InsuredDTO insuredDTO, DataTable insurances, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                bool isSuccessful = await AssignInsuanceToInsuredAsync(id, insuredDTO, insurances, conn, tran);
                tran.Commit();

                return isSuccessful;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}

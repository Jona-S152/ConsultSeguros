using BLL.Common;
using DAL.Common;
using DAL.Insured;
using DAL.Repositories;
using Entities;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Insured
{
    public class InsuredService : IInsuredService
    {
        private readonly IInsuredRepository _insuredRepository;
        public InsuredService(IInsuredRepository repo)
        {
            _insuredRepository = repo;
        }
        public async Task<ResponseJson> AddInsuredAsync(InsuredDTO insuredDTO)
        {
            ResponseJson response = new ResponseJson();

            try
            {
                bool isSuccesful = await _insuredRepository.AddInsuredAsync(insuredDTO);

                if (!isSuccesful) return new ResponseJson() { Message = MessageResponse.IdentificationAlreadyExist, Data = null, Error = true };

                response.Message = MessageResponse.SuccessfulRegistration;
                response.Data = null;
                response.Error = false;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Data = null;
                response.Error = true;

                return response;
            }
        }

        public async Task<ResponseJson> DeleteInsuredAsync(int id)
        {
            ResponseJson response = new ResponseJson();

            try
            {
                bool isSuccesful = await _insuredRepository.DeleteInsuredAsync(id);

                if (isSuccesful) return new ResponseJson() { Message = MessageResponse.InsuredNotFound, Data = null, Error = true };

                response.Message = MessageResponse.SuccessfulRemoval;
                response.Data = null;
                response.Error = false;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Data = null;
                response.Error = true;

                return response;
            }
        }

        public async Task<ResponseJson> GetAllInsuranceByInsuredAsync(string identification)
        {
            ResponseJson response = new ResponseJson();

            try
            {
                Dictionary<bool, List<InsuranceDTO>> result = await _insuredRepository.GetAllInsuranceByInsuredAsync(identification);

                if (!result.First().Key) return new ResponseJson() { Message = MessageResponse.InsuredNotFound, Data = null, Error = true };

                response.Message = MessageResponse.InsuranceByInsuredList;
                response.Data = result.First().Value;
                response.Error = false;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Data = null;
                response.Error = true;

                return response;
            }
        }

        public async Task<ResponseJson> GetAllInsuredAsync()
        {
            ResponseJson response = new ResponseJson();

            try
            {
                Dictionary<bool, List<InsuredDTO>> result = await _insuredRepository.GetAllInsuredAsync();

                if (!result.First().Key) return new ResponseJson() { Message = MessageResponse.InsuredListNotFound, Data = null, Error = true };

                response.Message = MessageResponse.InsuredList;
                response.Data = result.First().Value;
                response.Error = false;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Data = null;
                response.Error = true;

                return response;
            }
        }

        public async Task<ResponseJson> GetInsuredAsync(int id)
        {
            ResponseJson response = new ResponseJson();

            try
            {
                Dictionary<bool, InsuredDTO> result = await _insuredRepository.GetInsuredAsync(id);

                if (!result.First().Key) return new ResponseJson() { Message = MessageResponse.InsuredNotFound, Data = null, Error = true };

                response.Message = MessageResponse.Insured;
                response.Data = result.First().Value;
                response.Error = false;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Data = null;
                response.Error = true;

                return response;
            }
        }

        public async Task<ResponseJson> GetInsuredByIdentificationAsync(string identification)
        {
            ResponseJson response = new ResponseJson();

            try
            {
                Dictionary<bool, InsuredDTO> result = await _insuredRepository.GetInsuredByIdentificationAsync(identification);

                if (!result.First().Key) return new ResponseJson() { Message = MessageResponse.InsuredNotFound, Data = null, Error = true };

                response.Message = MessageResponse.Insured;
                response.Data = result.First().Value;
                response.Error = false;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Data = null;
                response.Error = true;

                return response;
            }
        }

        public async Task<ResponseJson> UpdateInsuredAsync(int id, InsuredDTO insuredDTO)
        {
            ResponseJson response = new ResponseJson();

            try
            {
                Dictionary<bool, InsuredDTO?> result = await _insuredRepository.UpdateInsuredAsync(id, insuredDTO);

                if (!result.First().Key) return new ResponseJson() { Message = MessageResponse.InsuredNotFound, Data = null, Error = true };

                response.Message = MessageResponse.SuccessfulUpdating;
                response.Data = result.First().Value;
                response.Error = false;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Data = null;
                response.Error = true;

                return response;
            }
        }

        public async Task<ResponseJson> UploadInsuredsAsync(IFormFile formFile)
        {
            try
            {
                if (formFile.Length > 10485760) return new ResponseJson { Message = MessageResponse.OutOfLimitFileLength, Data = null, Error = true };

                string fileExtension = Path.GetExtension(formFile.FileName)?.ToLower();

                DataTable insureds = null;

                if (fileExtension == FileExtensions.txt)
                {
                    insureds = await ReadTxtFileAsync(formFile);
                }
                else if (fileExtension == FileExtensions.xlsx)
                {
                    insureds = await ReadExcelFileAsync(formFile);
                }
                else
                {
                    return new ResponseJson { Message = MessageResponse.NotSupportedFormatFile, Data = null, Error = true };
                }

                bool isSuccesfulInsert = await _insuredRepository.UploadInsuredsAsync(insureds);

                return isSuccesfulInsert ? new ResponseJson() { Message = MessageResponse.SuccessfulRegistration, Data = null, Error = false } : new ResponseJson() { Message = MessageResponse.EmptyFile, Data = null, Error = true };
            }
            catch (Exception ex)
            {
                return new ResponseJson() { Message = ex.Message, Data = null, Error = true };
            }
        }

        private async Task<DataTable> ReadTxtFileAsync(IFormFile file)
        {
            using (StreamReader reader = new StreamReader(file.OpenReadStream()))
            {
                string[] columnNames = { Common.ColumnNamesInsured.Identification, Common.ColumnNamesInsured.Name, Common.ColumnNamesInsured.PhoneNumber, Common.ColumnNamesInsured.Age, Common.ColumnNamesInsured.Status };

                DataTable table = GetFormatDatatable(TableNames.Insured, columnNames);

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    DataRow dataRow = table.NewRow();

                    dataRow[Common.ColumnNamesInsured.Identification] = line.Substring(0, 13).Trim();
                    dataRow[Common.ColumnNamesInsured.Name] = line.Substring(13, 50).Trim();
                    dataRow[Common.ColumnNamesInsured.PhoneNumber] = line.Substring(63, 13).Trim();
                    dataRow[Common.ColumnNamesInsured.Age] = line.Substring(76, 3).Trim();
                    dataRow[Common.ColumnNamesInsured.Status] = true;

                    table.Rows.Add(dataRow);
                }

                return table;
            }
        }

        private async Task<DataTable> ReadExcelFileAsync(IFormFile formFile)
        {
            using (ExcelPackage package = new ExcelPackage(formFile.OpenReadStream()))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                string[] columnNames = { Common.ColumnNamesInsured.Identification, Common.ColumnNamesInsured.Name, Common.ColumnNamesInsured.PhoneNumber, Common.ColumnNamesInsured.Age, Common.ColumnNamesInsured.Status };

                DataTable table = GetFormatDatatable(TableNames.Insured, columnNames);

                for (int row = 1; row <= worksheet.Dimension.Rows; row++)
                {
                    DataRow dataRow = table.NewRow();

                    for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                    {
                        dataRow[col - 1] = worksheet.Cells[row, col].Text;
                    }

                    table.Rows.Add(dataRow);
                }

                return table;
            }
        }

        private DataTable GetFormatDatatable(string tableName, string[] columnNames)
        {
            DataTable dt = new DataTable();
            dt.TableName = tableName;

            foreach (string columnName in columnNames)
            {
                if (columnName == Common.ColumnNamesInsured.Age)
                {
                    dt.Columns.Add(columnName, typeof(int));
                    continue;
                }

                if (columnName == Common.ColumnNamesInsured.Status)
                {
                    dt.Columns.Add(columnName, typeof(bool));
                    continue;
                }

                dt.Columns.Add(columnName, typeof(string));
            }

            return dt;
        }
    }
}

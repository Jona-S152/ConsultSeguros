using DAL.Common;
using DAL.Context;
using Entities;
using Entities.DTOs;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class InsuranceRepository : IInsuranceRepository
    {
        private readonly DbDataContext _context;

        public InsuranceRepository(DbDataContext context)
        {
            _context = context;
        }

        public async Task<ResponseJson> Delete(int id)
        {
            try
            {
                Insurance insurance = await _context.Insurances.FindAsync(id);

                if (insurance == null) return new ResponseJson() { Message = MessageResponse.InsuranceNotFound, Data = null, Error = true };

                insurance.Status = false;

                await _context.SaveChangesAsync();

                return new ResponseJson() { Message = MessageResponse.SuccessfulRemoval, Data = null, Error = false };
            }
            catch (Exception ex)
            {
                return new ResponseJson() { Message = ex.Message, Data = null, Error = true };
            }
        }

        public async Task<ResponseJson> Get(int id)
        {
            try
            {
                Insurance insurance = await _context.Insurances.FirstOrDefaultAsync(i => i.Id == id && i.Status == true);

                return new ResponseJson() { Message = MessageResponse.Insurance, Data = insurance, Error = false };
            }
            catch(Exception ex)
            {
                return new ResponseJson() { Message = ex.Message, Data = null, Error = true }; 
            }
        }

        public async Task<ResponseJson> GetAll()
        {
            try
            {
                List<Insurance> insurances = await _context.Insurances.Where(i => i.Status == true).ToListAsync();

                return new ResponseJson() { Message = MessageResponse.InsuranceList, Data = insurances, Error = false };
            }
            catch (Exception ex)
            {
                return new ResponseJson() { Message = ex.Message, Data = null, Error = true };
            }
        }

        public async Task<ResponseJson> GetAllInsuredByInsurance(string code)
        {
            try
            {
                List<InsuredDTO> insureds = await (from insured in _context.Insureds
                                                   join insuranceInsured in _context.InsuranceInsureds
                                                   on insured.Id equals insuranceInsured.IdInsured
                                                   join insurance in _context.Insurances
                                                   on insuranceInsured.IdInsurance equals insurance.Id
                                                   where insured.Status == true
                                                   && insurance.Status == true
                                                   && insuranceInsured.Status == true
                                                   && insurance.InsuranceCode == code
                                                   select new InsuredDTO
                                                   {
                                                       Id = insured.Id,
                                                       Identification = insured.Identification,
                                                       InsuredName = insured.InsuredName,
                                                       PhoneNumber = insured.PhoneNumber,
                                                       Age = insured.Age ?? 0
                                                   }).ToListAsync();

                if (insureds.Count == 0) return new ResponseJson() { Message = MessageResponse.InsuredListNotFound, Data = null, Error = true };

                return new ResponseJson() { Message = MessageResponse.InsuredList, Data = insureds, Error = false };

            }
            catch (Exception ex)
            {
                return new ResponseJson() { Message = ex.Message, Data = null, Error = true };
            }
        }

        public async Task<ResponseJson> GetByCode(string code)
        {
            try
            {
                Insurance insurance = await _context.Insurances.FirstOrDefaultAsync(i => i.InsuranceCode == code && i.Status == true);

                if (insurance == null) return new ResponseJson() { Message = MessageResponse.InsuranceNotFound, Data = null, Error = true };

                return new ResponseJson() { Message = MessageResponse.Insurance, Data = insurance, Error = false };
            }
            catch (Exception ex)
            {
                return new ResponseJson() { Message = ex.Message, Data = null, Error = true };
            }
        }

        public async Task<ResponseJson> Insert(Insurance insurance)
        {
            try
            {
                Insurance insuranceExist = await _context.Insurances.FirstOrDefaultAsync(i => (i.InsuranceCode == insurance.InsuranceCode || i.InsuranceName == insurance.InsuranceName) && i.Status == true);

                if (insuranceExist != null) return new ResponseJson() { Message = MessageResponse.InsuranceAlredyExist, Data = null, Error = true };

                if (insurance.InsuredAmount == 0 ||
                    string.IsNullOrEmpty(insurance.InsuranceName) ||
                    insurance.Prima == 0 ||
                    string.IsNullOrEmpty(insurance.InsuranceCode)) return new ResponseJson() { Message = MessageResponse.NoValidFields, Data = null, Error = true };

                if (insurance.InsuredAmount < 0 || insurance.Prima < 0) return new ResponseJson() { Message = MessageResponse.NegativeValues, Data = null, Error = true };

                if (insurance.InsuredAmount > 100000 || insurance.Prima > 100000) return new ResponseJson() { Message = MessageResponse.MaxAmount, Data = null, Error = true };

                if (string.IsNullOrEmpty(insurance.InsuranceName.Trim()) || string.IsNullOrEmpty(insurance.InsuranceCode.Trim())) return new ResponseJson() { Message = MessageResponse.EmptyFields, Data = null, Error = true };

                _context.Insurances.Add(insurance);
                await _context.SaveChangesAsync();

                return new ResponseJson() { Message = MessageResponse.SuccessfulRegistration, Data = null, Error = false };
            }
            catch (Exception ex)
            {
                return new ResponseJson() { Message = ex.Message, Data = null, Error = true };
            }
        }

        public async Task<ResponseJson> Update(int id, Insurance insurance)
        {
            try
            {
                Insurance insuranceDB = await _context.Insurances.FirstOrDefaultAsync(i => i.Id == id && i.Status == true);

                if (insurance == null) return new ResponseJson() { Message = MessageResponse.InsuranceNotFound, Data = null, Error = true };

                if (insurance.InsuredAmount == 0 ||
                    string.IsNullOrEmpty(insurance.InsuranceName) ||
                    insurance.Prima == 0 ||
                    string.IsNullOrEmpty(insurance.InsuranceCode)) return new ResponseJson() { Message = MessageResponse.NoValidFields, Data = null, Error = true };

                if (insurance.InsuredAmount < 0 || insurance.Prima < 0) return new ResponseJson() { Message = MessageResponse.NegativeValues, Data = null, Error = true };

                if (insurance.InsuredAmount > 100000 || insurance.Prima > 100000) return new ResponseJson() { Message = MessageResponse.MaxAmount, Data = null, Error = true };

                if (string.IsNullOrEmpty(insurance.InsuranceName.Trim()) || string.IsNullOrEmpty(insurance.InsuranceCode.Trim())) return new ResponseJson() { Message = MessageResponse.EmptyFields, Data = null, Error = true };
                
                insuranceDB.InsuranceName = insurance.InsuranceName;
                insuranceDB.InsuranceCode = insurance.InsuranceCode;
                insuranceDB.InsuredAmount = insurance.InsuredAmount;
                insuranceDB.Prima = insurance.Prima;

                await _context.SaveChangesAsync();

                return new ResponseJson() { Message = MessageResponse.SuccessfulUpdating, Data =  insuranceDB, Error = false };
            }
            catch (Exception ex)
            {
                return new ResponseJson() { Message = ex.Message, Data = null, Error = true };
            }
        }
    }
}

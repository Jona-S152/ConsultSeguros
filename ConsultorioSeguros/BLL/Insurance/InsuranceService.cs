﻿using DAL.Repositories;
using Entities;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Insurance
{
    public class InsuranceService : IInsuranceService
    {
        private readonly IInsuranceRepository _insuranceRepository;
        public InsuranceService(IInsuranceRepository insuranceRepository)
        {
            _insuranceRepository = insuranceRepository;
        }

        public Task<ResponseJson> Delete(int id)
        {
            return _insuranceRepository.Delete(id);
        }

        public async Task<ResponseJson> Get(int id)
        {
            ResponseJson response = await _insuranceRepository.Get(id);

            Entities.Models.Insurance insurance = (Entities.Models.Insurance)response.Data;

            InsuranceDTO insuranceDTO = new InsuranceDTO();
            insuranceDTO.Id = insurance.Id;
            insuranceDTO.InsuranceName = insurance.InsuranceName;
            insuranceDTO.InsuranceCode = insurance.InsuranceCode;
            insuranceDTO.InsuranceAmount = insurance.InsuredAmount;
            insuranceDTO.Prima = insurance.Prima;

            response.Data = insuranceDTO;

            return response;
        }

        public async Task<ResponseJson> GetAll()
        {
            ResponseJson response = await _insuranceRepository.GetAll();

            List<Entities.Models.Insurance> insurances = (List<Entities.Models.Insurance>)response.Data;

            List<InsuranceDTO> insurancesDTO = new List<InsuranceDTO>();

            foreach (Entities.Models.Insurance insurance in insurances)
            {
                InsuranceDTO insuranceDTO = new InsuranceDTO();
                insuranceDTO.Id = insurance.Id;
                insuranceDTO.InsuranceName = insurance.InsuranceName;
                insuranceDTO.InsuranceCode = insurance.InsuranceCode;
                insuranceDTO.InsuranceAmount = insurance.InsuredAmount;
                insuranceDTO.Prima = insurance.Prima;

                insurancesDTO.Add(insuranceDTO);
            }

            response.Data = insurancesDTO;

            return response;
        }

        public async Task<ResponseJson> GetAllInsuredByInsurance(string code)
        {
            return await _insuranceRepository.GetAllInsuredByInsurance(code);
        }

        public async Task<ResponseJson> GetByCode(string code)
        {
            ResponseJson response = await _insuranceRepository.GetByCode(code);

            Entities.Models.Insurance insurance = (Entities.Models.Insurance)response.Data;

            if (response.Data != null)
            {
                InsuranceDTO insuranceDTO = new InsuranceDTO();
                insuranceDTO.Id = insurance.Id;
                insuranceDTO.InsuranceName = insurance.InsuranceName;
                insuranceDTO.InsuranceCode = insurance.InsuranceCode;
                insuranceDTO.InsuranceAmount = insurance.InsuredAmount;
                insuranceDTO.Prima = insurance.Prima;

                response.Data = insuranceDTO;
            }

            return response;
        }

        public Task<ResponseJson> Insert(InsuranceDTO insuranceDTO)
        {
            Entities.Models.Insurance insurance = new Entities.Models.Insurance();
            insurance.InsuranceCode = insuranceDTO.InsuranceCode;
            insurance.InsuranceName = insuranceDTO.InsuranceName;
            insurance.InsuredAmount = insuranceDTO.InsuranceAmount;
            insurance.Prima = insuranceDTO.Prima;
            insurance.Status = true;

            return _insuranceRepository.Insert(insurance);
        }

        public async Task<ResponseJson> Update(int id, InsuranceDTO insuranceDTO)
        {
            Entities.Models.Insurance insurance = new Entities.Models.Insurance();
            insurance.InsuranceCode = insuranceDTO.InsuranceCode;
            insurance.InsuranceName = insuranceDTO.InsuranceName;
            insurance.InsuredAmount = insuranceDTO.InsuranceAmount;
            insurance.Prima = insuranceDTO.Prima;

            ResponseJson response = await _insuranceRepository.Update(id, insurance);

            Entities.Models.Insurance insuranseUpdated = (Entities.Models.Insurance)response.Data;

            InsuranceDTO insuranceDTOUpdated = new InsuranceDTO();
            insuranceDTOUpdated.Id = insuranseUpdated.Id;
            insuranceDTOUpdated.InsuranceName = insuranseUpdated.InsuranceName;
            insuranceDTOUpdated.InsuranceCode = insuranseUpdated.InsuranceCode;
            insuranceDTOUpdated.InsuranceAmount = insuranseUpdated.InsuredAmount;
            insuranceDTOUpdated.Prima = insuranseUpdated.Prima;

            response.Data = insuranceDTOUpdated;

            return response;
        }
    }
}

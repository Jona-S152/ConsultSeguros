using DAL.Common;
using DAL.Insured;
using DAL.Repositories;
using Entities;
using Entities.DTOs;
using System;
using System.Collections.Generic;
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
    }
}

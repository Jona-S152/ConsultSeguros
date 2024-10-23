using Entities;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Insured
{
    public interface IInsuredService
    {
        public Task<ResponseJson> AddInsuredAsync(InsuredDTO insuredDTO);
        public Task<ResponseJson> UpdateInsuredAsync(int id, InsuredDTO insuredDTO);
        public Task<ResponseJson> DeleteInsuredAsync(int id);
        public Task<ResponseJson> GetAllInsuredAsync();
        public Task<ResponseJson> GetInsuredAsync(int id);
        public Task<ResponseJson> GetInsuredByIdentificationAsync(string identification);
        public Task<ResponseJson> GetAllInsuranceByInsuredAsync(string identification);
        public Task<ResponseJson> UploadInsuredsAsync(IFormFile formFile);
        public Task<ResponseJson> AssignInsuanceToInsuredAsync(string insurancesIds);
    }
}

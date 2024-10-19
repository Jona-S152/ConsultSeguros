using Entities;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Insurance
{
    public interface IInsuranceService
    {
        public Task<ResponseJson> Insert(InsuranceDTO insurance);
        public Task<ResponseJson> Update(int id, InsuranceDTO insurance);
        public Task<ResponseJson> Delete(int id);
        public Task<ResponseJson> Get(int id);
        public Task<ResponseJson> GetAll();
        public Task<ResponseJson> GetByCode(string code);
        public Task<ResponseJson> GetAllInsuredByInsurance(string code);
    }
}

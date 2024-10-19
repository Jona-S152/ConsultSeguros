using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IInsuranceRepository
    {
        public Task<ResponseJson> Insert(Insurance insurance);
        public Task<ResponseJson> Update(int id, Insurance insurance);
        public Task<ResponseJson> Delete(int id);
        public Task<ResponseJson> Get(int id);
        public Task<ResponseJson> GetAll();
        public Task<ResponseJson> GetByCode(string code);
        public Task<ResponseJson> GetAllInsuredByInsurance(string code);
    }
}

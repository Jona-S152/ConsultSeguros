using Entities;
using Entities.DTOs;
using System;
using System.Collections.Generic;
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
    }
}

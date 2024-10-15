using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Insured
{
    public interface IInsuredRepository
    {
        public Task<bool> AddInsuredAsync(InsuredDTO insuredDTO);
        public Task<bool> UpdateInsuredAsync(int id, InsuredDTO insuredDTO);
        public Task<bool> DeleteInsuredAsync(int id);
        public Task<List<InsuredDTO>> GetAllInsuredAsync();
        public Task<InsuredDTO> GetInsuredAsync(int id);
        public Task<InsuredDTO> GetInsuredByIdentificationAsync(string identification);
    }
}

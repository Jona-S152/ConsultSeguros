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
        public Task<Dictionary<bool, InsuredDTO?>> UpdateInsuredAsync(int id, InsuredDTO insuredDTO);
        public Task<bool> DeleteInsuredAsync(int id);
        public Task<Dictionary<bool, List<InsuredDTO>?>> GetAllInsuredAsync();
        public Task<Dictionary<bool, InsuredDTO>> GetInsuredAsync(int id);
        public Task<Dictionary<bool, InsuredDTO>> GetInsuredByIdentificationAsync(string identification);
        public Task<Dictionary<bool, List<InsuranceDTO>>> GetAllInsuranceByInsuredAsync(string identification);
    }
}

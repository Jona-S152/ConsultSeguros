using BLL.Insurance;
using ConsultorioSeguros.ApiRoutes;
using Entities;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ConsultorioSeguros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceController : Controller
    {
        private readonly IInsuranceService _insuranceService;

        public InsuranceController(IInsuranceService insuranceService)
        {
            _insuranceService = insuranceService;
        }

        [HttpPost(ApiRoutes.ApiRoutes.Insurance.Add)]
        public async Task<IActionResult> AddInsurance([FromBody] InsuranceDTO insurance)
        {
            ResponseJson response = await _insuranceService.Insert(insurance);

            if (response.Error) return BadRequest(response);

            return Ok(response);
        }

        [HttpGet(ApiRoutes.ApiRoutes.Insurance.GetById)]
        public async Task<IActionResult> GetInsurance(int id)
        {
            ResponseJson response = await _insuranceService.Get(id);

            if (response.Error) return BadRequest(response);

            return Ok(response);
        }

        [HttpGet(ApiRoutes.ApiRoutes.Insurance.GetAllInsurances)]
        public async Task<IActionResult> GetInsurances()
        {
            ResponseJson response = await _insuranceService.GetAll();

            if (response.Error) return BadRequest(response);

            return Ok(response);
        }

        [HttpPut(ApiRoutes.ApiRoutes.Insurance.UpdateById)]
        public async Task<IActionResult> UpdateInsurance(int id, [FromBody] InsuranceDTO insurance)
        {
            ResponseJson response = await _insuranceService.Update(id, insurance);

            if (response.Error) return BadRequest(Response);

            return Ok(response);
        }

        [HttpDelete(ApiRoutes.ApiRoutes.Insurance.DeleteById)]
        public async Task<IActionResult> DeleteInsurance(int id)
        {
            ResponseJson response = await _insuranceService.Delete(id);

            if (response.Error) return BadRequest(response);

            return Ok(response);
        }

        [HttpGet(ApiRoutes.ApiRoutes.Insurance.GetByCode)]
        public async Task<IActionResult> GetByCode(string code)
        {
            ResponseJson response = await _insuranceService.GetByCode(code);

            if (response.Error) return BadRequest(response);

            return Ok(response);
        }

        [HttpGet(ApiRoutes.ApiRoutes.Insurance.GetAllInsuredsByInsurance)]
        public async Task<IActionResult> GetAllInsuredsByInsurance(string code)
        {
            ResponseJson response = await _insuranceService.GetAllInsuredByInsurance(code);

            if (response.Error) return BadRequest(response);

            return Ok(response);
        }
    }
}

using BLL.Insured;
using Entities;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsultorioSeguros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuredController : ControllerBase
    {
        private readonly IInsuredService _insuredService;
        public InsuredController(IInsuredService service)
        {
            _insuredService = service;
        }

        [HttpPost(ApiRoutes.ApiRoutes.Insured.Add)]
        public async Task<IActionResult> InsertInsured(InsuredDTO insuredDTO)
        {
            ResponseJson response = await _insuredService.AddInsuredAsync(insuredDTO);

            if (response.Error) return BadRequest(response);

            return Ok(response);
        }

        [HttpPut(ApiRoutes.ApiRoutes.Insured.UpdateById)]
        public async Task<IActionResult> UpdateInsured(int id, InsuredDTO insuredDTO)
        {
            ResponseJson response = await _insuredService.UpdateInsuredAsync(id, insuredDTO);

            if (response.Error) return BadRequest(Response);

            return Ok(response);
        }

        [HttpGet(ApiRoutes.ApiRoutes.Insured.GetByIdentification)]
        public async Task<IActionResult> GetInsuredByIdentification(string identification)
        {
            ResponseJson response = await _insuredService.GetInsuredByIdentificationAsync(identification);

            if (response.Error) return BadRequest(response);

            return Ok(response);
        }

        [HttpGet(ApiRoutes.ApiRoutes.Insured.GetById)]
        public async Task<IActionResult> GetInsuredById(int id)
        {
            ResponseJson response = await _insuredService.GetInsuredAsync(id);

            if (response.Error) return BadRequest(response);

            return Ok(response);
        }

        [HttpGet(ApiRoutes.ApiRoutes.Insured.GetAll)]
        public async Task<IActionResult> GetAllInsureds()
        {
            ResponseJson response = await _insuredService.GetAllInsuredAsync();

            if (response.Error) return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete(ApiRoutes.ApiRoutes.Insured.DeleteById)]
        public async Task<IActionResult> DeleteInsured(int id)
        {
            ResponseJson response = await _insuredService.DeleteInsuredAsync(id);

            if (response.Error) return BadRequest(response);

            return Ok(response);
        }

        [HttpGet(ApiRoutes.ApiRoutes.Insured.GetAllInsurancesByInsured)]
        public async Task<IActionResult> GetInsuranceByInsuredIdentification(string identification)
        {
            ResponseJson response = await _insuredService.GetAllInsuranceByInsuredAsync(identification);

            if (response.Error) return BadRequest(response);

            return Ok(response);
        }
    }
}

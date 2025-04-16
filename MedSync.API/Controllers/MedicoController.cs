using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using static MedSync.Application.Requests.MedicoResquest;

namespace MedSync.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicoController : ControllerBase
    {
        private Response _response = new();
        private IMedicoService _medicoService;
        public MedicoController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> CreateAsync(AdicionarMedicoRequest medicoRequest)
        {
            _response = await _medicoService.CreateAsync(medicoRequest);
           return _response.Error ? BadRequest(_response) : Ok(_response);
        }
        
        [HttpGet("getall/{page}/{pageSize}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetAllAsync(int page, int pageSize)
        {
            var medicos = await _medicoService.GetAllAsync(page, pageSize);
           return !medicos.Itens.Any() ? NoContent() : Ok(medicos);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetIdAsync(Guid id)
        {
            var medico = await _medicoService.GetIdAsync(id);
           return medico == null ? NoContent() : Ok(medico);
        }
        
        [HttpGet("crm/{crm}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetCRMAsync(string crm)
        {
            var medico = await _medicoService.GetCRMAsync(crm);
            return medico == null ? NoContent() : Ok(medico);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> UpdateAsync(AtualizarMedicoRequest medicoRequest)
        {
            _response = await _medicoService.UpdateAsync(medicoRequest);
           return _response.Error ? BadRequest(_response) : Ok(_response);

        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            _response = await _medicoService.DeleteAsync(id);
           return _response.Error ? BadRequest(_response) : Ok(_response);
        }
    }
}

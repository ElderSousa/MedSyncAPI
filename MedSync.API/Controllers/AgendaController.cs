using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using MedSync.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using static MedSync.Application.Requests.AgendaRequest;

namespace MedSync.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendaController : ControllerBase
    {
        private Response _response = new();
        private readonly IAgendaSevice _agendaSevice;
        public AgendaController(IAgendaSevice agendamentosSevice)
        {
            _agendaSevice = agendamentosSevice;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> CreateAsync(AdicionarAgendaRequest agendaRequest)
        {
            _response = await _agendaSevice.CreateAsync(agendaRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }

        [HttpGet("getall/{page}/{pageSize}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetAllAsync(int page, int pageSize)
        {
            var agendas = await _agendaSevice.GetAllAsync(page, pageSize);
            return !agendas.Itens.Any() ? NoContent() : Ok(agendas);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetIdAsync(Guid id)
        {
            var agenda = await _agendaSevice.GetIdAsync(id);
            return agenda == null ? NoContent() : Ok(agenda);
        }
  
        [HttpGet("medicoId/{medicoId}/{page}/{pageSize}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetMedicoIdAsync(Guid medicoId, int page, int pageSize)
        {
            var agendas = await _agendaSevice.GetMedicoIdAsync(medicoId, page, pageSize);
            return !agendas.Itens.Any() ? NoContent() : Ok(agendas);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> UpdateAsync(AtualizarAgendaResquet agendaRequest)
        {
            _response = await _agendaSevice.UpdateAsync(agendaRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            _response = await _agendaSevice.DeleteAsync(id);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }
    }
}


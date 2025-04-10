using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using static MedSync.Application.Requests.AgendamentoRequest;

namespace MedSync.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentoController : ControllerBase
    {
        private Response _response = new();
        private readonly IAgendamentoService _agendamentoService;

        public AgendamentoController(IAgendamentoService agendamentoService)
        {
            _agendamentoService = agendamentoService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> CreateAsync(AdicionaAgendamentoRequest agendamentoRequest)
        {
            _response = await _agendamentoService.CreateAsync(agendamentoRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }

        [HttpGet("getall")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetAllAsync()
        {
            var agendamentos = await _agendamentoService.GetAllAsync();
            return !agendamentos.Any() ? NoContent() : Ok(agendamentos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetIdAsync(Guid id)
        {
            var agendamento = await _agendamentoService.GetIdAsync(id);
            return agendamento == null ? NoContent() : Ok(agendamento);
        }

        [HttpGet("agendaId/{agendaId}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetAgendamentoIdAsync(Guid agendaId)
        {
            var agendamentos = await _agendamentoService.GetAgendaIdAsync(agendaId);
            return !agendamentos.Any() ? NoContent() : Ok(agendamentos);
        }

        [HttpGet("medicoId/{medicoId}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetMedicoIdAsync(Guid medicoId)
        {
            var agendamentos = await _agendamentoService.GetMedicoIdAsync(medicoId);
            return !agendamentos.Any() ? NoContent() : Ok(agendamentos);
        }

        [HttpGet("pacienteId/{pacienteId}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetPacienteIdAsync(Guid pacienteId)
        {
            var agendamentos = await _agendamentoService.GetPacienteIdAsync(pacienteId);
            return !agendamentos.Any() ? NoContent() : Ok(agendamentos);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> UpdateAsync(AtualizarAgendamentoRequest agendamentoRequest)
        {
            _response = await _agendamentoService.UpdateAsync(agendamentoRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            _response = await _agendamentoService.DeleteAsync(id);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }
    }
}

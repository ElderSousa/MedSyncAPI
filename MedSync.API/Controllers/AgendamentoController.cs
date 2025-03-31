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
        private readonly IAgendamentoSevice _agendamentosSevice;
        public AgendamentoController(IAgendamentoSevice agendamentosSevice)
        {
            _agendamentosSevice = agendamentosSevice;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> CreateAsync(AdicionarAgendamentoRequest agendamentoRequest)
        {
            _response = await _agendamentosSevice.CreateAsync(agendamentoRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }

        [HttpGet("getall")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetAllAsync()
        {
            var agendamentos = await _agendamentosSevice.GetAllAsync();
            return !agendamentos.Any() ? NoContent() : Ok(agendamentos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetIdAsync(Guid id)
        {
            var agendamento = await _agendamentosSevice.GetIdAsync(id);
            return agendamento == null ? NoContent() : Ok(agendamento);
        }

        [HttpGet("pacienteId/{pacienteId}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetPacienteIdAsync(Guid pacienteId)
        {
            var agendamento = await _agendamentosSevice.GetPacienteIdAsync(pacienteId);
            return agendamento == null ? NoContent() : Ok(agendamento);
        }

        [HttpGet("medicoId/{medicoId}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetMedicoIdAsync(Guid medicoId)
        {
            var agendamento = await _agendamentosSevice.GetMedicoIdAsync(medicoId);
            return agendamento == null ? NoContent() : Ok(agendamento);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> UpdateAsync(AtualizarAgendamentoResquet agendamentoRequest)
        {
            _response = await _agendamentosSevice.UpdateAsync(agendamentoRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            _response = await _agendamentosSevice.DeleteAsync(id);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }
    }
}


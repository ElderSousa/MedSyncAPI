using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
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
        public async Task<IActionResult> CreateAsync(AdicionarAgendaRequest agendamentoRequest)
        {
            _response = await _agendaSevice.CreateAsync(agendamentoRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }

        [HttpGet("getall")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetAllAsync()
        {
            var agendamentos = await _agendaSevice.GetAllAsync();
            return !agendamentos.Any() ? NoContent() : Ok(agendamentos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetIdAsync(Guid id)
        {
            var agendamento = await _agendaSevice.GetIdAsync(id);
            return agendamento == null ? NoContent() : Ok(agendamento);
        }

        [HttpGet("pacienteId/{pacienteId}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetPacienteIdAsync(Guid pacienteId)
        {
            var agendamento = await _agendaSevice.GetPacienteIdAsync(pacienteId);
            return agendamento == null ? NoContent() : Ok(agendamento);
        }

        [HttpGet("medicoId/{medicoId}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetMedicoIdAsync(Guid medicoId)
        {
            var agendamento = await _agendaSevice.GetMedicoIdAsync(medicoId);
            return agendamento == null ? NoContent() : Ok(agendamento);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> UpdateAsync(AtualizarAgendaResquet agendamentoRequest)
        {
            _response = await _agendaSevice.UpdateAsync(agendamentoRequest);
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


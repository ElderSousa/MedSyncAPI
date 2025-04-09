using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using static MedSync.Application.Requests.HorarioRequest;

namespace MedSync.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorarioController : ControllerBase
    {
        private Response _response = new();
        private readonly IHorarioService _horarioService;

        public HorarioController(IHorarioService horarioService)
        {
            _horarioService = horarioService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> CreateAsync(AdicionarHorarioRequest horarioRequest)
        {
            _response = await _horarioService.CreateAsync(horarioRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }

        [HttpGet("getall")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetAllAsync()
        {
            var horarios = await _horarioService.GetAllAsync();
            return !horarios.Any() ? NoContent() : Ok(horarios);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetIdAsync(Guid id)
        {
            var horario = await _horarioService.GetIdAsync(id);
            return horario == null ? NoContent() : Ok(horario);
        }

        [HttpGet("agendaId/{agendaId}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetAgendaIdAsync(Guid agendaId)
        {
            var horarios = await _horarioService.GetAgendaIdAsync(agendaId);
            return !horarios.Any() ? NoContent() : Ok(horarios);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> UpdateAsync(AtualizarHorarioRequest horarioRequest)
        {
            _response = await _horarioService.UpdateAsync(horarioRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            _response = await _horarioService.DeleteAsync(id);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }
    }
}

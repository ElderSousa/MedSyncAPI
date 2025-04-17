using Asp.Versioning;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using static MedSync.Application.Requests.HorarioRequest;

namespace MedSync.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HorarioController : ControllerBase
    {
        private Response _response = new();
        private readonly IHorarioService _horarioService;

        public HorarioController(IHorarioService horarioService)
        {
            _horarioService = horarioService;
        }
        /// <summary>
        /// Cria horário com os dados informados
        /// </summary>
        /// <param name="horarioRequest">Objeto com os dados informados para criação do horário</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> CreateAsync(AdicionarHorarioRequest horarioRequest)
        {
            _response = await _horarioService.CreateAsync(horarioRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }
        /// <summary>
        /// Busca todos os horário da base de dados
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("getall/{page}/{pageSize}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetAllAsync(int page, int pageSize)
        {
            var horarios = await _horarioService.GetAllAsync(page, pageSize);
            return !horarios.Itens.Any() ? NoContent() : Ok(horarios);
        }
        /// <summary>
        /// Busca todos os horãrio em que o agendado é false
        /// </summary>
        /// <param name="page">Número da página</param>
        /// <param name="pageSize">Quantidade e itens na página</param>
        /// <returns></returns>
        [HttpGet("getagendadofalse/{page}/{pageSize}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetAagendadoFalseAsync(int page, int pageSize)
        {
            var horarios = await _horarioService.GetAgendadoFalseAsync(page, pageSize);
            return !horarios.Itens.Any() ? NoContent() : Ok(horarios);
        }
        /// <summary>
        /// Busca o horário pelo id informado
        /// </summary>
        /// <param name="id">Parâmetro informado para busca do horário</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetIdAsync(Guid id)
        {
            var horario = await _horarioService.GetIdAsync(id);
            return horario == null ? NoContent() : Ok(horario);
        }
        /// <summary>
        /// Busca o horário pelo id informado
        /// </summary>
        /// <param name="agendaId">Parâmetro informado para a busca do horário</param>
        /// <param name="page">Número da página</param>
        /// <param name="pageSize">Quantidade e itens na página</param>
        /// <returns></returns>
        [HttpGet("agendaId/{agendaId}/{page}/{pageSize}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetAgendaIdAsync(Guid agendaId, int page, int pageSize)
        {
            var horarios = await _horarioService.GetAgendaIdAsync(agendaId, page, pageSize);
            return !horarios.Itens.Any() ? NoContent() : Ok(horarios);
        }
        /// <summary>
        /// Atualiza horário com os dados informados
        /// </summary>
        /// <param name="horarioRequest">Objeto com dados informados para atualização do horário</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> UpdateAsync(AtualizarHorarioRequest horarioRequest)
        {
            _response = await _horarioService.UpdateAsync(horarioRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);

        }
        /// <summary>
        /// Atualização do status do horário conforme parâmetros informados
        /// </summary>
        /// <param name="id">Parâmetro informado para atualização do status</param>
        /// <param name="agendado">Parâmetro informado para atualização do status</param>
        /// <returns></returns>
        [HttpPut("status/{id}/{agendado}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> UpdateStatusAsync(Guid id, bool agendado)
        {
            _response = await _horarioService.UpdateStatusAsync(id, agendado);
            return _response.Error ? BadRequest(_response) : Ok(_response);

        }
        /// <summary>
        /// Deleta horário do id informado
        /// </summary>
        /// <param name="id">Parâmetro informado para excluir horário</param>
        /// <returns></returns>
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

using Asp.Versioning;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using static MedSync.Application.Requests.AgendamentoRequest;

namespace MedSync.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AgendamentoController : ControllerBase
    {
        private Response _response = new();
        private readonly IAgendamentoService _agendamentoService;

        public AgendamentoController(IAgendamentoService agendamentoService)
        {
            _agendamentoService = agendamentoService;
        }
        /// <summary>
        /// Cria agendamento com os dados informados
        /// </summary>
        /// <param name="agendamentoRequest">Objeto com os dados para criação da agenda</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> CreateAsync(AdicionaAgendamentoRequest agendamentoRequest)
        {
            _response = await _agendamentoService.CreateAsync(agendamentoRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }
        /// <summary>
        /// Traz todos os agendamentos da base de dados
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("getall/{page}/{pageSize}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetAllAsync(int page, int pageSize)
        {
            var agendamentos = await _agendamentoService.GetAllAsync(page, pageSize);
            return !agendamentos.Itens.Any() ? NoContent() : Ok(agendamentos);
        }
        /// <summary>
        /// Busca o agendamento pelo id informado
        /// </summary>
        /// <param name="id">Parâmetro informado para a busca do agendamento</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetIdAsync(Guid id)
        {
            var agendamento = await _agendamentoService.GetIdAsync(id);
            return agendamento == null ? NoContent() : Ok(agendamento);
        }
        /// <summary>
        /// Busca o agendamento pelo id informado
        /// </summary>
        /// <param name="agendaId"></param>
        /// <param name="page">Número da página</param>
        /// <param name="pageSize">Quantidade e itens na página</param>
        /// <returns></returns>
        [HttpGet("agendaId/{agendaId}/{page}/{pageSize}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetAgendamentoIdAsync(Guid agendaId, int page, int pageSize)
        {
            var agendamentos = await _agendamentoService.GetAgendaIdAsync(agendaId, page, pageSize);
            return !agendamentos.Itens.Any() ? NoContent() : Ok(agendamentos);
        }
        /// <summary>
        /// Busca o agendamento pelo id informado
        /// </summary>
        /// <param name="medicoId"></param>
        /// <param name="page">Número da página</param>
        /// <param name="pageSize">Quantidade e itens na página</param>
        /// <returns></returns>
        [HttpGet("medicoId/{medicoId}/{page}/{pageSize}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetMedicoIdAsync(Guid medicoId, int page, int pageSize)
        {
            var agendamentos = await _agendamentoService.GetMedicoIdAsync(medicoId, page, pageSize);
            return !agendamentos.Itens.Any() ? NoContent() : Ok(agendamentos);
        }
        /// <summary>
        /// Busca o agendamento pelo id informado
        /// </summary>
        /// <param name="pacienteId">Parâmetro informado para a busca do agendamento</param>
        /// <param name="page">Número da página</param>
        /// <param name="pageSize">Quantidade e itens na página</param>
        /// <returns></returns>
        [HttpGet("pacienteId/{pacienteId}/{page}/{pageSize}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetPacienteIdAsync(Guid pacienteId, int page, int pageSize)
        {
            var agendamentos = await _agendamentoService.GetPacienteIdAsync(pacienteId, page, pageSize);
            return !agendamentos.Itens.Any() ? NoContent() : Ok(agendamentos);
        }
        /// <summary>
        /// Atualiza o agendamento informado
        /// </summary>
        /// <param name="agendamentoRequest">Objeto com os dados informados para atualizar o agendamento</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> UpdateAsync(AtualizarAgendamentoRequest agendamentoRequest)
        {
            _response = await _agendamentoService.UpdateAsync(agendamentoRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);

        }
        /// <summary>
        /// Exclui o agendamento pelo id informado
        /// </summary>
        /// <param name="id">Parâmetro informado para a exclusão do agendamento</param>
        /// <returns></returns>
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

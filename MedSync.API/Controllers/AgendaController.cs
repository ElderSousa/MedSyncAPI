using Asp.Versioning;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using static MedSync.Application.Requests.AgendaRequest;

namespace MedSync.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AgendaController : ControllerBase
    {
        private Response _response = new();
        private readonly IAgendaSevice _agendaSevice;
        public AgendaController(IAgendaSevice agendaSevice)
        {
            _agendaSevice = agendaSevice;
        }
        /// <summary>
        /// Cria uma agenda com base nos dados fornecidos
        /// </summary>
        /// <param name="agendaRequest">Objeto para criação da agenda</param>
        [HttpPost]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> CreateAsync(AdicionarAgendaRequest agendaRequest)
        {
            _response = await _agendaSevice.CreateAsync(agendaRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }
        /// <summary>
        /// Retorna todas as agendas da base de dados.
        /// </summary>
        /// <param name="page">Número da página</param>
        /// <param name="pageSize">Quantidade e itens na página</param>
        /// <returns></returns>
        [HttpGet("getall/{page}/{pageSize}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetAllAsync(int page, int pageSize)
        {
            var agendas = await _agendaSevice.GetAllAsync(page, pageSize);
            return !agendas.Itens.Any() ? NoContent() : Ok(agendas);
        }
        /// <summary>
        /// Retorna agenda do id informado
        /// </summary>
        /// <param name="id">Parâmetro informado para a busca da agenda</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetIdAsync(Guid id)
        {
            var agenda = await _agendaSevice.GetIdAsync(id);
            return agenda == null ? NoContent() : Ok(agenda);
        }
        /// <summary>
        /// Retorna agenda do id informado
        /// </summary>
        /// <param name="medicoId">Parâmetro informado para a busca da agenda</param>
        /// <param name="page">Número da página</param>
        /// <param name="pageSize">Quantidade e itens na página</param>
        /// <returns></returns>
        [HttpGet("medicoId/{medicoId}/{page}/{pageSize}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetMedicoIdAsync(Guid medicoId, int page, int pageSize)
        {
            var agendas = await _agendaSevice.GetMedicoIdAsync(medicoId, page, pageSize);
            return !agendas.Itens.Any() ? NoContent() : Ok(agendas);
        }
        /// <summary>
        /// Atualiza a agenda informada
        /// </summary>
        /// <param name="agendaRequest">Objetos contendo dados da agenda para atualização</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> UpdateAsync(AtualizarAgendaResquet agendaRequest)
        {
            _response = await _agendaSevice.UpdateAsync(agendaRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);

        }
        /// <summary>
        /// Exclui agenda do id informado
        /// </summary>
        /// <param name="id">Parâmetro informado para a exclusão da agenda</param>
        /// <returns></returns>
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


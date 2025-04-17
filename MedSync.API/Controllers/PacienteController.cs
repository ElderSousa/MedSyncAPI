using Asp.Versioning;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using static MedSync.Application.Requests.PacienteRequest;

namespace MedSync.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PacienteController : ControllerBase
    {
        private Response _response = new();
        private readonly IPacienteService _pacienteService;

        public PacienteController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }
        /// <summary>
        /// Cria paciente com os dados informados
        /// </summary>
        /// <param name="pacienteRequest">Objeto com os dados para criação do paciente</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> CreateAsync(AdicionarPacienteRequest pacienteRequest)
        {
            _response = await _pacienteService.CreateAsync(pacienteRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }
        /// <summary>
        /// Busca todos os pacientes da base de dados
        /// </summary>
        /// <param name="page">Número da página</param>
        /// <param name="pageSize">Quantidade e itens na página</param>
        /// <returns></returns>
        [HttpGet("getall/{page}/{pageSize}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetAllAsync(int page, int pageSize)
        {
            var pacientes = await _pacienteService.GetAllAsync(page, pageSize);
            return !pacientes.Itens.Any() ? NoContent() : Ok(pacientes);
        }
        /// <summary>
        /// Busca médico do id informado
        /// </summary>
        /// <param name="id">Parâmetro informado para busca do paciente</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetIdAsync(Guid id)
        {
            var paciente = await _pacienteService.GetIdAsync(id);
            return paciente == null ? NoContent() : Ok(paciente);
        }
        /// <summary>
        /// Atualiza paciente com dados informados
        /// </summary>
        /// <param name="pacienteRequest">Objeto com dados para atualização do paciente</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> UpdateAsync(AtualizarPacienteRequest pacienteRequest)
        {
            _response = await _pacienteService.UpdateAsync(pacienteRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);

        }
        /// <summary>
        /// Exclusão do paciente do id informado
        /// </summary>
        /// <param name="id">Parâmetro informado para exclusão do paciente</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            _response = await _pacienteService.DeleteAsync(id);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }
    }
}


using Asp.Versioning;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using static MedSync.Application.Requests.MedicoResquest;

namespace MedSync.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MedicoController : ControllerBase
    {
        private Response _response = new();
        private IMedicoService _medicoService;
        public MedicoController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }
        /// <summary>
        /// Cria médico com os dados informados 
        /// </summary>
        /// <param name="medicoRequest">Objeto com dados para criação do médico</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> CreateAsync(AdicionarMedicoRequest medicoRequest)
        {
            _response = await _medicoService.CreateAsync(medicoRequest);
           return _response.Error ? BadRequest(_response) : Ok(_response);
        }
        /// <summary>
        /// Busca todos os médicos da base de dados
        /// </summary>
        /// <param name="page">Número da página</param>
        /// <param name="pageSize">Quantidade e itens na página</param>
        /// <returns></returns>
        [HttpGet("getall/{page}/{pageSize}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetAllAsync(int page, int pageSize)
        {
            var medicos = await _medicoService.GetAllAsync(page, pageSize);
           return !medicos.Itens.Any() ? NoContent() : Ok(medicos);
        }
        /// <summary>
        /// Busca médico do id informado
        /// </summary>
        /// <param name="id">Parâmetro informado para busca do médico</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetIdAsync(Guid id)
        {
            var medico = await _medicoService.GetIdAsync(id);
           return medico == null ? NoContent() : Ok(medico);
        }
        /// <summary>
        /// Busca médico pelo crm informado
        /// </summary>
        /// <param name="crm">Parâmetro informado para busca do médico</param>
        /// <returns></returns>
        [HttpGet("crm/{crm}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 204)]
        public async Task<IActionResult> GetCRMAsync(string crm)
        {
            var medico = await _medicoService.GetCRMAsync(crm);
            return medico == null ? NoContent() : Ok(medico);
        }
        /// <summary>
        /// Atualiza o médico informado
        /// </summary>
        /// <param name="medicoRequest">Objeto com os dados para atualização do médico</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> UpdateAsync(AtualizarMedicoRequest medicoRequest)
        {
            _response = await _medicoService.UpdateAsync(medicoRequest);
           return _response.Error ? BadRequest(_response) : Ok(_response);

        }
        /// <summary>
        /// Exclui médico do id informado
        /// </summary>
        /// <param name="id">Parâmetro informado para exclusão do médico</param>
        /// <returns></returns>
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

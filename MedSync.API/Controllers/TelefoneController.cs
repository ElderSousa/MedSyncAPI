using Asp.Versioning;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using static MedSync.Application.Requests.TelefoneRequest;

namespace MedSync.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TelefoneController : ControllerBase
    {
        private Response _response = new();
        private readonly ITelefoneService _telefoneService;

        public TelefoneController(ITelefoneService telefoneService)
        {
            _telefoneService = telefoneService;
        }
        /// <summary>
        /// Cria telefone com os dados informados
        /// </summary>
        /// <param name="telefoneRequest">Objeto com os dados para a criação do telefone</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(AdicionarTelefoneRequest telefoneRequest)
        {
            _response = await _telefoneService.CreateAsync(telefoneRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }

        /// <summary>
        /// Busca telefone pelo id informado
        /// </summary>
        /// <param name="page">Número da página</param>
        /// <param name="pageSize">Quantidade e itens na página</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(PessoaResponse), 200)]
        [ProducesResponseType(typeof(PessoaResponse), 400)]
        [HttpGet("{page}/{pageSize}")]
        public async Task<IActionResult> GetAllAsync(int  page, int pageSize)
        {
            var telefones = await _telefoneService.GetAllAsync(page, pageSize);
            return !telefones.Itens.Any() ? NoContent() : Ok(telefones);
        }

        /// <summary>
        /// Busca telefone pelo id informado
        /// </summary>
        /// <param name="id">Parâmetro informado para a busca do telefone</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(PessoaResponse), 200)]
        [ProducesResponseType(typeof(PessoaResponse), 400)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIdAsync(Guid id)
        {
            var telefone = await _telefoneService.GetIdAsync(id);
            return telefone is null ? NoContent() : Ok(telefone);
        }

        /// <summary>
        /// Busca telefone pelo id informado
        /// </summary>
        /// <param name="medicoId">Parâmetro informado para a busca do telefone</param>
        /// <param name="page">Número da página</param>
        /// <param name="pageSize">Quantidade e itens na página</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(PessoaResponse), 200)]
        [ProducesResponseType(typeof(PessoaResponse), 400)]
        [HttpGet("medico/{medicoId}/{page}/{pageSize}")]
        public async Task<IActionResult> GetMedicoIdAsync(Guid medicoId, int page, int pageSize)
        {
            var telefones = await _telefoneService.GetMedicoIdAsync(medicoId, page, pageSize);
            return !telefones.Itens.Any() ? NoContent() : Ok(telefones);
        } 
        
        /// <summary>
        /// Busca telefone pelo id informado
        /// </summary>
        /// <param name="pacienteId">Parâmetro informado para a busca do telefone</param>
        /// <param name="page">Número da página</param>
        /// <param name="pageSize">Quantidade e itens na página</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(PessoaResponse), 200)]
        [ProducesResponseType(typeof(PessoaResponse), 400)]
        [HttpGet("paciente/{pacienteId}/{page}/{pageSize}")]
        public async Task<IActionResult> GetPacienteIdAsync(Guid pacienteId, int page, int pageSize)
        {
            var telefones = await _telefoneService.GetPacienteIdAsync(pacienteId, page, pageSize);
            return !telefones.Itens.Any() ? NoContent() : Ok(telefones);
        }

        /// <summary>
        /// Busca telefone pelo número informado
        /// </summary>
        /// <param name="numero">Parâmetro informado para a busca do telefone</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(PessoaResponse), 200)]
        [ProducesResponseType(typeof(PessoaResponse), 400)]
        [HttpGet("numero/{numero}")]
        public async Task<IActionResult> GetNumeroAsync(string numero)
        {
            var telefone = await _telefoneService.GetNumeroAsync(numero);
            return telefone is null ? NoContent() : Ok(telefone);
        }
        /// <summary>
        /// Atualiza telefone com os dados informados
        /// </summary>
        /// <param name="telefoneRequest">Objeto com dados para a criação do telefone</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(AtualizarTelefoneRequest telefoneRequest)
        {
            _response = await _telefoneService.UpdateAsync(telefoneRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }
        /// <summary>
        /// Exclusão do telefone do id informado
        /// </summary>
        /// <param name="id">Parâmetro informado para a exclusão do telefone</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            _response = await _telefoneService.DeleteAsync(id);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }
    }
}

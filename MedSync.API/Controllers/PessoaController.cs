using Asp.Versioning;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using static MedSync.Application.Requests.PessoaRequest;

namespace MedSync.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PessoaController : ControllerBase
    {
        private Response _response = new();
        private readonly IPessoaService _pessoaService;
        public PessoaController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }
        /// <summary>
        /// Cria paciente com os dados informados
        /// </summary>
        /// <param name="pessoa">Objeto com dados para criação da pessoa</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(AdicionarPessoaRequest pessoa)
        {
            _response = await _pessoaService.CreateAsync(pessoa);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }
        /// <summary>
        /// Busca todas as pessoas da base de dados
        /// </summary>
        /// <param name="id">Parâmetro informado para a busca da pessoa</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(PessoaResponse), 200)]
        [ProducesResponseType(typeof(PessoaResponse), 400)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIdAsync(Guid id)
        {
            var pessoa = await _pessoaService.GetIdAsync(id);
            return pessoa is null ? NoContent() : Ok(pessoa);
        }
        /// <summary>
        /// Busca de pessoa pelo cpf informado
        /// </summary>
        /// <param name="cpf">Parãmetro informado para busca da pessoa</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(PessoaResponse), 200)]
        [ProducesResponseType(typeof(PessoaResponse), 400)]
        [HttpGet("cpf/{cpf}")]
        public async Task<IActionResult> GetCPFAsync(string cpf)
        {
            var pessoa = await _pessoaService.GetCPFAsync(cpf);
            return pessoa is null ? NoContent() : Ok(pessoa);
        }
        /// <summary>
        /// Atualiza a pessoa com os dados informados
        /// </summary>
        /// <param name="pessoa">Objeto com dados para atualização da pessoa</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(AtualizarPessoaRequest pessoa)
        {
            _response = await _pessoaService.UpdateAsync(pessoa);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }
        /// <summary>
        /// Exclui pessoa do id informado
        /// </summary>
        /// <param name="id">Parâmetro informado para exclusão da pessoa</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            _response = await _pessoaService.DeleteAsync(id);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }
    }
}

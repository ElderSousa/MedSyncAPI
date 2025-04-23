
using Asp.Versioning;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using static MedSync.Application.Requests.EnderecoRequest;

namespace MedSync.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EnderecoController : ControllerBase
    {
        private Response _response = new();
        private readonly IEnderecoService _enderecoService;
        public EnderecoController(IEnderecoService enderecoService)
        {
            _enderecoService = enderecoService;
        }
        /// <summary>
        /// Cria o endereço com os dados informados
        /// </summary>
        /// <param name="enderecoRequest">Objeto com os dados informados para a criação do endereço</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(AdicionarEnderecoRequest enderecoRequest)
        {
            _response = await _enderecoService.CreateAsync(enderecoRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }
        /// <summary>
        /// Busca o endereço pelo id informado
        /// </summary>
        /// <param name="id">Parâmetro informado para a busca do endereço</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(PessoaResponse), 200)]
        [ProducesResponseType(typeof(PessoaResponse), 400)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIdAsync(Guid id)
        {
            var endereco = await _enderecoService.GetIdAsync(id);
            return endereco is null ? NoContent() : Ok(endereco);
        }
        /// <summary>
        /// Busca o endereço pelo cep informado
        /// </summary>
        /// <param name="cep">Parâmetro informado para busca do endereço</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(PessoaResponse), 200)]
        [ProducesResponseType(typeof(PessoaResponse), 400)]
        [HttpGet("cep/{cep}")]
        public async Task<IActionResult> GetCEPAsync(string cep)
        {
            var endereco = await _enderecoService.GetCEPAsync(cep);
            return endereco is null ? NoContent() : Ok(endereco);
        }
        /// <summary>
        /// Atualiza o endereço com os dados informados
        /// </summary>
        /// <param name="enderecoRequest">Parâmetro informado para busca do endereço</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(AtualizarEnderecoRequest enderecoRequest)
        {
            _response = await _enderecoService.UpdateAsync(enderecoRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }
        /// <summary>
        /// Exclui o endereço do id informado
        /// </summary>
        /// <param name="id">Parametro informado para a exclusão do endereço</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            _response = await _enderecoService.DeleteAsync(id);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }
    }
}

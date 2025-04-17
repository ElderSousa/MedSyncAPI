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

        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(AdicionarTelefoneRequest telefoneRequest)
        {
            _response = await _telefoneService.CreateAsync(telefoneRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }

        [ProducesResponseType(typeof(PessoaResponse), 200)]
        [ProducesResponseType(typeof(PessoaResponse), 400)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIdAsync(Guid id)
        {
            var telefone = await _telefoneService.GetIdAsync(id);
            return telefone is null ? NoContent() : Ok(telefone);
        }

        [ProducesResponseType(typeof(PessoaResponse), 200)]
        [ProducesResponseType(typeof(PessoaResponse), 400)]
        [HttpGet("numero/{numero}")]
        public async Task<IActionResult> GetNumeroAsync(string numero)
        {
            var endereco = await _telefoneService.GetNumeroAsync(numero);
            return endereco is null ? NoContent() : Ok(endereco);
        }

        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(AtualizarTelefoneRequest telefoneRequest)
        {
            _response = await _telefoneService.UpdateAsync(telefoneRequest);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }

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

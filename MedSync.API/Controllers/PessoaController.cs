using System.Data.Common;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using static MedSync.Application.Requests.PessoaRequest;

namespace MedSync.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaController : ControllerBase
    {
        private Response _response = new();
        private readonly IPessoaService _pessoaService;
        public PessoaController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(AdicionarPessoaRequest pessoa)
        {
            _response = await _pessoaService.CreateAsync(pessoa);
            return _response.Error ? BadRequest(_response) : Ok(_response);
        }

        [ProducesResponseType(typeof(PessoaResponse), 200)]
        [ProducesResponseType(typeof(PessoaResponse), 400)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIdAsync(Guid id)
        {
            var pessoa = await _pessoaService.GetIdAsync(id);
            return pessoa is null ? BadRequest(StatusCodes.Status204NoContent) : Ok(pessoa);
        }

        [ProducesResponseType(typeof(PessoaResponse), 200)]
        [ProducesResponseType(typeof(PessoaResponse), 400)]
        [HttpGet("cpf/{cpf}")]
        public async Task<IActionResult> GetCPFAsync(string cpf)
        {
            try
            {
                var pessoa = await _pessoaService.GetCPFAsync(cpf);
                return pessoa is null ? NoContent() : Ok(pessoa);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(AtualizarPessoaRequest pessoa)
        {
            try
            {
                _response = await _pessoaService.UpdateAsync(pessoa);
                return _response.Error ? BadRequest(_response) : Ok(_response);
            }
            catch (DbException ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

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

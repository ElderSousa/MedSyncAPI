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
            try
            {
                _response = await _pessoaService.CreateAsync(pessoa);
                return _response.Error ? BadRequest(_response) : Ok(_response);
            }
            catch (Exception ex)
            {
                return BadRequest(_response.GerarErro(ex.Message, true));
            }
        }
    }
}

using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using MedSync.Application.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MedSync.Application.Services
{
    public class BaseService
    {
        protected readonly IMapper mapper;
        private HttpContext? _context;
        protected readonly ILogger logger;
        public BaseService(IMapper mapper, IHttpContextAccessor httpContextAccessor, ILogger logger)
        {
            this.mapper = mapper;
            _context = httpContextAccessor.HttpContext;
            this.logger = logger;
        }

        protected static Response ReturnResponse(string status, bool error)
        {
            Response response = new()
            {
                Status = status,
                Error = error,
            };

            return response;
        }

        protected static Response ReturnResponseSuccess()
        {
            Response response = new()
            {
                Status = "Sucesso",
                Error = false
            };

            return response;
        }

        protected Response ExecultarValidacaoResponse<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE>
        {
            var validator = validacao.Validate(entidade);
            if (validator.IsValid) return ReturnResponseSuccess();
            return ReturnResponse(validator.ToString(), true);
        }

        protected Guid ObterUsuarioLogadoId()
        {
            try
            {
                var identity = _context?.User.Identity as ClaimsIdentity;
                var usuarioId = identity?.FindFirst("UserId")?.Value;
                return usuarioId != null ? Guid.Parse(usuarioId) : Guid.Empty;
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }

        protected static DateTime DataHoraAtual() => DateTime.UtcNow.AddHours(-3);

        private static string ObterErro(Exception exception) => exception.InnerException is not null ? exception.InnerException.Message : exception.Message;

    }
}

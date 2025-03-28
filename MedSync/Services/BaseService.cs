using AutoMapper;
using FluentValidation;
using MedSync.Application.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MedSync.Application.Services
{
    public class BaseService
    {
        protected IMapper mapper;
        private HttpContext? _context;
        protected ILogger logger;
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

        protected static Response RetunrResponse(Exception exception)
        {
            Response response = new()
            {
                Status = ObterErro(exception),
                Error = true
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

        protected static string Notificar(string mensagem) => mensagem;

        protected static string Notificar(Exception exception) => ObterErro(exception);

        protected static DateTime DataHoraAtual() => DateTime.UtcNow.AddHours(-3);

        private static string ObterErro(Exception exception) => exception.InnerException is not null ? exception.InnerException.Message : exception.Message;

    }
}

using AutoMapper;
using FluentValidation;
using MedSync.Application.Responses;
using Microsoft.AspNetCore.Http;

namespace MedSync.Application.Services
{
    public class BaseService
    {
        public IMapper mapper;
        private HttpContext? _context;
        public BaseService(IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.mapper = mapper;
            _context = httpContextAccessor.HttpContext;
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

        protected static DateTime DataHoraAtual() => DateTime.UtcNow.AddHours(-3);

        private static string ObterErro(Exception exception) => exception.InnerException is not null ? exception.InnerException.Message : exception.Message;
    }
}

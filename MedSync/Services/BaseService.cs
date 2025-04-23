using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using MedSync.Application.PaginationModel;
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

        protected async Task<Response> ExecultarValidacaoResponse<T>(IValidator<T> validator, T entidade)
        {
            var result = await validator.ValidateAsync(entidade);
            return result.IsValid ? ReturnResponseSuccess() :
            ReturnResponse(result.ToString()!, true);
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

        protected static Pagination<T> Paginar<T>(IEnumerable<T> itens, int page, int pageSize)
        {
            var quantityOfPages = (int)Math.Ceiling((double)itens.Count() / pageSize);

            var lista = itens
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
   ;

            return new Pagination<T>
            {
                QuantityOfPages = quantityOfPages,
                TotalItens = itens.Count(),
                CurrentPage = page,
                Itens = lista
            };
        }
    }
}

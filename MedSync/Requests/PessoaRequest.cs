using MedSync.Application.DTOs;

namespace MedSync.Application.Requests
{
    public class PessoaRequest
    {
        public class AdicionarPessoaRequest : BaseModel
        {
            public string? Nome { get; set; }
            public string? CPF { get; set; }
            public string? RG { get; set; }
            public string? Email { get; set; }
            public char Sexo { get; set; }
            public DateOnly? DataNascimento { get; set; }
        }
    }
}

using System.Security.Principal;

namespace MedSync.Application.Requests;

public class PessoaRequest
{
    public class AdicionarPessoaRequest :PessoaRequest
    {
        public string? Nome { get; set; }
        public string? CPF { get; set; }
        public string? RG { get; set; }
        public string? Email { get; set; }

    }

    public class AtualizarPessoaRequest : PessoaRequest
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? CPF { get; set; }
        public string? RG { get; set; }
        public string? Email { get; set; }
        
    }

    private string? _sexo;
    public string? Sexo
    {
        get => _sexo?.ToUpper();
        set => _sexo = value;
    }

    private DateTime? _dataNascimento;

    public DateTime? DataNascimento
    {
        get => _dataNascimento?.Date;
        set => _dataNascimento = value;
    }
}

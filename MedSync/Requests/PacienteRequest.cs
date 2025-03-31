using System.Runtime;
using static MedSync.Application.Requests.EnderecoRequest;
using static MedSync.Application.Requests.PessoaRequest;
using static MedSync.Application.Requests.TelefoneRequest;

namespace MedSync.Application.Requests;

public class PacienteRequest
{
    public class AdicionarPacienteRequest
    {
        public AdicionarEnderecoRequest Endereco { get; set; } = new();
        public AdicionarPessoaRequest Pessoa { get; set; } = new();
        public List<AdicionarTelefoneRequest> Telefones { get; set; } = new();
    }

    public class AtualizarPacienteRequest
    {
        public Guid Id { get; set; }
        public Guid? PessoaId { get; set; }
        public AtualizarEnderecoRequest Endereco { get; set; } = new();
        public AtualizarPessoaRequest Pessoa { get; set; } = new();
        public List<AtualizarTelefoneRequest> Telefones { get; set; } = new();
    }
}

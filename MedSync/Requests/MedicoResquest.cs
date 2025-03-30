using MedSync.Domain.Enum;
using static MedSync.Application.Requests.PessoaRequest;
using static MedSync.Application.Requests.TelefoneRequest;

namespace MedSync.Application.Requests;

public class MedicoResquest
{
    public class AdicionarMedicoRequest
    {
        public string? CRM { get; set; }
        public Especialidade Especialidade { get; set; }

        public AdicionarPessoaRequest Pessoa { get; set; } = new();
        public List<AdicionarTelefoneRequest> Telefones { get; set; } = new();
    }

    public class AtualizarMedicoRequest
    {
        public Guid PessoaId { get; set; }
        public string? CRM { get; set; }
        public Especialidade Especialidade { get; set; }

        public AtualizarPessoaRequest Pessoa { get; set; } = new();
        public List<AtualizarTelefoneRequest> Telefones { get; set; } = new();
    }
}

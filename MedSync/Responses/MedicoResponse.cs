using MedSync.Domain.Enum;

namespace MedSync.Application.Responses;

public class MedicoResponse
{
    public Guid Id { get; set; }
    public string? CRM { get; set; }
    public MedicoEspecialidade Especialidade { get; set; }
    public Guid? CriadoPor { get; set; }
    public DateTime? CriadoEm { get; set; }
    public Guid? ModificadoPor { get; set; }
    public DateTime? ModificadoEm { get; set; }

    public PessoaResponse Pessoa { get; set; } = new();
    public List<TelefoneResponse> Telefones { get; set; } = new();
}

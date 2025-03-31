namespace MedSync.Application.Responses;

public class PacienteResponse
{
    public Guid Id { get; private set; }
    public Guid? CriadoPor { get; private set; }
    public DateTime? CriadoEm { get; private set; }
    public Guid? ModificadoPor { get; private set; }
    public DateTime? ModificadoEm { get; private set; }

    public PessoaResponse Pessoa { get; set; } = new();
    public EnderecoResponse Endereco { get; set; } =  new();
    public List<TelefoneResponse> Telefones { get; set; } = new();
}

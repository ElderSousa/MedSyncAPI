namespace MedSync.Application.Responses;

public class EnderecoResponse
{
    public Guid Id { get; set; }
    public Guid? PacienteId { get; set; }
    public Guid? MedicoId { get; set; }
    public string? Logradouro { get; set; }
    public int Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? CEP { get; set; }
    public Guid? CriadoPor { get; private set; }
    public DateTime? CriadoEm { get; private set; }
    public Guid? ModificadoPor { get; private set; }
    public DateTime? ModificadoEm { get; private set; }
}

namespace MedSync.Application.Responses;

public class PessoaResponse
{
    public Guid Id { get; private set; }
    public string? Nome { get; set; }
    public string? CPF { get; set; }
    public string? RG { get; set; }
    public string? Email { get; set; }
    public char? Sexo { get; set; }
    public DateTime DataNascimento { get; set; }
    public Guid? CriadoPor { get; private set; }
    public DateTime? CriadoEm { get; private set; }
    public Guid? ModificadoPor { get; private set; }
    public DateTime? ModificadoEm { get; private set; }
}

namespace MedSync.Domain.Entities;

public class Pessoa
{
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public string? CPF { get; set; }
    public string? RG { get; set; }
    public string? Email { get; set; }
    public char Sexo { get; set; }
    public DateOnly? DataNascimento { get; set; }
    public DateTime? CriadoEm { get; set; }
    public DateTime? ModificadoEm { get; set; }
    public Guid? CriadoPor { get; set; }
    public Guid? ModificadoPor { get; set; }
}

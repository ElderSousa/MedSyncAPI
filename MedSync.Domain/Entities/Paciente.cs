namespace MedSync.Domain.Entities;

public class Paciente : BaseModel
{
    public Guid? PessoaId { get; set; }

    public Pessoa Pessoa { get; set; } = new();
    public Endereco Endereco { get; set; } = new();
    public List<Telefone> Telefones { get; set; } = new();
}

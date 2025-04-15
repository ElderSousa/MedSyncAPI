using System.ComponentModel.DataAnnotations.Schema;

namespace MedSync.Domain.Entities;

public class Pessoa : BaseModel
{
    public string? Nome { get; set; }
    public string? CPF { get; set; }
    public string? RG { get; set; }
    public string? Email { get; set; }
    public string? Sexo { get; set; }
    public DateTime? DataNascimento { get; set; }

    [NotMapped]
    public bool ValidacaoCadastrar { get; set; }
}




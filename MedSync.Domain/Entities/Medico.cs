using System.ComponentModel.DataAnnotations.Schema;
using MedSync.Domain.Enum;

namespace MedSync.Domain.Entities
{
    public class Medico : BaseModel
    {
        public Guid? PessoaId { get; set; }
        public string? CRM { get; set; }
        public MedicoEspecialidade Especialidade { get; set; }

        public Pessoa Pessoa { get; set; } = new();
        public List<Telefone> Telefones { get; set; } = new();

        [NotMapped]
        public bool ValidacaoCadastrar { get; set; }
    }
}


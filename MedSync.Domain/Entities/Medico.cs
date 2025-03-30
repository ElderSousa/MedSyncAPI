using MedSync.Domain.Enum;

namespace MedSync.Domain.Entities
{
    public class Medico : BaseModel
    {
        public Guid PessoaId { get; set; }
        public string? CRM { get; set; }
        public Especialidade Especialidade { get; set; }

        public Pessoa Pessoa { get; set; } = new();
        public List<Telefone> Telefones { get; set; } = new();
    }
}

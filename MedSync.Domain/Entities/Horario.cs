using System.ComponentModel.DataAnnotations.Schema;

namespace MedSync.Domain.Entities;

public class Horario : BaseModel
{
    public Guid AgendaId { get; set; }
    public TimeSpan Hora { get; set; }
    public bool Agendado { get; set; }

    [NotMapped]
    public bool ValidacaoCadastrar { get; set; }
}


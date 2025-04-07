using MedSync.Domain.Enum;

namespace MedSync.Domain.Entities;

public class Agendamento : BaseModel
{
    public Guid AgendaId { get; set; }
    public Guid MedicoId { get; set; }
    public DiaSemana DiaSemana { get; set; }
    public DateTime DataHora { get; set; }
}

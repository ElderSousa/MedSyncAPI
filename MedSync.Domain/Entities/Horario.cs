namespace MedSync.Domain.Entities;

public class Horario : BaseModel
{
    public Guid AgendaId { get; set; }
    public TimeSpan HorarioInicial { get; set; }
    public TimeSpan HorarioFinal { get; set; }
}

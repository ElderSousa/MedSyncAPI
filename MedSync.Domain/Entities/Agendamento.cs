using MedSync.Domain.Enum;

namespace MedSync.Domain.Entities;

public class Agendamento : BaseModel
{
    public Guid AgendaId { get; set; }
    public Guid MedicoId { get; set; }
    public Guid PacienteId { get; set; }
    public AgendamentoTipo Tipo { get; set; }
    public AgendamentoStatus status { get; set; }
    public DayOfWeek DiaSemana { get; set; }
    public DateTime AgendadoPara { get; set; }
    public TimeSpan Horario { get; set; }

    public Medico Medico { get; set; } = new();
    public Paciente Paciente { get; set; } = new();
}

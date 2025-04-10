using MedSync.Domain.Entities;
using MedSync.Domain.Enum;

namespace MedSync.Application.Responses;

public class AgendamentoResponse
{
    public Guid Id { get; private set; }
    public Guid AgendaId { get; set; }
    public Guid MedicoId { get; set; }
    public Guid PacienteId { get; set; }
    public AgendamentoTipo Tipo { get; set; }
    public AgendamentoStatus status { get; set; }
    public DayOfWeek DiaSemana { get; set; }
    public DateTime AgendadoPara { get; set; }
    public Guid? CriadoPor { get; private set; }
    public DateTime? CriadoEm { get; private set; }
    public Guid? ModificadoPor { get; private set; }
    public DateTime? ModificadoEm { get; private set; }

    public PacienteResponse Paciente { get; set; } = new();
    public Agenda Agenda { get; set; } = new();
}

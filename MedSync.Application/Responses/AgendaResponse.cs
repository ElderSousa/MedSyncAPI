using MedSync.Domain.Entities;

namespace MedSync.Application.Responses;

public class AgendaResponse
{
    public Guid Id { get; set; }
    public Guid MedicoId { get; set; }
    public bool Agendado { get; set; }
    public DayOfWeek DiaSemana { get; set; }
    public DateTime DataDisponivel { get; set; }
    public Guid? CriadoPor { get; private set; }
    public DateTime? CriadoEm { get; private set; }
    public Guid? ModificadoPor { get; private set; }
    public DateTime? ModificadoEm { get; private set; }

    public MedicoResponse Medico { get; set; } = new();
    public List<HorarioResponse> Horarios { get; set; } = new();
}

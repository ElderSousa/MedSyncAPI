using MedSync.Domain.Entities;
using MedSync.Domain.Enum;

namespace MedSync.Application.Responses;

public class AgendaResponse
{
    public Guid Id { get; private set; }
    public Guid PacienteId { get; set; }
    public Guid MedicoId { get; set; }
    public string? Observacao { get; set; }
    public DateTime AgendadoPara { get; set; }
    public AgendamentoTipo TipoAgendamento { get; set; }
    public AgendamentoStatus Status { get; set; }
    public Guid? CriadoPor { get; private set; }
    public DateTime? CriadoEm { get; private set; }
    public Guid? ModificadoPor { get; private set; }
    public DateTime? ModificadoEm { get; private set; }

    public PacienteResponse Paciente { get; set; } = new();
    public MedicoResponse Medico { get; set; } = new();
}

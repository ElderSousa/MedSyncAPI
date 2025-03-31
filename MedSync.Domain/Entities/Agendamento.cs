using MedSync.Domain.Enum;

namespace MedSync.Domain.Entities;

public class Agendamento : BaseModel
{
    public Guid PacienteId { get; set; }
    public Guid MedicoId { get; set; }
    public string ? Observacao { get; set; }
    public DateTime AgendadoPara { get; set; }
    public AgendamentoTipo Tipo { get; set; }
    public AgendamentoStatus Status { get; set; }

    public Paciente Paciente { get; set; } = new();
    public Medico Medico { get; set; } = new();
}

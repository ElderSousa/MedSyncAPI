using MedSync.Domain.Enum;

namespace MedSync.Application.Requests;

public class AgendaRequest
{
    public class AdicionarAgendaRequest
    {
        public Guid PacienteId { get; set; }
        public Guid MedicoId { get; set; }
        public string? Observacao { get; set; }
        public DateTime AgendadoPara { get; set; }
        public AgendamentoTipo TipoAgendamento { get; set; }
        public AgendamentoStatus Status { get; set; }

    }

    public class AtualizarAgendaResquet
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public Guid MedicoId { get; set; }
        public string? Observacao { get; set; }
        public DateTime AgendadoPara { get; set; }
        public AgendamentoTipo TipoAgendamento { get; set; }
        public AgendamentoStatus Status { get; set; }

    }
}

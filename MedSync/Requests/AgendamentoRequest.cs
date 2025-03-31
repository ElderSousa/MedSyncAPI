using MedSync.Domain.Enum;
using static MedSync.Application.Requests.MedicoResquest;
using static MedSync.Application.Requests.PacienteRequest;

namespace MedSync.Application.Requests;

public class AgendamentoRequest
{
    public class AdicionarAgendamentoRequest
    {
        public Guid PacienteId { get; set; }
        public Guid MedicoId { get; set; }
        public string? Observacao { get; set; }
        public DateTime AgendadoPara { get; set; }
        public AgendamentoTipo TipoAgendamento { get; set; }
        public AgendamentoStatus Status { get; set; }


        public AdicionarPacienteRequest Paciente { get; set; } = new();
        public AdicionarMedicoRequest Medico { get; set; } = new();
    }

    public class AtualizarAgendamentoResquet
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public Guid MedicoId { get; set; }
        public string? Observacao { get; set; }
        public DateTime AgendadoPara { get; set; }
        public AgendamentoTipo TipoAgendamento { get; set; }
        public AgendamentoStatus Status { get; set; }


        public AtualizarPacienteRequest Paciente { get; set; } = new();
        public AtualizarMedicoRequest Medico { get; set; } = new();
    }
}

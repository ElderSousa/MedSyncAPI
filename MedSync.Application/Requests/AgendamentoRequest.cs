using System.Reflection.Metadata.Ecma335;
using MedSync.Domain.Entities;
using MedSync.Domain.Enum;

namespace MedSync.Application.Requests;

public class AgendamentoRequest
{
    public class AdicionaAgendamentoRequest
    {
        public Guid AgendaId { get; set; }
        public Guid MedicoId { get; set; }
        public Guid PacienteId { get; set; }
        public AgendamentoTipo Tipo { get; set; }
        public AgendamentoStatus status { get; set; }
        public DayOfWeek DiaSemana { get; set; }
        public DateTime AgendadoPara { get; set; }
        public TimeSpan Horario { get; set; }

    }
    public class AtualizarAgendamentoRequest
    {
        public Guid Id { get; set; }
        public Guid AgendaId { get; set; }
        public Guid MedicoId { get; set; }
        public Guid PacienteId { get; set; }
        public AgendamentoTipo Tipo { get; set; }
        public AgendamentoStatus status { get; set; }
        public DayOfWeek DiaSemana { get; set; }
        public DateTime AgendadoPara { get; set; }
        public TimeSpan Horario { get; set; }

    }
}

using MedSync.Domain.Enum;

namespace MedSync.Application.Requests;

public class AgendamentoRequest
{
    public class AdicionaAgendamentoRequest
    {
        public Guid AgendaId { get; set; }
        public Guid MedicoId { get; set; }
        public DateTime DataHora { get; set; }
        public DiaSemana DiaSemana { get; set; }
    }
    public class AtualizarAgendamentoRequest
    {
        public Guid Id { get; set; }
        public Guid AgendaId { get; set; }
        public Guid MedicoId { get; set; }
        public DateTime DataHora { get; set; }
        public DiaSemana DiaSemana { get; set; }
    }
}

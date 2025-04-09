using static MedSync.Application.Requests.HorarioRequest;

namespace MedSync.Application.Requests;

public class AgendaRequest
{
    public class AdicionarAgendaRequest
    {
        public Guid MedicoId { get; set; }
        public bool Agendado { get; set; }
        public DayOfWeek DiaSemana { get; set; }
        public DateTime DataDisponivel { get; set; }

        public List<AdicionarHorarioRequest> Horarios { get; set; } = new();
    }

    public class AtualizarAgendaResquet
    {
        public Guid Id { get; set; }
        public Guid MedicoId { get; set; }
        public bool Agendado { get; set; }
        public DayOfWeek DiaSemana { get; set; }
        public DateTime DataDisponivel { get; set; }

        public List<AtualizarHorarioRequest> Horarios { get; set; } = new();
    }
}

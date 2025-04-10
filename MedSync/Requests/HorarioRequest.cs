namespace MedSync.Application.Requests;

public class HorarioRequest
{
    public class AdicionarHorarioRequest
    {
        public Guid AgendaId { get; set; }
        public TimeSpan Hora { get; set; }
        public bool Agendado { get; set; }
    }

    public class AtualizarHorarioRequest
    {
        public Guid Id { get; set; }
        public Guid AgendaId { get; set; }
        public TimeSpan Hora { get; set; }
        public bool Agendado { get; set; }
    }
}

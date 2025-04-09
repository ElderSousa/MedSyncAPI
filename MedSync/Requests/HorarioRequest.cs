namespace MedSync.Application.Requests;

public class HorarioRequest
{
    public class AdicionarHorarioRequest
    {
        public Guid AgendaId { get; set; }
        public TimeSpan HorarioInicial { get; set; }
        public TimeSpan HorarioFinal { get; set; }
    }

    public class AtualizarHorarioRequest
    {
        public Guid Id { get; set; }
        public Guid AgendaId { get; set; }
        public TimeSpan HorarioInicial { get; set; }
        public TimeSpan HorarioFinal { get; set; }
    }
}

namespace MedSync.Application.Responses;

public class HorarioResponse
{
    public Guid Id { get; set; }
    public Guid AgendaId { get; set; }
    public TimeSpan Hora { get; set; }
    public bool Agendado { get; set; }
    public Guid? CriadoPor { get; private set; }
    public DateTime? CriadoEm { get; private set; }
    public Guid? ModificadoPor { get; private set; }
    public DateTime? ModificadoEm { get; private set; }
}

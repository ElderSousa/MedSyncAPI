using MedSync.Domain.Enum;

namespace MedSync.Application.Responses;

public class AgendamentoResponse
{
    public Guid Id { get; private set; }
    public Guid AgendaId { get; set; }
    public Guid MedicoId { get; set; }
    public DateTime DataHora { get; set; }
    public DiaSemana DiaSemana { get; set; }
    public Guid? CriadoPor { get; private set; }
    public DateTime? CriadoEm { get; private set; }
    public Guid? ModificadoPor { get; private set; }
    public DateTime? ModificadoEm { get; private set; }
}

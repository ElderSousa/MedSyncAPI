using MedSync.Domain.Enum;

namespace MedSync.Application.Responses;

public class TelefoneResponse
{
    public Guid Id { get; set; }
    public Guid? PacienteId { get; set; }
    public Guid? MedicoId { get; set; }
    public string? Numero { get; set; }
    public TelefoneTipo Tipo { get; set; }
    public Guid? CriadoPor { get; set; }
    public DateTime? CriadoEm { get; set; }
    public Guid? ModificadoPor { get; set; }
    public DateTime? ModificadoEm { get; set; }
}

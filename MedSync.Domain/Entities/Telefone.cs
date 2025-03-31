using MedSync.Domain.Enum;

namespace MedSync.Domain.Entities;

public class Telefone : BaseModel
{
    public Guid? PacienteId { get; set; }
    public Guid? MedicoId { get; set; }
    public string? Numero { get; set; }
    public TelefoneTipo? Tipo { get; set; }
}

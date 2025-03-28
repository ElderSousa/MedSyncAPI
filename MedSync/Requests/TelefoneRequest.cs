using MedSync.Domain.Enum;

namespace MedSync.Application.Requests;

public class TelefoneRequest
{
    public class AdicionarTelefoneRequest
    {
        public Guid? PacienteId { get; set; }
        public Guid? MedicoId { get; set; }
        public string? Numero { get; set; }
        public Tipo Tipo { get; set; }
    }

    public class AtualizarTelefoneRequest
    {
        public Guid Id { get; set; }
        public Guid? PacienteId { get; set; }
        public Guid? MedicoId { get; set; }
        public string? Numero { get; set; }
        public Tipo Tipo { get; set; }
    }
}

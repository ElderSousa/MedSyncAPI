namespace MedSync.Application.DTOs;

public class BaseModel
{
    public Guid Id { get; set; }
    public Guid? CriadoPor { get; set; }
    public DateTime? CriadoEm { get; set; }
    public Guid? ModificadoPor { get; set; }
    public DateTime? ModificadoEm { get; set; }

    public virtual void AdicionarBaseModel(Guid? usuarioId, DateTime dataHora, bool cadastrar)
    {
        if (cadastrar)
        {
            Id = Guid.NewGuid();
            CriadoPor = usuarioId;
            CriadoEm = dataHora;
        }
        else
        {
            ModificadoPor = usuarioId;
            ModificadoEm = dataHora;
        }
    }
}

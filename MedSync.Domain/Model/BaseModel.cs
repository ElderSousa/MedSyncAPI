public abstract class BaseModel
{
    public Guid Id { get; set; }
    public Guid? CriadoPor { get; private set; }
    public DateTime? CriadoEm { get; private set; }
    public Guid? ModificadoPor { get; private set; }
    public DateTime? ModificadoEm { get; private set; }

    public void AdicionarBaseModel(Guid? usuarioId, DateTime dataHora, bool cadastrar)
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

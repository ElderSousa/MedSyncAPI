using System.Reflection.Metadata.Ecma335;

namespace MedSync.Application.Requests;

public class EnderecoRequest
{
    public class AdicionarEnderecoRequest : EnderecoRequest
    {
        public Guid? PacienteId { get; set; }
        public Guid? MedicoId { get; set; }
        public string? Logradouro { get; set; }
        public int Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? CEP { get; set; }
    }

    public class AtualizarEnderecoRequest : EnderecoRequest
    {
        public Guid Id { get; set; }
        public Guid? PacienteId { get; set; }
        public Guid? MedicoId { get; set; }
        public string? Logradouro { get; set; }
        public int Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? CEP { get; set; }
    }

    private string? _estado { get; set; }

    public string Estado
    {
        get => _estado!.ToUpper();
        set => _estado = value;
    }
}

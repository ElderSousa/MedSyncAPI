namespace MedSync.Application.Requests;

public class EnderecoRequest
{
    public class AdicionarEnderecoRequest
    {
        public Guid? PacienteId { get; set; }
        public Guid? MedicoId { get; set; }
        public string? Logradouro { get; set; }
        public int Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? CEP { get; set; }
        public string? Estado { get; set; }

    }

    public class AtualizarEnderecoRequest
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
        public string? Estado { get; set; }

    }

 
}

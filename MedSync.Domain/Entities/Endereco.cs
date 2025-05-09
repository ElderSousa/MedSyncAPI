﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MedSync.Domain.Entities;

public class Endereco : BaseModel
{
   public Guid? PacienteId { get; set; }
   public Guid? MedicoId { get; set; }
    public string? Logradouro { get; set; }
   public int Numero { get; set; }
   public string? Complemento { get; set; }
   public string? Bairro { get; set; }
   public string? Cidade { get; set; }
   public string? Estado { get; set; }
   public string? CEP { get; set; }

    [NotMapped]
    public bool ValidacaoCadastrar { get; set; }
}


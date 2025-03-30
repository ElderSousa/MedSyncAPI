﻿using MedSync.Domain.Enum;

namespace MedSync.Application.Responses;

public class MedicoResponse
{
    public Guid Id { get; private set; }
    public string? CRM { get; set; }
    public Especialidade Especialidade { get; set; }
    public DateTime DataNascimento { get; set; }
    public Guid? CriadoPor { get; private set; }
    public DateTime? CriadoEm { get; private set; }
    public Guid? ModificadoPor { get; private set; }
    public DateTime? ModificadoEm { get; private set; }

    public PessoaResponse Pessoa { get; set; } = new();
    public List<TelefoneResponse> Telefones { get; set; } = new();
}

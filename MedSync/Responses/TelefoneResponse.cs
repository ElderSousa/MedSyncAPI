﻿using MedSync.Domain.Enum;

namespace MedSync.Application.Responses;

public class TelefoneResponse
{
    public Guid Id { get; private set; }
    public Guid? PacienteId { get; set; }
    public Guid? MedicoId { get; set; }
    public string? Numero { get; set; }
    public Tipo Tipo { get; set; }
    public Guid? CriadoPor { get; private set; }
    public DateTime? CriadoEm { get; private set; }
    public Guid? ModificadoPor { get; private set; }
    public DateTime? ModificadoEm { get; private set; }
}

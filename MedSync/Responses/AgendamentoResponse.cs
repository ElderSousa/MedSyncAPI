﻿using MedSync.Domain.Enum;

namespace MedSync.Application.Responses;

public class AgendamentoResponse
{
    public Guid Id { get; set; }
    public Guid AgendaId { get; set; }
    public Guid MedicoId { get; set; }
    public Guid PacienteId { get; set; }
    public AgendamentoTipo Tipo { get; set; }
    public AgendamentoStatus status { get; set; }
    public DayOfWeek DiaSemana { get; set; }
    public DateTime AgendadoPara { get; set; }
    public TimeSpan Horario { get; set; }
    public Guid? CriadoPor { get; set; }
    public DateTime? CriadoEm { get; set; }
    public Guid? ModificadoPor { get; set; }
    public DateTime? ModificadoEm { get; set; }

    public PacienteResponse Paciente { get; set; } = new();
    public MedicoResponse Medico { get; set; } = new();
}

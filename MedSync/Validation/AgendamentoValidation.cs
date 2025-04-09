using FluentValidation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;

namespace MedSync.Application.Validation;

public class AgendamentoValidation : AbstractValidator<Agendamento>
{    
    public AgendamentoValidation(IAgendamentoRepository agendamentoRepository,
        IAgendaRepository agendaRepository,
        IMedicoRepository medicoRepository,
        IPacienteRepository pacienteRepository,
        bool cadastrar)
    {

        RuleFor(a => a.Id)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio);

        RuleFor(a => a.AgendaId)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio)
            .Must(agendaRepository.Existe)
            .WithMessage(MessagesValidation.NaoEncontrado); 
        
        RuleFor(a => a.MedicoId)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio)
            .Must(medicoRepository.Existe)
            .WithMessage(MessagesValidation.NaoEncontrado); 
        
        RuleFor(a => a.PacienteId)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio)
            .Must(pacienteRepository.Existe)
            .WithMessage(MessagesValidation.NaoEncontrado);

        RuleFor(a => a.DiaSemana)
            .IsInEnum()
            .WithMessage(MessagesValidation.CampoObrigatorio);

        RuleFor(a => a.AgendadoPara)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio);


        RuleFor(a => agendaRepository.AgendaPeriodoExiste(a.Agenda.DataDisponivel, a.Agenda.DiaSemana, a.Agenda.Agendado))
           .Equal(true)
           .WithMessage(MessagesValidation.PeriodoInvalido);

        RuleFor(a => agendamentoRepository.AgendamentoPeriodoExiste(a.DiaSemana, a.AgendadoPara, a.Horario))
            .Equal(true)
            .WithMessage(MessagesValidation.PeriodoInvalido);

        When(a => cadastrar, () =>
        {
            RuleFor(a => a.CriadoEm)
                .NotEmpty()
                .WithMessage(MessagesValidation.CampoObrigatorio);
        });

        When(a => !cadastrar, () =>
        {

            RuleFor(a => a.Id)
                .Must(agendaRepository.Existe)
                .WithMessage(MessagesValidation.NaoEncontrado);

            RuleFor(a => a.ModificadoEm)
                .NotEmpty()
                .WithMessage(MessagesValidation.CampoObrigatorio);
        });
    }
}

using FluentValidation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;

namespace MedSync.Application.Validation;

public class AgendamentoValidation : AbstractValidator<Agendamento>
{
    public AgendamentoValidation(IAgendamentoRepository agendamentoRepository, IAgendaRepository agendaRepository, IMedicoRepository medicoRepository, bool cadastrar)
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

        RuleFor(a => a.DiaSemana)
            .IsInEnum()
            .WithMessage(MessagesValidation.CampoObrigatorio);

        RuleFor(a => a.DataHora)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio)
            .GreaterThanOrEqualTo(a => a.DataHora.AddMinutes(20))
            .WithMessage(MessagesValidation.agendamentoInvalido);

        RuleFor(a => agendamentoRepository.AgendamentoPeriodoExiste(a.DiaSemana, a.DataHora))
            .Equal(true)
            .WithMessage(MessagesValidation.AgendamentoPeriodo);

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

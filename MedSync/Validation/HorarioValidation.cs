using FluentValidation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;

namespace MedSync.Application.Validation;

public class HorarioValidation : AbstractValidator<Horario>
{
    public HorarioValidation(IHorarioRepository horarioRepository, IAgendaRepository agendaRepository, bool cadastrar)
    {
        RuleFor(h => h.Id)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio);

        RuleFor(h => h.AgendaId)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio)
            .Must(agendaRepository.Existe)
            .WithMessage(MessagesValidation.NaoEncontrado);

        RuleFor(h => h.HorarioInicial)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio);

        RuleFor(h => h.HorarioFinal)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio);

        RuleFor(h => horarioRepository.HorarioPeriodoExiste(h.HorarioInicial, h.HorarioFinal))
            .Equal(false)
            .WithMessage(MessagesValidation.PeriodoInvalido);

        When(h => cadastrar, () =>
        {
            RuleFor(h => h.CriadoEm)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio);
        });
        
        When(h => !cadastrar, () =>
        {
            RuleFor(h => h.Id)
               .Must(horarioRepository.Existe)
               .WithMessage(MessagesValidation.NaoEncontrado);

            RuleFor(h => h.ModificadoEm)
              .NotEmpty()
              .WithMessage(MessagesValidation.CampoObrigatorio);
        });
          
    }
}

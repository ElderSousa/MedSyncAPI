using FluentValidation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;

namespace MedSync.Application.Validation;

public class HorarioValidation : AbstractValidator<Horario>
{
    public HorarioValidation(IHorarioRepository horarioRepository, IAgendaRepository agendaRepository)
    {
        RuleFor(h => h.Id)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio);

        RuleFor(h => h.AgendaId)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio)
            .Must(agendaRepository.Existe)
            .WithMessage(MessagesValidation.NaoEncontrado);

        RuleFor(h => h.Hora)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio);

        RuleFor(h => horarioRepository.HorarioExiste(h.Hora, h.Agendado))
            .Equal(false)
            .WithMessage(MessagesValidation.PeriodoInvalido);

        When(h => h.ValidacaoCadastrar, () =>
        {
            RuleFor(h => h.CriadoEm)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio);
        });
        
        When(h => !h.ValidacaoCadastrar, () =>
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

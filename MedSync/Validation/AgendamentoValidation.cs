using FluentValidation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;

namespace MedSync.Application.Validation;

public class AgendamentoValidation : AbstractValidator<Agendamento>
{
    public AgendamentoValidation(IAgendamentoRepository agendamentoRepository, bool cadastrar)
    {
        RuleFor(a => a.Id)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio);
        
        RuleFor(a => a.PacienteId)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio);

        RuleFor(a => a.Tipo)
            .IsInEnum()
            .WithMessage(MessagesValidation.CampoObrigatorio);
        
        RuleFor(a => a.Status)
            .IsInEnum()
            .WithMessage(MessagesValidation.CampoObrigatorio);
        
        RuleFor(a => a.AgendadoPara)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio);

        RuleFor(a => a)
            .Must(a => !agendamentoRepository.AgendamentoPeriodoExiste(a.AgendadoPara.AddMinutes(20)))
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
           .Must(agendamentoRepository.Existe)
           .WithMessage(MessagesValidation.NaoEncontrado);

            RuleFor(a => a.ModificadoEm)
           .NotEmpty()
           .WithMessage(MessagesValidation.CampoObrigatorio);
        });
    }
}

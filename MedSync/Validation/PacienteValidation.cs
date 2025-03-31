using FluentValidation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;

namespace MedSync.Application.Validation;

public class PacienteValidation : AbstractValidator<Paciente>
{
    public PacienteValidation(IPacienteRepository pacienteRepository, bool cadastar)
    {
        RuleFor(p => p.Id)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio);

        When(p => cadastar, () =>
        {
            RuleFor(p => p.CriadoEm)
                .NotEmpty()
                .WithMessage(MessagesValidation.CampoObrigatorio);
        });

        When(p => !cadastar, () =>
        {
            RuleFor(p => p.Id)
                .Must(pacienteRepository.Existe)
                .WithMessage(MessagesValidation.NaoEncontrado);

            RuleFor(p => p.ModificadoEm)
                .NotEmpty()
                .WithMessage(MessagesValidation.CampoObrigatorio);
        });
    }
}

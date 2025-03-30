using FluentValidation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;

namespace MedSync.Application.Validation;

public class TelefoneValidation : AbstractValidator<Telefone>
{
    public TelefoneValidation(ITelefoneRepository telefoneRepository, bool cadastrar)
    {
        RuleFor(t => t.Id)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio); 

        RuleFor(t => t.Tipo)
            .IsInEnum().WithMessage(MessagesValidation.CampoObrigatorio);

        RuleFor(t => t.Numero)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio)
            .Matches(@"^\d{2}-?\d{5}\d{4}$")
            .WithMessage(MessagesValidation.NumeroInvalido);

        When(t => cadastrar, () =>
        {
            RuleFor(t => t.CriadoEm)
                .NotEmpty()
                .WithMessage(MessagesValidation.CampoObrigatorio);
        });

        When(t => !cadastrar, () =>
        {
            RuleFor(t => t.Id)
                .Must(telefoneRepository.Existe)
                .WithMessage(MessagesValidation.NaoEncontrado);

            RuleFor(t => t.ModificadoEm)
                .NotEmpty()
                .WithMessage(MessagesValidation.CampoObrigatorio);
        });
    }
}

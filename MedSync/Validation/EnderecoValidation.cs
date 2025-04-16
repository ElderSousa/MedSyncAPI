using FluentValidation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;

namespace MedSync.Application.Validation;

public class EnderecoValidation : AbstractValidator<Endereco>
{
    public EnderecoValidation(IEnderecoRepository enderecoRepository)
    {
        RuleFor(e => e.Id)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio);
        
        RuleFor(e => e.Logradouro)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio); 

        RuleFor(e => e.Numero)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio); 

        RuleFor(e => e.Bairro)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio); 

        RuleFor(e => e.Cidade)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio)
            .MinimumLength(3).WithMessage(MessagesValidation.NomeInvalido); 

        RuleFor(e => e.CEP)
           .Matches(@"^\d{5}-?\d{3}$")
           .WithMessage(MessagesValidation.CEPInvalido);

        When(e => e.ValidacaoCadastrar, () =>
        {
            RuleFor(e => e.Id)
                .NotEmpty()
                .WithMessage(MessagesValidation.CampoObrigatorio);
        });

        When(e => !e.ValidacaoCadastrar, () =>
        {
            RuleFor(e => e.Id)
                .Must(enderecoRepository.Existe)
                .WithMessage(MessagesValidation.NaoEncontrado);

            RuleFor(e => e.ModificadoEm)
               .NotEmpty()
               .WithMessage(MessagesValidation.CampoObrigatorio);
        });

    }
}

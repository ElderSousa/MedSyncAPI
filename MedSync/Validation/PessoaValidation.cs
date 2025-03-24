using System.Text.RegularExpressions;
using FluentValidation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;

namespace MedSync.Application.Validation;

public class PessoaValidation : AbstractValidator<Pessoa>
{
    public PessoaValidation(IPessoaRepository pessoaRepository, bool cadastrar)
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage(MessagesValidation.CampoObrigatorio);

        RuleFor(p => p.CPF)
            .NotEmpty().WithMessage(MessagesValidation.CampoObrigatorio)
            .Must(IsValidCpf).WithMessage(MessagesValidation.CPFInvalido)
            .Must(pessoaRepository.CPFExiste).WithMessage(MessagesValidation.CPFCadastrado);

        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage(MessagesValidation.CampoObrigatorio)
            .MinimumLength(3).WithMessage(MessagesValidation.NomeInvalido);

        RuleFor(p => p.DataNascimento)
            .NotEmpty().WithMessage(MessagesValidation.CampoObrigatorio);

        RuleFor(p => p.Email)
            .EmailAddress().WithMessage(MessagesValidation.EmailInvalido);

        When(p => cadastrar, () =>
        {
            RuleFor(p => p.CriadoEm)
                .NotEmpty().WithMessage(MessagesValidation.CampoObrigatorio);
        });
        
    }

    #region MÉTODOSPRIVADOS
    private bool IsValidCpf(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;

        cpf = Regex.Replace(cpf, "[^0-9]", "");
        if (cpf.Length != 11 || cpf.Distinct().Count() == 1) return false;

        int[] multiplier1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplier2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf.Substring(0, 9);
        int sum = tempCpf.Select((t, i) => (t - '0') * multiplier1[i]).Sum();
        int remainder = sum % 11;
        int digit1 = remainder < 2 ? 0 : 11 - remainder;

        tempCpf += digit1;
        sum = tempCpf.Select((t, i) => (t - '0') * multiplier2[i]).Sum();
        remainder = sum % 11;
        int digit2 = remainder < 2 ? 0 : 11 - remainder;

        return cpf.EndsWith(digit1.ToString() + digit2.ToString());
    }
    #endregion
}

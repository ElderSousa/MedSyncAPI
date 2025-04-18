﻿using System.Text.RegularExpressions;
using FluentValidation;
using MedSync.Domain.Entities;
using MedSync.Domain.Enum;
using MedSync.Domain.Interfaces;

namespace MedSync.Application.Validation;

public class MedicoValidation : AbstractValidator<Medico>
{
    public MedicoValidation(IMedicoRepository medicoRepository)
    {
        RuleFor(m => m.Id)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio);

        RuleFor(m => m.Especialidade)
            .IsInEnum()
            .WithMessage(MessagesValidation.CampoObrigatorio);
           
        RuleFor(m => m.CRM)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio)
            .Matches(@"^\d{4,6}(\/[A-Z]{2})?$", RegexOptions.IgnoreCase)
            .WithMessage(MessagesValidation.CRMInvalido); 

        When(m => m.ValidacaoCadastrar, () =>
        {
            RuleFor(m => medicoRepository.CRMExiste(m.CRM))
           .Equal(false)
           .WithMessage(MessagesValidation.CRMExiste);

            RuleFor(m => m.CriadoEm)
                .NotEmpty()
                .WithMessage(MessagesValidation.CampoObrigatorio);
        }); 
        
        When(m => !m.ValidacaoCadastrar, () =>
        {
            RuleFor(m => m.Id)
                .Must(medicoRepository.Existe)
                .WithMessage(MessagesValidation.NaoEncontrado);
        
            RuleFor(m => m.ModificadoEm)
                .NotEmpty()
                .WithMessage(MessagesValidation.CampoObrigatorio);
        });

    }
}

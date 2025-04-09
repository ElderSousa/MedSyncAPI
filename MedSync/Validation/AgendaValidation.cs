using FluentValidation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;

namespace MedSync.Application.Validation;

public class AgendaValidation : AbstractValidator<Agenda>
{
   
    public AgendaValidation(IAgendaRepository agendaRepository, IMedicoRepository medicoRepository, bool cadastrar)
    {

        RuleFor(a => a.Id)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio);
         
        RuleFor(a => a.MedicoId)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio)
            .Must(medicoRepository.Existe)
            .WithMessage(MessagesValidation.NaoEncontrado);
        
        RuleFor(a => a.DataDisponivel)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio); 
        
        RuleFor(a => !(agendaRepository.AgendaPeriodoExiste(a.DataDisponivel, a.DiaSemana, a.Agendado)))
            .Equal(true)
            .WithMessage(MessagesValidation.PeriodoInvalido);
 
        RuleFor(a => a.DiaSemana)
            .IsInEnum()
            .WithMessage(MessagesValidation.CampoObrigatorio);

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

using FluentValidation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;

namespace MedSync.Application.Validation;

public class AgendamentoValidation : AbstractValidator<Agendamento>
{    
    public AgendamentoValidation(IAgendamentoRepository agendamentoRepository,
        IAgendaRepository agendaRepository,
        IMedicoRepository medicoRepository,
        IPacienteRepository pacienteRepository,
        IHorarioRepository horarioRepository)
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
        
        RuleFor(a => a.PacienteId)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio)
            .Must(pacienteRepository.Existe)
            .WithMessage(MessagesValidation.NaoEncontrado);

        RuleFor(a => a.DiaSemana)
            .IsInEnum()
            .WithMessage(MessagesValidation.CampoObrigatorio);

        RuleFor(a => a.AgendadoPara)
            .NotEmpty()
            .WithMessage(MessagesValidation.CampoObrigatorio)
            .GreaterThan(DateTime.Now)
            .WithMessage(MessagesValidation.DataInvalida);


        RuleFor(a => agendaRepository.AgendaPeriodoExiste(a.AgendadoPara, a.DiaSemana))
           .Equal(true)
           .WithMessage(MessagesValidation.PeriodoInvalido);

        RuleFor(a => a)
            .Must(a => horarioRepository.HorarioExiste(a.Horario, false))
            .WithMessage(MessagesValidation.HorarioInvalido);
            
        RuleFor(a => agendamentoRepository.AgendamentoPeriodoExiste(a.DiaSemana, a.AgendadoPara, a.Horario))
            .Equal(false)
            .WithMessage(MessagesValidation.AgendamentoPeriodo);

        When(a => a.ValidacaoCadastrar, () =>
        {
            RuleFor(a => a.CriadoEm)
                .NotEmpty()
                .WithMessage(MessagesValidation.CampoObrigatorio);
        });

        When(a => !a.ValidacaoCadastrar, () =>
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

using FluentValidation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;

namespace MedSync.Application.Validation;

public class AgendaValidation : AbstractValidator<Agenda>
{
    private IHorarioRepository _horarioRepository;
    private IAgendaRepository _agendaRepository;
    public AgendaValidation(IAgendaRepository agendaRepository, IMedicoRepository medicoRepository, IHorarioRepository horarioRepository, bool cadastrar)
    {
        _horarioRepository = horarioRepository;
        _agendaRepository = agendaRepository;

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

        RuleFor(a => VerificaPeriodo(a.DataDisponivel, a.DiaSemana, a.Horarios.Exists(a => a.Agendado), a.Horarios))
            .Equal(false)
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

    private bool VerificaPeriodo(DateTime data, DayOfWeek dia, bool agendado, List<Horario> horarios)
    {
        if (_agendaRepository.AgendaPeriodoExiste(data, dia))
        {
            foreach (var horario in horarios)
            {
                if (!_horarioRepository.HorarioExiste(horario.Hora, horario.Agendado))
                    continue;
                return true;
            }
            return false;
        }   
        return false;
    }
}

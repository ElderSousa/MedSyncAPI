using AutoMapper;
using FluentValidation;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using MedSync.Application.Services;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using static MedSync.Application.Requests.AgendaRequest;
using static MedSync.Application.Requests.HorarioRequest;

namespace MedSync.Test.ApplicationTest.ServiceTest;

public class AgendaServiceTest
{
    private readonly Mock<IAgendaRepository> _mockAgendaRepository;
    private readonly Mock<IMedicoService> _mockMedicoService;
    private readonly Mock<IHorarioService> _mockHorarioService;
    private readonly Mock<IValidator<Agenda>> _mockAgendaValidation;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAcessor;
    private readonly Mock<ILogger<AgendaService>> _mockLogger;

    private readonly AgendaService _agendaService;
    public AgendaServiceTest()
    {
        _mockAgendaRepository = new Mock<IAgendaRepository>();
        _mockMedicoService = new Mock<IMedicoService>();
        _mockHorarioService = new Mock<IHorarioService>();
        _mockAgendaValidation = new Mock<IValidator<Agenda>>();
        _mockMapper = new Mock<IMapper>();
        _mockHttpContextAcessor = new Mock<IHttpContextAccessor>();
        _mockLogger = new Mock<ILogger<AgendaService>>();

        _agendaService = new AgendaService
            (
                _mockAgendaRepository.Object,
                _mockMedicoService.Object,
                _mockHorarioService.Object,
                _mockAgendaValidation.Object,
                _mockMapper.Object,
                _mockHttpContextAcessor.Object,
                _mockLogger.Object
            );
    }

    [Fact]
    public async Task CreateAsync_DeveCriarAgenda_Quando_DadosValidos()
    {
        //Arrange

        MedicoResponse medicoResponse = new() { Id = Guid.Parse("d99883e1-22bb-4332-9c93-af3d7edf8eaa") };
        var agendaRequest = new AdicionarAgendaRequest()
        {
            MedicoId = medicoResponse.Id,
            DiaSemana = DayOfWeek.Monday,
            DataDisponivel = DateTime.Now.AddHours(1),
            Horarios = new List<AdicionarHorarioRequest> { new AdicionarHorarioRequest() }
        };

        var medico = new Medico();

        var agenda = new Agenda()
        {
            Id = Guid.NewGuid(),
            MedicoId = agendaRequest.MedicoId,
            DiaSemana = agendaRequest.DiaSemana,
            DataDisponivel = agendaRequest.DataDisponivel  
        };

        _mockMapper.Setup(m => m.Map<Agenda>(It.IsAny<AdicionarAgendaRequest>()))
            .Returns(agenda);
        _mockAgendaValidation.Setup(v => v.ValidateAsync(It.IsAny<Agenda>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockMapper.Setup(m => m.Map<Medico>(It.IsAny<MedicoResponse>()))
            .Returns(medico);
        _mockMedicoService.Setup(med => med.GetIdAsync(agenda.MedicoId))
            .ReturnsAsync(medicoResponse);
        _mockAgendaRepository.Setup(a => a.CreateAsync(It.IsAny<Agenda>()))
            .ReturnsAsync(true);
        _mockHorarioService.Setup(h => h.CreateAsync(It.IsAny<AdicionarHorarioRequest>()))
            .ReturnsAsync(new Response { Status = "Success", Error = false });

        //Act
        var response = await _agendaService.CreateAsync(agendaRequest);

        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error);
        _mockMapper.Verify(m => m.Map<Agenda>(It.IsAny<AdicionarAgendaRequest>()), Times.Once);
        _mockAgendaRepository.Verify(a => a.CreateAsync(It.IsAny<Agenda>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_DeveRetornar_InvalidOperationException_Quando_Falha_AoCriarAgenda()
    {
        //Arrange
        MedicoResponse medicoResponse = new() { Id = Guid.NewGuid() };
        var agendaRequest = new AdicionarAgendaRequest()
        {
            MedicoId = medicoResponse.Id,
            DiaSemana = DayOfWeek.Monday,
            DataDisponivel = DateTime.Now.AddHours(1),
            Horarios = new List<AdicionarHorarioRequest> { new AdicionarHorarioRequest() }
        };

        var agenda = new Agenda();
        var medico = new Medico();

        _mockMapper.Setup(m => m.Map<Agenda>(It.IsAny<AdicionarAgendaRequest>()))
            .Returns(agenda);
        _mockAgendaValidation.Setup(v => v.ValidateAsync(It.IsAny<Agenda>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockMapper.Setup(m => m.Map<Medico>(It.IsAny<MedicoResponse>()))
            .Returns(medico);
        _mockMedicoService.Setup(med => med.GetIdAsync(agenda.MedicoId))
             .ReturnsAsync(medicoResponse);

        //Act & Assert
        var exception = await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _agendaService.CreateAsync(agendaRequest));
        Assert.Equal("Falha ao criar agenda.", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_Deve_Atualizar_Agenda_Quando_Dados_Validos()
    {
        //Arrange
        var medicoResponse = new MedicoResponse() { Id = Guid.NewGuid()};

        var agendaRequest = new AtualizarAgendaResquet()
        {
            MedicoId = medicoResponse.Id,
            DiaSemana = DayOfWeek.Monday,
            DataDisponivel = DateTime.Now.AddHours(1),
            Horarios = new List<AtualizarHorarioRequest> { new AtualizarHorarioRequest() { AgendaId = Guid.NewGuid(), Hora = TimeSpan.Parse("08:00:00"), Agendado = false} }
        };

        var agenda = new Agenda();
        _mockMapper.Setup(m => m.Map<Agenda>(It.IsAny<AtualizarAgendaResquet>()))
            .Returns(agenda);
        _mockAgendaValidation.Setup(v => v.ValidateAsync(It.IsAny<Agenda>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockAgendaRepository.Setup(a => a.UpdateAsync(It.IsAny<Agenda>()))
            .ReturnsAsync(true);
        _mockHorarioService.Setup(a => a.UpdateAsync(agendaRequest.Horarios.First(h => h.AgendaId != Guid.Empty)))
            .ReturnsAsync(new Response() { Status = "Sucesso", Error = false});

        //Act
        var response = await _agendaService.UpdateAsync(agendaRequest);

        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error);
        _mockAgendaRepository.Verify(v => v.UpdateAsync(It.IsAny<Agenda>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Retorna_InvalidOperationException_Quando_Falha_Ao_Atualizar_Agenda()
    {
        //Arrange
        var agendaRequest = new AtualizarAgendaResquet()
        {
            MedicoId = Guid.NewGuid(),
            DiaSemana = DayOfWeek.Monday,
            DataDisponivel = DateTime.Now.AddHours(1),
            Horarios = new List<AtualizarHorarioRequest> { new AtualizarHorarioRequest() }
        };

        var agenda = new Agenda();

        _mockMapper.Setup(m => m.Map<Agenda>(It.IsAny<AtualizarAgendaResquet>()))
            .Returns(agenda);
        _mockAgendaValidation.Setup(v => v.ValidateAsync(It.IsAny<Agenda>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        //Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _agendaService.UpdateAsync(agendaRequest));
        Assert.Equal("Falha ao atualiza agendamento.", exception.Message);
    }

    [Fact]
    public async Task DeleteAsync_Deve_Excluir_Agenda_Quando_Dados_Validos()
    {
        //Arrange
        var agendaId = Guid.NewGuid();

        _mockAgendaRepository.Setup(a => a.DeleteAsync(agendaId))
            .ReturnsAsync(true);

        //Act
        var response = await _agendaService.DeleteAsync(agendaId);

        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error);
        _mockAgendaRepository.Verify(a => a.DeleteAsync(agendaId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Deve_Retornar_InvalidOperationException_Quando_Falha_Ao_Excluir()
    {
        //Arrange
        var agendaId = Guid.NewGuid();

        _mockAgendaRepository.Setup(a => a.DeleteAsync(agendaId))
            .ReturnsAsync(false);

        //Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _agendaService.DeleteAsync(agendaId));
        Assert.Equal("Falha ao deletar agendamento de nossa base de dados.", exception.Message);
    }
}

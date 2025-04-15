using System.ComponentModel.DataAnnotations;
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
}

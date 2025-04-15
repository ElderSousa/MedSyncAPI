using AutoMapper;
using FluentValidation;
using MedSync.Application.Interfaces;
using MedSync.Application.PaginationModel;
using MedSync.Application.Responses;
using MedSync.Application.Services;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using static MedSync.Application.Requests.AgendamentoRequest;

namespace MedSync.Test.ApplicationTest.ServiceTest;

public class AgendamentoServiceTest
{
    private readonly Mock<IAgendamentoRepository> _mockAgendamentoRepository;
    private readonly Mock<IHorarioService>_mockHorarioService;
    private readonly Mock<IValidator<Agendamento>> _mockAgendamentoValidator;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAcessor;
    private readonly Mock<ILogger<AgendamentoService>> _mockLogger;

    private readonly AgendamentoService _agendamentoService;

    public AgendamentoServiceTest()
    {
        _mockAgendamentoRepository = new Mock<IAgendamentoRepository>();
        _mockHorarioService = new Mock<IHorarioService>();
        _mockAgendamentoValidator = new Mock<IValidator<Agendamento>>();
        _mockMapper = new Mock<IMapper>();
        _mockHttpContextAcessor = new Mock<IHttpContextAccessor>();
        _mockLogger = new Mock<ILogger<AgendamentoService>>();

        _agendamentoService = new AgendamentoService(
            _mockAgendamentoRepository.Object,
            _mockHorarioService.Object,
            _mockAgendamentoValidator.Object,
            _mockMapper.Object,
            _mockHttpContextAcessor.Object,
            _mockLogger.Object   
        );
    }

    [Fact]
    public async Task CreateAsync_Deve_Criar_Agendamento_Quando_Dados_Validos()
    {
        //Arrange
        var agendementoRequest = new AdicionaAgendamentoRequest()
        {
            PacienteId = Guid.NewGuid(),
            MedicoId = Guid.NewGuid(),
            AgendaId = Guid.NewGuid(),
            DiaSemana = DayOfWeek.Wednesday,
            Horario = TimeSpan.Parse("08:00:00"),
            status = Domain.Enum.AgendamentoStatus.Pendetende,
            Tipo = Domain.Enum.AgendamentoTipo.PrimeiraVez,
            AgendadoPara = DateTime.Parse("2025-04-16")
        };

        var agendamento = new Agendamento()
        {
            PacienteId = agendementoRequest.PacienteId,
            MedicoId = agendementoRequest.MedicoId,
            AgendaId = agendementoRequest.AgendaId,
            DiaSemana = agendementoRequest.DiaSemana,
            Horario = agendementoRequest.Horario,
            status = agendementoRequest.status,
            Tipo = agendementoRequest.Tipo,
            AgendadoPara = agendementoRequest.AgendadoPara
        };

        var horario = new Pagination<HorarioResponse>
        {
            Itens = new List<HorarioResponse>
            {
                new HorarioResponse
                {
                    Id = Guid.NewGuid(),
                    Hora = agendamento.Horario,
                    AgendaId = agendamento.AgendaId,
                    Agendado = false
                }
            }
        };


        _mockMapper.Setup(m => m.Map<Agendamento>(It.IsAny<AdicionaAgendamentoRequest>()))
            .Returns(agendamento);
        _mockAgendamentoValidator.Setup(v => v.ValidateAsync(It.IsAny<Agendamento>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockAgendamentoRepository.Setup(a => a.CreateAsync(agendamento))
            .ReturnsAsync(true);
        _mockHorarioService.Setup(h => h.GetAgendaIdAsync(agendamento.AgendaId, int.MaxValue, int.MaxValue))
            .ReturnsAsync(horario);
        _mockHorarioService.Setup(h => h.UpdateStatusAsync(Guid.NewGuid(), false));


        //Act
        var response = await _agendamentoService.CreateAsync(agendementoRequest);

        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error);
        _mockAgendamentoRepository.Verify(v => v.CreateAsync(agendamento), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Deve_Retornar_InvalidOperationException_Quando_Dados_Invalidos()
    {
        //Arrange
        var agendementoRequest = new AdicionaAgendamentoRequest()
        {
            PacienteId = Guid.NewGuid(),
            MedicoId = Guid.NewGuid(),
            AgendaId = Guid.NewGuid(),
            DiaSemana = DayOfWeek.Wednesday,
            Horario = TimeSpan.Parse("08:00:00"),
            status = Domain.Enum.AgendamentoStatus.Pendetende,
            Tipo = Domain.Enum.AgendamentoTipo.PrimeiraVez,
            AgendadoPara = DateTime.Parse("2025-04-16")
        };

        var agendamento = new Agendamento()
        {
            PacienteId = agendementoRequest.PacienteId,
            MedicoId = agendementoRequest.MedicoId,
            AgendaId = agendementoRequest.AgendaId,
            DiaSemana = agendementoRequest.DiaSemana,
            Horario = agendementoRequest.Horario,
            status = agendementoRequest.status,
            Tipo = agendementoRequest.Tipo,
            AgendadoPara = agendementoRequest.AgendadoPara
        };

        _mockMapper.Setup(m => m.Map<Agendamento>(It.IsAny<AdicionaAgendamentoRequest>()))
            .Returns(agendamento);
        _mockAgendamentoValidator.Setup(v => v.ValidateAsync(It.IsAny<Agendamento>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockAgendamentoRepository.Setup(a => a.CreateAsync(agendamento))
            .ReturnsAsync(false);

        //Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _agendamentoService.CreateAsync(agendementoRequest));
        Assert.Equal("Falha ao criar agenda.", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_Deve_Atualizar_Agendamento_Quando_Dados_Validos()
    {
        //Arrange
        var agendementoRequest = new AtualizarAgendamentoRequest()
        {
            PacienteId = Guid.NewGuid(),
            MedicoId = Guid.NewGuid(),
            AgendaId = Guid.NewGuid(),
            DiaSemana = DayOfWeek.Wednesday,
            Horario = TimeSpan.Parse("08:00:00"),
            status = Domain.Enum.AgendamentoStatus.Pendetende,
            Tipo = Domain.Enum.AgendamentoTipo.PrimeiraVez,
            AgendadoPara = DateTime.Parse("2025-04-16")
        };

        var agendamento = new Agendamento()
        {
            PacienteId = agendementoRequest.PacienteId,
            MedicoId = agendementoRequest.MedicoId,
            AgendaId = agendementoRequest.AgendaId,
            DiaSemana = agendementoRequest.DiaSemana,
            Horario = agendementoRequest.Horario,
            status = agendementoRequest.status,
            Tipo = agendementoRequest.Tipo,
            AgendadoPara = agendementoRequest.AgendadoPara
        };

        _mockMapper.Setup(m => m.Map<Agendamento>(It.IsAny<AtualizarAgendamentoRequest>()))
            .Returns(agendamento);
        _mockAgendamentoValidator.Setup(v => v.ValidateAsync(It.IsAny<Agendamento>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockAgendamentoRepository.Setup(a => a.UpdateAsync(agendamento))
            .ReturnsAsync(true);

        //Act
        var response = await _agendamentoService.UpdateAsync(agendementoRequest);

        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error);
        _mockAgendamentoRepository.Verify(v => v.UpdateAsync(agendamento), Times.Once);    
    }

    [Fact]
    public async Task UpdateAsync_Deve_Retornar_InvalidOperationException_Quando_Dados_Invalidos()
    {
        //Arrange
        var agendementoRequest = new AtualizarAgendamentoRequest()
        {
            PacienteId = Guid.NewGuid(),
            MedicoId = Guid.NewGuid(),
            AgendaId = Guid.NewGuid(),
            DiaSemana = DayOfWeek.Wednesday,
            Horario = TimeSpan.Parse("08:00:00"),
            status = Domain.Enum.AgendamentoStatus.Pendetende,
            Tipo = Domain.Enum.AgendamentoTipo.PrimeiraVez,
            AgendadoPara = DateTime.Parse("2025-04-16")
        };

        var agendamento = new Agendamento()
        {
            PacienteId = agendementoRequest.PacienteId,
            MedicoId = agendementoRequest.MedicoId,
            AgendaId = agendementoRequest.AgendaId,
            DiaSemana = agendementoRequest.DiaSemana,
            Horario = agendementoRequest.Horario,
            status = agendementoRequest.status,
            Tipo = agendementoRequest.Tipo,
            AgendadoPara = agendementoRequest.AgendadoPara
        };

        _mockMapper.Setup(m => m.Map<Agendamento>(It.IsAny<AtualizarAgendamentoRequest>()))
            .Returns(agendamento);
        _mockAgendamentoValidator.Setup(v => v.ValidateAsync(It.IsAny<Agendamento>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockAgendamentoRepository.Setup(a => a.UpdateAsync(agendamento))
            .ReturnsAsync(false);

        //Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _agendamentoService.UpdateAsync(agendementoRequest));
        Assert.Equal("Falha ao atualizar agendamento.", exception.Message);
    }

    [Fact]
    public async Task DeleteAsync_Deve_Excluir_Agendamento_Quando_Dados_Validos()
    {
        //Arrange
        var agendamentoId = Guid.NewGuid();

        _mockAgendamentoRepository.Setup(a => a.DeleteAsync(agendamentoId))
            .ReturnsAsync(true);

        //Act
        var response = await _agendamentoService.DeleteAsync(agendamentoId);

        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error);
        _mockAgendamentoRepository.Verify(a => a.DeleteAsync(agendamentoId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Deve_Retornar_InvalidOperationException_Quando_Dados_Invalidos()
    {
        //Arrange
        var agendamentoId = Guid.NewGuid();

        _mockAgendamentoRepository.Setup(a => a.DeleteAsync(agendamentoId))
            .ReturnsAsync(false);

        //Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _agendamentoService.DeleteAsync(agendamentoId));
        Assert.Equal("Falha ao excluir o agendamento.", exception.Message);
    }
}
              
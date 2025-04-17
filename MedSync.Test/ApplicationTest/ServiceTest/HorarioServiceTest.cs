using AutoMapper;
using FluentValidation;
using MedSync.Application.Requests;
using MedSync.Application.Services;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using static MedSync.Application.Requests.HorarioRequest;

namespace MedSync.Test.ApplicationTest.ServiceTest;

public class HorarioServiceTest
{
    private readonly Mock<IHorarioRepository> _mockHorarioRepository;
    private readonly Mock<IValidator<Horario>> _mockHorarioValidation;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAcessor;
    private readonly Mock<ILogger<HorarioService>> _mockLogger;

    private readonly HorarioService _horarioService;

    public HorarioServiceTest()
    {
        _mockHorarioRepository = new Mock<IHorarioRepository>();
        _mockHorarioValidation = new Mock<IValidator<Horario>>();
        _mockMapper = new Mock<IMapper>();
        _mockHttpContextAcessor = new Mock<IHttpContextAccessor>();
        _mockLogger = new Mock<ILogger<HorarioService>>();

        _horarioService = new HorarioService(
            _mockHorarioRepository.Object,
            _mockHorarioValidation.Object,
            _mockMapper.Object,
            _mockHttpContextAcessor.Object,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task CreateAsync_Deve_Retornar_Horario_Quando_Dados_Validos()
    {
        //Arrange
        var horarioRequest = new AdicionarHorarioRequest
        {
            AgendaId = Guid.NewGuid(),
            Hora = TimeSpan.Parse("08:00:00"),
            Agendado = false
        };

        var horario = new Horario
        {
            AgendaId = horarioRequest.AgendaId,
            Hora = horarioRequest.Hora,
            Agendado = horarioRequest.Agendado
        };

        _mockMapper.Setup(m => m.Map<Horario>(It.IsAny<AdicionarHorarioRequest>()))
            .Returns(horario);
        _mockHorarioValidation.Setup(v => v.ValidateAsync(It.IsAny<Horario>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockHorarioRepository.Setup(h => h.CreateAsync(horario))
            .ReturnsAsync(true);
        //Act
        var response = await _horarioService.CreateAsync(horarioRequest);

        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error);
        _mockHorarioRepository.Verify(v => v.CreateAsync(horario), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Deve_Retornar_InvalidOperationException_Quando_Dados_Invalidos()
    {
        //Arrange
        var horarioRequest = new AdicionarHorarioRequest
        {
            AgendaId = Guid.NewGuid(),
            Hora = TimeSpan.Parse("08:00:00"),
            Agendado = false
        };

        var horario = new Horario
        {
            AgendaId = horarioRequest.AgendaId,
            Hora = horarioRequest.Hora,
            Agendado = horarioRequest.Agendado
        };

        _mockMapper.Setup(m => m.Map<Horario>(It.IsAny<AdicionarHorarioRequest>()))
            .Returns(horario);
        _mockHorarioValidation.Setup(v => v.ValidateAsync(It.IsAny<Horario>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockHorarioRepository.Setup(h => h.CreateAsync(horario))
            .ReturnsAsync(false);
        //Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _horarioService.CreateAsync(horarioRequest));
        Assert.Equal("Falha ao criar horário.", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_Deve_Retornar_Horario_Quando_Dados_Validos()
    {
        //Arrange
        //Arrange
        var horarioRequest = new AtualizarHorarioRequest
        {
            AgendaId = Guid.NewGuid(),
            Hora = TimeSpan.Parse("08:00:00"),
            Agendado = false
        };

        var horario = new Horario
        {
            AgendaId = horarioRequest.AgendaId,
            Hora = horarioRequest.Hora,
            Agendado = horarioRequest.Agendado
        };

        _mockMapper.Setup(m => m.Map<Horario>(It.IsAny<AtualizarHorarioRequest>()))
            .Returns(horario);
        _mockHorarioValidation.Setup(v => v.ValidateAsync(It.IsAny<Horario>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockHorarioRepository.Setup(h => h.UpdateAsync(horario))
            .ReturnsAsync(true);
        //Act
        var response = await _horarioService.UpdateAsync(horarioRequest);

        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error);
        _mockHorarioRepository.Verify(v => v.UpdateAsync(horario), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Deve_Retornar_InvalidOperationException_Quando_Dados_Invalidos()
    {
        //Arrange
        var horarioRequest = new AtualizarHorarioRequest
        {
            AgendaId = Guid.NewGuid(),
            Hora = TimeSpan.Parse("08:00:00"),
            Agendado = false
        };

        var horario = new Horario
        {
            AgendaId = horarioRequest.AgendaId,
            Hora = horarioRequest.Hora,
            Agendado = horarioRequest.Agendado
        };

        _mockMapper.Setup(m => m.Map<Horario>(It.IsAny<AtualizarHorarioRequest>()))
            .Returns(horario);
        _mockHorarioValidation.Setup(v => v.ValidateAsync(It.IsAny<Horario>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockHorarioRepository.Setup(h => h.UpdateAsync(horario))
            .ReturnsAsync(false);
        //Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _horarioService.UpdateAsync(horarioRequest));
        Assert.Equal("Falha ao atualizar horário.", exception.Message);
    }

    [Fact]
    public async Task DeleteAsync_Deve_Excluir_Horario_Quando_Dados_Validos()
    {
        //Arrange
        var horarioId = Guid.NewGuid();

        _mockHorarioRepository.Setup(h => h.DeleteAsync(horarioId))
            .ReturnsAsync(true);
        //Act
        var response = await _horarioService.DeleteAsync(horarioId);
        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error);
        _mockHorarioRepository.Verify(v => v.DeleteAsync(horarioId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Deve_Retornar_InvalidOperationException_Quando_Dados_InValidos()
    {
        //Arrange
        var horarioId = Guid.NewGuid();

        _mockHorarioRepository.Setup(h => h.DeleteAsync(horarioId))
            .ReturnsAsync(false);
        //Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _horarioService.DeleteAsync(horarioId));
        Assert.Equal("Falha ao excluir horário.", exception.Message);
    }
}

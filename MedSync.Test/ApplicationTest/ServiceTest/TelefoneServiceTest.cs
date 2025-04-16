using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MedSync.Application.Responses;
using MedSync.Application.Services;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using static MedSync.Application.Requests.TelefoneRequest;

namespace MedSync.Test.ApplicationTest.ServiceTest;

public class TelefoneServiceTest
{
    private readonly Mock<ITelefoneRepository> _mockTelefoneRepository;
    private readonly Mock<IValidator<Telefone>> _mockTelefoneValidation;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IHttpContextAccessor> _mockHttpContext;
    private readonly Mock<ILogger<TelefoneService>> _mockLogger;
    private readonly TelefoneService _telefoneService;

    public TelefoneServiceTest()
    {
        _mockTelefoneRepository = new Mock<ITelefoneRepository>();
        _mockTelefoneValidation = new Mock<IValidator<Telefone>>();
        _mockMapper = new Mock<IMapper>();
        _mockHttpContext = new Mock<IHttpContextAccessor>();
        _mockLogger = new Mock<ILogger<TelefoneService>>();

        _telefoneService = new TelefoneService(
                _mockTelefoneRepository.Object,
                _mockTelefoneValidation.Object,
                _mockMapper.Object,
                _mockHttpContext.Object,
                _mockLogger.Object
        );
    }

    [Fact]
    public async Task CreateAsync_CriarTelefone_QuandoDadosValidos()
    {

        //Arrange
        var telefoneRequest= new AdicionarTelefoneRequest()
        {
            PacienteId = Guid.NewGuid(),
            MedicoId = null,
            Numero = "85999393845",
            Tipo = 0 
        };

        var telefone = new Telefone()
        {
            PacienteId = telefoneRequest.PacienteId,
            MedicoId = telefoneRequest.MedicoId,
            Numero = telefoneRequest.Numero,
            Tipo = telefoneRequest.Tipo,
            ValidacaoCadastrar = true   
        };

        _mockMapper.Setup(m => m.Map<Telefone>(It.IsAny<AdicionarTelefoneRequest>()))
            .Returns(telefone);
        _mockTelefoneValidation.Setup(v => v.ValidateAsync(It.IsAny<Telefone>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockTelefoneRepository.Setup(t => t.CreateAsync(It.IsAny<Telefone>()))
            .ReturnsAsync(true);

        //Act
        var response = await _telefoneService.CreateAsync(telefoneRequest);
        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error == true);
        _mockMapper.Verify(m => m.Map<Telefone>(It.IsAny<AdicionarTelefoneRequest>()), Times.Once);
        _mockTelefoneRepository.Verify(t => t.CreateAsync(It.IsAny<Telefone>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_RetornarInvalidOperationException_QuandoDadosInValidos()
    {

        //Arrange
        var telefoneRequest = new AdicionarTelefoneRequest()
        {
            PacienteId = Guid.NewGuid(),
            MedicoId = null,
            Numero = "85999393845",
            Tipo = 0
        };

        var telefone = new Telefone()
        {
            PacienteId = telefoneRequest.PacienteId,
            MedicoId = telefoneRequest.MedicoId,
            Numero = telefoneRequest.Numero,
            Tipo = telefoneRequest.Tipo,
            ValidacaoCadastrar = true
        };

        _mockMapper.Setup(m => m.Map<Telefone>(It.IsAny<AdicionarTelefoneRequest>()))
           .Returns(telefone);
        _mockTelefoneValidation.Setup(v => v.ValidateAsync(It.IsAny<Telefone>(), default))
           .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        //Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => 
        _telefoneService.CreateAsync(telefoneRequest));

        Assert.Contains("Telefone", ex.Message);
    }

    [Fact]
    public async Task CreateAsync_RetornarAutoMapperMappingException_QuandoMapeamentoInvalido()
    {
        // Arrange
        var telefoneRequest = new AdicionarTelefoneRequest()
        {
            PacienteId = Guid.NewGuid(),
            MedicoId = null,
            Numero = "85999393845",
            Tipo = 0
        };

        _mockMapper
            .Setup(m => m.Map<Telefone>(It.IsAny<AdicionarTelefoneRequest>()))
            .Throws(new AutoMapperMappingException("Erro no mapeamento"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AutoMapperMappingException>(() =>
            _telefoneService.CreateAsync(telefoneRequest));

        Assert.Contains("Erro no mapeamento", ex.Message);
    }

    [Fact]
    public async Task CreateAsync_RetornaNullReferenceException_QuandoDadosInvalidos()
    {
        //Arrange
        AdicionarTelefoneRequest telefoneRequest = new();

        //Act & Assert
        var ex = await Assert.ThrowsAsync<NullReferenceException>(() => 
        _telefoneService.CreateAsync(telefoneRequest));

        Assert.Contains("object", ex.Message);
    }

    [Fact]
    public async Task CreateAsync_RetornaArgumentException_QuandoDadosInvalidos()
    {
        //Arrange
        var telefoneRequest = new AdicionarTelefoneRequest()
        {
            PacienteId = Guid.NewGuid(),
            MedicoId = null,
            Numero = "8599",
            Tipo = 0
        };

        var telefone = new Telefone()
        {
            PacienteId = telefoneRequest.PacienteId,
            MedicoId = null,
            Numero = telefoneRequest.Numero,
            Tipo = telefoneRequest.Tipo
        };

        _mockMapper.Setup(m => m.Map<Telefone>(It.IsAny<AdicionarTelefoneRequest>()))
            .Returns(telefone);
        _mockTelefoneValidation.Setup(v => v.ValidateAsync(It.IsAny<Telefone>(), default))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Numero", "Número inválido")
            }));

        //Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => 
        _telefoneService.CreateAsync(telefoneRequest));

        Assert.Contains("Número", ex.Message);
    }

    [Fact]
    public async Task GetIdAsync_Retorna_Telefone_QuandoIdValido()
    {
        //Arrange
        var telefoneId = Guid.Empty;
        var telefone = new Telefone()
        {
            Id = telefoneId,
            PacienteId = Guid.Empty,
            MedicoId = null,
            Numero = "85999454649",
            Tipo = 0
        };

        var telefoneResponse = new TelefoneResponse()
        {
            Id = telefone.Id,
            PacienteId = Guid.Empty,
            MedicoId = null,
            Numero = "85999454649",
            Tipo = 0
        };


        _mockTelefoneRepository.Setup(r => r.GetIdAsync(telefoneId))
            .ReturnsAsync(telefone);
        _mockMapper.Setup(m => m.Map<TelefoneResponse>(It.IsAny<Telefone>()))
            .Returns(telefoneResponse);

        //Action
        var response = await _telefoneService.GetIdAsync(telefoneId);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(telefoneResponse.Id, response.Id);
        _mockTelefoneRepository.Verify(t => t.GetIdAsync(telefoneId), Times.Once);
        _mockMapper.Verify(m => m.Map<TelefoneResponse>(It.IsAny<Telefone>()), Times.Once);
    }

    [Fact]
    public async Task GetIdAsync_RetornaException_QuandoDadosInvalidos()
    {
        //Arrange
        var telefoneId = Guid.Empty;
        var telefone = new Telefone()
        {
           
        };

        var telefoneResponse = new TelefoneResponse()
        {
            Id = telefone.Id,
            PacienteId = Guid.Empty,
            MedicoId = null,
            Numero = "85999454649",
            Tipo = 0
        };

        _mockMapper.Setup(m => m.Map<TelefoneResponse>(It.IsAny<Telefone>()))
            .Throws(new AutoMapperMappingException("Erro no mapeamento"));

        //Act & Assert
        var ex = await Assert.ThrowsAsync<AutoMapperMappingException>(() => 
        _telefoneService.GetIdAsync(telefoneId));

        Assert.Contains("Erro", ex.Message);
    }

    [Fact]
    public async Task UpdateAsync_AtualizarTelefone_QuandoDadosValidos()
    {
        //Arrange
        var telefoneId = Guid.NewGuid();
        var telefoneRequest = new AtualizarTelefoneRequest()
        {
            Id = telefoneId,
            PacienteId = Guid.Empty,
            MedicoId = null,
            Numero = "85999454649",
            Tipo = 0
        };

        var telefone = new Telefone()
        {
            Id = telefoneRequest.Id,
            PacienteId = Guid.Empty,
            MedicoId = null,
            Numero = "85999454649",
            Tipo = 0,
            ValidacaoCadastrar = false
        };

        _mockMapper.Setup(m => m.Map<Telefone>(It.IsAny<AtualizarTelefoneRequest>()))
            .Returns(telefone);
        _mockTelefoneValidation.Setup(v => v.ValidateAsync(It.IsAny<Telefone>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockTelefoneRepository.Setup(t => t.UpdateAsync(It.IsAny<Telefone>()))
            .ReturnsAsync(true);

        //Act
        var response = await _telefoneService.UpdateAsync(telefoneRequest);

        //Assert
        Assert.NotNull(response);
        Assert.Contains("Sucesso", response.Status);
        Assert.False(response.Error == true);
        _mockMapper.Verify(m => m.Map<Telefone>(It.IsAny<AtualizarTelefoneRequest>()), Times.Once);
        _mockTelefoneRepository.Verify(t => t.UpdateAsync(It.IsAny<Telefone>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_RetornarArgumentException_QuandoDadosInValidos()
    {
        //Arrange
        var telefoneId = Guid.NewGuid();
        var telefoneRequest = new AtualizarTelefoneRequest()
        {
            
            PacienteId = Guid.Empty,
            MedicoId = null,
            Numero = "85999454649",
            Tipo = 0
        };

        var telefone = new Telefone()
        {
            Id = telefoneRequest.Id,
            PacienteId = Guid.Empty,
            MedicoId = null,
            Numero = "85999454649",
            Tipo = 0
        };

        _mockMapper.Setup(m => m.Map<Telefone>(It.IsAny<AtualizarTelefoneRequest>()))
            .Returns(telefone);
        _mockTelefoneRepository.Setup(t => t.UpdateAsync(It.IsAny<Telefone>()))
            .ReturnsAsync(true);
        _mockTelefoneValidation.Setup(v => v.ValidateAsync(It.IsAny<Telefone>(), default))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
            {
            new ValidationFailure("Campo", "Campo obrigatório")
            }));

        //Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
        _telefoneService.UpdateAsync(telefoneRequest));

        Assert.Contains("Campo", ex.Message);
      
    }

    [Fact]
    public async Task DeleteAsync_DeveRetornarSucesso_QuandoTelefoneExcluido()
    {
        //Arrange
        var telefoneId = Guid.NewGuid();

        _mockTelefoneRepository.Setup(t => t.DeleteAsync(telefoneId))
            .ReturnsAsync(true);

        //Act
        var response = await _telefoneService.DeleteAsync(telefoneId);

        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error == true);
        _mockTelefoneRepository.Verify(t => t.DeleteAsync(telefoneId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveRetornarInvalidOperationException_QuandoDadosInvalidos()
    {
        //Arrange
        var telefoneId = Guid.NewGuid();

        _mockTelefoneRepository.Setup(t => t.DeleteAsync(telefoneId))
            .ReturnsAsync(false);

        //Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => 
        _telefoneService.DeleteAsync(telefoneId));

        Assert.Contains("Telefone", ex.Message);
    }
}

using AutoMapper;
using MedSync.Application.Responses;
using MedSync.Application.Services;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using static MedSync.Application.Requests.EnderecoRequest;

namespace MedSync.Test.ApplicationTest.ServiceTest;

public class EnderecoServiceTest
{
    private readonly Mock<IEnderecoRepository> _mockEnderecoRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAcessor;
    private readonly Mock<ILogger<EnderecoService>> _mockLogger;
    private readonly EnderecoService _enderecoService;

    public EnderecoServiceTest()
    {
        _mockEnderecoRepository = new Mock<IEnderecoRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockHttpContextAcessor = new Mock<IHttpContextAccessor>();
        _mockLogger = new Mock<ILogger<EnderecoService>>();

        _enderecoService = new EnderecoService(
            _mockEnderecoRepository.Object,
            _mockMapper.Object,
            _mockHttpContextAcessor.Object,
            _mockLogger.Object     
        );
       
    }

    [Fact]
    public async Task CreateAsync_DeveCriarEndereco_ComDadosDeEnderecoValido()
    {
        //Arrange
        var enderecoRequest = new AdicionarEnderecoRequest()
        {
            PacienteId = Guid.Parse("d99883e1-22bb-4332-9c93-af3d7edf8eaa"),
            MedicoId = null,
            Logradouro = "Rua francisco leandro",
            Numero = 123,
            Bairro = "Curio",
            Cidade = "Fortaleza",
            Estado = "ce",
            CEP = "12345650"
        };

        Endereco endereco = new();

        _mockMapper.Setup(m => m.Map<Endereco>(It.IsAny<AdicionarEnderecoRequest>()))
     .Returns(new Endereco
     {
         PacienteId = enderecoRequest.PacienteId,
         Logradouro = enderecoRequest.Logradouro,
         Numero = enderecoRequest.Numero,
         Bairro = enderecoRequest.Bairro,
         Cidade = enderecoRequest.Cidade,
         Estado = enderecoRequest.Estado,
         CEP = enderecoRequest.CEP
     });
        _mockEnderecoRepository.Setup(e => e.CreateAsync(It.IsAny<Endereco>()))
            .ReturnsAsync(true);

        //Act
        var response = await _enderecoService.CreateAsync(enderecoRequest);


        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error == true);
        _mockMapper.Verify(m => m.Map<Endereco>(It.IsAny<AdicionarEnderecoRequest>()), Times.Once);
        _mockEnderecoRepository.Verify(e => e.CreateAsync(It.IsAny<Endereco>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_DeveLancarArgumentException_QuandoDadosSaoInvalidos()
    {
        // Arrange
        var enderecoRequest = new AdicionarEnderecoRequest
        {
            PacienteId = Guid.NewGuid(),
            Logradouro = "",  // inválido
            Numero = 0,       // inválido
            Bairro = "",
            Cidade = "",
            Estado = "",
            CEP = "123"       // inválido
        };

        var endereco = new Endereco
        {
            PacienteId = enderecoRequest.PacienteId,
            Logradouro = enderecoRequest.Logradouro,
            Numero = enderecoRequest.Numero,
            Bairro = enderecoRequest.Bairro,
            Cidade = enderecoRequest.Cidade,
            Estado = enderecoRequest.Estado,
            CEP = enderecoRequest.CEP
        };

        _mockMapper.Setup(m => m.Map<Endereco>(It.IsAny<AdicionarEnderecoRequest>()))
            .Returns(endereco);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _enderecoService.CreateAsync(enderecoRequest));
        Assert.Contains("O campo", ex.Message);
    }


    [Fact]
    public async Task GetIdAsync_DeveRetornarEnderecoResponse_QuandoIdExistir()
    {
        // Arrange
        var enderecoId = Guid.NewGuid();

        var endereco = new Endereco
        {
            Id = enderecoId,
            Logradouro = "Rua Exemplo",
            Numero = 123,
            Bairro = "Centro",
            Cidade = "Fortaleza",
            Estado = "CE",
            CEP = "60000000"
        };

        var enderecoResponse = new EnderecoResponse
        {
            Id = enderecoId,
            Logradouro = endereco.Logradouro,
            Numero = endereco.Numero,
            Bairro = endereco.Bairro,
            Cidade = endereco.Cidade,
            Estado = endereco.Estado,
            CEP = endereco.CEP
        };

        _mockEnderecoRepository
            .Setup(r => r.GetIdAsync(enderecoId))
            .ReturnsAsync(endereco);

        _mockMapper
            .Setup(m => m.Map<EnderecoResponse>(endereco))
            .Returns(enderecoResponse);

        // Act
        var result = await _enderecoService.GetIdAsync(enderecoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(enderecoResponse.Id, result!.Id);
        Assert.Equal("Rua Exemplo", result.Logradouro);
        _mockEnderecoRepository.Verify(r => r.GetIdAsync(enderecoId), Times.Once);
        _mockMapper.Verify(m => m.Map<EnderecoResponse>(endereco), Times.Once);
    }

    [Fact]
    public async Task GetIdAsync_DeveLancarExcecao_QuandoErroAcontecer()
    {
        // Arrange
        var enderecoId = Guid.NewGuid();
        var exception = new Exception("Erro inesperado");

        _mockEnderecoRepository
            .Setup(r => r.GetIdAsync(enderecoId))
            .ThrowsAsync(exception);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _enderecoService.GetIdAsync(enderecoId));
        Assert.Equal("Erro inesperado", ex.Message);

        _mockLogger.Verify(logger => logger.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Erro inesperado")),
            exception,
            It.IsAny<Func<It.IsAnyType, Exception, string>>()
        ), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_DeveAtualizarEndereco_QuandoDadosValidos()
    {
        // Arrange
        var enderecoId = Guid.NewGuid();
        var enderecoRequest = new AtualizarEnderecoRequest
        {
            Id = enderecoId,
            Logradouro = "Rua A",
            Numero = 123,
            Bairro = "Centro",
            Cidade = "Fortaleza",
            Estado = "CE",
            CEP = "60000-000"
        };

        var endereco = new Endereco
        {
            Id = enderecoId,
            Logradouro = "Rua A",
            Numero = 123,
            Bairro = "Centro",
            Cidade = "Fortaleza",
            Estado = "CE",
            CEP = "60000-000",
            ModificadoEm = DateTime.Now
        };

        _mockMapper.Setup(m => m.Map<Endereco>(enderecoRequest)).Returns(endereco);

        // Aqui está o mock necessário para passar na validação:
        _mockEnderecoRepository.Setup(r => r.Existe(enderecoId)).Returns(true);

        _mockEnderecoRepository.Setup(r => r.UpdateAsync(It.IsAny<Endereco>())).ReturnsAsync(true);

        // Act
        var response = await _enderecoService.UpdateAsync(enderecoRequest);

        // Assert
        Assert.NotNull(response);
        Assert.False(response.Error);
        Assert.Equal("Sucesso", response.Status);
    }

    [Fact]
    public async Task UpdateAsync_DeveLancarArgumentException_QuandoValidacaoFalhar()
    {
        // Arrange
        var enderecoRequest = new AtualizarEnderecoRequest
        {
            Id = Guid.NewGuid(),
            Logradouro = "Rua Teste",
            Numero = 123,
            Bairro = "Centro",
            Cidade = "Cidade Teste",
            Estado = "CE",
            CEP = "60000-000"
        };

        var endereco = new Endereco
        {
            Id = enderecoRequest.Id,
            Logradouro = enderecoRequest.Logradouro,
            Numero = enderecoRequest.Numero,
            Bairro = enderecoRequest.Bairro,
            Cidade = enderecoRequest.Cidade,
            Estado = enderecoRequest.Estado,
            CEP = enderecoRequest.CEP,
            ModificadoEm = DateTime.Now
        };

        _mockMapper.Setup(m => m.Map<Endereco>(It.IsAny<AtualizarEnderecoRequest>()))
            .Returns(endereco);

        // Aqui simulamos que o endereço não existe
        _mockEnderecoRepository.Setup(r => r.Existe(It.IsAny<Guid>())).Returns(false);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _enderecoService.UpdateAsync(enderecoRequest));
        Assert.Contains("Id não foi encontrado", ex.Message);

        _mockLogger.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("Id não foi encontrado")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }


    [Fact]
    public async Task DeleteAsync_DeveRetornarSucesso_QuandoEnderecoForExcluido()
    {
        // Arrange
        var enderecoId = Guid.NewGuid();
        _mockEnderecoRepository
            .Setup(r => r.DeleteAsync(enderecoId))
            .ReturnsAsync(true);

        // Act
        var response = await _enderecoService.DeleteAsync(enderecoId);

        // Assert
        Assert.NotNull(response);
        Assert.False(response.Error); // Sucesso esperado
        Assert.Equal("Sucesso", response.Status);

        _mockEnderecoRepository.Verify(r => r.DeleteAsync(enderecoId), Times.Once);
    }


    [Fact]
    public async Task DeleteAsync_DeveLancarArgumentException_QuandoEnderecoNaoForExcluido()
    {
        // Arrange
        var enderecoId = Guid.NewGuid();

        _mockEnderecoRepository
            .Setup(r => r.DeleteAsync(enderecoId))
            .ReturnsAsync(false); // Força falha

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _enderecoService.DeleteAsync(enderecoId));
        Assert.Equal("Endereço não excluído da nossa base de dados.", exception.Message);

        _mockLogger.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("Endereço não excluído")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once
        );
    }

}

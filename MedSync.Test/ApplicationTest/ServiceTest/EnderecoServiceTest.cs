
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
using static MedSync.Application.Requests.EnderecoRequest;

namespace MedSync.Test.ApplicationTest.ServiceTest;

public class EnderecoServiceTest
{
    private readonly Mock<IEnderecoRepository> _mockEnderecoRepository;
    private readonly Mock<IValidator<Endereco>> _mockEnderecoValidation;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAcessor;
    private readonly Mock<ILogger<EnderecoService>> _mockLogger;
    private readonly EnderecoService _enderecoService;

    public EnderecoServiceTest()
    {
        _mockEnderecoRepository = new Mock<IEnderecoRepository>();
        _mockEnderecoValidation = new Mock<IValidator<Endereco>>();
        _mockMapper = new Mock<IMapper>();
        _mockHttpContextAcessor = new Mock<IHttpContextAccessor>();
        _mockLogger = new Mock<ILogger<EnderecoService>>();

        _enderecoService = new EnderecoService(
            _mockEnderecoRepository.Object,
            _mockEnderecoValidation.Object,
            _mockMapper.Object,
            _mockHttpContextAcessor.Object,
            _mockLogger.Object     
        );
       
    }

    [Fact]
    public async Task CreateAsync_Deve_Criar_Endereco_Quando_DadosValidos()
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
         CEP = enderecoRequest.CEP,
         ValidacaoCadastrar = true
     });
        _mockEnderecoRepository.Setup(e => e.CreateAsync(It.IsAny<Endereco>()))
            .ReturnsAsync(true);
        _mockEnderecoValidation.Setup(v => v.ValidateAsync(It.IsAny<Endereco>(), default))
            .ReturnsAsync(new ValidationResult());

        //Act
        var response = await _enderecoService.CreateAsync(enderecoRequest);


        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error);
        _mockEnderecoRepository.Verify(e => e.CreateAsync(It.IsAny<Endereco>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Deve_Retornar_ArgumentException_Quando_Dados_Invalidos()
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
            CEP = enderecoRequest.CEP,
            ValidacaoCadastrar = false,
        };

        _mockMapper.Setup(m => m.Map<Endereco>(It.IsAny<AdicionarEnderecoRequest>()))
            .Returns(endereco);
        _mockEnderecoValidation.Setup(v => v.ValidateAsync(It.IsAny<Endereco>(), default))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Campo", "Campo obrigatório")
            }));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _enderecoService.CreateAsync(enderecoRequest));
        Assert.Contains("Campo", ex.Message);
    }


    [Fact]
    public async Task GetIdAsync_Deve_Retornar_EnderecoResponse_Quando_IdExistir()
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

        _mockEnderecoRepository.Setup(r => r.GetIdAsync(enderecoId))
            .ReturnsAsync(endereco);
        _mockMapper.Setup(m => m.Map<EnderecoResponse>(endereco))
            .Returns(enderecoResponse);

        // Act
        var result = await _enderecoService.GetIdAsync(enderecoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(enderecoResponse.Id, result!.Id);
        Assert.Equal("Rua Exemplo", result.Logradouro);
        _mockEnderecoRepository.Verify(r => r.GetIdAsync(enderecoId), Times.Once);
    }

    [Fact]
    public async Task GetIdAsync_Deve_Retornar_Exception_Quando_Dados_Invalidos()
    {
        // Arrange
        var enderecoId = Guid.NewGuid();
        var exception = new Exception("Erro inesperado");

        _mockEnderecoRepository.Setup(r => r.GetIdAsync(enderecoId))
            .ThrowsAsync(exception);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _enderecoService.GetIdAsync(enderecoId));
        Assert.Equal("Erro inesperado", ex.Message);
    }

    [Fact]
    public async Task UpdateAsync_Deve_Atualizar_Endereco_Quando_Dados_Validos()
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
            ModificadoEm = DateTime.Now,
            ValidacaoCadastrar = false
        };

        _mockMapper.Setup(m => m.Map<Endereco>(enderecoRequest)).Returns(endereco);
        _mockEnderecoValidation.Setup(v => v.ValidateAsync(It.IsAny<Endereco>(), default))
            .ReturnsAsync(new ValidationResult());
        _mockEnderecoRepository.Setup(r => r.UpdateAsync(It.IsAny<Endereco>())).ReturnsAsync(true);

        // Act
        var response = await _enderecoService.UpdateAsync(enderecoRequest);

        // Assert
        Assert.NotNull(response);
        Assert.False(response.Error);
        Assert.Equal("Sucesso", response.Status);
        _mockEnderecoRepository.Verify(v => v.UpdateAsync(endereco),Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Deve_Retornar_ArgumentException_Quand_Dados_Invalidos()
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
            ModificadoEm = DateTime.Now,
            ValidacaoCadastrar = false
        };

        _mockMapper.Setup(m => m.Map<Endereco>(It.IsAny<AtualizarEnderecoRequest>()))
            .Returns(endereco);
        _mockEnderecoValidation.Setup(v => v.ValidateAsync(It.IsAny<Endereco>(), default))
             .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
             {
                new ValidationFailure("Id", "Id não foi encontrado")
             }));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _enderecoService.UpdateAsync(enderecoRequest));
        Assert.Contains("Id não foi encontrado", ex.Message);
    }

    [Fact]
    public async Task DeleteAsync_Deve_Excluir_Endereco_Quando_Dados_Invalidos()
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
        Assert.False(response.Error);
        Assert.Equal("Sucesso", response.Status);
        _mockEnderecoRepository.Verify(r => r.DeleteAsync(enderecoId), Times.Once);
    }


    [Fact]
    public async Task DeleteAsync_Deve_Retornar_ArgumentException_Quando_Dados_Invalidos()
    {
        // Arrange
        var enderecoId = Guid.NewGuid();

        _mockEnderecoRepository
            .Setup(r => r.DeleteAsync(enderecoId))
            .ReturnsAsync(false); 

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _enderecoService.DeleteAsync(enderecoId));
        Assert.Equal("Endereço não excluído da nossa base de dados.", exception.Message);

    }

}

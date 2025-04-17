using AutoMapper;
using FluentValidation;
using MedSync.Application.Requests;
using MedSync.Application.Services;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using static MedSync.Application.Requests.PessoaRequest;

namespace MedSync.Test.ApplicationTest.ServiceTest;

public class PessoaServiceTest
{
    private readonly Mock<IPessoaRepository> _mockPessoaRepository;
    private readonly Mock<IValidator<Pessoa>> _mockPessoaValidation;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAcessor;
    private readonly Mock<ILogger<PessoaService>> _mockLogger;

    private readonly PessoaService _pessoaService;

    public PessoaServiceTest()
    {
        _mockPessoaRepository = new Mock<IPessoaRepository>();
        _mockPessoaValidation = new Mock<IValidator<Pessoa>>();
        _mockMapper = new Mock<IMapper>();
        _mockHttpContextAcessor = new Mock<IHttpContextAccessor>();
        _mockLogger = new Mock<ILogger<PessoaService>>();

        _pessoaService = new PessoaService(
            _mockPessoaRepository.Object,
            _mockPessoaValidation.Object,
            _mockMapper.Object,
            _mockHttpContextAcessor.Object,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task CreateAsync_Deve_Criar_Pessoa_Quando_Dados_Validos()
    {
        //Arrange
        var pessoaRequest = new AdicionarPessoaRequest
        {
            Nome = "Teste teste",
            CPF = "12312312300",
            RG = "123456",
            DataNascimento = DateTime.Parse("1984-03-12"),
            Sexo = "f",
            Email = "Teste@gmail.com"            
        };

        var pessoa = new Pessoa
        {
            Nome = pessoaRequest.Nome,
            CPF = pessoaRequest.CPF,
            RG = pessoaRequest.RG,
            DataNascimento = pessoaRequest.DataNascimento,
            Sexo = pessoaRequest.Sexo,
            Email = pessoaRequest.Email,
        };

        _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<AdicionarPessoaRequest>()))
            .Returns(pessoa);
        _mockPessoaValidation.Setup(v => v.ValidateAsync(It.IsAny<Pessoa>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPessoaRepository.Setup(p => p.CreateAsync(pessoa))
            .ReturnsAsync(true);

        //Act
        var response = await _pessoaService.CreateAsync(pessoaRequest);

        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(false);
        _mockPessoaRepository.Verify(v => v.CreateAsync(pessoa), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Deve_Retornar_InvalidOperationException_Quando_Dados_InValidos()
    {
        //Arrange
        var pessoaRequest = new AdicionarPessoaRequest
        {
            Nome = "Teste teste",
            CPF = "12312312300",
            RG = "123456",
            DataNascimento = DateTime.Parse("1984-03-12"),
            Sexo = "f",
            Email = "Teste@gmail.com"
        };

        var pessoa = new Pessoa
        {
            Nome = pessoaRequest.Nome,
            CPF = pessoaRequest.CPF,
            RG = pessoaRequest.RG,
            DataNascimento = pessoaRequest.DataNascimento,
            Sexo = pessoaRequest.Sexo,
            Email = pessoaRequest.Email,
        };

        _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<AdicionarPessoaRequest>()))
            .Returns(pessoa);
        _mockPessoaValidation.Setup(v => v.ValidateAsync(It.IsAny<Pessoa>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPessoaRepository.Setup(p => p.CreateAsync(pessoa))
            .ReturnsAsync(false);

        //Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _pessoaService.CreateAsync(pessoaRequest));
        Assert.Equal("Falha ao criar pessoa.", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_Deve_Criar_Pessoa_Quando_Dados_Validos()
    {
        //Arrange
        var pessoaRequest = new AtualizarPessoaRequest
        {
            Nome = "Teste teste",
            CPF = "12312312300",
            RG = "123456",
            DataNascimento = DateTime.Parse("1984-03-12"),
            Sexo = "f",
            Email = "Teste@gmail.com"
        };

        var pessoa = new Pessoa
        {
            Nome = pessoaRequest.Nome,
            CPF = pessoaRequest.CPF,
            RG = pessoaRequest.RG,
            DataNascimento = pessoaRequest.DataNascimento,
            Sexo = pessoaRequest.Sexo,
            Email = pessoaRequest.Email,
        };

        _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<AtualizarPessoaRequest>()))
            .Returns(pessoa);
        _mockPessoaValidation.Setup(v => v.ValidateAsync(It.IsAny<Pessoa>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPessoaRepository.Setup(p => p.UpdateAsync(pessoa))
            .ReturnsAsync(true);

        //Act
        var response = await _pessoaService.UpdateAsync(pessoaRequest);

        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(false);
        _mockPessoaRepository.Verify(v => v.UpdateAsync(pessoa), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Deve_Retornar_InvalidOperationException_Quando_Dados_InValidos()
    {
        //Arrange
        var pessoaRequest = new AtualizarPessoaRequest
        {
            Nome = "Teste teste",
            CPF = "12312312300",
            RG = "123456",
            DataNascimento = DateTime.Parse("1984-03-12"),
            Sexo = "f",
            Email = "Teste@gmail.com"
        };

        var pessoa = new Pessoa
        {
            Nome = pessoaRequest.Nome,
            CPF = pessoaRequest.CPF,
            RG = pessoaRequest.RG,
            DataNascimento = pessoaRequest.DataNascimento,
            Sexo = pessoaRequest.Sexo,
            Email = pessoaRequest.Email,
        };

        _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<AtualizarPessoaRequest>()))
            .Returns(pessoa);
        _mockPessoaValidation.Setup(v => v.ValidateAsync(It.IsAny<Pessoa>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPessoaRepository.Setup(p => p.UpdateAsync(pessoa))
            .ReturnsAsync(false);

        //Act && Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _pessoaService.UpdateAsync(pessoaRequest));
        Assert.Equal("Falha na atualização em nossa base de dados.", exception.Message);
    }

    [Fact]
    public async Task DeleteAsync_Deve_Excluir_Quando_Dados_Validos()
    {
        //Arrange
        var pessoaId = Guid.NewGuid();

        _mockPessoaRepository.Setup(p => p.DeleteAsync(pessoaId))
            .ReturnsAsync(true);
        //Act
        var response = await _pessoaService.DeleteAsync(pessoaId);
        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error);
        _mockPessoaRepository.Verify(v => v.DeleteAsync(pessoaId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Retorna_InvalidOperation_Quando_Dados_InValidos()
    {
        //Arrange
        var pessoaId = Guid.NewGuid();

        _mockPessoaRepository.Setup(p => p.DeleteAsync(pessoaId))
            .ReturnsAsync(false);
        //Act && Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _pessoaService.DeleteAsync(pessoaId));
        Assert.Equal("Falha ao realizar exclusão.", exception.Message);
    }
}

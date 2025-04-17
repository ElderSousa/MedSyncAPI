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
using static MedSync.Application.Requests.EnderecoRequest;
using static MedSync.Application.Requests.PacienteRequest;
using static MedSync.Application.Requests.PessoaRequest;
using static MedSync.Application.Requests.TelefoneRequest;

namespace MedSync.Test.ApplicationTest.ServiceTest;

public class PacienteServiceTests
{
    private readonly Mock<IPacienteRepository> _mockPacienteRepository;
    private readonly Mock<IPessoaService> _mockPessoaService;
    private readonly Mock<IEnderecoService> _mockEnderecoService;
    private readonly Mock<ITelefoneService> _mockTelefoneService;
    private readonly Mock<IValidator<Paciente>> _mockPacienteValidation;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly Mock<ILogger<PacienteService>> _mockLogger;
    private readonly PacienteService _pacienteService;


    public PacienteServiceTests()
    {
        _mockPacienteRepository = new Mock<IPacienteRepository>();
        _mockPessoaService = new Mock<IPessoaService>();
        _mockEnderecoService = new Mock<IEnderecoService>();
        _mockTelefoneService = new Mock<ITelefoneService>();
        _mockPacienteValidation = new Mock<IValidator<Paciente>>();
        _mockMapper = new Mock<IMapper>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockLogger = new Mock<ILogger<PacienteService>>();

        _pacienteService = new PacienteService(
             _mockPacienteRepository.Object,
             _mockPessoaService.Object,
             _mockEnderecoService.Object,
             _mockTelefoneService.Object,
             _mockPacienteValidation.Object,
             _mockMapper.Object,
             _mockHttpContextAccessor.Object,
             _mockLogger.Object
         );

    }

    [Fact]
    public async Task CreatAsync_Deve_CriarPaciente_Quando_Dados_Validos()
    {
        //Arrange
        var pacienteResquest = new AdicionarPacienteRequest
        {
            Pessoa = new AdicionarPessoaRequest { CPF = "02749836305" },
            Endereco = new AdicionarEnderecoRequest(),
            Telefones = new List<AdicionarTelefoneRequest> { new AdicionarTelefoneRequest() }
        };

        Paciente paciente = new();
        paciente.ValidacaoCadastrar = true;

        var pessoa = new PessoaResponse { Id = Guid.Parse("d99883e1-22bb-4332-9c93-af3d7edf8eaa") };

        _mockMapper.Setup(m => m.Map<Paciente>(It.IsAny<AdicionarPacienteRequest>()))
            .Returns(paciente);
        _mockPacienteValidation.Setup(v => v.ValidateAsync(It.IsAny<Paciente>(), default))
           .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPessoaService.Setup(p => p.GetCPFAsync("02749836305"))
            .ReturnsAsync(pessoa);
        _mockPacienteRepository.Setup(pa => pa.CreateAsync(It.IsAny<Paciente>()))
            .ReturnsAsync(true);
        _mockEnderecoService.Setup(e => e.CreateAsync(It.IsAny<AdicionarEnderecoRequest>()))
            .ReturnsAsync(new Response { Status = "Success", Error = false });
        _mockTelefoneService.Setup(t => t.CreateAsync(It.IsAny<AdicionarTelefoneRequest>()))
            .ReturnsAsync(new Response { Status = "Success", Error = false });

        //Act
        var response = await _pacienteService.CreateAsync(pacienteResquest);

        //Assert
        Assert.NotNull(response);
        _mockPacienteRepository.Verify(pa => pa.CreateAsync(It.IsAny<Paciente>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_DeveCriarPessoa_QuandoCpfNaoExiste()
    {
        // Arrange
        var pacienteRequest = new AdicionarPacienteRequest
        {
            Pessoa = new AdicionarPessoaRequest { CPF = "99999999999" },
            Endereco = new AdicionarEnderecoRequest(),
            Telefones = new List<AdicionarTelefoneRequest> { new AdicionarTelefoneRequest() }
        };

        var paciente = new Paciente();
        paciente.ValidacaoCadastrar = true;
        var pessoaCriada = new PessoaResponse { Id = Guid.Parse("d99883e1-22bb-4332-9c93-af3d7edf8eaa") };

        _mockMapper.Setup(m => m.Map<Paciente>(It.IsAny<AdicionarPacienteRequest>()))
            .Returns(paciente);
        _mockPacienteValidation.Setup(v => v.ValidateAsync(It.IsAny<Paciente>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPessoaService.Setup(p => p.GetCPFAsync("99999999999"))
            .ReturnsAsync(new PessoaResponse());
        _mockPessoaService.Setup(p => p.CreateAsync(It.IsAny<AdicionarPessoaRequest>()))
            .ReturnsAsync(new Response { Status = "Success", Error = false });
        _mockPacienteRepository.Setup(r => r.CreateAsync(It.IsAny<Paciente>()))
            .ReturnsAsync(true);
        _mockEnderecoService.Setup(e => e.CreateAsync(It.IsAny<AdicionarEnderecoRequest>()))
            .ReturnsAsync(new Response { Status = "Success", Error = false });
        _mockTelefoneService.Setup(t => t.CreateAsync(It.IsAny<AdicionarTelefoneRequest>()))
            .ReturnsAsync(new Response { Status = "Success", Error = false });

        // Act
        var response = await _pacienteService.CreateAsync(pacienteRequest);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error);
        _mockPessoaService.Verify(p => p.CreateAsync(It.IsAny<AdicionarPessoaRequest>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Deve_Retornar_Excecao_Quando_Dados_Invalidos()
    {
        // Arrange

        var enderecoRequest = new AdicionarEnderecoRequest
        {
            Logradouro = "Rua teste",
            Numero = 123,
            Bairro = "Centro",
            Cidade = "São Paulo",
            Estado = "SP",
            CEP = "12345678"
        };

        var pacienteResquest = new AdicionarPacienteRequest
        {
            Pessoa = new AdicionarPessoaRequest { CPF = "02749836305" },
            Endereco = new AdicionarEnderecoRequest(),
            Telefones = new List<AdicionarTelefoneRequest> { new AdicionarTelefoneRequest() }
        };

        Paciente paciente = new();
        paciente.ValidacaoCadastrar = true;

        var pessoa = new PessoaResponse { Id = Guid.Parse("d99883e1-22bb-4332-9c93-af3d7edf8eaa") };

        _mockMapper.Setup(m => m.Map<Paciente>(It.IsAny<AdicionarPacienteRequest>()))
            .Returns(paciente);
        _mockPacienteValidation.Setup(v => v.ValidateAsync(It.IsAny<Paciente>(), default))
           .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPessoaService.Setup(p => p.GetCPFAsync("02749836305"))
            .ReturnsAsync(pessoa);
        _mockPacienteRepository.Setup(pa => pa.CreateAsync(It.IsAny<Paciente>()))
           .ReturnsAsync(false);


        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _pacienteService.CreateAsync(pacienteResquest));
        Assert.Equal("Falha ao adicionar paciente em nossa base de dados.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_Deve_Retornar_Excecao_Quando_Dados_Endereco_Invalidos()
    {
        // Arrange
        var pacienteRequest = new AdicionarPacienteRequest
        {
            Pessoa = new AdicionarPessoaRequest { CPF = "12345678900" },
            Endereco = new AdicionarEnderecoRequest(),
            Telefones = new List<AdicionarTelefoneRequest> { new AdicionarTelefoneRequest() }
        };

        var paciente = new Paciente();
        var pessoa = new PessoaResponse { Id = Guid.Parse("d99883e1-22bb-4332-9c93-af3d7edf8eaa") };

        _mockMapper.Setup(m => m.Map<Paciente>(It.IsAny<AdicionarPacienteRequest>()))
             .Returns(paciente);
        _mockPacienteValidation.Setup(v => v.ValidateAsync(It.IsAny<Paciente>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPessoaService.Setup(p => p.GetCPFAsync("12345678900"))
            .ReturnsAsync(pessoa);
        _mockPacienteRepository.Setup(r => r.CreateAsync(It.IsAny<Paciente>()))
            .ReturnsAsync(true);
        _mockEnderecoService.Setup(e => e.CreateAsync(It.IsAny<AdicionarEnderecoRequest>()))
            .ThrowsAsync(new Exception("Erro ao criar endereço"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _pacienteService.CreateAsync(pacienteRequest));
        Assert.Equal("Erro ao criar endereço", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_Deve_Retornar_Exception_Quando_Dados_Telefone_Invalidos()
    {
        // Arrange
        var pacienteRequest = new AdicionarPacienteRequest
        {
            Pessoa = new AdicionarPessoaRequest { CPF = "12345678900" },
            Endereco = new AdicionarEnderecoRequest(),
            Telefones = new List<AdicionarTelefoneRequest> { new AdicionarTelefoneRequest() }
        };

        var paciente = new Paciente();
        var pessoa = new PessoaResponse { Id = Guid.Parse("d99883e1-22bb-4332-9c93-af3d7edf8eaa") };

        _mockMapper.Setup(m => m.Map<Paciente>(It.IsAny<AdicionarPacienteRequest>()))
            .Returns(paciente);
        _mockPacienteValidation.Setup(v => v.ValidateAsync(It.IsAny<Paciente>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPessoaService.Setup(p => p.GetCPFAsync("12345678900"))
            .ReturnsAsync(pessoa);
        _mockPacienteRepository.Setup(r => r.CreateAsync(It.IsAny<Paciente>()))
            .ReturnsAsync(true);
        _mockTelefoneService.Setup(e => e.CreateAsync(It.IsAny<AdicionarTelefoneRequest>()))
            .ThrowsAsync(new Exception("Erro ao criar telefone"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _pacienteService.CreateAsync(pacienteRequest));
        Assert.Equal("Erro ao criar telefone", exception.Message);
    }
}

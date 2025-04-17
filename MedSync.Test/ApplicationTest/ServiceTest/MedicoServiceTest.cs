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
using static MedSync.Application.Requests.MedicoResquest;
using static MedSync.Application.Requests.PessoaRequest;
using static MedSync.Application.Requests.TelefoneRequest;

namespace MedSync.Test.ApplicationTest.ServiceTest;

public class MedicoServiceTest
{
    private readonly Mock<IMedicoRepository> _mockMedicoRepository;
    private readonly Mock<IPessoaService> _mockPessoaService;
    private readonly Mock<ITelefoneService> _mockTelefoneService;
    private readonly Mock<IValidator<Medico>> _mockMedicoValidator;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAcessor;
    private readonly Mock<ILogger<MedicoService>> _mockLogger;

    private readonly MedicoService _medicoService;

    public MedicoServiceTest()
    {
        _mockMedicoRepository = new Mock<IMedicoRepository>();
        _mockPessoaService = new Mock<IPessoaService>();
        _mockTelefoneService = new Mock<ITelefoneService>();
        _mockMedicoValidator = new Mock<IValidator<Medico>>();
        _mockMapper = new Mock<IMapper>();
        _mockHttpContextAcessor = new Mock<IHttpContextAccessor>();
        _mockLogger = new Mock<ILogger<MedicoService>>();

        _medicoService = new MedicoService(
            _mockMedicoRepository.Object,
            _mockPessoaService.Object,
            _mockTelefoneService.Object,
            _mockMedicoValidator.Object,
            _mockMapper.Object,
            _mockHttpContextAcessor.Object,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task CreateAsync_Deve_Criar_Medico_Quando_Dados_Validos()
    {
        //Arrange
        var medicoRequest = new AdicionarMedicoRequest
        {
            Pessoa = new AdicionarPessoaRequest { CPF = "02749836305" },
            Telefones = new List<AdicionarTelefoneRequest> 
            {
                new AdicionarTelefoneRequest
                {
                    MedicoId = Guid.NewGuid(),
                    Numero = "85999191919",
                    Tipo = Domain.Enum.TelefoneTipo.Celular
                } 
            }
        };

        var medico = new Medico();
        var pessoaResponse = new PessoaResponse { Id = Guid.NewGuid() };
        var telefone = new Telefone
        {
            MedicoId = medicoRequest.Telefones[0].MedicoId,
            Numero = medicoRequest.Telefones[0].Numero,
            Tipo = medicoRequest.Telefones[0].Tipo
        };

        _mockMapper.Setup(m => m.Map<Medico>(It.IsAny<AdicionarMedicoRequest>()))
            .Returns(medico);
        _mockMedicoValidator.Setup(v => v.ValidateAsync(It.IsAny<Medico>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPessoaService.Setup(p => p.GetCPFAsync(medicoRequest.Pessoa.CPF)).
            ReturnsAsync(new PessoaResponse());
        _mockPessoaService.Setup(p => p.CreateAsync(medicoRequest.Pessoa))
            .ReturnsAsync(new Response { Status = "Sucesso", Error = false});
        _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaResponse>()))
            .Returns(medico.Pessoa);
        _mockMedicoRepository.Setup(m => m.CreateAsync(medico))
            .ReturnsAsync(true);
        _mockTelefoneService.Setup(t => t.CreateAsync(medicoRequest.Telefones[0]))
            .ReturnsAsync(new Response { Status = "Sucesso", Error = false });
        _mockMapper.Setup(m => m.Map<Telefone>(medicoRequest.Telefones[0]))
            .Returns(telefone);

        //Act
        var response = await _medicoService.CreateAsync(medicoRequest);

        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error);
        _mockMedicoRepository.Verify(v => v.CreateAsync(medico), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Deve_Criar_Medico_Quando_TemCpf()
    {
        //Arrange
        var medicoRequest = new AdicionarMedicoRequest
        {
            Pessoa = new AdicionarPessoaRequest { CPF = "02749836305" },
            Telefones = new List<AdicionarTelefoneRequest>
            {
                new AdicionarTelefoneRequest
                {
                    MedicoId = Guid.NewGuid(),
                    Numero = "85999191919",
                    Tipo = Domain.Enum.TelefoneTipo.Celular
                }
            }
        };

        var medico = new Medico();
        var pessoaResponse = new PessoaResponse { Id = Guid.NewGuid() };
        var telefone = new Telefone
        {
            MedicoId = medicoRequest.Telefones[0].MedicoId,
            Numero = medicoRequest.Telefones[0].Numero,
            Tipo = medicoRequest.Telefones[0].Tipo
        };

        _mockMapper.Setup(m => m.Map<Medico>(It.IsAny<AdicionarMedicoRequest>()))
            .Returns(medico);
        _mockMedicoValidator.Setup(v => v.ValidateAsync(It.IsAny<Medico>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPessoaService.Setup(p => p.GetCPFAsync(medicoRequest.Pessoa.CPF)).
            ReturnsAsync(pessoaResponse);
        _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaResponse>()))
            .Returns(medico.Pessoa);
        _mockMedicoRepository.Setup(m => m.CreateAsync(medico))
            .ReturnsAsync(true);
        _mockTelefoneService.Setup(t => t.CreateAsync(medicoRequest.Telefones[0]))
            .ReturnsAsync(new Response { Status = "Sucesso", Error = false });
        _mockMapper.Setup(m => m.Map<Telefone>(medicoRequest.Telefones[0]))
            .Returns(telefone);

        //Act
        var response = await _medicoService.CreateAsync(medicoRequest);

        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error);
        _mockMedicoRepository.Verify(v => v.CreateAsync(medico), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Deve_Retornar_InvalidOperationException_Quando_Dados_Validos()
    {
        //Arrange
        var medicoRequest = new AdicionarMedicoRequest
        {
            Pessoa = new AdicionarPessoaRequest { CPF = "02749836305" },
            Telefones = new List<AdicionarTelefoneRequest>
            {
                new AdicionarTelefoneRequest
                {
                    MedicoId = Guid.NewGuid(),
                    Numero = "85999191919",
                    Tipo = Domain.Enum.TelefoneTipo.Celular
                }
            }
        };

        var medico = new Medico();
        var pessoaResponse = new PessoaResponse { Id = Guid.NewGuid() };
        var telefone = new Telefone
        {
            MedicoId = medicoRequest.Telefones[0].MedicoId,
            Numero = medicoRequest.Telefones[0].Numero,
            Tipo = medicoRequest.Telefones[0].Tipo
        };

        _mockMapper.Setup(m => m.Map<Medico>(It.IsAny<AdicionarMedicoRequest>()))
            .Returns(medico);
        _mockMedicoValidator.Setup(v => v.ValidateAsync(It.IsAny<Medico>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPessoaService.Setup(p => p.GetCPFAsync(medicoRequest.Pessoa.CPF)).
            ReturnsAsync(new PessoaResponse());
        _mockPessoaService.Setup(p => p.CreateAsync(medicoRequest.Pessoa))
            .ReturnsAsync(new Response { Status = "Sucesso", Error = false });
        _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaResponse>()))
            .Returns(medico.Pessoa);
        _mockMedicoRepository.Setup(m => m.CreateAsync(medico))
            .ReturnsAsync(false);

        //Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _medicoService.CreateAsync(medicoRequest));
        Assert.Equal("Falha ao adicionar médico em nossa base de dados.", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_Deve_Criar_Medico_Quando_Dados_Validos()
    {
        //Arrange
        var medicoRequest = new AtualizarMedicoRequest
        {
            Pessoa = new AtualizarPessoaRequest { CPF = "02749836305" },
            Telefones = new List<AtualizarTelefoneRequest>
            {
                new AtualizarTelefoneRequest
                {
                    MedicoId = Guid.NewGuid(),
                    Numero = "85999191919",
                    Tipo = Domain.Enum.TelefoneTipo.Celular
                }
            }
        };

        var medico = new Medico();
        var pessoaResponse = new PessoaResponse { Id = Guid.NewGuid() };
        var telefone = new Telefone
        {
            MedicoId = medicoRequest.Telefones[0].MedicoId,
            Numero = medicoRequest.Telefones[0].Numero,
            Tipo = medicoRequest.Telefones[0].Tipo
        };

        _mockMapper.Setup(m => m.Map<Medico>(It.IsAny<AtualizarMedicoRequest>()))
            .Returns(medico);
        _mockMedicoValidator.Setup(v => v.ValidateAsync(It.IsAny<Medico>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockMedicoRepository.Setup(m => m.UpdateAsync(medico))
            .ReturnsAsync(true);
        _mockPessoaService.Setup(m => m.UpdateAsync(medicoRequest.Pessoa))
            .ReturnsAsync(new Response { Status = "Sucesso", Error = false});
        _mockTelefoneService.Setup(t => t.UpdateAsync(medicoRequest.Telefones[0]))
            .ReturnsAsync(new Response { Status = "Sucesso", Error = false });

        //Act
        var response = await _medicoService.UpdateAsync(medicoRequest);

        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error);
        _mockMedicoRepository.Verify(v => v.UpdateAsync(medico), Times.Once);
    }


    [Fact]
    public async Task UpdateAsync_Deve_Retornar_InvalidOperationException_Quando_Dados_InValidos()
    {
        //Arrange
        var medicoRequest = new AtualizarMedicoRequest
        {
            Pessoa = new AtualizarPessoaRequest { CPF = "02749836305" },
            Telefones = new List<AtualizarTelefoneRequest>
            {
                new AtualizarTelefoneRequest
                {
                    MedicoId = Guid.NewGuid(),
                    Numero = "85999191919",
                    Tipo = Domain.Enum.TelefoneTipo.Celular
                }
            }
        };

        var medico = new Medico();
        var pessoaResponse = new PessoaResponse { Id = Guid.NewGuid() };
        var telefone = new Telefone
        {
            MedicoId = medicoRequest.Telefones[0].MedicoId,
            Numero = medicoRequest.Telefones[0].Numero,
            Tipo = medicoRequest.Telefones[0].Tipo
        };

        _mockMapper.Setup(m => m.Map<Medico>(It.IsAny<AtualizarMedicoRequest>()))
            .Returns(medico);
        _mockMedicoValidator.Setup(v => v.ValidateAsync(It.IsAny<Medico>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockMedicoRepository.Setup(m => m.UpdateAsync(medico))
            .ReturnsAsync(false);

        //Act && Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _medicoService.UpdateAsync(medicoRequest));
        Assert.Equal("Falha na atualização de médico em nossa base de dados.", exception.Message);
    }

    [Fact]
    public async Task DeleteAsync_Deve_Excluir_Medico_Quando_Dados_validos()
    {
        //Arrange
        var medicoId = Guid.NewGuid();

        _mockMedicoRepository.Setup(m => m.DeleteAsync(medicoId))
            .ReturnsAsync(true);
        //Act
        var response = await _medicoService.DeleteAsync(medicoId);
        //Assert
        Assert.NotNull(response);
        Assert.True(response.Status == "Sucesso");
        Assert.False(response.Error);
        _mockMedicoRepository.Verify(v => v.DeleteAsync(medicoId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Deve_Retorna_InvalidOperationException_Quando_Dados_Invalidos()
    {
        //Arrange
        var medicoId = Guid.NewGuid();

        _mockMedicoRepository.Setup(m => m.DeleteAsync(medicoId))
            .ReturnsAsync(false);
        //Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _medicoService.DeleteAsync(medicoId));
        Assert.Equal("Falha na exclusão da nossa base de dados.", exception.Message);
    }
}

using MedSync.Domain.Entities;

namespace MedSync.Domain.Interfaces;

public interface IPessoaRepository
{
    Task<bool> CreateAsync(Pessoa pessoa);
    bool Existe(Guid id, string CPF);
}

using MedSync.Domain.Entities;

namespace MedSync.Domain.Interfaces;

public interface IPessoaRepository
{
    Task<bool> CreateAsync(Pessoa pessoa);
    Task<Pessoa?> GetIdAsync(Guid id);
    Task <Pessoa?>GetCPFAsync(string cpf);
    Task<bool> UpdateAsync(Pessoa pessoa);
    Task<bool> DeleteAsync(Guid id);
    bool Existe(Guid id);
    bool CPFExiste(string? CPF);

}

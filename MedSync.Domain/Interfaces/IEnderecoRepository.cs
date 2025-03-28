using MedSync.Domain.Entities;

namespace MedSync.Domain.Interfaces;

public interface IEnderecoRepository
{
    Task<bool> CreateAsync(Endereco endereco);
    Task<Endereco?> GetIdAsync(Guid id);
    Task<Endereco?> GetCEPAsync(string cep);
    Task<bool> UpdateAsync(Endereco endereco);
    Task<bool> DeleteAsync(Guid id);
    bool Existe(Guid id);
}

using MedSync.Domain.Entities;

namespace MedSync.Domain.Interfaces;

public interface ITelefoneRepository
{
    Task<bool> CreateAsync(Telefone telefone);
    Task<Telefone?> GetIdAsync(Guid id);
    Task<Telefone?> GetNumeroAsync(string numero);
    Task<bool> UpdateAsync(Telefone telefone);
    Task<bool> DeleteAsync(Guid id);
    bool Existe(Guid id);
}

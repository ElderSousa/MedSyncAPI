using MedSync.Domain.Entities;

namespace MedSync.Domain.Interfaces;

public interface ITelefoneRepository
{
    Task<bool> CreateAsync(Telefone telefone);
    Task<IEnumerable<Telefone?>> GetAllAsync();
    Task<Telefone?> GetIdAsync(Guid id);
    Task<IEnumerable<Telefone?>> GetMedicoIdAsync(Guid medicoId);
    Task<IEnumerable<Telefone?>> GetPacienteIdAsync(Guid pacienteId);
    Task<Telefone?> GetNumeroAsync(string numero);
    Task<bool> UpdateAsync(Telefone telefone);
    Task<bool> DeleteAsync(Guid id);
    bool Existe(Guid id);
}

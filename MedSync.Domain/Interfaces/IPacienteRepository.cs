using MedSync.Domain.Entities;

namespace MedSync.Domain.Interfaces;

public interface IPacienteRepository
{
    Task<bool> CreateAsync(Paciente paciente);
    Task<Paciente?> GetIdAsync(Guid id);
    Task<IEnumerable<Paciente?>> GetAllAsync();
    Task<bool> UpdateAsync(Paciente paciente);
    Task<bool> DeleteAsync(Guid id);
    bool Existe(Guid id);
}

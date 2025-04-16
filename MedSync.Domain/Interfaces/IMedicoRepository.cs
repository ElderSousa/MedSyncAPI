using MedSync.Domain.Entities;

namespace MedSync.Domain.Interfaces
{
    public interface IMedicoRepository
    {
        Task<bool> CreateAsync(Medico medico);
        Task<Medico?> GetIdAsync(Guid id);
        Task<IEnumerable<Medico?>> GetAllAsync();
        Task<Medico?> GetCRMAsync(string crm);
        Task<bool> UpdateAsync(Medico pessoa);
        Task<bool> DeleteAsync(Guid id);
        bool Existe(Guid id);
        bool CRMExiste(string? crm);
    }
}

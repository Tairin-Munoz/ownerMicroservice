using ownerMicroservice.Domain.Entities;

namespace ownerMicroservice.Domain.Ports
{
    public interface IOwnerRepository
    {
        Task<IEnumerable<Owner>> GetAllAsync();
        Task<Owner?> GetByIdAsync(int id);
        Task<bool> CreateAsync(Owner owner, int userId);
        Task<bool> UpdateAsync(Owner owner, int userId);
        Task<bool> DeleteByIdAsync(int id, int userId);
    }
}

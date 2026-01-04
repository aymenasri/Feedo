using Feedo.Entities;

namespace Feedo.Repository
{
    /// <summary>
    /// Repository interface specific to Client entity.
    /// Extends the generic repository with Client-specific operations.
    /// </summary>
    public interface IClientRepository : IRepository<Client>
    {
        // Future custom methods can be added here, for example:
        // Task<IEnumerable<Client>> GetClientsByCityAsync(string city);
        // Task<IEnumerable<Client>> GetActiveClientsAsync();
    }
}

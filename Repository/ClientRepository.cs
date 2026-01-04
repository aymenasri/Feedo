using Feedo.Data;
using Feedo.Entities;

namespace Feedo.Repository
{
    /// <summary>
    /// Repository implementation for Client entity.
    /// Inherits generic CRUD operations and can add Client-specific methods.
    /// </summary>
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Client-specific methods can be implemented here
    }
}

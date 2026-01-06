using Feedo.Entities;
using Feedo.Repository;

namespace Feedo.Services
{
    /// <summary>
    /// Service implementation for Client business logic.
    /// Handles validation, business rules, and orchestrates repository operations.
    /// </summary>
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUtilisateurRepository _utilisateurRepository;
        private readonly Data.ApplicationDbContext _context;

        public ClientService(IClientRepository clientRepository, IUtilisateurRepository utilisateurRepository, Data.ApplicationDbContext context)
        {
            _clientRepository = clientRepository;
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _clientRepository.GetAllAsync();
        }

        public async Task<Client> GetClientByIdAsync(int id)
        {
            // Validate input
            if (id <= 0)
            {
                throw new ArgumentException("Client ID must be a positive number.", nameof(id));
            }

            // Retrieve from repository
            var client = await _clientRepository.GetByIdAsync(id);

            // Validate result
            if (client == null)
            {
                throw new KeyNotFoundException($"Client with ID {id} was not found.");
            }

            return client;
        }

    

        public async Task<Client> CreateClientAsync(Client client)
        {
            // Validate input
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Client cannot be null.");
            }

            // Apply business rules
            client.CreationA = DateTime.Now;

            // Delegate to repository
            return await _clientRepository.AddAsync(client);
        }

        public async Task UpdateClientAsync(Client client)
        {
            // Validate input
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Client cannot be null.");
            }

            // Check if client exists
            var existingClient = await _clientRepository.GetByIdAsync(client.Id);
            if (existingClient == null)
            {
                throw new KeyNotFoundException($"Client with ID {client.Id} was not found.");
            }

            // Delegate to repository
            await _clientRepository.UpdateAsync(client);
        }

        public async Task DeleteClientAsync(int id)
        {
            // Validate input
            if (id <= 0)
            {
                throw new ArgumentException("Client ID must be a positive number.", nameof(id));
            }

            // Check if client exists
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
            {
                throw new KeyNotFoundException($"Client with ID {id} was not found.");
            }

            // Delegate to repository
            
            // 1. Delete associated Orders (Cascade Delete manually)
            var clientOrders = _context.Orders.Where(o => o.ClientId == id);
            _context.Orders.RemoveRange(clientOrders);
            await _context.SaveChangesAsync();

            // 2. Delete the associated Utilisateur account to prevent orphan login
            if (!string.IsNullOrEmpty(client.Email))
            {
                var user = await _utilisateurRepository.GetByEmailAsync(client.Email);
                if (user != null)
                {
                    await _utilisateurRepository.DeleteAsync(user.Id);
                }
            }

            // 3. Delete the Client record
            await _clientRepository.DeleteAsync(id);
        }

        public async Task DeleteUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return;

            var user = await _utilisateurRepository.GetByEmailAsync(email);
            if (user != null)
            {
                await _utilisateurRepository.DeleteAsync(user.Id);
            }
        }
    }
}

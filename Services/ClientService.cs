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

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
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
            await _clientRepository.DeleteAsync(id);
        }
    }
}

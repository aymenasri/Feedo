using Feedo.Entities;

namespace Feedo.Services
{
    /// <summary>
    /// Service interface for Client business operations.
    /// Defines the contract for Client-related business logic.
    /// </summary>
    public interface IClientService
    {
        /// <summary>
        /// Retrieves all clients.
        /// </summary>
        /// <returns>Collection of all clients</returns>
        Task<IEnumerable<Client>> GetAllClientsAsync();

        /// <summary>
        /// Retrieves a specific client by ID with validation.
        /// </summary>
        /// <param name="id">The client ID</param>
        /// <returns>The client if found</returns>
        /// <exception cref="ArgumentException">Thrown when ID is invalid</exception>
        /// <exception cref="KeyNotFoundException">Thrown when client is not found</exception>
        Task<Client> GetClientByIdAsync(int id);

        /// <summary>
        /// Creates a new client with business rules applied.
        /// </summary>
        /// <param name="client">The client to create</param>
        /// <returns>The created client</returns>
        /// <exception cref="ArgumentNullException">Thrown when client is null</exception>
        Task<Client> CreateClientAsync(Client client);

        /// <summary>
        /// Updates an existing client with validation.
        /// </summary>
        /// <param name="client">The client to update</param>
        /// <exception cref="ArgumentNullException">Thrown when client is null</exception>
        /// <exception cref="KeyNotFoundException">Thrown when client is not found</exception>
        Task UpdateClientAsync(Client client);

        /// <summary>
        /// Deletes a client by ID.
        /// </summary>
        /// <param name="id">The ID of the client to delete</param>
        /// <exception cref="ArgumentException">Thrown when ID is invalid</exception>
        /// <exception cref="KeyNotFoundException">Thrown when client is not found</exception>
        Task DeleteClientAsync(int id);

        /// <summary>
        /// Hard deletes a user account by email (orphaned cleanup).
        /// </summary>
        /// <param name="email">Email of the user to delete</param>
        Task DeleteUserByEmailAsync(string email);
    }
}

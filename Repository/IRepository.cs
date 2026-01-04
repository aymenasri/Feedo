namespace Feedo.Repository
{
    /// <summary>
    /// Generic repository interface for basic CRUD operations.
    /// This interface can be used with any entity type.
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves all entities from the database.
        /// </summary>
        /// <returns>Collection of all entities</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Retrieves a single entity by its ID.
        /// </summary>
        /// <param name="id">The entity ID</param>
        /// <returns>The entity if found, otherwise null</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new entity to the database.
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>The added entity</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete</param>
        Task DeleteAsync(int id);
    }
}

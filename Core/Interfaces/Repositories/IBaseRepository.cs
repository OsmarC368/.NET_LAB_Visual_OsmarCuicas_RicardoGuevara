namespace Core.Interfaces.Repositories
{
    public interface IBaseRepository<Entity> where Entity : class 
    {
        ValueTask<Entity> GetByIdAsync(int id);
        Task<IEnumerable<Entity>> GetAllAsync();
        void Remove(Entity entity);
        void RemoveRange(IEnumerable<Entity> entities);
        Task Update(Entity entity);
        Task<Entity> AddAsync(Entity entity);
    }
}
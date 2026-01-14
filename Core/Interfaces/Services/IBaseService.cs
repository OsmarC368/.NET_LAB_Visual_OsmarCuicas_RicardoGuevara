using Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IBaseService<Entity> where Entity : class
    {
        
        Task<Response<Entity>> GetByIdAsync(int id);
        Task<Response<IEnumerable<Entity>>> GetAllAsync();
        Task<Response<Entity>> Create(Entity newEntity);
        Task<Response<Entity>> Update(int entityToBeUpdatedId, Entity newEntityValues);
        Task<Response<Entity>> Remove(int entityId);
    }
}
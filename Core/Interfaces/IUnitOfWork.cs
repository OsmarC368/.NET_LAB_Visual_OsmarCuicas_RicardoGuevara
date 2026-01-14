using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces.Entities;
using Core.Interfaces.Repositories;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserTypeRepository UserTypeRepository{ get; }
        IMeasureRepository  MeasureRepository{ get; }
        IUserRepository UserRepository { get; }
        IIngredientRepository IngredientRepository { get; }
        IIngredientsPerRecipeRepository IngredientsPerRecipeRepository { get; }
        IStepRepository StepRepository { get; }
        IRecipeRepository RecipeRepository { get; }

        Task<int> CommitAsync();
    }
}
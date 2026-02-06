using AppBlazor.Data;
using AppBlazor.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppBlazor.Data.Models.Core;

namespace AppBlazor.Data.Services
{
    public class RecipeService
    {
        private readonly TokenContainer _tokenContainer;

        public RecipeService(TokenContainer tokenContainer)
        {
            _tokenContainer = tokenContainer;
        }

        public async Task<Models.Core.Responses.Response<IEnumerable<Recipe>>> GetAllAsync()
        {
            var apiResponse = await Consumer.Execute<Models.Core.Responses.Response<IEnumerable<Recipe>>, object>(
                "Recipe",
                methodHttp.GET,
                null!,
                _tokenContainer.token
            );

            return apiResponse.Data ?? new Models.Core.Responses.Response<IEnumerable<Recipe>> { Ok = false, Mensaje = apiResponse.message };
        }

        public async Task<Models.Core.Responses.Response<Recipe>> GetByIdAsync(int id)
        {
            var apiResponse = await Consumer.Execute<Recipe, object>(
                $"Recipe/{id}",
                methodHttp.GET,
                null!,
                _tokenContainer.token
            );

            return new Models.Core.Responses.Response<Recipe>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }

        public async Task<Models.Core.Responses.Response<Recipe>> Create(Recipe newEntity)
        {
            var apiResponse = await Consumer.Execute<Recipe, Recipe>(
                "Recipe",
                methodHttp.POST,
                newEntity,
                _tokenContainer.token
            );

            return new Models.Core.Responses.Response<Recipe>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }

        public async Task<Models.Core.Responses.Response<Recipe>> Update(int id, Recipe newEntityValues)
        {
            var apiResponse = await Consumer.Execute<Recipe, Recipe>(
                $"Recipe/{id}",
                methodHttp.PUT,
                newEntityValues,
                _tokenContainer.token
            );

            return new Models.Core.Responses.Response<Recipe>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }

        public async Task<Models.Core.Responses.Response<Recipe>> Remove(int id)
        {
            var apiResponse = await Consumer.Execute<Recipe, object>(
                $"Recipe/{id}",
                methodHttp.DELETE,
                null!,
                _tokenContainer.token
            );

            return new Models.Core.Responses.Response<Recipe>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }
    }
}

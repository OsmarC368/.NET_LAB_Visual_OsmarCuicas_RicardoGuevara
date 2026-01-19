using Core.Entities;
using Core.Interfaces.Services;
using AppBlazor.Data;
using AppBlazor.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppBlazor.Data.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly TokenContainer _tokenContainer;

        public IngredientService(TokenContainer tokenContainer)
        {
            _tokenContainer = tokenContainer;
        }

        public async Task<Core.Responses.Response<IEnumerable<Ingredient>>> GetAllAsync()
        {
            var apiResponse = await Consumer.Execute<Core.Responses.Response<IEnumerable<Ingredient>>, object>(
                "Ingredient",
                methodHttp.GET,
                null!,
                _tokenContainer.token
            );

            return apiResponse.Data ?? new Core.Responses.Response<IEnumerable<Ingredient>> { Ok = false, Mensaje = apiResponse.message };
        }

        public async Task<Core.Responses.Response<Ingredient>> GetByIdAsync(int id)
        {
            var apiResponse = await Consumer.Execute<
                Core.Responses.Response<Ingredient>,
                object
            >(
                $"Ingredient/{id}",
                methodHttp.GET,
                null!,
                _tokenContainer.token
            );

            return apiResponse.Data
                ?? new Core.Responses.Response<Ingredient>
                {
                    Ok = false,
                    Mensaje = apiResponse.message
                };
        }

        public async Task<Core.Responses.Response<Ingredient>> Create(Ingredient newEntity)
        {
            //Console.WriteLine("TOKEN ACTUAL EN Create: " + _tokenContainer.token); 

            var apiResponse = await Consumer.Execute<Ingredient, Ingredient>(
                "Ingredient",
                methodHttp.POST,
                newEntity,
                _tokenContainer.token
            );

            //if (!apiResponse.Ok)
            //    Console.WriteLine("ERROR Create: " + apiResponse.message);

            return new Core.Responses.Response<Ingredient>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }

        public async Task<Core.Responses.Response<Ingredient>> Update(int id, Ingredient newEntityValues)
        {
            //Console.WriteLine("TOKEN ACTUAL EN Update: " + _tokenContainer.token); 

            var apiResponse = await Consumer.Execute<Ingredient, Ingredient>(
                $"Ingredient/{id}",
                methodHttp.PUT,
                newEntityValues,
                _tokenContainer.token
            );

            //if (!apiResponse.Ok)
            //    Console.WriteLine($"ERROR Update ({id}): " + apiResponse.message);

            return new Core.Responses.Response<Ingredient>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }

        public async Task<Core.Responses.Response<Ingredient>> Remove(int id)
        {
            //Console.WriteLine("TOKEN ACTUAL EN Remove: " + _tokenContainer.token); 

            var apiResponse = await Consumer.Execute<Ingredient, object>(
                $"Ingredient/{id}",
                methodHttp.DELETE,
                null!,
                _tokenContainer.token
            );

            //if (!apiResponse.Ok)
            //    Console.WriteLine($"ERROR Remove ({id}): " + apiResponse.message);

            return new Core.Responses.Response<Ingredient>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }
    }
}

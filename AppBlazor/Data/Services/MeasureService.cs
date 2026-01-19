using Core.Entities;
using Core.Interfaces.Services;
using AppBlazor.Data;
using AppBlazor.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppBlazor.Data.Services
{
    public class MeasureService
    {
        private readonly TokenContainer _tokenContainer;

        public MeasureService(TokenContainer tokenContainer)
        {
            _tokenContainer = tokenContainer;
        }

        public async Task<Core.Responses.Response<IEnumerable<Measure>>> GetAllAsync()
        {
            var apiResponse = await Consumer.Execute<Core.Responses.Response<IEnumerable<Measure>>, object>(
                "Measure",
                methodHttp.GET,
                null!,
                _tokenContainer.token
            );

            return apiResponse.Data ?? new Core.Responses.Response<IEnumerable<Measure>> { Ok = false, Mensaje = apiResponse.message };
        }

        public async Task<Response<Core.Responses.Response<Measure>>> GetByIdAsync(int id)
        {
            //Console.WriteLine("TOKEN ACTUAL EN GetByIdAsync: " + _tokenContainer.token); 

            var apiResponse = await Consumer.Execute<Core.Responses.Response<Measure>, object>(
                $"Measure/{id}",
                methodHttp.GET,
                null!,
                _tokenContainer.token
            );

            //if (!apiResponse.Ok)
            //    Console.WriteLine($"ERROR GetByIdAsync ({id}): " + apiResponse.message);

            return apiResponse;
        }

        public async Task<Core.Responses.Response<Measure>> Create(Measure newEntity)
        {
            //Console.WriteLine("TOKEN ACTUAL EN Create: " + _tokenContainer.token); 

            var apiResponse = await Consumer.Execute<Measure, Measure>(
                "Measure",
                methodHttp.POST,
                newEntity,
                _tokenContainer.token
            );

            //if (!apiResponse.Ok)
            //    Console.WriteLine("ERROR Create: " + apiResponse.message);

            return new Core.Responses.Response<Measure>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }

        public async Task<Core.Responses.Response<Measure>> Update(int id, Measure newEntityValues)
        {
            //Console.WriteLine("TOKEN ACTUAL EN Update: " + _tokenContainer.token); 

            var apiResponse = await Consumer.Execute<Measure, Measure>(
                $"Measure/{id}",
                methodHttp.PUT,
                newEntityValues,
                _tokenContainer.token
            );

            //if (!apiResponse.Ok)
            //    Console.WriteLine($"ERROR Update ({id}): " + apiResponse.message);

            return new Core.Responses.Response<Measure>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }

        public async Task<Core.Responses.Response<Measure>> Remove(int id)
        {
            //Console.WriteLine("TOKEN ACTUAL EN Remove: " + _tokenContainer.token); 

            var apiResponse = await Consumer.Execute<Measure, object>(
                $"Measure/{id}",
                methodHttp.DELETE,
                null!,
                _tokenContainer.token
            );

            //if (!apiResponse.Ok)
            //    Console.WriteLine($"ERROR Remove ({id}): " + apiResponse.message);

            return new Core.Responses.Response<Measure>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }
    }
}

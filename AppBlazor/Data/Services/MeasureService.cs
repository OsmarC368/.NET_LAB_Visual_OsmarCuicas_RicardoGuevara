using AppBlazor.Data;
using AppBlazor.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppBlazor.Data.Models.Core.Interfaces.Services;
using AppBlazor.Data.Models.Core;

namespace AppBlazor.Data.Services
{
    public class MeasureService
    {
        private readonly TokenContainer _tokenContainer;

        public MeasureService(TokenContainer tokenContainer)
        {
            _tokenContainer = tokenContainer;
        }

        public async Task<Models.Core.Responses.Response<IEnumerable<Measure>>> GetAllAsync()
        {
            var apiResponse = await Consumer.Execute<Models.Core.Responses.Response<IEnumerable<Measure>>, object>(
                "Measure",
                methodHttp.GET,
                null!,
                _tokenContainer.token
            );

            return apiResponse.Data ?? new Models.Core.Responses.Response<IEnumerable<Measure>> { Ok = false, Mensaje = apiResponse.message };
        }

        public async Task<Models.Core.Responses.Response<Measure>> GetByIdAsync(int id)
        {
            var apiResponse = await Consumer.Execute<
                Models.Core.Responses.Response<Measure>,
                object
            >(
                $"Measure/{id}",
                methodHttp.GET,
                null!,
                _tokenContainer.token
            );

            return apiResponse.Data 
                ?? new Models.Core.Responses.Response<Measure>
                {
                    Ok = false,
                    Mensaje = apiResponse.message
                };
        }

        public async Task<Models.Core.Responses.Response<Measure>> Create(Measure newEntity)
        {

            var apiResponse = await Consumer.Execute<Measure, Measure>(
                "Measure",
                methodHttp.POST,
                newEntity,
                _tokenContainer.token
            );


            return new Models.Core.Responses.Response<Measure>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }

        public async Task<Models.Core.Responses.Response<Measure>> Update(int id, Measure newEntityValues)
        {

            var apiResponse = await Consumer.Execute<Measure, Measure>(
                $"Measure/{id}",
                methodHttp.PUT,
                newEntityValues,
                _tokenContainer.token
            );


            return new Models.Core.Responses.Response<Measure>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }

        public async Task<Models.Core.Responses.Response<Measure>> Remove(int id)
        {

            var apiResponse = await Consumer.Execute<Measure, object>(
                $"Measure/{id}",
                methodHttp.DELETE,
                null!,
                _tokenContainer.token
            );


            return new Models.Core.Responses.Response<Measure>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBlazor.Components;
using AppBlazor.Data.Models.Core;

namespace AppBlazor.Data.Services
{
    public class StepUserService
    {
        private readonly TokenContainer _tokenContainer;

        public StepUserService(TokenContainer tokenContainer)
        {
            _tokenContainer = tokenContainer;
        }

        public async Task<Models.Core.Responses.Response<IEnumerable<StepUser>>> GetAllAsync()
        {
            var apiResponse = await Consumer.Execute<Models.Core.Responses.Response<IEnumerable<StepUser>>, object>(
                "StepUser",
                methodHttp.GET,
                null!,
                _tokenContainer.token
            );

            return apiResponse.Data ?? new Models.Core.Responses.Response<IEnumerable<StepUser>> { Ok = false, Mensaje = apiResponse.message };
        }

        public async Task<Models.Core.Responses.Response<StepUser>> GetByIdAsync(int id)
        {
            var apiResponse = await Consumer.Execute<StepUser, object>(
                $"StepUser/{id}",
                methodHttp.GET,
                null!,
                _tokenContainer.token
            );

            return new Models.Core.Responses.Response<StepUser>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }

        public async Task<Models.Core.Responses.Response<StepUser>> Create(StepUser newEntity)
        {
            var apiResponse = await Consumer.Execute<StepUser, StepUser>(
                "StepUser",
                methodHttp.POST,
                newEntity,
                _tokenContainer.token
            );

            return new Models.Core.Responses.Response<StepUser>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }

        public async Task<Models.Core.Responses.Response<StepUser>> Update(int id, StepUser newEntityValues)
        {
            var apiResponse = await Consumer.Execute<StepUser, StepUser>(
                $"StepUser/{id}",
                methodHttp.PUT,
                newEntityValues,
                _tokenContainer.token
            );

            return new Models.Core.Responses.Response<StepUser>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }

        public async Task<Models.Core.Responses.Response<StepUser>> Remove(int id)
        {
            var apiResponse = await Consumer.Execute<StepUser, object>(
                $"StepUser/{id}",
                methodHttp.DELETE,
                null!,
                _tokenContainer.token
            );

            return new Models.Core.Responses.Response<StepUser>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }
    
    }
}
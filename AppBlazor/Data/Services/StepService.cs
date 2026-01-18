using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBlazor.Components;
using Core.Entities;
using Microsoft.VisualBasic;

namespace AppBlazor.Data.Services
{
    public class StepService
    {
        private TokenContainer? _tokenContainer;
        public StepService (TokenContainer tokenContainer)
        {
            _tokenContainer = tokenContainer;
        }

        public async Task<Core.Responses.Response<IEnumerable<Step>>> GetAllAsync()
        {
            var response = await Consumer.Execute<Core.Responses.Response<IEnumerable<Step>>>(
                "Step",
                methodHttp.GET,
                null,
                _tokenContainer?.token);

            return response.Data ?? new Core.Responses.Response<IEnumerable<Step>> { Ok = false, Mensaje = response.message };
        }

        public async Task<Core.Responses.Response<Step>> GetByIdAsync(int id)
        {
            var apiResponse = await Consumer.Execute<Step, object>(
                $"Step/{id}",
                methodHttp.GET,
                null!,
                _tokenContainer.token
            );

            return new Core.Responses.Response<Step>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }

    }
}
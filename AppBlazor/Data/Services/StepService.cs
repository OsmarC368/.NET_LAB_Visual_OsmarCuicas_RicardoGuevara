using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBlazor.Components;
using AppBlazor.Data.Models;
using Core.Entities;
using Microsoft.VisualBasic;

namespace AppBlazor.Data.Services
{
    public class StepService
    {
        private TokenContainer? _tokenContainer;
        const string url = "Step";
        public StepService (TokenContainer tokenContainer)
        {
            _tokenContainer = tokenContainer;
        }

        public async Task<Core.Responses.Response<IEnumerable<Step>>> GetAllAsync()
        {
            var response = await Consumer.Execute<Core.Responses.Response<IEnumerable<Step>>>(
                url,
                methodHttp.GET,
                null,
                _tokenContainer?.token);

            return response.Data ?? new Core.Responses.Response<IEnumerable<Step>> { Ok = false, Mensaje = response.message };
        }

        public async Task<Core.Responses.Response<Step>> GetByIdAsync(int id)
        {
            var apiResponse = await Consumer.Execute<Step, object>(
                $"{url}/{id}",
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

        public async Task<Response<Core.Responses.Response<StepDTO>>> UpdateStep (StepDTO stepDTO)
        {
            Response<Core.Responses.Response<StepDTO>> response = new ();
            try
            {
                response = await 
                    Consumer
                    .Execute<Core.Responses.Response<StepDTO>, StepDTO>(
                        $"{url}/{stepDTO.id}",
                        methodHttp.PUT,
                        stepDTO,
                        _tokenContainer.token
                    );
            }
            catch (Exception ex)
            {
                response.Ok = false;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<Response<Core.Responses.Response<Step>>> DeleteStep (int id)
        {
            Response<Core.Responses.Response<Step>> response = new ();
            try
            {
                response = await 
                    Consumer
                    .Execute<Core.Responses.Response<Step>, object>(
                        $"{url}/{id}",
                        methodHttp.DELETE,
                        null,
                        _tokenContainer.token
                    );
            }
            catch (Exception ex)
            {
                response.Ok = false;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<Response<Core.Responses.Response<StepDTO>>> CreateStep(StepDTO stepDTO)
        {
            Response<Core.Responses.Response<StepDTO>> response = new Response<Core.Responses.Response<StepDTO>>();
            try
            {
                response = await 
                    Consumer
                    .Execute<Core.Responses.Response<StepDTO>, StepDTO>(
                        url,
                        methodHttp.POST,
                        stepDTO,
                        _tokenContainer.token
                        );

                return response;
            }
            catch (Exception ex)
            {
                response.Ok = false;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<Response<Core.Responses.Response<StepDTO>>> CreateStepImage (StepDTO stepDTO)
        {
            Response<Core.Responses.Response<StepDTO>> response = new ();
            try
            {
                var formData = new MultipartFormDataContent();


                formData.Add(new StringContent(stepDTO.name), "name");
                formData.Add(new StringContent(stepDTO.description), "description");
                formData.Add(new StringContent(stepDTO.Duration), "duration");
                formData.Add(new StringContent("Temp"), "imageURL");
                formData.Add(new StringContent(stepDTO.RecipeID.ToString()), "recipeID");
                formData.Add(new StringContent(stepDTO.RecipeID.ToString()), "recipeIDs");


                if (stepDTO.ImageFile != null)
                {
                    var stream = stepDTO.ImageFile.OpenReadStream();
                    var fileContent = new StreamContent(stream);
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(stepDTO.ImageFile.ContentType);
                    formData.Add(fileContent, "imageFile", stepDTO.ImageFile.Name);
                }

                response = await 
                    Consumer
                    .ExecuteMultipart<Core.Responses.Response<StepDTO>>(
                        $"{url}/image",
                        formData,
                        _tokenContainer.token
                    );
            }
            catch (Exception ex)
            {
                response.Ok = false;
                response.message = ex.Message;
            }
            return response;
        }


    }
}
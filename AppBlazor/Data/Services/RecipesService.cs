using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBlazor.Components;
using AppBlazor.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using System.IO;
using AppBlazor.Data.Models.Core;

namespace AppBlazor.Data.Services
{
    public class RecipesService
    {
        private TokenContainer? _tokenContainer;
        const string url = "Recipe";

        public RecipesService (TokenContainer tokenContainer)
        {
            _tokenContainer = tokenContainer;
        }

        public async Task<Models.Core.Responses.Response<IEnumerable<Recipe>>> GetAllRecipes ()
        {
            
            Response<Models.Core.Responses.Response<IEnumerable<Recipe>>> response = new ();
            try
            {
                response = await 
                    Consumer
                    .Execute<Models.Core.Responses.Response<IEnumerable<Recipe>>>(
                        $"{url}",
                        methodHttp.GET,
                        null,
                        _tokenContainer.token
                    );
                return response.Data ?? new Models.Core.Responses.Response<IEnumerable<Recipe>> { Ok = false, Mensaje = response.message };
            }
            catch (Exception ex)
            {
                response.Ok = false;
                response.message = ex.Message;
                return new Models.Core.Responses.Response<IEnumerable<Recipe>> { Ok = false, Mensaje = response.message };
            }
        }

        public async Task<Response<Models.Core.Responses.Response<Recipe>>> GetRecipeById (int id)
        {
            Response<Models.Core.Responses.Response<Recipe>> response = new ();
            try
            {
                response = await 
                    Consumer
                    .Execute<Models.Core.Responses.Response<Recipe>, object>(
                        $"{url}/{id}",
                        methodHttp.GET,
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

        public async Task<Response<Models.Core.Responses.Response<Recipe>>> DeleteRecipe (int id)
        {
            Response<Models.Core.Responses.Response<Recipe>> response = new ();
            try
            {
                response = await 
                    Consumer
                    .Execute<Models.Core.Responses.Response<Recipe>, object>(
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

        public async Task<Response<Models.Core.Responses.Response<RecipeDTO>>> CreateRecipeImage (RecipeDTO recipeDTO)
        {
            Response<Models.Core.Responses.Response<RecipeDTO>> response = new ();
            try
            {
                var formData = new MultipartFormDataContent();

                formData.Add(new StringContent(recipeDTO.name), "name");
                formData.Add(new StringContent(recipeDTO.description), "description");
                formData.Add(new StringContent(recipeDTO.type), "type");
                formData.Add(new StringContent(recipeDTO.difficultyLevel), "difficultyLevel");
                formData.Add(new StringContent(recipeDTO.visibility.ToString()), "visibility");
                formData.Add(new StringContent(recipeDTO.userIDR.ToString()), "userIDR");
                formData.Add(new StringContent(recipeDTO.userRID.ToString()), "userRID");
                formData.Add(new StringContent(recipeDTO.imageUrl), "imageUrl");

                if (recipeDTO.ImageFile != null)
                {
                    var stream = recipeDTO.ImageFile.OpenReadStream();
                    var fileContent = new StreamContent(stream);
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(recipeDTO.ImageFile.ContentType);
                    formData.Add(fileContent, "imageFile", recipeDTO.ImageFile.Name);
                }

                response = await 
                    Consumer
                    .ExecuteMultipart<Models.Core.Responses.Response<RecipeDTO>>(
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

        public async Task<Response<Models.Core.Responses.Response<RecipeDTO>>> CreateRecipe(RecipeDTO recipeDTO)
        {
            Response<Models.Core.Responses.Response<RecipeDTO>> response = new Response<Models.Core.Responses.Response<RecipeDTO>>();
            try
            {
                response = await 
                    Consumer
                    .Execute<Models.Core.Responses.Response<RecipeDTO>, RecipeDTO>(
                        url,
                        methodHttp.POST,
                        recipeDTO,
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

        public async Task<Response<Models.Core.Responses.Response<RecipeDTO>>> UpdateRecipe (RecipeDTO recipeDTO)
        {
            Response<Models.Core.Responses.Response<RecipeDTO>> response = new ();
            try
            {
                response = await 
                    Consumer
                    .Execute<Models.Core.Responses.Response<RecipeDTO>, RecipeDTO>(
                        $"{url}/{recipeDTO.Id}",
                        methodHttp.PUT,
                        recipeDTO,
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
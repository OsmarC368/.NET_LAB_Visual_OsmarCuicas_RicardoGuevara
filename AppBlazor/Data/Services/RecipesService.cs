using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBlazor.Components;
using AppBlazor.Data.Models;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

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

        public async Task<Core.Responses.Response<IEnumerable<Recipe>>> GetAllRecipes ()
        {
            
            Response<Core.Responses.Response<IEnumerable<Recipe>>> response = new ();
            try
            {
                response = await 
                    Consumer
                    .Execute<Core.Responses.Response<IEnumerable<Recipe>>>(
                        $"{url}",
                        methodHttp.GET,
                        null,
                        _tokenContainer.token
                    );
                return response.Data ?? new Core.Responses.Response<IEnumerable<Recipe>> { Ok = false, Mensaje = response.message };
            }
            catch (Exception ex)
            {
                response.Ok = false;
                response.message = ex.Message;
                return new Core.Responses.Response<IEnumerable<Recipe>> { Ok = false, Mensaje = response.message };
            }
        }

        public async Task<Response<Core.Responses.Response<Recipe>>> GetRecipeById (int id)
        {
            Response<Core.Responses.Response<Recipe>> response = new ();
            try
            {
                response = await 
                    Consumer
                    .Execute<Core.Responses.Response<Recipe>, object>(
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

        public async Task<Response<Core.Responses.Response<Recipe>>> DeleteRecipe (int id)
        {
            Response<Core.Responses.Response<Recipe>> response = new ();
            try
            {
                response = await 
                    Consumer
                    .Execute<Core.Responses.Response<Recipe>, object>(
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

        public async Task<Response<Core.Responses.Response<RecipeDTO>>> CreateRecipe (RecipeDTO recipeDTO)
        {
            Response<Core.Responses.Response<RecipeDTO>> response = new ();
            try
            {
                response = await 
                    Consumer
                    .Execute<Core.Responses.Response<RecipeDTO>, RecipeDTO>(
                        $"{url}",
                        methodHttp.POST,
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

        public async Task<Response<Core.Responses.Response<RecipeDTO>>> UpdateRecipe (RecipeDTO recipeDTO)
        {
            Response<Core.Responses.Response<RecipeDTO>> response = new ();
            try
            {
                response = await 
                    Consumer
                    .Execute<Core.Responses.Response<RecipeDTO>, RecipeDTO>(
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
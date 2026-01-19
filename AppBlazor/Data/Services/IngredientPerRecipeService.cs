using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBlazor.Components;
using AppBlazor.Data.Models;
using Core.Entities;

namespace AppBlazor.Data.Services
{
    public class IngredientPerRecipeService
    {
        private TokenContainer? _tokenContainer;
        const string url = "IngredientsPerRecipe";
        public IngredientPerRecipeService (TokenContainer tokenContainer)
        {
            _tokenContainer = tokenContainer;
        }

        public async Task<Core.Responses.Response<IEnumerable<IngredientsPerRecipe>>> GetAllAsync()
        {
            var response = await Consumer.Execute<Core.Responses.Response<IEnumerable<IngredientsPerRecipe>>>(
                url,
                methodHttp.GET,
                null,
                _tokenContainer?.token);

            return response.Data ?? new Core.Responses.Response<IEnumerable<IngredientsPerRecipe>> { Ok = false, Mensaje = response.message };
        }

        public async Task<Core.Responses.Response<IngredientsPerRecipe>> GetByIdAsync(int id)
        {
            var apiResponse = await Consumer.Execute<IngredientsPerRecipe, object>(
                $"{url}/{id}",
                methodHttp.GET,
                null!,
                _tokenContainer.token
            );

            return new Core.Responses.Response<IngredientsPerRecipe>
            {
                Ok = apiResponse.Ok,
                Mensaje = apiResponse.message,
                Datos = apiResponse.Data
            };
        }

        public async Task<Response<Core.Responses.Response<IngredientPerRecipeDTO>>> Update (IngredientPerRecipeDTO ingredientPerRecipeDTO)
        {
            Response<Core.Responses.Response<IngredientPerRecipeDTO>> response = new ();
            try
            {
                response = await 
                    Consumer
                    .Execute<Core.Responses.Response<IngredientPerRecipeDTO>, IngredientPerRecipeDTO>(
                        $"{url}/{ingredientPerRecipeDTO.id}",
                        methodHttp.PUT,
                        ingredientPerRecipeDTO,
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

        public async Task<Response<Core.Responses.Response<IngredientsPerRecipe>>> Delete (int id)
        {
            Response<Core.Responses.Response<IngredientsPerRecipe>> response = new ();
            try
            {
                response = await 
                    Consumer
                    .Execute<Core.Responses.Response<IngredientsPerRecipe>, object>(
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

        public async Task<Response<Core.Responses.Response<IngredientPerRecipeDTO>>> Create(IngredientPerRecipeDTO ingredientPerRecipeDTO)
        {
            Response<Core.Responses.Response<IngredientPerRecipeDTO>> response = new Response<Core.Responses.Response<IngredientPerRecipeDTO>>();
            try
            {
                response = await 
                    Consumer
                    .Execute<Core.Responses.Response<IngredientPerRecipeDTO>, IngredientPerRecipeDTO>(
                        url,
                        methodHttp.POST,
                        ingredientPerRecipeDTO,
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


    }
}
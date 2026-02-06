using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBlazor.Components;
using AppBlazor.Data.Models;
using AppBlazor.Data.Models.Core;

namespace AppBlazor.Data.Services
{
    public class AuthService
    {
        const string url = "User";
         private TokenContainer? _tokenContainer;
         public AuthService (TokenContainer tokenContainer)
        {
            _tokenContainer = tokenContainer;
        }
        public async Task<Response<Models.Core.Responses.Response<Models.Core.Responses.ResponseLogin>>> Login(UserDTO user)
        {
            Response<Models.Core.Responses.Response<Models.Core.Responses.ResponseLogin>> response = new Response<Models.Core.Responses.Response<Models.Core.Responses.ResponseLogin>>();
            try
            {
                response = await 
                    Consumer
                    .Execute<Models.Core.Responses.Response<Models.Core.Responses.ResponseLogin>, UserDTO>(
                        $"{url}/login",
                        methodHttp.POST,
                        user);

                return response;
            }
            catch (Exception ex)
            {
                response.Ok = false;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<Response<Models.Core.Responses.Response<User>>> GetByIdAsync(int id)
        {
            var apiResponse = await Consumer.Execute<Models.Core.Responses.Response<User>, object>(
                $"{url}/{id}",
                methodHttp.GET,
                null!,
                _tokenContainer?.token
            );

            return apiResponse;


        }
        public async Task<Response<Models.Core.Responses.Response<UserDTO>>> Register(UserDTO user)
        {
            Response<Models.Core.Responses.Response<UserDTO>> response = new Response<Models.Core.Responses.Response<UserDTO>>();
            try
            {
                response = await 
                    Consumer
                    .Execute<Models.Core.Responses.Response<UserDTO>, UserDTO>(
                        $"{url}/register",
                        methodHttp.POST,
                        user);

                return response;
            }
            catch (Exception ex)
            {
                response.Ok = false;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<Response<User>> UpdateUser(UserDTO updatedUser, string token)
        {
            Response<User> response = new Response<User>();
            try
            {
                response = await 
                    Consumer
                    .Execute<User, UserDTO>(
                        url,
                        methodHttp.PUT,
                        updatedUser,
                        token);

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
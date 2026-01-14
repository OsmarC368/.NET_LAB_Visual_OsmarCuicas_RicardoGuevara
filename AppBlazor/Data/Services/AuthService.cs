using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBlazor.Data.Models;
using Core.Entities;

namespace AppBlazor.Data.Services
{
    public class AuthService
    {
        const string url = "User";
        public async Task<Response<Core.Responses.Response<Core.Responses.ResponseLogin>>> Login(UserDTO user)
        {
            Response<Core.Responses.Response<Core.Responses.ResponseLogin>> response = new Response<Core.Responses.Response<Core.Responses.ResponseLogin>>();
            try
            {
                response = await 
                    Consumer
                    .Execute<Core.Responses.Response<Core.Responses.ResponseLogin>, UserDTO>(
                        $"{url}/login",
                        methodHttp.POST,
                        user);

                var x = response.Data.Datos.jwt;
                return response;
            }
            catch (Exception ex)
            {
                response.Ok = false;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<Response<string>> Register(UserDTO user)
        {
            Response<string> response = new Response<string>();
            try
            {
                response = await 
                    Consumer
                    .Execute<string, UserDTO>(
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
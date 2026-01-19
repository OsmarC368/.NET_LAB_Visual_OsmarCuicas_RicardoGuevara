using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace AppBlazor.Data
{
    public class Consumer
    {
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public static HttpMethod GetHttpMethod(methodHttp method)
        {
            switch (method)
            {
                case methodHttp.POST:
                    return HttpMethod.Post;
                case methodHttp.GET:
                    return HttpMethod.Get;
                case methodHttp.PUT:
                    return HttpMethod.Put;
                case methodHttp.DELETE:
                    return HttpMethod.Delete;
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, null);
            }
        }

        public static async Task<Response<R>> Execute<R,T>(string endpoint, methodHttp method, T Data, string? token = null)
        {
            string urlBaseApi = "https://api-recipes-tg3p.onrender.com/api";

            Response<R> response = new();

            try
            {
                using (HttpClient client = new())
                {
                    string url = $"{urlBaseApi}/{endpoint}";
                    string dataString = JsonConvert.SerializeObject(method != methodHttp.GET ? method != methodHttp.DELETE  ? Data : "" : "");
                    var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(dataString));
                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    var request = new HttpRequestMessage(GetHttpMethod(method), url)
                    {
                        Content = method != methodHttp.GET ? method != methodHttp.DELETE ? byteContent : null : null
                    };

                    if(token != null)
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    using (HttpResponseMessage responseApi = await client.SendAsync(request))
                    {
                        using (HttpContent content = responseApi.Content)
                        {
                            response.statusCode = responseApi.StatusCode.ToString();
                            
                            string dataResponse = await content.ReadAsStringAsync();
                            if (dataResponse != null)
                            {
                                try
                                {
                                    //var temp = JsonConvert.DeserializeObject<Core.Responses.Response<R>>(dataResponse);
                                    response.Data = JsonConvert.DeserializeObject<R>(dataResponse);
                                    response.Ok = true;
                                }
                                catch (Exception ex)
                                {
                                    // response.Ok = response.statusCode != "400";
                                    // if (response.statusCode == "InternalServerError" || response.statusCode == "NotFound")
                                    //     response.Ok = false;
                                    response.message = dataResponse;
                                }
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }

        public static async Task<Response<T>> Execute<T>(string endpoint, methodHttp methodHttp, T Data, string? token = null)
        {
            string urlBaseApi = "https://api-recipes-tg3p.onrender.com/api";
            Response<T> response = new();
            try
            {
                // Instancia de la clase HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // URL
                    string url = @$"{urlBaseApi}/{endpoint}";
                    //Data - informacion a mandar
                    string dataString = JsonConvert.SerializeObject(methodHttp != methodHttp.GET ? methodHttp != methodHttp.DELETE ? Data : "" : "");
                    var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(dataString));
                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    if(token != null)
                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", token);
                    //var content = new StringContent(dataString, Encoding.UTF8, "application/json");
                    // Hacer la peticion

                    //el tipo de peticion 
                    var request = new HttpRequestMessage(GetHttpMethod(methodHttp), url)
                    {
                        Content = methodHttp != methodHttp.GET ? methodHttp != methodHttp.DELETE ? byteContent : null : null,
                    };
                    //request.Content = byteContent;

                    using (HttpResponseMessage responseApi = await client.SendAsync(request))
                    {
                        using (HttpContent content = responseApi.Content)
                        {
                            string dataResponse = await content.ReadAsStringAsync();
                            if (dataResponse != null)
                            {
                                try
                                {
                                    response.Data = JsonConvert.DeserializeObject<T>(dataResponse);
                                    response.Ok = true;
                                }
                                catch (Exception ex) {
                                    response.message = dataResponse;
                                }
                            }
                            response.statusCode = responseApi.StatusCode.ToString();
                        }

                    }
                    ;
                }



            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }

        public static async Task<Response<T>> ExecuteMultipart<T>(string endpoint, MultipartFormDataContent formData, string? token = null)
        {
            string urlBaseApi = "https://api-recipes-tg3p.onrender.com/api";
            Response<T> response = new();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"{urlBaseApi}/{endpoint}";
                    if (token != null)
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var request = new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = formData
                    };

                    using (HttpResponseMessage responseApi = await client.SendAsync(request))
                    {
                        using (HttpContent content = responseApi.Content)
                        {
                            response.statusCode = responseApi.StatusCode.ToString();
                            string dataResponse = await content.ReadAsStringAsync();
                            if (dataResponse != null)
                            {
                                try
                                {
                                    response.Data = JsonConvert.DeserializeObject<T>(dataResponse);
                                    try
                                    {
                                        if(response.Data?.Equals("Bad Request") == true)
                                        {
                                            response.Ok = false;
                                            return response;
                                        }
                
                                    }
                                    catch(Exception ex) 
                                    { 
                                        response.Ok = true;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    response.message = dataResponse;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }
    }
}
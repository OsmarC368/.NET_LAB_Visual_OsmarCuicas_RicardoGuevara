using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AppBlazor.Data
{
    public class Consumer
    {
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public static HttpMethod GetHttpMethod(methodHttp method)
        {
            return method switch
            {
                methodHttp.POST => HttpMethod.Post,
                methodHttp.GET => HttpMethod.Get,
                methodHttp.PUT => HttpMethod.Put,
                methodHttp.DELETE => HttpMethod.Delete,
                _ => throw new ArgumentOutOfRangeException(nameof(method), method, null)
            };
        }

        public static async Task<Response<R>> Execute<R, T>(
            string endpoint,
            methodHttp method,
            T Data,
            string? token = null)
        {
            string urlBaseApi = "https://api-recipes-tg3p.onrender.com/api";
            Response<R> response = new();

            try
            {
                using (HttpClient client = new())
                {
                    string url = $"{urlBaseApi}/{endpoint}";

                    string dataString = JsonConvert.SerializeObject(
                        method != methodHttp.GET && method != methodHttp.DELETE ? Data : ""
                    );

                    var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(dataString));
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var request = new HttpRequestMessage(GetHttpMethod(method), url)
                    {
                        Content = method != methodHttp.GET && method != methodHttp.DELETE
                            ? byteContent
                            : null
                    };

                    if (!string.IsNullOrEmpty(token))
                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", token);

                    using (HttpResponseMessage responseApi = await client.SendAsync(request))
                    {
                        response.statusCode = responseApi.StatusCode.ToString();

                        string apiContent = await responseApi.Content.ReadAsStringAsync();

                        if (!responseApi.IsSuccessStatusCode)
                        {
                            response.Ok = false;
                            response.message = apiContent;
                            return response;
                        }

                        if (!string.IsNullOrEmpty(apiContent))
                        {
                            response.Data = JsonConvert.DeserializeObject<R>(apiContent);
                            response.Ok = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Ok = false;
                response.message = ex.Message;
            }

            return response;
        }

        public static async Task<Response<T>> Execute<T>(
            string endpoint,
            methodHttp method,
            T Data,
            string? token = null)
        {
            string urlBaseApi = "https://api-recipes-tg3p.onrender.com/api";
            Response<T> response = new();

            try
            {
                using (HttpClient client = new())
                {
                    string url = $"{urlBaseApi}/{endpoint}";

                    string dataString = JsonConvert.SerializeObject(
                        method != methodHttp.GET && method != methodHttp.DELETE ? Data : ""
                    );

                    var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(dataString));
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    if (!string.IsNullOrEmpty(token))
                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", token);

                    var request = new HttpRequestMessage(GetHttpMethod(method), url)
                    {
                        Content = method != methodHttp.GET && method != methodHttp.DELETE
                            ? byteContent
                            : null
                    };

                    using (HttpResponseMessage responseApi = await client.SendAsync(request))
                    {
                        response.statusCode = responseApi.StatusCode.ToString();

                        string apiContent = await responseApi.Content.ReadAsStringAsync();

                        if (!responseApi.IsSuccessStatusCode)
                        {
                            response.Ok = false;
                            response.message = apiContent;
                            return response;
                        }

                        if (!string.IsNullOrEmpty(apiContent))
                        {
                            response.Data = JsonConvert.DeserializeObject<T>(apiContent);
                            response.Ok = true;
                        }
                    }
                }
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

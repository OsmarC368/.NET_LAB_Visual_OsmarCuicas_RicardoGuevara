using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;

namespace AppBlazor.Data.Services
{
    public class responseUpload
    {
        public string videoId { get; set;} = string.Empty;
        public string title { get; set;} = string.Empty;
        public string signedUrl { get; set;} = string.Empty;
    }

    public class UploadVideoService
    {

        public string token = "7a78243d0e8735398f2d03b2422d3477020e3da78a2cb7fab6b7ffd0af928900";

        public async Task fetchVideo()
        {
            string url = "https://app.ignitevideo.cloud/api/videos/69857fed30af8da7eeba0749";
            try
            {
                using (HttpClient client = new())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var response = await client.GetAsync(url);
                    var x = 0;
                }
            }
            catch (Exception ex)
            {
                var x = ex.ToString();
            }
        }

        public async Task<string> Upload(IBrowserFile browserFile)
        {
            string igniteURL = "https://app.ignitevideo.cloud/api/videos/upload";
            try
            {
                using (HttpClient client = new ())
                {
                    object data = new { title = browserFile.Name, visibility = "public"};
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var response = await client.PutAsJsonAsync(igniteURL, data);
                    var igniteData = await response.Content.ReadFromJsonAsync<responseUpload>();
                    if(igniteData?.signedUrl != null)
                    {
                        client.DefaultRequestHeaders.Authorization = null;
                        using var stream = browserFile.OpenReadStream(maxAllowedSize: 1024 * 1024 * 500);
                        var content = new StreamContent(stream);
                        content.Headers.ContentType = new MediaTypeHeaderValue(browserFile.ContentType);
                        content.Headers.ContentLength = browserFile.Size;

                        var uploadURL = await client.PutAsync(igniteData.signedUrl, content);
                        if (!uploadURL.IsSuccessStatusCode)
                        {
                            var errorContent = await uploadURL.Content.ReadAsStringAsync();
                            Console.WriteLine($"Error de S3: {errorContent}");
                        }
                        return igniteData.videoId;
                    }
                }

            }
            catch (Exception ex)
            {
                var x = ex.ToString();
            }
            return "";
        }
    }
}
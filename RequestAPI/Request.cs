using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace RequestAPI
{
    public static class Request
    {
        static public DataReturned<T> RequestAPISensedia<T>(string requestUrl, string bodyRaw, Dictionary<string, string> Headers = null, Authentication authentication = null)
        {
            var sw = new Stopwatch();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                if (authentication != null)
                {
                    var byteArray = Encoding.ASCII.GetBytes(authentication.ToString());
                    var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    client.DefaultRequestHeaders.Authorization = header;
                }
                if ((Headers != null))
                    if (Headers.Count > 0)
                    {
                        foreach (var header in Headers)
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                        }
                    }
                try
                {
                    sw.Start();
                    HttpResponseMessage respToken = client.PostAsync(requestUrl, new StringContent(bodyRaw, Encoding.UTF8, "application/json")).Result;
                    sw.Stop();
                    string contentJson = respToken.Content.ReadAsStringAsync().Result;
                    if (typeof(T) == typeof(String))
                        return (new DataReturned<T>(respToken.StatusCode, sw.ElapsedMilliseconds, contentJson.Length, (T)(object)contentJson));
                    else
                        return (new DataReturned<T>(respToken.StatusCode, sw.ElapsedMilliseconds, contentJson.Length, JsonConvert.DeserializeObject<T>(contentJson)));
                }
                catch (AggregateException ex)
                {
                    sw.Stop();
                    //System.Net.Http.HttpRequestException ou System.Net.Sockets.SocketException
                    if (ex.InnerExceptions.Any(x => x.GetType().ToString() == "System.Net.Http.HttpRequestException"))
                    {
                        throw new HttpRequestException("Comunicação não estabelecida. Não foi possível estabelecer comunicação com o servidor.");
                    }
                }
                return null;
            }
        }
        static public DataReturned<T> RequestAPISensedia<T>(string requestUrl, object bodyRaw, Dictionary<string, string> Headers = null, Authentication authentication = null)
        {
            string content = JsonConvert.SerializeObject(bodyRaw);
            return RequestAPISensedia<T>(requestUrl, content, Headers, authentication);
        }

    }
}

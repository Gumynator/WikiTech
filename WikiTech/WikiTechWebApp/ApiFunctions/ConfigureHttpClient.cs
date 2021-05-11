//Auteur    : Loris habegger
//Date      : 01.05.2021
//Fichier   : ConfigureHttpClient.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WikiTechWebApp.ApiFunctions
{
    public class ConfigureHttpClient
    {

        internal static string apiUrl = "http://localhost:59601/Api/";
        public static HttpClient configureHttpClient(HttpClient client)
        {
            if (client.BaseAddress != null)
            {
                client = new HttpClient();
            }
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("ApiKey", "61c08ad1-0823-4c38-9853-700675e3c8fc");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            return client;


        }
    }
}

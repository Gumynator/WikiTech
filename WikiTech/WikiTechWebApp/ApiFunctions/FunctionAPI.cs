
//Auteur    : Loris habegger, Marco Pancini
//Date      : 01.05.2021
//Fichier   : FunctionAPI.cs

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WikiTechAPI.Models;
using WikiTechWebApp.Exceptions;

namespace WikiTechWebApp.ApiFunctions
{
    public class FunctionAPI
    {

        /* //Create a new Article
         internal static async Task<Article> AddArticle(Article _article)
         {

             Article resultarticle;

             using (var httpClient = new HttpClient())
             {
                 StringContent content = new StringContent(JsonConvert.SerializeObject(_article), Encoding.UTF8, "application/json");

                 using var response = await httpClient.PostAsync(ConfigureHttpClient.apiUrl + "Articles", content);
                 string apiResponse = await response.Content.ReadAsStringAsync();
                 resultarticle = JsonConvert.DeserializeObject<Article>(apiResponse);
             }
             return resultarticle;
         }

         ///get all articles - inutile
         internal static async Task<Article[]> GetArticlesAsync(HttpClient client)
         {
             Article[] product = null;
             HttpResponseMessage response = await client.GetAsync("api/Articles");
             if (response.IsSuccessStatusCode)
             {
                 product = await response.Content.ReadAsAsync<Article[]>();
             }
             return product;
         }

         ///get article by id - inutile
         internal static async Task<Article> GetArticleAsync(HttpClient client, short? id)
         {
             Article product = null;
             HttpResponseMessage response = await client.GetAsync("api/Article/" + id);
             if (response.IsSuccessStatusCode)
             {
                 product = await response.Content.ReadAsAsync<Article>();
             }
             return product;
         }

         */

        //Add Tag(s) to an article
        internal static async Task<List<Referencer>> AddTagToArticle(List<string> _idtags, int _idArticle)
        {

            var idtags = _idtags;
            int idArticle = _idArticle;

            List<Referencer> resultatReferences = new List<Referencer>();

            try
            {

                for (int i = 0; i < idtags.Count; i++)
                {
                    Referencer currentAdd = new Referencer();
                    String currentTag = idtags[i];

                    currentAdd.IdArticle = idArticle;
                    currentAdd.IdTag = Int32.Parse(currentTag);

                    Referencer resultRef;

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("ApiKey", "61c08ad1-0823-4c38-9853-700675e3c8fc");
                        StringContent referenceContent = new StringContent(JsonConvert.SerializeObject(currentAdd), Encoding.UTF8, "application/json");

                        using var ReferenceResponse = await httpClient.PostAsync(ConfigureHttpClient.apiUrl + "Referencers", referenceContent);
                        string ReferenceApiResponse = await ReferenceResponse.Content.ReadAsStringAsync();
                        resultRef = JsonConvert.DeserializeObject<Referencer>(ReferenceApiResponse);

                        resultatReferences.Add(resultRef);
                    }

                }
                return resultatReferences;

            }
            catch (ExceptionLiaisonApi e)
            {
                Console.WriteLine(e.getMessage());
                return resultatReferences;
            }

        }

        ///get abonnement by id
        internal static async Task<Abonnement> GetAbonnementByIdAsync(HttpClient client, int? id)
        {
            Abonnement subscription = null;
            HttpResponseMessage response = await client.GetAsync("Abonnements/" + id);
            if (response.IsSuccessStatusCode)
            {

                subscription = await response.Content.ReadAsAsync<Abonnement>();
            }

            return subscription;
        }

        internal static async Task<AspNetUsers> GetUserByIdAsync(HttpClient client, string? id)
        {
            AspNetUsers user = null;
            HttpResponseMessage response = await client.GetAsync("AspNetUsers/" + id);
            if (response.IsSuccessStatusCode)
            {

                user = await response.Content.ReadAsAsync<AspNetUsers>();
            }

            return user;
        }



        internal static async Task<Ville> GetVilleByIdAsync(HttpClient client, int? id)
        {
            Ville ville = null;
            HttpResponseMessage response = await client.GetAsync("Villes/" + id);
            if (response.IsSuccessStatusCode)
            {
                ville = await response.Content.ReadAsAsync<Ville>();
            }

            return ville;
        }


        internal static async Task<Facture> GetFacturesByUserIdAsync(HttpClient client, string? userId)
        {
            Facture factures = null;
            HttpResponseMessage response = await client.GetAsync("Factures/FacturesByUserId/" + userId);
            if (response.IsSuccessStatusCode)
            {

                factures = await response.Content.ReadAsAsync<Facture>();
            }

            return factures;
        }
        internal static async void IncreasePointForUser(HttpClient client, string _idUser, int _nbPointToAdd)
        {

            using var response = await client.PutAsJsonAsync(ConfigureHttpClient.apiUrl + "AspNetUsers/addpoint/" + _idUser, _nbPointToAdd);
            string apiResponse = await response.Content.ReadAsStringAsync();


        }
    }
}

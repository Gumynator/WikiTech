using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WikiTechAPI.Models;

namespace WikiTechWebApp.ApiFunctions
{
    public class FunctionAPI
    {

        //Create a new Article
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



        //Add Tag to an article
        internal static async Task<List<Referencer>> AddTagToArticle(List<string> _idtags, int _idArticle)
        {

            var idtags = _idtags;
            int idArticle = _idArticle;

            List<Referencer> resultatReferences = new List<Referencer>();



            for (int i = 0; i < idtags.Count; i++)
            {
                Referencer currentAdd = new Referencer();
                String currentTag = idtags[i];

                currentAdd.IdArticle = idArticle;
                currentAdd.IdTag = Int32.Parse(currentTag);

                Referencer resultRef;

                using (var httpClient = new HttpClient())
                {
                    StringContent referenceContent = new StringContent(JsonConvert.SerializeObject(currentAdd), Encoding.UTF8, "application/json");

                    using var ReferenceResponse = await httpClient.PostAsync(ConfigureHttpClient.apiUrl + "Referencers", referenceContent);
                    string ReferenceApiResponse = await ReferenceResponse.Content.ReadAsStringAsync();
                    resultRef = JsonConvert.DeserializeObject<Referencer>(ReferenceApiResponse);

                    resultatReferences.Add(resultRef);
                }

            }
            return resultatReferences;
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

    }
}

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
    public class FunctionArticles
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






    }
}

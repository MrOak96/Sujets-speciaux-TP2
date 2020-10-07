using ApiHelper.Models;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiHelper
{
    public class DogApiProcessor
    {

        public static async Task<List<string>> LoadBreedList()
        {
            string url = "https://dog.ceo/api/breeds/list/all";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    BreedModel result = await response.Content.ReadAsAsync<BreedModel>();
                    return result.message.Keys.ToList();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<string> GetImageUrl(string breed)
        {
            string url = $"https://dog.ceo/api/breed/{breed}/images/random";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    DogModel res = await response.Content.ReadAsAsync<DogModel>();
                    return res.message;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}

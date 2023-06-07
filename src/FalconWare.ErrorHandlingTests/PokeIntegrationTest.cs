using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace FalconWare.ErrorHandling.Tests
{
    class Pokemon
    {
        
    }
    
    [TestClass()]
    public class PokeIntegrationTest
    {
        private readonly HttpClient _httpClient;

        public PokeIntegrationTest()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
        }

        private async Task<OpResult<float>> TryGetPokemonBmiAsync(string name)
        {
            string jsonString;
            try
            {
                var response = await _httpClient.GetAsync($"pokemon/{name}");
                if (response.IsSuccessStatusCode)
                {
                    jsonString = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return OpResultFactory.CreateFailure<float>($"Failed getting pokemon '{name}', HTTP status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                return OpResultFactory.CreateFailure<float>(ex);
            }

            var pokemon = JObject.Parse(jsonString);
            var height = pokemon["height"].Value<float>();
            var weight = pokemon["weight"].Value<float>();

            var bmi = weight / (height * height);

            return OpResultFactory.CreateSuccess(bmi);
        } 


        [TestMethod()]
        public async Task GetUsersReposTestAsync_KnownPokemon()
        {
            var name = "squirtle";

            var result = await TryGetPokemonBmiAsync(name);

            Assert.IsTrue(result.WasSuccess);
        }

        [TestMethod]
        public async Task GetUsersReposTest_UnknownPokemon()
        {
            var name = "yoda";

            var result = await TryGetPokemonBmiAsync(name);

            Assert.IsFalse(result.WasSuccess);
        }

    }
}
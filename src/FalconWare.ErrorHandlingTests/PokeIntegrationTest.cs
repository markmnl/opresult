using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FalconWare.ErrorHandling.Tests
{
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
            // query the pokeapi for pokemon with name supplied
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

            // parse pokemon json
            JObject pokemon;
            try
            {
                pokemon = JObject.Parse(jsonString);
            }
            catch (JsonReaderException jre)
            {
                return OpResultFactory.CreateFailure<float>(jre);
            }

            // try extract height and weight, calc bmi and return that
            try
            {
                var height = pokemon["height"].Value<float>();
                var weight = pokemon["weight"].Value<float>();
                if (height < 1.0f)
                {
                    return OpResultFactory.CreateFailure<float>("Failed to parse pokemon - height cannot be less than 1");
                }
                if (weight < 1.0f)
                {
                    return OpResultFactory.CreateFailure<float>("Failed to parse pokemon - weight cannot be less than 1");
                }
                var bmi = weight / (height * height);
                return OpResultFactory.CreateSuccess(bmi);
            }
            catch (NullReferenceException nre)
            {
                var msg = $"Failed parse pokemon response height or weight missing: {nre.Message}";
                return OpResultFactory.CreateFailure<float>(msg);
            }
            catch (FormatException fe)
            {
                var msg = $"Failed parse pokemon response height or weight: {fe.Message}";
                return OpResultFactory.CreateFailure<float>(msg);
            }
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
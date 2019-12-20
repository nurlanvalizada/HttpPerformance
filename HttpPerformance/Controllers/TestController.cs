using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HttpPerformance.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HttpPerformance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGithubClient _githubClient;

        public TestController(HttpClient httpClient, IHttpClientFactory httpClientFactory, IGithubClient githubClient)
        {
            _httpClient = httpClient;
            _httpClientFactory = httpClientFactory;
            _githubClient = githubClient;
        }

        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<GithubRepo>>> GetMyRepos()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.nightshade-preview+json");
                client.DefaultRequestHeaders.Add("User-Agent", "my-agent");

                var url = "https://api.github.com/users/nurlanvalizada/repos";
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                var response = await client.SendAsync(request);

                var data = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<IEnumerable<GithubRepo>>(data);

                return Ok(obj);
            }
        }

        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<GithubRepo>>> GetMyRepos2()
        {
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.nightshade-preview+json");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "my-agent");

            var url = "https://api.github.com/users/nurlanvalizada/repos";
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await _httpClient.SendAsync(request);

            var data = await response.Content.ReadAsStreamAsync();
            var obj = StreamExtensions.DeserializeJsonFromStream<IEnumerable<GithubRepo>>(data);

            return Ok(obj);
        }

        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<GithubRepo>>> GetMyRepos3()
        {
            var url = "users/nurlanvalizada/repos";
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var httpClient = _httpClientFactory.CreateClient("github");

            var response = await httpClient.SendAsync(request);

            var data = await response.Content.ReadAsStreamAsync();
            var obj = StreamExtensions.DeserializeJsonFromStream<IEnumerable<GithubRepo>>(data);

            return Ok(obj);
        }

        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<GithubRepo>>> GetMyRepos4()
        {
            var obj = await _githubClient.GetMyRepos();

            return Ok(obj);
        }
    }
}
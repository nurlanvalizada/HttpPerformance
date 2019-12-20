using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpPerformance.Models
{
    public interface IGithubClient
    {
        Task<IEnumerable<GithubRepo>> GetMyRepos();
    }

    public class GithubClient : IGithubClient
    {
        private readonly HttpClient _httpClient;

        public GithubClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://api.github.com/");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.nightshade-preview+json");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "my-agent");

            _httpClient = httpClient;
        }

        public async Task<IEnumerable<GithubRepo>> GetMyRepos()
        {
            var url = "users/nurlanvalizada/repos";
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await _httpClient.SendAsync(request);

            var data = await response.Content.ReadAsStreamAsync();
            var obj = StreamExtensions.DeserializeJsonFromStream<IEnumerable<GithubRepo>>(data);

            return obj;
        }
    }
}

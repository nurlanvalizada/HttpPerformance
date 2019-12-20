using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HttpPerformance.Models
{
    public class GithubRepo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonProperty("html_url")]
        public string Url { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HttpPerformance.Models
{
    public class StreamExtensions
    {
        public static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default;

            using var sr = new StreamReader(stream);
            using var jtr = new JsonTextReader(sr);

            var js = new JsonSerializer();
            var result = js.Deserialize<T>(jtr);

            return result;
        }
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Acklann.Picmin.Configuration
{
    public class JobConfig
    {
        public JobConfig()
        {
            CompressionSettings = new CompressionOptions();
        }

        public string Name { get; set; }

        public IList<string> SourceFiles { get; set; }

        public string OutputDirectory { get; set; }

        public string WorkingDirectory { get; set; }

        [JsonProperty("compressor")]
        public CompressionOptions CompressionSettings { get; set; }
    }
}
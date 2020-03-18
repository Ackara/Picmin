using Acklann.Picmin.Compression;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Acklann.GlobN;

namespace Acklann.Picmin.Configuration
{


    public class BatchConfiguration
    {


        public BatchConfiguration()
        {

        }

        public static void Run(string configurationFilePath, string jpath = default)
        {
            if (!File.Exists(configurationFilePath)) throw new FileNotFoundException($"Could not find file at '{configurationFilePath}'.");

            foreach (var item in Parse(configurationFilePath, jpath))
            {
                switch (item)
                {
                    case ICompressionOptions compressor:
                        
                        break;
                }
            }
        }

        #region Backing Members

        private const string ROOT_JPATH = "$.images";

        public static IEnumerable<object> Parse(string configurationFilePath, string jpath = default, string outputDirectory = default, string workingDirectory = default)
        {
            if (!File.Exists(configurationFilePath)) throw new FileNotFoundException($"Could not find file at '{configurationFilePath}'.");

            JArray config;
            JToken token = JToken.Parse(File.ReadAllText(configurationFilePath));
            if (token.Type == JTokenType.Array) config = (JArray)token;
            else
            {
                token = token.SelectToken(jpath ?? ROOT_JPATH);
                if (token.Type == JTokenType.Array) config = (JArray)token;
                else yield break;
            }

            foreach (ICompressionOptions item in GetCompressionOptions(config))
                yield return item;
        }

        private static IEnumerable<ICompressionOptions> GetCompressionOptions(JArray configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            foreach (var group in configuration.GroupBy(x => (x as JObject)?.Property("name")))
            {
                foreach (JObject item in group)
                {
                    
                }
            }

            throw new System.NotImplementedException();
        }

        #endregion
    }
}
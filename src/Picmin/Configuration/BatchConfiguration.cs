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

        #region Backing Members

        private const string ROOT_JPATH = "$.images";

        internal static void foo(string configurationFilePath, string jpath = default)
        {
            if (!File.Exists(configurationFilePath)) throw new FileNotFoundException($"Could not find file at '{configurationFilePath}'.");

            JArray config;
            JToken token = JToken.Parse(File.ReadAllText(configurationFilePath));
            if (token.Type == JTokenType.Array) config = (JArray)token;
            else
            {
                token = token.SelectToken(jpath ?? ROOT_JPATH);
                if (token.Type == JTokenType.Array) config = (JArray)token;
                else return;
            }
        }

        private static IEnumerable<ICompressionOptions> GetCompressionOptions(JArray configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var f = configuration.GroupBy(x => (x as JObject)?.Property("name"));
            foreach (var item in f)
            {

            }

            throw new System.NotImplementedException();
        }

        #endregion
    }
}
using Acklann.GlobN;
using Acklann.Picmin.Compression;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Acklann.Picmin.Configuration
{
    public class Processor
    {
        public Processor()
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

            foreach (ICompressionOptions item in GetCompressionOptions(config, workingDirectory))
                yield return item;
        }

        private static IEnumerable<ICompressionOptions> GetCompressionOptions(JArray configuration, string cwd)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var results = new Dictionary<string, ICompressionOptions>();

            foreach (var group in configuration.GroupBy(x => (x as JObject)?.Property("name")))
            {
                System.Diagnostics.Debug.WriteLine($"on group: {group.Key}");

                foreach (JObject item in group)
                {
                    foreach (string filePath in GetFileList(item, cwd))
                    {
                        results[filePath] = CreateCompressionOptions(item, filePath);
                    }
                }
            }

            return results.Values;
        }

        private static IEnumerable<string> GetFileList(JObject obj, string cwd)
        {
            string[] sourceFiles = obj.Property(nameof(ICompressionOptions.SourceFile), StringComparison.InvariantCultureIgnoreCase)?.Value<string[]>();
            foreach (Glob pattern in sourceFiles)
                foreach (string filePath in pattern.ResolvePath(cwd))
                {
                    yield return filePath;
                }
        }

        private static ICompressionOptions CreateCompressionOptions(JObject obj, string sourceFile)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var settings = (JObject)obj.Property("compressor", StringComparison.OrdinalIgnoreCase)?.Value;
            int quality = (settings?.Property(nameof(ICompressionOptions.Quality), StringComparison.OrdinalIgnoreCase)?.Value<int>() ?? default);
            bool removeMetadata = (settings?.Property(nameof(ICompressionOptions.RemoveMetadata), StringComparison.OrdinalIgnoreCase).Value<bool>() ?? default);
            string suffix = (settings?.Property("suffix", StringComparison.OrdinalIgnoreCase).Value<string>() ?? ".min");
            string baseName = Path.GetFileNameWithoutExtension(sourceFile);
            string extension = Path.GetExtension(sourceFile);

            string outputFolder = settings?.Property("outputDirectory", StringComparison.OrdinalIgnoreCase)?.Value<string>();
            string outFile = (string.IsNullOrEmpty(outputFolder) ?
                Path.Combine(Path.GetDirectoryName(sourceFile), string.Concat(baseName, suffix, extension)) :
                Path.Combine(outputFolder, string.Concat(baseName, suffix, extension)));

            switch (Path.GetExtension(sourceFile).ToLowerInvariant())
            {
                case ".png":
                    return new PngquantOptions(sourceFile, outFile, (quality - 10), quality, removeMetadata);

                case ".jpg":
                case ".jpeg":
                    return new PingoOptions(sourceFile, outFile, quality, removeMetadata);

                case ".svg":
                    return new SvgoOptions(sourceFile, outFile, removeMetadata);
            }

            return null;
        }

        #endregion Backing Members
    }
}
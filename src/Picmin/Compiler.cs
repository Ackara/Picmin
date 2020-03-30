using Acklann.GlobN;
using Acklann.Picmin.Compression;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Acklann.Picmin.Configuration
{
    public class Compiler
    {
        public Compiler()
        {
        }

        public static void Run(string configurationFilePath, string jpath = default)
        {
            if (!File.Exists(configurationFilePath)) throw new FileNotFoundException($"Could not find file at '{configurationFilePath}'.");

            foreach (IPlugin plugin in ReadFile(configurationFilePath, jpath))
            {
                CompilerResult result = plugin.Run();
            }
        }

        public static IEnumerable<IPlugin> ReadFile(string configurationFilePath, string jpath = default, string workingDirectory = default)
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

            foreach (var item in GetCompressionOptions(config, workingDirectory))
                yield return PluginFactory.CreateInstance(item);
        }

        #region Backing Members

        private const string ROOT_JPATH = "$.images";

        private static IEnumerable<ICompressionOptions> GetCompressionOptions(JArray configuration, string cwd)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var results = new Dictionary<string, ICompressionOptions>();

            foreach (JObject item in configuration)
            {
                foreach (string filePath in GetFileList(item, cwd))
                {
                    results[filePath] = CreateCompressionOptions(item, filePath, cwd);
                    System.Diagnostics.Debug.WriteLine(Path.GetFileName(filePath));
                }
            }

            return results.Values;
        }

        private static IEnumerable<string> GetFileList(JObject obj, string cwd)
        {
            IEnumerable<string> sourceFiles = obj.Property("sourceFiles", StringComparison.InvariantCultureIgnoreCase)?.Value?.Values<string>();
            if (sourceFiles == null) yield break;

            foreach (Glob pattern in sourceFiles)
                foreach (string filePath in pattern.ResolvePath(cwd))
                {
                    yield return filePath;
                }
        }

        private static ICompressionOptions CreateCompressionOptions(JObject obj, string sourceFile, string cwd)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var settings = (JObject)obj.Property("compressor", StringComparison.OrdinalIgnoreCase)?.Value;
            int quality = (settings?.Property(nameof(ICompressionOptions.Quality), StringComparison.OrdinalIgnoreCase)?.Value.Value<int>() ?? default);
            bool removeMetadata = (settings?.Property(nameof(ICompressionOptions.RemoveMetadata), StringComparison.OrdinalIgnoreCase)?.Value?.Value<bool>() ?? default);
            string suffix = (settings?.Property("suffix", StringComparison.OrdinalIgnoreCase)?.Value?.Value<string>() ?? ".min");
            string baseName = Path.GetFileNameWithoutExtension(sourceFile);
            string extension = Path.GetExtension(sourceFile);

            string outputFolder = settings?.Property("outputDirectory", StringComparison.OrdinalIgnoreCase)?.Value?.Value<string>();
            string outFile = (string.IsNullOrEmpty(outputFolder) ?
                Path.Combine(Path.GetDirectoryName(sourceFile), string.Concat(baseName, suffix, extension)) :
                Path.Combine(outputFolder.ExpandPath(cwd), string.Concat(baseName, suffix, extension)));
            outFile = Environment.ExpandEnvironmentVariables(outFile);

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
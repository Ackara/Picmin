using Acklann.GlobN;
using Acklann.Picmin.Compression;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Acklann.Picmin.Configuration
{
    public class Compiler
    {
        public Compiler()
        {
        }

        public static IEnumerable<CompilerResult> Run(string configurationFilePath, string jpath = default)
        {
            if (!File.Exists(configurationFilePath)) throw new FileNotFoundException($"Could not find file at '{configurationFilePath}'.");

            foreach (ICommand command in ReadFile(configurationFilePath, jpath))
                yield return command.Run();
        }

        public static Task<CompilerResult[]> RunAsync(string configurationFilePath, string jpath = default)
        {
            if (!File.Exists(configurationFilePath)) throw new FileNotFoundException($"Could not find file at '{configurationFilePath}'.");

            var threads = new Stack<Task<CompilerResult>>();
            foreach (ICommand command in ReadFile(configurationFilePath, jpath))
                threads.Push(Task.Run(() => command.Run()));

            return Task.WhenAll(threads);
        }

        public static IEnumerable<ICommand> ReadFile(string configurationFilePath, string jpath = default, string workingDirectory = default)
        {
            if (!File.Exists(configurationFilePath)) throw new FileNotFoundException($"Could not find file at '{configurationFilePath}'.");

            JArray config;
            JToken token = JToken.Parse(File.ReadAllText(configurationFilePath));
            if (token.Type == JTokenType.Array) config = (JArray)token;
            else
            {
                token = token.SelectToken(jpath ?? ROOT_JPATH);
                if (token?.Type == JTokenType.Array) config = (JArray)token;
                else yield break;
            }

            foreach (ICompressionOptions options in GetCompressionOptions(config, workingDirectory))
                if (options != null)
                    yield return CommandFactory.CreateInstance(options);
        }

        #region Backing Members

        private const string
            ROOT_JPATH = "$.images",
            MIN = ".min";

        private static IEnumerable<ICompressionOptions> GetCompressionOptions(JArray settings, string cwd)
        {
            var results = new Dictionary<string, ICompressionOptions>();

            foreach (JObject item in settings)
                foreach (string filePath in GetSourceFiles(item, cwd))
                {
                    results[filePath] = CreateCompressionOptions(item, filePath, cwd);
                    System.Diagnostics.Debug.WriteLine(Path.GetFileName(filePath));
                }

            return results.Values;
        }

        private static IEnumerable<string> GetSourceFiles(JObject obj, string cwd)
        {
            IEnumerable<string> sourceFiles = obj.Property("sourceFiles", StringComparison.InvariantCultureIgnoreCase)?.Value?.Values<string>();
            if (sourceFiles == null) yield break;

            string suffix = (obj.SelectToken("$.compressor.suffix")?.Value<string>() ?? MIN);

            foreach (Glob pattern in sourceFiles)
                foreach (string filePath in pattern.ResolvePath(cwd))
                {
                    if (filePath.EndsWith(string.Concat(suffix, Path.GetExtension(filePath)))) continue;
                    yield return filePath;
                }
        }

        private static ICompressionOptions CreateCompressionOptions(JObject obj, string sourceFile, string cwd)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var settings = (JObject)obj.Property("compressor", StringComparison.OrdinalIgnoreCase)?.Value;
            int quality = (settings?.Property(nameof(ICompressionOptions.Quality), StringComparison.OrdinalIgnoreCase)?.Value.Value<int>() ?? default);
            bool removeMetadata = (settings?.Property(nameof(ICompressionOptions.RemoveMetadata), StringComparison.OrdinalIgnoreCase)?.Value?.Value<bool>() ?? default);
            string suffix = (settings?.Property("suffix", StringComparison.OrdinalIgnoreCase)?.Value?.Value<string>() ?? MIN);

            string outputFolder = settings?.Property("outputDirectory", StringComparison.OrdinalIgnoreCase)?.Value?.Value<string>();
            string outFile = (string.IsNullOrEmpty(outputFolder) ?
                Path.Combine(sourceFile.WithSuffix(suffix)) :
                Path.Combine(sourceFile.WithSuffix(suffix, outputFolder.ExpandPath(cwd))));
            outFile = Environment.ExpandEnvironmentVariables(outFile ?? string.Empty);

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
using System.Diagnostics;
using System.IO;

namespace Acklann.Picmin.Compression
{
    public class Svgo : ICommand
    {
        public Svgo(SvgoOptions options)
        {
            _options = options;
        }

        public string SourceFile
        {
            get => _options.SourceFile;
        }

        public static CompilerResult Compress(SvgoOptions options)
        {
            if (!File.Exists(options.SourceFile)) throw new FileNotFoundException($"Could not find file at '{options.SourceFile}'.");

            string folder = Path.GetDirectoryName(options.OutputFile);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            using (Process script = BootLoader.Invoke("node", options.ToString()))
            {
                return new CompilerResult(
                    "svgo",
                    script.ExitCode == 0,
                    options.SourceFile,
                    options.OutputFile,
                    (new FileInfo(options.SourceFile).Length),
                    (new FileInfo(options.OutputFile).Length),
                    (script.ExitTime.Ticks - script.StartTime.Ticks),
                    string.Concat(script.StandardError.ReadToEnd(), "\r\n", script.StandardOutput.ReadToEnd())
                    );
            }
        }

        public CompilerResult Run() => Compress(_options);

        #region Backing Members

        private readonly SvgoOptions _options;

        #endregion Backing Members
    }
}
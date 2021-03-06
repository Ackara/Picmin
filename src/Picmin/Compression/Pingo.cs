using System.Diagnostics;
using System.IO;

namespace Acklann.Picmin.Compression
{
    public class Pingo : ICommand
    {
        public Pingo(PingoOptions options)
        {
            _options = options;
        }

        public string SourceFile
        {
            get => _options.SourceFile;
        }

        public static CompilerResult Compress(PingoOptions options)
        {
            if (!File.Exists(options.SourceFile)) throw new FileNotFoundException($"Could not find file at '{options.SourceFile}'.");

            string folder = Path.GetDirectoryName(options.OutputFile);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            File.Copy(options.SourceFile, options.OutputFile, overwrite: true);

            using (Process executable = BootLoader.InvokeExe("pingo.exe", options.ToString()))
            {
                return new CompilerResult(
                    "pingo",
                    executable.ExitCode == 0,
                    options.SourceFile, options.OutputFile,
                    (new FileInfo(options.SourceFile).Length),
                    (new FileInfo(options.OutputFile).Length),
                    (executable.ExitTime.Ticks - executable.StartTime.Ticks),
                    string.Concat(executable.StandardError?.ReadToEnd(), "\r\n", executable.StandardOutput?.ReadToEnd()).Trim()
                    );
            }
        }

        public CompilerResult Run() => Compress(_options);

        #region Backing Members

        private readonly PingoOptions _options;

        #endregion Backing Members
    }
}
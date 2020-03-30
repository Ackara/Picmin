using System.Diagnostics;
using System.IO;

namespace Acklann.Picmin.Compression
{
    public class Pngquant : IPlugin
    {
        public Pngquant(PngquantOptions options)
        {
            _options = options;
        }

        private readonly PngquantOptions _options;


        public string SourceFile
        {
            get => _options.SourceFile;
        }

        public static CompilerResult Compress(PngquantOptions options)
        {
            if (!File.Exists(options.SourceFile)) throw new FileNotFoundException($"Could not find file at '{options.SourceFile}'.");

            string folder = Path.GetDirectoryName(options.OutputFile);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            using (Process executable = Executor.InvokeExe("pngquant.exe", options.ToString()))
            {
                return new CompilerResult(
                    "pngquant",
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
    }
}
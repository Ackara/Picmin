using System.Diagnostics;
using System.IO;

namespace Acklann.Picmin.Compression
{
    public class Pngquant
    {
        public static CompressionResult Compress(PngquantOptions options)
        {
            if (!File.Exists(options.SourceFile)) throw new FileNotFoundException($"Could not find file at '{options.SourceFile}'.");

            string folder = Path.GetDirectoryName(options.OutputFile);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            using (Process executable = Executor.InvokeExe("pngquant.exe", options.ToString()))
            {
                return new CompressionResult(
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
    }
}
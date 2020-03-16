using System.Diagnostics;
using System.IO;

namespace Acklann.Picmin.Compression
{
    public class Svgo
    {
        public static CompressionResult Compress(SvgoOptions options)
        {
            if (!File.Exists(options.SourceFile)) throw new FileNotFoundException($"Could not find file at '{options.SourceFile}'.");

            string folder = Path.GetDirectoryName(options.OutputFile);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            using (Process script = Executor.Invoke("node", options.ToString()))
            {
                return new CompressionResult(
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
    }
}
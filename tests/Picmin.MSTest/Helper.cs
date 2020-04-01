using System.IO;

namespace Acklann.Picmin
{
    public static class Helper
    {
        public static string TempDirectory
        {
            get => Path.Combine(Path.GetTempPath(), nameof(Picmin));
        }

        public static string CleanDirectory()
        {
            if (Directory.Exists(TempDirectory)) Directory.Delete(TempDirectory, recursive: true);
            Directory.CreateDirectory(TempDirectory);
            return TempDirectory;
        }

        public static string CopyFile(string sourceFile)
        {
            string destination = Path.Combine(TempDirectory, Path.GetFileName(sourceFile));
            File.Copy(sourceFile, destination, overwrite: true);
            return destination;
        }
    }
}

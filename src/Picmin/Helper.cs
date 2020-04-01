using System.IO;

namespace Acklann.Picmin
{
    internal static class Helper
    {
        public static string WithSuffix(this string filePath, string suffix = ".min", string cwd = default)
        {
            return Path.Combine(
                (cwd ?? Path.GetDirectoryName(filePath)),
                string.Concat(
                    Path.GetFileNameWithoutExtension(filePath),
                    suffix,
                    Path.GetExtension(filePath)));
        }

        public static string GetFileName(this string filePath, string suffix = ".min")
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(filePath)
                );
        }
    }
}

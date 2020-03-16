using System.IO;

namespace Acklann.Picmin
{
    internal static class Helper
    {
        public static string WithSuffix(this string file, string suffix = ".min")
        {
            return Path.Combine(
                Path.GetDirectoryName(file),
                string.Concat(
                    Path.GetFileNameWithoutExtension(file),
                    suffix,
                    Path.GetExtension(file)));
        }
    }
}

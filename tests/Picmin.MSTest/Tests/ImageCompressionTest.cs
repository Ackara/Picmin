using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Acklann.Picmin.Tests
{
    [TestClass]
    public class ImageCompressionTest
    {
        #region Backing Members

        public static string ToDisplayName(MethodInfo info, object[] args)
        {
            switch (info.Name)
            {

                default: return string.Join(", ", args);
            }
        }

        private static IEnumerable<object[]> GetJpegFiles()
        {
            return from x in Directory.EnumerateFiles(Sample.DirectoryPath, "*", SearchOption.AllDirectories)
                   let ext = Path.GetExtension(x)
                   where string.Equals(ext, ".jpg", StringComparison.OrdinalIgnoreCase) || string.Equals(ext, ".jpeg", StringComparison.OrdinalIgnoreCase)
                   select new object[] { x };
        }

        private static IEnumerable<object[]> GetPngFiles() => GetFiles(".png");

        private static IEnumerable<object[]> GetFiles(string extension)
        {
            return from x in Directory.EnumerateFiles(Sample.DirectoryPath, $"*{extension}", SearchOption.AllDirectories)
                   select new object[] { x };
        }

        #endregion Backing Members
    }
}

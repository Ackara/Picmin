using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Acklann.Picmin.Tests
{
    [TestClass]
    public class OptimizationTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetJpegFiles), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(ToDisplayName))]
        public void Can_minify_jpeg_file(string imagePath)
        {
            // Act
            var result = Optimizer.Compress(imagePath);

            // Assert
            result.Success.ShouldBeTrue();
        }

        #region Backing Members

        public static string ToDisplayName(MethodInfo info, object[] args)
        {
            switch (info.Name)
            {
                case nameof(Can_minify_jpeg_file):
                    return Path.GetFileName(Convert.ToString(args[0]));

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

        #endregion Backing Members
    }
}

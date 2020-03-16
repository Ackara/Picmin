using Acklann.Diffa;
using Acklann.Picmin.Compression;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using System.IO;

namespace Acklann.Picmin.Tests
{
    [TestClass]
    public class PngquantTest
    {
        [ClassInitialize]
        public static void Cleanup(TestContext _)
        {
            if (Directory.Exists(TempFolder)) Directory.Delete(TempFolder, recursive: true);
            Directory.CreateDirectory(TempFolder);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetOptions), DynamicDataSourceType.Method)]
        public void Can_compress_image_with_pngquant(PngquantOptions options)
        {
            // Act
            var result = Pngquant.Compress(options);

            // Assert
            result.Success.ShouldBeTrue(result.Message);
            result.Elapse.Ticks.ShouldBeGreaterThan(0);
            result.NewSize.ShouldBeLessThanOrEqualTo(result.OriginalSize);

            File.Exists(result.SourceFile).ShouldBeTrue(result.Message);
            File.Exists(result.OuputFile).ShouldBeTrue(result.Message);

            Diff.ApproveFile(result.OuputFile, Path.GetFileNameWithoutExtension(result.OuputFile));
        }

        #region Backing Members

        private static readonly string TempFolder = Path.Combine(Path.GetTempPath(), "picmin");

        private static IEnumerable<object[]> GetOptions()
        {
            static string getOutFile(string x) => Path.Combine(TempFolder, $"pngquant-{x}.png");
            var imageFile = Sample.GetImg5PNG().FullName;

            var sample1 = new PngquantOptions(imageFile);
            if (File.Exists(sample1.OutputFile)) File.Delete(sample1.OutputFile);
            yield return new object[] { sample1 };

            yield return new object[]
            {
                new PngquantOptions(imageFile, getOutFile("metadata"), removeMetadata: false)
            };

            yield return new object[]
            {
                new PngquantOptions(imageFile, getOutFile("mid-quality"), 50, 50, 11)
            };

            yield return new object[]
            {
                new PngquantOptions(imageFile, getOutFile("low-quality"), 10, 15)
            };
        }

        #endregion Backing Members
    }
}

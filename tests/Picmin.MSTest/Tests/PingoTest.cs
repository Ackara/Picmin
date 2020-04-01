using Acklann.Diffa;
using Acklann.Picmin.Compression;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using System.IO;

namespace Acklann.Picmin.Tests
{
    [TestClass]
    public class PingoTest
    {
        [ClassInitialize]
        public static void Cleanup(TestContext _)
        {
            Helper.CleanDirectory();
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPingoOptions), DynamicDataSourceType.Method)]
        public void Can_compress_image_with_pingo(PingoOptions options)
        {
            // Act
            CompilerResult result = Pingo.Compress(options);

            // Assert
            result.Success.ShouldBeTrue(result.Message);
            result.Elapse.Ticks.ShouldBeGreaterThan(0);
            result.NewSize.ShouldBeLessThanOrEqualTo(result.OriginalSize);

            File.Exists(result.SourceFile).ShouldBeTrue(result.Message);
            File.Exists(result.OuputFile).ShouldBeTrue(result.Message);

            Diff.ApproveFile(result.OuputFile, Path.GetFileNameWithoutExtension(result.OuputFile));
        }

        #region Backing Members

        

        private static IEnumerable<object[]> GetPingoOptions()
        {
            static string outFile(string x) => Path.Combine(Helper.TempDirectory, $"pingo-{x}");

            yield return new object[]
            {
                new PingoOptions(Helper.CopyFile(Sample.GetImg10PNG().FullName))
            };

            yield return new object[]
            {
                new PingoOptions(Helper.CopyFile(Sample.GetImg8JPG().FullName), outFile("metadata.jpg"), removeMetadata: false)
            };

            yield return new object[]
            {
                new PingoOptions(Helper.CopyFile(Sample.GetImg9JPG().FullName), outFile("low.jpg"), 10)
            };

            yield return new object[]
            {
                new PingoOptions(Helper.CopyFile(Sample.GetImg5PNG().FullName), outFile("high.png"), 100)
            };
        }

        #endregion Backing Members
    }
}

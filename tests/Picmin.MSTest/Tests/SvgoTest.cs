using Acklann.Diffa;
using Acklann.Picmin.Compression;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using System.IO;

namespace Acklann.Picmin.Tests
{
    [TestClass]
    public class SvgoTest
    {
        [ClassInitialize]
        public static void Cleanup(TestContext _)
        {
            Helper.CleanDirectory();

            if (!BootLoader.CheckForSystemPrerequisites()) throw new System.Exception("NPM is not installed on this machine.");
            BootLoader.InitializeComponents();
        }

        [DataTestMethod]
        [DynamicData(nameof(GetSvgoOptions), DynamicDataSourceType.Method)]
        public void Can_compress_image_with_svgo(SvgoOptions options)
        {
            // Act
            var results = Svgo.Compress(options);

            // Assert
            results.Success.ShouldBeTrue();
            results.Elapse.ShouldNotBe(default);

            File.Exists(results.OuputFile).ShouldBeTrue();
            File.Exists(results.SourceFile).ShouldBeTrue();

            results.NewSize.ShouldBeLessThan(results.OriginalSize);
            results.NewSize.ShouldBeGreaterThan(0);

            Diff.ApproveFile(options.OutputFile, Path.GetFileNameWithoutExtension(options.OutputFile));
        }

        #region Backing Members

        private static IEnumerable<object[]> GetSvgoOptions()
        {
            var imageFile = Helper.CopyFile(Sample.GetImg7SVG().FullName);
            string outFile(string x) => Path.Combine(Helper.TempDirectory, x);

            yield return new object[] { new SvgoOptions(imageFile) };
            yield return new object[] { new SvgoOptions(imageFile, outFile("with-metadata.svg"), removeMetadata: false) };
        }

        #endregion Backing Members
    }
}

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
            if (Directory.Exists(TempFolder)) Directory.Delete(TempFolder, recursive: true);
            Directory.CreateDirectory(TempFolder);

            if (!Executor.CheckForSystemPrerequisites()) throw new System.Exception("NPM is not installed on this machine.");
            Executor.InitializeComponents();
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

        private static string TempFolder => Path.Combine(Path.GetTempPath(), "picmin", nameof(SvgoTest));

        private static IEnumerable<object[]> GetSvgoOptions()
        {
            var imageFile = Sample.GetImg7SVG().FullName;
            string outFile(string x) => Path.Combine(TempFolder, x);

            yield return new object[] { new SvgoOptions(imageFile) };
            yield return new object[] { new SvgoOptions(imageFile, outFile("with-metadata.svg"), removeMetadata: false) };
        }

        #endregion Backing Members
    }
}

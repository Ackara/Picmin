using Acklann.Picmin.Compression;
using Acklann.Picmin.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.IO;
using System.Linq;

namespace Acklann.Picmin.Tests
{
    [TestClass]
    public class ConfigurationTest
    {
        [ClassInitialize]
        public static void Setup(TestContext _)
        {
            Helper.CleanDirectory();
        }

        [TestMethod]
        public void Can_read_configuration_file()
        {
            // Arrange
            var configurationFile = Sample.GetFullConfigJSON().FullName;

            // Act
            var result = Compiler.ReadFile(configurationFile).ToArray();
            var plugin = result.First(x => Path.GetExtension(x.SourceFile) == ".png");

            // Assert
            result.ShouldNotBeEmpty();
            plugin.ShouldBeOfType<Pngquant>();
        }

        [TestMethod]
        public void Should_ignore_output_files_when_enumerating()
        {
            // Arrange
            var configFile = Sample.GetFullConfigJSON().FullName;
            var cwd = Path.Combine(Helper.TempDirectory, "enumerate");
            string fname(string x, string suffix = ".min") => Path.Combine(cwd, string.Concat(Path.GetFileNameWithoutExtension(x), suffix, Path.GetExtension(x)));

            if (Directory.Exists(cwd)) Directory.Delete(cwd, recursive: true);
            Directory.CreateDirectory(cwd);

            var sourceFiles = Directory.GetFiles(Sample.DirectoryPath, "*.png");

            // Act
            foreach (var item in sourceFiles)
            {
                File.Copy(item, fname(item, null), overwrite: true);
                if (File.Exists(item)) File.Create(fname(item)).Dispose();
            }

            var result = Compiler.ReadFile(configFile, null, cwd).Select(x => x.SourceFile).ToArray();

            // Assert
            result.ShouldNotBeEmpty();
            result.Length.ShouldBe(sourceFiles.Length);
        }
    }
}

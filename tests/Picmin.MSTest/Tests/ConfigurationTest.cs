using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using Acklann.Picmin.Configuration;
using Acklann.Picmin.Compression;

namespace Acklann.Picmin.Tests
{
    [TestClass]
    public class ConfigurationTest
    {
        [ClassInitialize]
        public static void Setup(TestContext _)
        {
            OutputDirectory = Path.Combine(Path.GetTempPath(), "picmin", "images");
            if (Directory.Exists(OutputDirectory)) Directory.Delete(OutputDirectory, recursive: true);
            Directory.CreateDirectory(OutputDirectory);
        }

        [TestMethod]
        public void Can_read_configuration_file()
        {
            // Arrange
            var configurationFile = Sample.GetFullConfigJSON().FullName;

            // Act
            var result = Compiler.ReadFile(configurationFile).ToArray();
            var plugin = result.First(x => Path.GetExtension(x.SourceFile) == ".png" && Path.GetFileName(x.SourceFile).StartsWith("img"));

            // Assert
            result.ShouldNotBeEmpty();
            plugin.ShouldBeOfType<Pngquant>();
        }

        #region Backing Members
        private static string OutputDirectory;
        #endregion
    }
}

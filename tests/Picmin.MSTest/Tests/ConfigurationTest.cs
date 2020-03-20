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
		public void Can_parse_batch_file()
		{
			// Arrange
			var configurationFile = Sample.GetFullConfigJSON().FullName;

			// Act
			var result =  Processor.Parse(configurationFile);


			// Assert
			result.ShouldNotBeEmpty();
		}

		#region Backing Members
		private static string OutputDirectory;
		#endregion
	}
}

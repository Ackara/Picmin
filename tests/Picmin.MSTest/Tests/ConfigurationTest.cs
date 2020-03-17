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
			ImagesDirectory = Path.Combine(Path.GetTempPath(), "picmin", "images");
			if (Directory.Exists(ImagesDirectory)) Directory.Delete(ImagesDirectory, recursive: true);
		}

		[TestMethod]
		public void Can_parse_batch_file()
		{
			// Arrange
			var sampleImage = Sample.GetFullConfigJSON().FullName;

			// Act
			


			// Assert
			throw new System.NotImplementedException();
		}

		#region Backing Members
		private static string ImagesDirectory;
		#endregion
	}
}

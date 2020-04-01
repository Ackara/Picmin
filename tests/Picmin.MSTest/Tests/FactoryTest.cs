using Acklann.Picmin.Compression;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acklann.Picmin.Tests
{
    [TestClass]
    public class FactoryTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCommandTypeMapping), DynamicDataSourceType.Method)]
        public void Can_instantiate_command_by_name(object options, Type expectedResult)
        {
            // Act
            var result = CommandFactory.CreateInstance(options);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType(expectedResult);
        }

        #region Backing Members

        internal static IEnumerable<object[]> GetCommandTypeMapping()
        {
            var knownTypes = typeof(ICommand).Assembly.GetExportedTypes();

            var compressionOptions = from t in knownTypes
                                     where typeof(ICompressionOptions).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface
                                     select t;

            foreach (var item in compressionOptions)
                yield return new object[] { Activator.CreateInstance(item), Type.GetType(item.AssemblyQualifiedName.Replace("Options", string.Empty, StringComparison.InvariantCultureIgnoreCase)) };
        }

        #endregion Backing Members
    }
}

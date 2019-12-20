using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Acklann.Picmin
{
	public class Optimizer
	{
		public static CompressionResult Compress(string imagePath)
		{
			if (!File.Exists(imagePath)) throw new FileNotFoundException($"Could not find file at '{imagePath}'.");

			throw new System.NotImplementedException();
		}
	}
}
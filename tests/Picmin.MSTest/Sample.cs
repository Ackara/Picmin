using System;
using System.IO;
using System.Linq;

namespace Acklann.Picmin
{
	internal static partial class Sample
	{
		public const string FOLDER_NAME = "samples";

		public static string DirectoryPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FOLDER_NAME);
		
		public static FileInfo GetFile(string fileName, string directory = null)
        {
            fileName = Path.GetFileName(fileName);
            string searchPattern = $"*{Path.GetExtension(fileName)}";

            string targetDirectory = directory?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FOLDER_NAME);
            return new DirectoryInfo(targetDirectory).EnumerateFiles(searchPattern, SearchOption.AllDirectories)
                .First(x => x.Name.Equals(fileName, StringComparison.CurrentCultureIgnoreCase));
        }

		public static FileInfo GetImg1aGIF() => GetFile(@"img1A.gif");
		public static FileInfo GetImg2BMP() => GetFile(@"img2.bmp");
		public static FileInfo GetImg3GIF() => GetFile(@"img3.gif");
		public static FileInfo GetImg4ICO() => GetFile(@"img4.ico");
		public static FileInfo GetImg5PNG() => GetFile(@"img5.png");
		public static FileInfo GetImg10PNG() => GetFile(@"lvl2\img10.png");
		public static FileInfo GetImg6TIF() => GetFile(@"lvl2\img6.tif");
		public static FileInfo GetImg7SVG() => GetFile(@"lvl2\img7.svg");
		public static FileInfo GetImg8JPG() => GetFile(@"lvl2\img8.jpg");
		public static FileInfo GetImg9JPG() => GetFile(@"lvl2\img9.jpg");

		public struct File
		{
			public const string Img1aGIF = @"img1A.gif";
			public const string Img2BMP = @"img2.bmp";
			public const string Img3GIF = @"img3.gif";
			public const string Img4ICO = @"img4.ico";
			public const string Img5PNG = @"img5.png";
			public const string Img10PNG = @"lvl2\img10.png";
			public const string Img6TIF = @"lvl2\img6.tif";
			public const string Img7SVG = @"lvl2\img7.svg";
			public const string Img8JPG = @"lvl2\img8.jpg";
			public const string Img9JPG = @"lvl2\img9.jpg";
		}
	}	
}

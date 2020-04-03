using Acklann.Picmin.Configuration;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Acklann.Picmin.MSBuild
{
	public class TransformImages : ITask
	{
		
		public string ConfiguraitonFile { get; set; }

		public string JPath { get; set; }


		public bool Execute()
		{
			if (string.IsNullOrEmpty(ConfiguraitonFile)) ConfiguraitonFile = Path.Combine(BuildEngine.ProjectFileOfTaskNode, "transpiler.json");
			if (!File.Exists(ConfiguraitonFile)) throw new FileNotFoundException($"Could not find file at '{ConfiguraitonFile}'.");

			IEnumerable<CompilerResult> results = Compiler.Run(ConfiguraitonFile, JPath);

			return true;
		}

		#region ITask
		public ITaskHost HostObject { get; set; }
		public IBuildEngine BuildEngine { get; set; }

		#endregion

		#region Backing Members

		private void WriteResult(CompilerResult result)
		{
			
		}

		private void WriteInfo(string message)
		{
			BuildEngine.LogMessageEvent(new BuildMessageEventArgs(message, null, nameof(TransformImages), MessageImportance.Normal));
		}
		#endregion
	}
}
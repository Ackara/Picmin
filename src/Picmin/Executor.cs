using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Acklann.Picmin
{
    public delegate void ProgressHandler(string message, int progress, int max);

    public static class Executor
    {
        static Executor()
        {
            BaseDirectory = Path.Combine(AppContext.BaseDirectory, ('v' + typeof(Executor).Assembly.GetName().Version.ToString()));
            if (!Directory.Exists(BaseDirectory)) Directory.CreateDirectory(BaseDirectory);
        }

        internal static readonly string BaseDirectory;

        public static bool CheckForSystemPrerequisites()
        {
            Process npm = CreateProcess("cmd", "/c npm --version");

            try
            {
                npm.Start();
                npm.WaitForExit();
                return npm.ExitCode == 0;
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Console.WriteLine(ex.Message);
                return false;
#endif
            }
            finally { npm.Dispose(); }
        }

        public static void InitializeComponents(Action<string, int, int, bool> callback = default)
        {
            string[] node_modlues = new string[] { "svgo@1.3.2" };

            // Unload embedded resources.
            Assembly assembly = typeof(Executor).Assembly;
            string[] resources = assembly.GetManifestResourceNames();
            int progress = 0, max = (resources.Length + node_modlues.Length);

            foreach (string name in resources)
            {
                string extension = Path.GetExtension(name);
                string baseName = Path.GetFileNameWithoutExtension(name);
                string fullPath = Path.Combine(BaseDirectory, (baseName.Substring(baseName.LastIndexOf('.') + 1) + extension));

                if (!File.Exists(fullPath))
                    using (Stream stream = assembly.GetManifestResourceStream(name))
                    using (Stream file = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        stream.CopyTo(file);
                        stream.Flush();
                    }

                callback?.Invoke(name, ++progress, max, false);
            }

            // Installing reqired node modules
            Process npm = null;

            if (!Directory.Exists(Path.Combine(BaseDirectory, "node_modules")))
                try
                {
                    npm = CreateProcess("cmd", null);

                    foreach (string item in node_modlues)
                    {
                        npm.StartInfo.Arguments = $"/c npm install {item} --save-dev";
                        npm.Start();
                        npm.WaitForExit();

                        callback?.Invoke(item, ++progress, node_modlues.Length, false);
                        if (npm.ExitCode != 0) throw new Exception($"NPM was unable to install {item}.");
                    }
                }
                finally { npm?.Dispose(); }

            callback?.Invoke(default, max, max, true);
        }

        internal static Process Invoke(string exe, string arguments)
        {
            if (string.IsNullOrEmpty(exe)) throw new ArgumentNullException(nameof(exe));
            if (string.IsNullOrEmpty(arguments)) throw new ArgumentNullException(nameof(arguments));

            Process nodejs = CreateProcess(exe, arguments);
            nodejs.Start();
            nodejs.WaitForExit(300 * 1000);
            return nodejs;
        }

        internal static Process InvokeExe(string name, string arguments, bool withSuffix = true)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            string executable = Path.Combine(BaseDirectory, (withSuffix ? WithSuffix(name) : name));
            if (!File.Exists(executable)) throw new FileNotFoundException($"Could not find file at '{executable}'.");

            Process exe = CreateProcess(executable, arguments);
            exe.Start();
            exe.WaitForExit(300 * 1000);
            return exe;
        }

        #region Backing Members

        private static Process CreateProcess(string executable, string args)
        {
            var info = new ProcessStartInfo(executable, args)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = BaseDirectory
            };
#if DEBUG
            System.Diagnostics.Debug.WriteLine("{0}> {1}", Path.GetFileName(executable), args);
#endif
            return new Process { StartInfo = info };
        }

        private static string WithSuffix(string name)
        {
            string suffix = "win";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                suffix = "linux";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                suffix = "osx";

            return string.Format("{0}-{1}{2}",
                Path.GetFileNameWithoutExtension(name),
                suffix,
                Path.GetExtension(name));
        }

        #endregion Backing Members
    }
}
//
// CCompiler.cs: asbtract class that provides some basic implementation for ICompiler
//
// Authors:
//   Marcos David Marin Amador <MarcosMarin@gmail.com>
//
// Copyright (C) 2007 Marcos David Marin Amador
//
//
// This source code is licenced under The MIT License:
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.IO;
using System.Text;
using System.CodeDom.Compiler;
using System.Threading.Tasks;

using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Projects;
using MonoDevelop.Ide;
using System.Linq;


namespace CBinding
{
	public abstract class CCompiler : FolderBasedProject, ICompiler
	{
		protected string compilerCommand;
		protected string linkerCommand;
		FilePath file;

		public abstract string Name {
			get;
		}

		public abstract Language Language {
			get;
		}

		public string CompilerCommand {
			get { return compilerCommand; }
		}

		public abstract bool SupportsCcache {
			get;
		}

		public abstract bool SupportsPrecompiledHeaders {
			get;
		}

		public abstract string GetCompilerFlags (Project project, CProjectConfiguration configuration);

		public abstract string GetDefineFlags (Project project, CProjectConfiguration configuration);

		public abstract BuildResult Compile (
			Project project,
			ProjectFileCollection projectFiles,
			ProjectPackageCollection packages,
			CProjectConfiguration configuration,
			ProgressMonitor monitor);

		public abstract void Clean (ProjectFileCollection projectFiles, CProjectConfiguration configuration, ProgressMonitor monitor);

		protected abstract void ParseCompilerOutput (string errorString, CompilerResults cr);

		protected abstract void ParseLinkerOutput (string errorString, CompilerResults cr);

		protected string GeneratePkgLinkerArgs (ProjectPackageCollection packages)
		{
			return GeneratePkgConfigArgs (packages, "--libs");
		}

		protected string GeneratePkgCompilerArgs (ProjectPackageCollection packages)
		{
			return GeneratePkgConfigArgs (packages, "--cflags");
		}

		protected static string GeneratePkgConfigArgs (ProjectPackageCollection packages, string pkgConfigArg)
		{
			if (packages == null || packages.Count < 1)
				return string.Empty;
			string originalPkgConfigPath = Environment.GetEnvironmentVariable ("PKG_CONFIG_PATH");
			string pkgConfigPath = originalPkgConfigPath;

			StringBuilder libs = new StringBuilder ();

			foreach (Package p in packages) {
				if (Path.IsPathRooted (p.File)) {
					pkgConfigPath = string.Format ("{0}{1}{2}", pkgConfigPath, Path.PathSeparator, Path.GetDirectoryName (p.File));
					libs.Append (Path.GetFileNameWithoutExtension (p.File) + " ");
				} else {
					libs.Append (p.File + " ");
				}
			}

			string args = string.Format ("{0} \"{1}\"", pkgConfigArg, libs.ToString ().Trim ());

			StringWriter output = new StringWriter ();
			ProcessWrapper proc = new ProcessWrapper ();

			try {
				Environment.SetEnvironmentVariable ("PKG_CONFIG_PATH", pkgConfigPath);
				proc = Runtime.ProcessService.StartProcess ("pkg-config", args, null, null);
				proc.WaitForExit ();

				string line;
				while ((line = proc.StandardOutput.ReadLine ()) != null)
					output.WriteLine (line);
			} catch (Exception ex) {
				MessageService.ShowError ("You need to have pkg-config installed");
			} finally {
				proc.Close ();
				Environment.SetEnvironmentVariable ("PKG_CONFIG_PATH", originalPkgConfigPath);
			}

			return output.ToString ();
		}

		Tuple<int, string> GetFileAndLine (string line, string separator)
		{
			int lineNumber = 0;
			string fileName = "";
			string s = line.Split (new string [] { separator }, StringSplitOptions.RemoveEmptyEntries) [1].Trim ();
			string [] args = s.Split (':');
			if (args [0].Length > 0) fileName = args [0];
			if (args.Length > 1 && args [1].Length > 0) {
				if (args [1].Contains ("("))
					int.TryParse (args [1].Split ('(') [0], out lineNumber);
				else
					int.TryParse (args [1], out lineNumber);
			}

			return Tuple.Create (lineNumber, fileName);
		}

		protected Stream ExecuteCommand (string command, string args, string workingDir, ProgressMonitor monitor)
		{
			var stream = new MemoryStream ();
			var streamWriter = new StreamWriter (stream);
			FilePath path = file.ParentDirectory.Combine (workingDir);
			ProcessWrapper p = Runtime.ProcessService.StartProcess (command, args, path, monitor.Log, streamWriter, null);
			p.WaitForExit ();
			streamWriter.Flush ();
			stream.Position = 0;
			return stream;
		}

		protected BuildResult ParseGenerationResult (Stream result, ProgressMonitor monitor)
		{
			var results = new BuildResult ();
			result.Position = 0;
			var sr = new StreamReader (result);
			var sb = new StringBuilder ();
			string line;
			string fileName = "";
			int lineNumber = 0;
			bool isWarning = false;

			while ((line = sr.ReadLine ()) != null) {
				//e.g.	CMake Warning in/at CMakeLists.txt:10 (COMMAND):
				//or:	CMake Warning:
				if (line.StartsWith ("CMake Warning", StringComparison.OrdinalIgnoreCase)) {
					//reset everything and add last error or warning.
					if (sb.Length > 0) {
						if (isWarning)
							results.AddWarning (BaseDirectory.Combine (fileName), lineNumber, 0, "", sb.ToString ());
						else
							results.AddError (BaseDirectory.Combine (fileName), lineNumber, 0, "", sb.ToString ());
					}

					sb.Clear ();
					fileName = "";
					lineNumber = 0;
					isWarning = true;

					// in/at CMakeLists.txt:10 (COMMAND):
					if (line.Contains (" in ")) {
						Tuple<int, string> t = GetFileAndLine (line, " in ");
						lineNumber = t.Item1;
						fileName = t.Item2;
					} else if (line.Contains (" at ")) {
						Tuple<int, string> t = GetFileAndLine (line, " at ");
						lineNumber = t.Item1;
						fileName = t.Item2;
					} else {
						string [] warning = line.Split (':');
						if (!string.IsNullOrEmpty (warning.ElementAtOrDefault (1))) {
							sb.Append (warning [1]);
						}
					}
				} else if (line.StartsWith ("CMake Error", StringComparison.OrdinalIgnoreCase)) {
					//reset everything and add last error or warning.
					if (sb.Length > 0) {
						if (isWarning)
							results.AddWarning (BaseDirectory.Combine (fileName), lineNumber, 0, "", sb.ToString ());
						else
							results.AddError (BaseDirectory.Combine (fileName), lineNumber, 0, "", sb.ToString ());
					}

					sb.Clear ();
					fileName = "";
					lineNumber = 0;
					isWarning = false;

					// in/at CMakeLists.txt:10 (COMMAND):
					if (line.Contains (" in ")) {
						Tuple<int, string> t = GetFileAndLine (line, " in ");
						lineNumber = t.Item1;
						fileName = t.Item2;
					} else if (line.Contains (" at ")) {
						Tuple<int, string> t = GetFileAndLine (line, " at ");
						lineNumber = t.Item1;
						fileName = t.Item2;
					} else {
						string [] error = line.Split (':');
						if (!string.IsNullOrEmpty (error.ElementAtOrDefault (1))) {
							sb.Append (error [1]);
						}
					}
				} else {
					sb.Append (line);
				}
			}

			return results;
		}
	}
}
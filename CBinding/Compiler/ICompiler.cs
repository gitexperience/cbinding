//
// ICompiler.cs: interface that must be implemented by any class that wants
// to provide a compiler for the CBinding addin.
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
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;

using Mono.Addins;

using MonoDevelop.Core;
using MonoDevelop.Projects;
using MonoDevelop.Core.Execution;


namespace CBinding
{
	public interface ICompiler
	{
		string Name {
			get;
		}

		Language Language {
			get;
		}

		string CompilerCommand {
			get;
		}

		bool SupportsCcache {
			get;
		}

		bool SupportsPrecompiledHeaders {
			get;
		}

		string GetCompilerFlags (Project project, CProjectConfiguration configuration);

		string GetDefineFlags (Project project, CProjectConfiguration configuration);

		BuildResult Compile (
			Project project,
			ProjectFileCollection projectFiles,
			ProjectPackageCollection packages,
			CProjectConfiguration configuration,
			ProgressMonitor monitor);

		void Clean (ProjectFileCollection projectFiles, CProjectConfiguration configuration, ProgressMonitor monitor);
	}

	[TypeExtensionPoint("/CBinding/Toolchains")]
	public abstract class CMakeToolchain : FolderBasedProject
	{
		FilePath file;
		public override FilePath FileName {
			get {
				return file;
			}
			set {
				file = value;
			}
		}

		/// The name of this toolchain.
		public abstract string ToolchainName { 
			get;
		}

		///Whether this toolchain is installed.
		bool IsInstalled { 
			get;
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

		public static CMakeToolchain GetToolchain (string toolchainName )
		{
			OperatingSystem os = Environment.OSVersion;
			if (os.Platform == PlatformID.Win32Windows || os.Platform == PlatformID.Win32S || os.Platform == PlatformID.WinCE || os.Platform == PlatformID.Win32NT) {
				if (toolchainName == "msvc")
					return new VS15MSBuildToolchain ();
				else return new MinGW32Toolchain ();
			} else if (os.Platform == PlatformID.Unix)
					return new UnixMakeToolchain ();
				else if (os.Platform == PlatformID.MacOSX)
					return new MacMakeToolchain ();
			else return new MinGW32Toolchain ();

		}

		/// Use cmake to generate makefiles for this toolchain.
		public abstract Task<BuildResult> GenerateMakefiles (string projectName, FilePath outputDirectory, ProgressMonitor monitor);

	}	

	[Extension("/CBinding/Toolchains")]
	public class VS15MSBuildToolchain : CMakeToolchain
	{

	/// <summary>
	/// The name of this Toolchain.
	/// </summary>
	/// <value>The name.</value>
		public override string ToolchainName {
			get {
				return "VS15 MSBuild Toolchain"; }
		}

		/// Use cmake to generate makefiles for this toolchain.
		public override Task<BuildResult> GenerateMakefiles (string projectName, FilePath outputDirectory, ProgressMonitor monitor)
		{
			monitor.BeginStep ("Generating build files...");
			Stream generationResult = ExecuteCommand ("cmake", "../ -G \"Visual Studio 15 2017\"", outputDirectory, monitor);
			BuildResult results = ParseGenerationResult (generationResult, monitor);
			monitor.EndStep ();

			string projectToBuild = $"{projectName}.\"sln\""; 
			monitor.BeginStep ("Building...");
			Stream buildResult = ExecuteCommand ("msbuild", projectToBuild, outputDirectory, monitor);
			monitor.EndStep ();

			return Task.FromResult (results);
		}

	}

	[Extension ("/CBinding/Toolchains")]
	public class MinGW32Toolchain : CMakeToolchain
	{
		/// <summary>
		/// The name of this Toolchain.
		/// </summary>
		/// <value>The name.</value>
		public override string ToolchainName {
			get { 
				return "MinGW32 Toolchain";
			}
		}

		/// Use cmake to generate makefiles for this toolchain.
		public override Task<BuildResult> GenerateMakefiles (string projectName, FilePath outputDirectory, ProgressMonitor monitor)
		{
			monitor.BeginStep ("Generating build files...");
			Stream generationResult = ExecuteCommand ("cmake", "../ -G \"MinGW Makefiles\"", outputDirectory, monitor);
			BuildResult results = ParseGenerationResult (generationResult, monitor);
			monitor.EndStep ();

			monitor.BeginStep ("Building...");
			Stream buildResult = ExecuteCommand ("mingw32-make", "", outputDirectory, monitor);
			monitor.EndStep ();

			return Task.FromResult (results);
		}
	}	

	[Extension ("/CBinding/Toolchains")]
	public class UnixMakeToolchain : CMakeToolchain
	{
		/// <summary>
		/// The name of this Toolchain.
		/// </summary>
		/// <value>The name.</value>
		public override string ToolchainName {
			get {
				return "Unix Make Toolchain";
			}
		}

		/// Use cmake to generate makefiles for this toolchain.
		public override Task<BuildResult> GenerateMakefiles (string projectName, FilePath outputDirectory, ProgressMonitor monitor)
		{
			monitor.BeginStep ("Generating build files...");
			Stream generationResult = ExecuteCommand ("cmake", "../", outputDirectory, monitor);
			BuildResult results = ParseGenerationResult (generationResult, monitor);
			monitor.EndStep ();

			monitor.BeginStep ("Building...");
			Stream buildResult = ExecuteCommand ("cmake", "--build ./ --clean-first", outputDirectory, monitor);
			monitor.EndStep ();

			return Task.FromResult (results);
		}
	}

	[Extension ("/CBinding/Toolchains")]
	public class MacMakeToolchain : CMakeToolchain
	{
		/// <summary>
		/// The name of this Toolchain.
		/// </summary>
		/// <value>The name.</value>
		public override string ToolchainName {
			get {
				return "Mac Make Toolchain";
			}
		}

		/// Use cmake to generate makefiles for this toolchain.
		public override Task<BuildResult> GenerateMakefiles (string projectName, FilePath outputDirectory, ProgressMonitor monitor)
		{
		monitor.BeginStep("Generating build files...");
		Stream generationResult = ExecuteCommand ("cmake", "../", outputDirectory, monitor);
		BuildResult results = ParseGenerationResult (generationResult, monitor);
		monitor.EndStep();

		monitor.BeginStep("Building...");
		Stream buildResult = ExecuteCommand ("cmake", "--build ./ --clean-first", outputDirectory, monitor);
		monitor.EndStep();

			return Task.FromResult(results);
		}
	}
}
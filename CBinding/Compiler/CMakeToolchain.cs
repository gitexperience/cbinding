//
// CMakeToolchain.cs: Abstract class that provides functionality to compile using various CMake Toolchains
//
// Authors:
//   Anubhav Singh <mailtoanubhav02@gmail.com>
// Copyright (C) 2017 Anubhav Singh
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

using Mono.Addins;

using MonoDevelop.Core;
using MonoDevelop.Projects;
using MonoDevelop.Core.Execution;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CBinding
{
	[TypeExtensionPoint ("/CBinding/Toolchains")]
	public class CMakeToolchain
	{
		FilePath file;

		/// <summary>
		/// Name of the toolchain.
		/// </summary>
		public virtual string ToolchainName {
			get;
		}

		/// <summary>
		/// Gets a value indicating whether this Toolchain is supported for the Platform or not.
		/// </summary>
		public virtual bool IsSupported {
			get;
		}

		/// <summary>
		/// CMake generator id for this toolchain.
		/// </summary>
		public virtual string GeneratorID => "";

		/// <summary>
		/// CMake toolchain id for this toolchain.
		/// </summary>
		public virtual string ToolchainID => "";

		/// <summary>
		/// Creates the CMake cache entry.
		/// </summary>
		public virtual string CMakeCacheEntry => "";

		public virtual string ProjectToBuild {
			get;
			set;
		}

		public string [] GetCompilerFlagsAsArray ()
		{
			FilePath outputDirectory = new FilePath ("./bin");
			List<string> compileCommands = new List<string> ();
			FilePath f = (file.ParentDirectory.Combine (outputDirectory)).Combine ("compile_commands.json");
			if (File.Exists (f))
			{
				using (StreamReader r = new StreamReader ("compile_commands.json"))
				{
					string json = r.ReadToEnd ();
					dynamic compilationDatabase = JsonConvert.DeserializeObject (json);
					foreach (var commandObject in compilationDatabase) {
						if (commandObject.command.Contains ("-o"))			//FIXME:- changes needed - Only two flags added.. 
							compileCommands.Add ("-o");
						if (commandObject.command.Contains ("-c"))
							compileCommands.Add ("-c");
					}
				}
			}
			return compileCommands.ToArray ();
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

		public void setFileLocation (string file)
		{
			this.file = file;
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
							results.AddWarning (fileName, lineNumber, 0, "", sb.ToString ());
						else
							results.AddError (fileName, lineNumber, 0, "", sb.ToString ());
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
							results.AddWarning (fileName, lineNumber, 0, "", sb.ToString ());
						else
							results.AddError (fileName, lineNumber, 0, "", sb.ToString ());
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

		public static CMakeToolchain GetToolchain ()
		{
			string toolchainName;
			toolchainName = PropertyService.Get<string> ("CBinding.DefaultToolchain", null);
			return AddinManager.GetExtensionObjects<CMakeToolchain> ("/CBinding/Toolchains")
				               .FirstOrDefault<CMakeToolchain> (toolchain => toolchain.ToolchainName == toolchainName)
				               ?? GetDefaultToolchain ();
		}

		public static CMakeToolchain GetDefaultToolchain ()
		{
			if (Platform.IsWindows)
				return new MinGW32Toolchain ();
			else if (Platform.IsLinux)
				return new UnixMakeToolchain ();
			else if (Platform.IsMac)
				return new MacMakeToolchain ();
			else return new MinGW32Toolchain ();
		}

		/// <summary>
		/// Use cmake to generate makefiles for this toolchain.
		/// </summary>
		/// <returns>The makefiles.</returns>
		/// <param name="projectName">Project name.</param>
		/// <param name="outputDirectory">Output directory.</param>
		/// <param name="monitor">Monitor.</param>
		public virtual Task<BuildResult> GenerateMakefiles (string projectName, FilePath outputDirectory, ProgressMonitor monitor)
		{
			string arguments = "../";
			if(!string.IsNullOrEmpty (GeneratorID)) {
				arguments += $" -G\"{GeneratorID}\"";
			}
			if(!string.IsNullOrEmpty (ToolchainID)) {
				arguments += $" -T\"{ToolchainID}\"";
			}
			if(!string.IsNullOrEmpty (CMakeCacheEntry)) {
				arguments += $" -D{CMakeCacheEntry}";
			}

			monitor.BeginStep ("Generating build files...");
			Stream generationResult = ExecuteCommand ("cmake", arguments, outputDirectory, monitor);
			BuildResult results = ParseGenerationResult (generationResult, monitor);
			monitor.EndStep ();

			Task<Stream> buildResult = Build (projectName, outputDirectory, monitor);

			return Task.FromResult (results);
		}

		public virtual Task<Stream> Build (string projectName, FilePath outputDirectory, ProgressMonitor monitor)
		{
			monitor.BeginStep ("Building...");
			Stream buildResult = ExecuteCommand ("cmake", "--build ./", outputDirectory, monitor);
			monitor.EndStep ();
			return Task.FromResult (buildResult);
		}

		public virtual Task<BuildResult> Clean (string projectName, FilePath outputDirectory, ProgressMonitor monitor)
		{
			monitor.BeginStep ("Cleaning...");
			Stream buildResult = ExecuteCommand ("cmake", "--build ./ --target clean", outputDirectory, monitor);
			monitor.EndStep ();
			BuildResult results = ParseGenerationResult (buildResult, monitor);
			return Task.FromResult (results);
		}

		public virtual Task<Stream> Rebuild (string projectName, FilePath outputDirectory, ProgressMonitor monitor)
		{
			monitor.BeginStep ("Rebuilding...");
			Stream buildResult = ExecuteCommand ("cmake", "--build ./ --clean-first", outputDirectory, monitor);
			monitor.EndStep ();
			return Task.FromResult (buildResult);
		}
	}
}
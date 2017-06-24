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
using System.Threading.Tasks;
using System.IO;
using Mono.Addins;

using MonoDevelop.Core;
using MonoDevelop.Projects;


namespace CBinding
{
	[TypeExtensionPoint ("/CBinding/Compilers")]
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

	abstract class CMakeToolchain :CCompiler
	{
		CMakeToolchain cmakeToolchain;
		VS15MSBuildToolchain vs15BuildToolchain;
		MinGW32Toolchain mingw32Toolchain;
		/// The name of this toolchain
		string Name { get; }

		///Whether this toolchain is installed.
		bool IsInstalled { get; }

		/// Use cmake to generate makefiles for this toolchain.
		public virtual Task<BuildResult> GenerateMakefiles (
			string projectName, 
			FilePath outputDirectory, 
			ProgressMonitor monitor, 
			string buildConfiguration)
		{
			if (buildConfiguration == "msvc")
			{
				return (vs15BuildToolchain.GenerateMakefiles (projectName, outputDirectory, monitor));
			} else {
				return (mingw32Toolchain.GenerateMakefiles (projectName, outputDirectory, monitor));	//testing
			}
		}

		/// Build using the makefiles for this toolchain.
		public abstract Task<BuildResult> Build (string projectName, FilePath outputDirectory, ProgressMonitor monitor);
	}	


	abstract class VS15MSBuildToolchain : CMakeToolchain
	{
		VS15MSBuildToolchain vs15BuildToolchain;
		/// The name of this toolchain
		string Name { get; }

		///Whether this toolchain is installed.
		bool IsInstalled { get; }

		/// Use cmake to generate makefiles for this toolchain.
		public Task<BuildResult> GenerateMakefiles (string projectName, FilePath outputDirectory, ProgressMonitor monitor)
		{
			Stream generationResult = vs15BuildToolchain.ExecuteCommand ("cmake", "../ -G \"Visual Studio 15 2017\"", outputDirectory, monitor);
			BuildResult results = vs15BuildToolchain.ParseGenerationResult (generationResult, monitor);

			monitor.BeginStep ("Building...");		//building
			Stream buildResult = ExecuteCommand ("msbuild", projectName, outputDirectory, monitor);
			monitor.EndStep ();

			return Task.FromResult (results);
		}

		/// Build using the makefiles for this toolchain.
/*		public override Task<BuildResult> Build (string projectName, FilePath outputDirectory, ProgressMonitor monitor)
		{
			Stream buildResult = ExecuteCommand ("msbuild", projectName, outputDirectory, monitor);
			BuildResult bs;
			return Task.FromResult <BuildResult> (bs);
		}
		*/
	}
	abstract class MinGW32Toolchain : CMakeToolchain
	{
		MinGW32Toolchain mingw32Toolchain;
		/// The name of this toolchain
		string Name { get; }

		///Whether this toolchain is installed.
		bool IsInstalled { get; }

		/// Use cmake to generate makefiles for this toolchain.
		public Task<BuildResult> GenerateMakefiles (string projectName, FilePath outputDirectory, ProgressMonitor monitor)
		{
			Stream generationResult = mingw32Toolchain.ExecuteCommand ("cmake", "../ -G \"Visual Studio 15 2017\"", outputDirectory, monitor);
			BuildResult results = mingw32Toolchain.ParseGenerationResult (generationResult, monitor);

			monitor.BeginStep ("Building...");		//building 
			Stream buildResult = ExecuteCommand ("mingw32-make", "", outputDirectory, monitor);
			monitor.EndStep ();

			return Task.FromResult (results);
		}

		/// Build using the makefiles for this toolchain.
	//	Task<BuildResult> Build (string projectName, FilePath outputDirectory, ProgressMonitor monitor);
	}

/*	abstract class UnixMakeToolchain : CMakeToolchain
	{
		/// The name of this toolchain
		string Name { get; }

		///Whether this toolchain is installed.
		bool IsInstalled { get; }

		/// Use cmake to generate makefiles for this toolchain.
		Task<BuildResult> GenerateMakefiles (string projectName, FilePath outputDirectory, ProgressMonitor monitor);

		/// Build using the makefiles for this toolchain.
		Task<BuildResult> Build (string projectName, FilePath outputDirectory, ProgressMonitor monitor);
	}

	abstract class MacMakeToolchain : CMakeToolchain
	{
		/// The name of this toolchain
		string Name { get; }

		///Whether this toolchain is installed.
		bool IsInstalled { get; }

		/// Use cmake to generate makefiles for this toolchain.
		Task<BuildResult> GenerateMakefiles (string projectName, FilePath outputDirectory, ProgressMonitor monitor);

		/// Build using the makefiles for this toolchain.
		Task<BuildResult> Build (string projectName, FilePath outputDirectory, ProgressMonitor monitor);
	} */
}
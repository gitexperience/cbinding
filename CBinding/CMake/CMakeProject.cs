//
// CMakeProject.cs
//
// Author:
//       Elsayed Awdallah <comando4ever@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Gtk;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace CBinding
{
	public class CMakeProject : FolderBasedProject
	{
		FilePath file;
		string name;
		FilePath outputDirectory = new FilePath ("./bin");
		CMakeFileFormat fileFormat;
		CMakeToolchain cmakeToolchain;

		/// <summary>
		/// Occurs when a file is removed from this project.
		/// </summary>
		public event ProjectFileEventHandler FileRemovedFromProject;

		/// <summary>
		/// Occurs when a file is added to this project.
		/// </summary>
		public event ProjectFileEventHandler FileAddedToProject;

		/// <summary>
		/// Occurs when a file of this project has been modified
		/// </summary>
		public event ProjectFileEventHandler FileChangedInProject;

		/// <summary>
		/// Occurs when a file of this project has been renamed
		/// </summary>
		public event ProjectFileRenamedEventHandler FileRenamedInProject;


		public bool HasLibClang { get; private set; }

		public CLangManager ClangManager { get; private set; }

		public SymbolDatabaseMediator DB { get; private set; }

		public UnsavedFilesManager UnsavedFiles { get; private set; }

		ProjectItemCollection items;
		/// <summary>
		/// Files of the project
		/// </summary>
		public ProjectFileCollection Files {
			get { return projectFiles; }
		}
		public ProjectFileCollection projectFiles;

		public ProjectItemCollection Items {
			get { return items; }
		}

		static readonly string [] supportedLanguages = { "C", "C++", "Objective-C", "Objective-C++" };

		public static Regex extensions = new Regex (@"(\.c|\.c\+\+|\.cc|\.cpp|\.cxx|\.m|\.mm|\.h|\.hh|\.h\+\+|\.hm|\.hpp|\.hxx|\.in|\.txx)$",
									  RegexOptions.IgnoreCase);

		public override FilePath FileName {
			get {
				return file;
			}
			set {
				file = value;
			}
		}

		bool CheckCMake ()
		{
			try {
				ProcessWrapper p = Runtime.ProcessService.StartProcess ("cmake", "--version", null, null);
				p.WaitForOutput ();
				return true;
			} catch {
				return false;
			}
		}

		public void RemoveTarget (string targetName)
		{
			fileFormat.RemoveTarget (targetName);
		}

		protected override string OnGetBaseDirectory ()
		{
			return file.ParentDirectory.ToString ();
		}

		protected override string OnGetName ()
		{
			return name;
		}

		public string MatchingFile (string sourceFile)
		{
			string filenameStub = Path.GetFileNameWithoutExtension (sourceFile);
			bool wantHeader = !CMakeProject.IsHeaderFile (sourceFile);

			foreach (ProjectFile file in this.Files) {
				if (filenameStub == Path.GetFileNameWithoutExtension (file.Name)
				   && (wantHeader == IsHeaderFile (file.Name))) {
					return file.Name;
				}
			}

			return null;
		}

		/// <summary>
		/// Determines if a header file is specified by filename.
		/// </summary>
		/// <returns><c>true</c> if a header file is specified by filename; otherwise, <c>false</c>.</returns>
		/// <param name="filename">Filename.</param>
		public static bool IsHeaderFile (string filename)
		{
			return (0 <= Array.IndexOf (extensions.Split ("|").ToArray (), Path.GetExtension (filename.ToUpper ())));
		}

		public void LoadFrom (FilePath file)
		{
			this.file = file;
			CMakeFileFormat fileFormat = new CMakeFileFormat (file, this);
			name = fileFormat.ProjectName;
			this.fileFormat = fileFormat;
		}

		protected override Task OnSave (ProgressMonitor monitor)
		{
			return Task.Factory.StartNew (() => {
				fileFormat.SaveAll ();
			});
		}

		protected override IEnumerable<WorkspaceObject> OnGetChildren ()
		{
			foreach (CMakeTarget target in fileFormat.Targets.Values)
				target.ParentObject = this;
			return fileFormat.Targets.Values.ToList ();
		}

		List<FilePath> files = new List<FilePath> ();

		protected override IEnumerable<FilePath> OnGetItemFiles (bool includeReferencedFiles)
		{
			files.Clear ();
			foreach (CMakeTarget target in fileFormat.Targets.Values) {
				files = files.Concat (target.GetFiles ()).ToList ();
			}
			return files;
		}

		CMakeTarget GetTarget (FilePath fileName)
		{
			string filename = fileName.FileName;
			foreach (CMakeTarget target in fileFormat.Targets.Values) {
				foreach (string file in target.Files) {
					if (file.EndsWith (filename, StringComparison.OrdinalIgnoreCase))
						return target;
				}
			}
			return null;
		}

		void AddFile (FilePath fileName, string targetName)
		{
			fileName = fileName.ToRelative (file.ParentDirectory);
			foreach (string target in fileFormat.Targets.Keys) {
				if (target.StartsWith (targetName, StringComparison.OrdinalIgnoreCase))
					fileFormat.Targets [target].AddFile (fileName.ToString ());
			}
		}

		protected async override Task<BuildResult> OnBuild (ProgressMonitor monitor, ConfigurationSelector configuration,
													  OperationContext operationContext)
		{
			BuildResult results;

			if (!CheckCMake ()) {
				results = new BuildResult ();
				results.AddError ("CMake cannot be found.");
				return results;
			}

			FileService.CreateDirectory (file.ParentDirectory.Combine (outputDirectory));

			cmakeToolchain = CMakeToolchain.GetToolchain ();
			cmakeToolchain.setFileLocation (file);
			results = await cmakeToolchain.GenerateMakefiles (fileFormat.ProjectName, outputDirectory, monitor);

			return results;

		}

		protected async override Task<BuildResult> OnClean (ProgressMonitor monitor, ConfigurationSelector configuration,
															OperationContext buildSession)
		{
			BuildResult results = await cmakeToolchain.Clean (fileFormat.ProjectName, outputDirectory, monitor);
			return results;
		}

		protected override Task OnExecute (ProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration)
		{
			return Task.Factory.StartNew (async () => {
				ExternalConsole console = context.ExternalConsoleFactory.CreateConsole (false, monitor.CancellationToken);
				string targetName = "";
				foreach (var target in fileFormat.Targets.Values) {
					if (target.Type == CMakeTarget.Types.Binary) {
						targetName = target.Name;
						break;
					}
				}

				if (string.IsNullOrEmpty (targetName)) {
					monitor.ReportError ("Can't find an executable target.");
					return;
				}
				FilePath f = BaseDirectory.Combine (outputDirectory);
				NativeExecutionCommand cmd;
				if (File.Exists (f.Combine (targetName)))
					cmd = new NativeExecutionCommand (f.Combine (targetName));
				else if (File.Exists (f.Combine (string.Format ("{0}.{1}", targetName, "exe"))))
					cmd = new NativeExecutionCommand (f.Combine (string.Format ("{0}.{1}", targetName, "exe")));
				else if (File.Exists (f.Combine ("./Debug", targetName)))
					cmd = new NativeExecutionCommand (f.Combine ("./Debug", targetName));
				else if (File.Exists (f.Combine ("./Debug", string.Format ("{0}.{1}", targetName, "exe"))))
					cmd = new NativeExecutionCommand (f.Combine ("./Debug", string.Format ("{0}.{1}", targetName, "exe")));
				else {
					monitor.ReportError ("Can't determine executable path.");
					return;
				}

				try {
					var handler = Runtime.ProcessService.GetDefaultExecutionHandler (cmd);
					var op = handler.Execute (cmd, console);

					using (var t = monitor.CancellationToken.Register (op.Cancel))
						await op.Task;

					monitor.Log.WriteLine ("The operation exited with code: {0}", op.ExitCode);
				} catch (Exception ex) {
					monitor.ReportError ("Can't execute the target.", ex);
				} finally {
					console.Dispose ();
				}
			});
		}

		public override string [] OnGetSupportedLanguages ()
		{
			return supportedLanguages;
		}

		protected override bool OnGetCanExecute (ExecutionContext context, ConfigurationSelector configuration)
		{
			foreach (CMakeTarget target in fileFormat.Targets.Values) {
				if (target.Type == CMakeTarget.Types.Binary) return true;
			}
			return false;
		}

		public override void OnFileRemoved (FilePath file)
		{
			base.OnFileRemoved (file);

			FileRemovedFromProject?.Invoke (this, new ProjectFileEventArgs ());

			foreach (var target in fileFormat.Targets.Values.ToList ()) {
				target.RemoveFile (file);
			}

			fileFormat.SaveAll ();
		}

		public override void OnFilesRemoved (List<FilePath> files)
		{
			base.OnFilesRemoved (files);

			foreach (var target in fileFormat.Targets.Values.ToList ()) {
				target.RemoveFiles (files);
			}

			fileFormat.SaveAll ();
		}

		public override void OnFileRenamed (FilePath oldFile, FilePath newFile)
		{
			base.OnFileRenamed (oldFile, newFile);

		//	FileRenamedInProject?.Invoke (this, new ProjectFileRenamedEventArgs ());   FIXME:- Need a fix

			var oldFiles = new List<FilePath> () { oldFile };
			var newFiles = new List<FilePath> () { newFile };

			foreach (var target in fileFormat.Targets.Values.ToList ()) {
				target.RenameFiles (oldFiles, newFiles);
			}

			fileFormat.SaveAll ();
		}

		public override void OnFileMoved (FilePath src, FilePath dst)
		{
			base.OnFileMoved (src, dst);

			var oldFiles = new List<FilePath> () { src };
			var newFiles = new List<FilePath> () { dst };

			foreach (var target in fileFormat.Targets.Values.ToList ()) {
				target.RenameFiles (oldFiles, newFiles);
			}

			fileFormat.SaveAll ();
		}

		public override void OnFilesMoved (List<FilePath> src, List<FilePath> dst)
		{
			base.OnFilesMoved (src, dst);

			foreach (var target in fileFormat.Targets.Values.ToList ()) {
				target.RenameFiles (src, dst);
			}

			fileFormat.SaveAll ();
		}

		public override void OnFilesRenamed (List<FilePath> oldFiles, List<FilePath> newFiles)
		{
			base.OnFilesRenamed (oldFiles, newFiles);

			foreach (var target in fileFormat.Targets.Values.ToList ()) {
				target.RenameFiles (oldFiles, newFiles);
			}

			fileFormat.SaveAll ();
		}

		public override void OnFileChanged (FilePath file)
		{
			base.OnFileChanged (file);

			if (!extensions.IsMatch (file))
				return;

			FileChangedInProject?.Invoke (this, new ProjectFileEventArgs ());
		}

		public override void OnFileAdded (FilePath file)
		{
			base.OnFileAdded (file);

			if (!extensions.IsMatch (file))
				return;

			FileAddedToProject?.Invoke (this, new ProjectFileEventArgs ());

			using (var dlg = new TargetPickerDialog ("Pick a target", fileFormat)) {
				if (MessageService.ShowCustomDialog (dlg) != (int)ResponseType.Ok)
					return;

				foreach (var target in dlg.SelectedTargets) {
					target.AddFile (file.CanonicalPath.ToRelative (fileFormat.File.ParentDirectory));
				}
			}

			fileFormat.SaveAll ();
		}

		public override void OnFilesAdded (List<FilePath> files)
		{
			base.OnFilesAdded (files);

			var filesToAdd = new List<FilePath> ();
			foreach (var file in files) {
				if (extensions.IsMatch (file))
					filesToAdd.Add (file);
			}

			if (filesToAdd.Count == 0)
				return;

			using (var dlg = new TargetPickerDialog ("Pick a target", fileFormat)) {
				if (MessageService.ShowCustomDialog (dlg) != (int)ResponseType.Ok)
					return;

				foreach (var target in dlg.SelectedTargets) {
					foreach (var file in filesToAdd)
						target.AddFile (file.CanonicalPath.ToRelative (fileFormat.File.ParentDirectory));
				}
			}

			fileFormat.SaveAll ();
		}

		public override void OnFileCopied (FilePath src, FilePath dst)
		{
			base.OnFileCopied (src, dst);

			if (dst.IsDirectory) {
				dst = dst + Path.DirectorySeparatorChar + src.FileName;
			}

			OnFileAdded (dst);
		}

		public override void OnFilesCopied (List<FilePath> src, List<FilePath> dst)
		{
			base.OnFilesCopied (src, dst);

			for (int i = 0; i < src.Count; i++) {
				if (dst [i].IsDirectory) {
					dst [i] = dst [i] + Path.DirectorySeparatorChar + src [i].FileName;
				}
			}

			OnFilesAdded (dst);
		}

		public CMakeProject ()
		{
			Initialize (this);
		}

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		protected override void OnInitialize ()
		{
			base.OnInitialize ();
			try {
				ClangManager = new CLangManager (this);
				DB = new SymbolDatabaseMediator (this, ClangManager);
				UnsavedFiles = new UnsavedFilesManager (this);
				HasLibClang = true;
			} catch (DllNotFoundException ex) {
				LoggingService.LogError ("Could not load libclang", ex);
				HasLibClang = false;
			}
		}
	}
}
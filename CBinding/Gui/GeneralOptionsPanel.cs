//
// GeneralOptionsPanel.cs
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
using System.Collections.Generic;

using Mono.Addins;

using MonoDevelop.Core;
using MonoDevelop.Components;
using MonoDevelop.Ide.Gui.Dialogs;

namespace CBinding
{
	public partial class GeneralOptionsPanel : Gtk.Bin
	{

		CMakeToolchain default_toolchain;
		List<CMakeToolchain> toolchain = new List<CMakeToolchain> ();

		public GeneralOptionsPanel ()
		{
			this.Build ();

			object [] toolchains = AddinManager.GetExtensionObjects ("/CBinding/Toolchains");
			foreach (CMakeToolchain Toolchain in toolchains) {
				toolchain.Add (Toolchain); 
			}

			foreach (CMakeToolchain Toolchain in toolchain) {
				if (Toolchain.IsSupported) {
					cCombo.AppendText (Toolchain.ToolchainName);
				}
			}
				
			
			string toolchainName = PropertyService.Get<string> ("CBinding.DefaultToolchain", null)
			                                      ?? CMakeToolchain.GetDefaultToolchain ().ToolchainName;
			ctagsEntry.Text = PropertyService.Get<string> ("CBinding.CTagsExecutable", "ctags");
			parseSystemTagsCheck.Active = PropertyService.Get<bool> ("CBinding.ParseSystemTags", true);
			parseLocalVariablesCheck.Active = PropertyService.Get<bool> ("CBinding.ParseLocalVariables", false);

			foreach (CMakeToolchain Toolchain in toolchains) {
				if (Toolchain.ToolchainName == toolchainName) {
					default_toolchain = Toolchain;
				}
			}

			if (default_toolchain == null)
				default_toolchain = new MinGW32Toolchain ();
			
			int active;
			Gtk.TreeIter iter;
			Gtk.ListStore store;

			active = 0;
			store = (Gtk.ListStore)cCombo.Model;
			store.GetIterFirst (out iter);

			while (store.IterIsValid (iter)) {
				if ((string)store.GetValue (iter, 0) == default_toolchain.ToolchainName) {
					break;
				}
				store.IterNext (ref iter);
				active++;
			}

			cCombo.Active = active;
		
		}
		public bool Store ()
		{
			PropertyService.Set ("CBinding.DefaultToolchain", default_toolchain.ToolchainName);
			PropertyService.Set ("CBinding.CTagsExecutable", ctagsEntry.Text.Trim ());
			PropertyService.Set ("CBinding.ParseSystemTags", parseSystemTagsCheck.Active);
			PropertyService.Set ("CBinding.ParseLocalVariables", parseLocalVariablesCheck.Active);
			PropertyService.SaveProperties ();
			return true;
		}

		protected virtual void OnCComboChanged (object sender, System.EventArgs e)
		{
			string activeToolchain = cCombo.ActiveText;
			
			foreach (CMakeToolchain Toolchain in toolchain) {
				if (Toolchain.ToolchainName == activeToolchain) {
					default_toolchain = Toolchain;
				}
			}

			if (default_toolchain == null)
				default_toolchain = new MinGW32Toolchain ();
		}

		protected virtual void OnCtagsBrowseClicked (object sender, EventArgs e)
		{
			var dialog = new OpenFileDialog (GettextCatalog.GetString ("Choose ctags executable"), FileChooserAction.Open);
			if (dialog.Run ())
				ctagsEntry.Text = dialog.SelectedFile;
		}
	}
	
	public class GeneralOptionsPanelBinding : OptionsPanel
	{
		private GeneralOptionsPanel panel;
		
		public override Control CreatePanelWidget ()
		{
			panel = new GeneralOptionsPanel ();
			return panel;
		}
		
		public override void ApplyChanges ()
		{
			panel.Store ();
		}
	}
}

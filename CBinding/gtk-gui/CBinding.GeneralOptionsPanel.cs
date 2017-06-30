
// This file has been generated by the GUI designer. Do not modify.
namespace CBinding
{
	public partial class GeneralOptionsPanel
	{
		private global::Gtk.Table table1;

		private global::Gtk.ComboBox cCombo;

		private global::Gtk.Button ctagsBrowse;

		private global::Gtk.Entry ctagsEntry;

		private global::Gtk.Label label1;

		private global::Gtk.Label label3;

		private global::Gtk.CheckButton parseLocalVariablesCheck;

		private global::Gtk.CheckButton parseSystemTagsCheck;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget CBinding.GeneralOptionsPanel
			global::Stetic.BinContainer.Attach (this);
			this.Name = "CBinding.GeneralOptionsPanel";
			// Container child CBinding.GeneralOptionsPanel.Gtk.Container+ContainerChild
			this.table1 = new global::Gtk.Table (((uint)(4)), ((uint)(3)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.cCombo = global::Gtk.ComboBox.NewText ();
			this.cCombo.TooltipMarkup = "Default Toolchain to use for C/C++ projects.";
			this.cCombo.Name = "cCombo";
			this.table1.Add (this.cCombo);
			global::Gtk.Table.TableChild w1 = ((global::Gtk.Table.TableChild)(this.table1 [this.cCombo]));
			w1.LeftAttach = ((uint)(1));
			w1.RightAttach = ((uint)(2));
			w1.XOptions = ((global::Gtk.AttachOptions)(4));
			w1.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ctagsBrowse = new global::Gtk.Button ();
			this.ctagsBrowse.CanFocus = true;
			this.ctagsBrowse.Name = "ctagsBrowse";
			this.ctagsBrowse.UseUnderline = true;
			this.ctagsBrowse.Label = global::Mono.Unix.Catalog.GetString ("Browse");
			global::Gtk.Image w2 = new global::Gtk.Image ();
			w2.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-open", global::Gtk.IconSize.Menu);
			this.ctagsBrowse.Image = w2;
			this.table1.Add (this.ctagsBrowse);
			global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.table1 [this.ctagsBrowse]));
			w3.TopAttach = ((uint)(1));
			w3.BottomAttach = ((uint)(2));
			w3.LeftAttach = ((uint)(2));
			w3.RightAttach = ((uint)(3));
			w3.XOptions = ((global::Gtk.AttachOptions)(4));
			w3.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ctagsEntry = new global::Gtk.Entry ();
			this.ctagsEntry.CanFocus = true;
			this.ctagsEntry.Name = "ctagsEntry";
			this.ctagsEntry.IsEditable = true;
			this.ctagsEntry.InvisibleChar = '●';
			this.table1.Add (this.ctagsEntry);
			global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.table1 [this.ctagsEntry]));
			w4.TopAttach = ((uint)(1));
			w4.BottomAttach = ((uint)(2));
			w4.LeftAttach = ((uint)(1));
			w4.RightAttach = ((uint)(2));
			w4.XOptions = ((global::Gtk.AttachOptions)(4));
			w4.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.Xpad = 10;
			this.label1.Xalign = 1F;
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Default Toolchain");
			this.table1.Add (this.label1);
			global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.table1 [this.label1]));
			w5.XOptions = ((global::Gtk.AttachOptions)(4));
			w5.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.Xpad = 10;
			this.label3.Xalign = 1F;
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("CTags: ");
			this.label3.Justify = ((global::Gtk.Justification)(1));
			this.table1.Add (this.label3);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table1 [this.label3]));
			w6.TopAttach = ((uint)(1));
			w6.BottomAttach = ((uint)(2));
			w6.XOptions = ((global::Gtk.AttachOptions)(4));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.parseLocalVariablesCheck = new global::Gtk.CheckButton ();
			this.parseLocalVariablesCheck.CanFocus = true;
			this.parseLocalVariablesCheck.Name = "parseLocalVariablesCheck";
			this.parseLocalVariablesCheck.Label = global::Mono.Unix.Catalog.GetString ("Parse Local Variables");
			this.parseLocalVariablesCheck.DrawIndicator = true;
			this.parseLocalVariablesCheck.UseUnderline = true;
			this.table1.Add (this.parseLocalVariablesCheck);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table1 [this.parseLocalVariablesCheck]));
			w7.TopAttach = ((uint)(3));
			w7.BottomAttach = ((uint)(4));
			w7.XPadding = ((uint)(10));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.parseSystemTagsCheck = new global::Gtk.CheckButton ();
			this.parseSystemTagsCheck.TooltipMarkup = "Choose whether you want to parse system tags or not, if you do you will get compl" +
				"etion for things like printf, but the tag parsing process will take considerably" +
				" longer.";
			this.parseSystemTagsCheck.CanFocus = true;
			this.parseSystemTagsCheck.Name = "parseSystemTagsCheck";
			this.parseSystemTagsCheck.Label = global::Mono.Unix.Catalog.GetString ("Parse System Tags");
			this.parseSystemTagsCheck.DrawIndicator = true;
			this.parseSystemTagsCheck.UseUnderline = true;
			this.table1.Add (this.parseSystemTagsCheck);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.table1 [this.parseSystemTagsCheck]));
			w8.TopAttach = ((uint)(2));
			w8.BottomAttach = ((uint)(3));
			w8.XPadding = ((uint)(10));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			this.Add (this.table1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Show ();
			this.ctagsBrowse.Clicked += new global::System.EventHandler (this.OnCtagsBrowseClicked);
			this.cCombo.Changed += new global::System.EventHandler (this.OnCComboChanged);
		}
	}
}

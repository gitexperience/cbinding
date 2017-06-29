
// This file has been generated by the GUI designer. Do not modify.
namespace CBinding
{
	public partial class OutputOptionsPanel
	{
		private global::Gtk.VBox vbox2;

		private global::Gtk.Table table1;

		private global::Gtk.Label label1;

		private global::Gtk.Label label2;

		private global::Gtk.Label label3;

		private global::Gtk.Label label4;

		private global::MonoDevelop.Components.FolderEntry outputEntry;

		private global::Gtk.Entry outputNameTextEntry;

		private global::Gtk.Entry parametersTextEntry;

		private global::Gtk.CheckButton externalConsoleCheckbox;

		private global::Gtk.CheckButton pauseCheckbox;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget CBinding.OutputOptionsPanel
			global::Stetic.BinContainer.Attach(this);
			this.Name = "CBinding.OutputOptionsPanel";
			// Container child CBinding.OutputOptionsPanel.Gtk.Container+ContainerChild
			this.vbox2 = new global::Gtk.VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			this.vbox2.BorderWidth = ((uint)(3));
			// Container child vbox2.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table(((uint)(4)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			this.table1.BorderWidth = ((uint)(3));
			// Container child table1.Gtk.Table+TableChild
			this.label1 = new global::Gtk.Label();
			this.label1.Name = "label1";
			this.label1.Xalign = 0F;
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Output</b>");
			this.label1.UseMarkup = true;
			this.table1.Add(this.label1);
			global::Gtk.Table.TableChild w1 = ((global::Gtk.Table.TableChild)(this.table1[this.label1]));
			w1.XOptions = ((global::Gtk.AttachOptions)(4));
			w1.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label2 = new global::Gtk.Label();
			this.label2.Name = "label2";
			this.label2.Xalign = 0F;
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString("Output Name:");
			this.table1.Add(this.label2);
			global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.table1[this.label2]));
			w2.TopAttach = ((uint)(1));
			w2.BottomAttach = ((uint)(2));
			w2.XPadding = ((uint)(15));
			w2.XOptions = ((global::Gtk.AttachOptions)(4));
			w2.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label();
			this.label3.Name = "label3";
			this.label3.Xalign = 0F;
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString("Output Path:");
			this.table1.Add(this.label3);
			global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.table1[this.label3]));
			w3.TopAttach = ((uint)(2));
			w3.BottomAttach = ((uint)(3));
			w3.XPadding = ((uint)(15));
			w3.XOptions = ((global::Gtk.AttachOptions)(4));
			w3.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label4 = new global::Gtk.Label();
			this.label4.Name = "label4";
			this.label4.Xalign = 0F;
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString("Parameters:");
			this.table1.Add(this.label4);
			global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.table1[this.label4]));
			w4.TopAttach = ((uint)(3));
			w4.BottomAttach = ((uint)(4));
			w4.XPadding = ((uint)(15));
			w4.XOptions = ((global::Gtk.AttachOptions)(4));
			w4.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.outputEntry = null;
			this.table1.Add(this.outputEntry);
			global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.table1[this.outputEntry]));
			w5.TopAttach = ((uint)(2));
			w5.BottomAttach = ((uint)(3));
			w5.LeftAttach = ((uint)(1));
			w5.RightAttach = ((uint)(2));
			w5.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.outputNameTextEntry = new global::Gtk.Entry();
			this.outputNameTextEntry.CanFocus = true;
			this.outputNameTextEntry.Name = "outputNameTextEntry";
			this.outputNameTextEntry.IsEditable = true;
			this.outputNameTextEntry.InvisibleChar = '●';
			this.table1.Add(this.outputNameTextEntry);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table1[this.outputNameTextEntry]));
			w6.TopAttach = ((uint)(1));
			w6.BottomAttach = ((uint)(2));
			w6.LeftAttach = ((uint)(1));
			w6.RightAttach = ((uint)(2));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.parametersTextEntry = new global::Gtk.Entry();
			this.parametersTextEntry.CanFocus = true;
			this.parametersTextEntry.Name = "parametersTextEntry";
			this.parametersTextEntry.IsEditable = true;
			this.parametersTextEntry.InvisibleChar = '●';
			this.table1.Add(this.parametersTextEntry);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table1[this.parametersTextEntry]));
			w7.TopAttach = ((uint)(3));
			w7.BottomAttach = ((uint)(4));
			w7.LeftAttach = ((uint)(1));
			w7.RightAttach = ((uint)(2));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox2.Add(this.table1);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.table1]));
			w8.Position = 0;
			w8.Expand = false;
			w8.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.externalConsoleCheckbox = new global::Gtk.CheckButton();
			this.externalConsoleCheckbox.CanFocus = true;
			this.externalConsoleCheckbox.Name = "externalConsoleCheckbox";
			this.externalConsoleCheckbox.Label = global::Mono.Unix.Catalog.GetString("Run on e_xternal console");
			this.externalConsoleCheckbox.Active = true;
			this.externalConsoleCheckbox.DrawIndicator = true;
			this.externalConsoleCheckbox.UseUnderline = true;
			this.vbox2.Add(this.externalConsoleCheckbox);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.externalConsoleCheckbox]));
			w9.Position = 1;
			w9.Expand = false;
			w9.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.pauseCheckbox = new global::Gtk.CheckButton();
			this.pauseCheckbox.Sensitive = false;
			this.pauseCheckbox.CanFocus = true;
			this.pauseCheckbox.Name = "pauseCheckbox";
			this.pauseCheckbox.Label = global::Mono.Unix.Catalog.GetString("Pause _console output");
			this.pauseCheckbox.DrawIndicator = true;
			this.pauseCheckbox.UseUnderline = true;
			this.vbox2.Add(this.pauseCheckbox);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.pauseCheckbox]));
			w10.Position = 2;
			w10.Expand = false;
			w10.Fill = false;
			this.Add(this.vbox2);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Show();
			this.externalConsoleCheckbox.Clicked += new global::System.EventHandler(this.OnExternalConsoleCheckboxClicked);
		}
	}
}

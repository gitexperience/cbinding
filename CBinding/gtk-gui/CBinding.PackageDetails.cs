
// This file has been generated by the GUI designer. Do not modify.
namespace CBinding
{
	public partial class PackageDetails
	{
		private global::Gtk.VBox vbox3;

		private global::Gtk.Table table1;

		private global::Gtk.Label descriptionLabel;

		private global::Gtk.Label label7;

		private global::Gtk.Label label8;

		private global::Gtk.Label label9;

		private global::Gtk.Label nameLabel;

		private global::Gtk.Label versionLabel;

		private global::Gtk.VBox vbox4;

		private global::Gtk.Label label13;

		private global::Gtk.ScrolledWindow scrolledwindow1;

		private global::Gtk.TreeView requiresTreeView;

		private global::Gtk.VBox vbox2;

		private global::Gtk.Label label1;

		private global::Gtk.HBox hbox1;

		private global::Gtk.ScrolledWindow scrolledwindow2;

		private global::Gtk.TreeView libPathsTreeView;

		private global::Gtk.ScrolledWindow scrolledwindow3;

		private global::Gtk.TreeView libsTreeView;

		private global::Gtk.VBox vbox5;

		private global::Gtk.Label label2;

		private global::Gtk.ScrolledWindow scrolledwindow4;

		private global::Gtk.TreeView cflagsTreeView;

		private global::Gtk.Button buttonOk;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget CBinding.PackageDetails
			this.Name = "CBinding.PackageDetails";
			this.Title = global::Mono.Unix.Catalog.GetString("Package Details");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child CBinding.PackageDetails.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.vbox3 = new global::Gtk.VBox();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			this.vbox3.BorderWidth = ((uint)(3));
			// Container child vbox3.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table(((uint)(3)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.descriptionLabel = new global::Gtk.Label();
			this.descriptionLabel.Name = "descriptionLabel";
			this.descriptionLabel.Xalign = 0F;
			this.descriptionLabel.Yalign = 0F;
			this.descriptionLabel.LabelProp = "label12";
			this.table1.Add(this.descriptionLabel);
			global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.table1[this.descriptionLabel]));
			w2.TopAttach = ((uint)(2));
			w2.BottomAttach = ((uint)(3));
			w2.LeftAttach = ((uint)(1));
			w2.RightAttach = ((uint)(2));
			w2.XOptions = ((global::Gtk.AttachOptions)(4));
			w2.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label7 = new global::Gtk.Label();
			this.label7.Name = "label7";
			this.label7.Xalign = 0F;
			this.label7.LabelProp = global::Mono.Unix.Catalog.GetString("Name:");
			this.table1.Add(this.label7);
			global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.table1[this.label7]));
			w3.XOptions = ((global::Gtk.AttachOptions)(4));
			w3.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label8 = new global::Gtk.Label();
			this.label8.Name = "label8";
			this.label8.Xalign = 0F;
			this.label8.LabelProp = global::Mono.Unix.Catalog.GetString("Version:");
			this.table1.Add(this.label8);
			global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.table1[this.label8]));
			w4.TopAttach = ((uint)(1));
			w4.BottomAttach = ((uint)(2));
			w4.XOptions = ((global::Gtk.AttachOptions)(4));
			w4.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label9 = new global::Gtk.Label();
			this.label9.Name = "label9";
			this.label9.Xalign = 0F;
			this.label9.Yalign = 0F;
			this.label9.LabelProp = global::Mono.Unix.Catalog.GetString("Description:");
			this.table1.Add(this.label9);
			global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.table1[this.label9]));
			w5.TopAttach = ((uint)(2));
			w5.BottomAttach = ((uint)(3));
			w5.XOptions = ((global::Gtk.AttachOptions)(4));
			w5.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.nameLabel = new global::Gtk.Label();
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Xalign = 0F;
			this.nameLabel.LabelProp = "label10";
			this.table1.Add(this.nameLabel);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table1[this.nameLabel]));
			w6.LeftAttach = ((uint)(1));
			w6.RightAttach = ((uint)(2));
			w6.XOptions = ((global::Gtk.AttachOptions)(4));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.versionLabel = new global::Gtk.Label();
			this.versionLabel.Name = "versionLabel";
			this.versionLabel.Xalign = 0F;
			this.versionLabel.LabelProp = "label11";
			this.table1.Add(this.versionLabel);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table1[this.versionLabel]));
			w7.TopAttach = ((uint)(1));
			w7.BottomAttach = ((uint)(2));
			w7.LeftAttach = ((uint)(1));
			w7.RightAttach = ((uint)(2));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox3.Add(this.table1);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.table1]));
			w8.Position = 0;
			w8.Expand = false;
			w8.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.vbox4 = new global::Gtk.VBox();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			// Container child vbox4.Gtk.Box+BoxChild
			this.label13 = new global::Gtk.Label();
			this.label13.Name = "label13";
			this.label13.Xalign = 0F;
			this.label13.LabelProp = global::Mono.Unix.Catalog.GetString("Requires:");
			this.vbox4.Add(this.label13);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.vbox4[this.label13]));
			w9.Position = 0;
			w9.Expand = false;
			w9.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.scrolledwindow1 = new global::Gtk.ScrolledWindow();
			this.scrolledwindow1.CanFocus = true;
			this.scrolledwindow1.Name = "scrolledwindow1";
			this.scrolledwindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child scrolledwindow1.Gtk.Container+ContainerChild
			global::Gtk.Viewport w10 = new global::Gtk.Viewport();
			w10.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child GtkViewport.Gtk.Container+ContainerChild
			this.requiresTreeView = new global::Gtk.TreeView();
			this.requiresTreeView.CanFocus = true;
			this.requiresTreeView.Name = "requiresTreeView";
			w10.Add(this.requiresTreeView);
			this.scrolledwindow1.Add(w10);
			this.vbox4.Add(this.scrolledwindow1);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.vbox4[this.scrolledwindow1]));
			w13.Position = 1;
			this.vbox3.Add(this.vbox4);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.vbox4]));
			w14.Position = 1;
			// Container child vbox3.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label();
			this.label1.Name = "label1";
			this.label1.Xalign = 0F;
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString("Libs:");
			this.vbox2.Add(this.label1);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.label1]));
			w15.Position = 0;
			w15.Expand = false;
			w15.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.scrolledwindow2 = new global::Gtk.ScrolledWindow();
			this.scrolledwindow2.CanFocus = true;
			this.scrolledwindow2.Name = "scrolledwindow2";
			this.scrolledwindow2.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child scrolledwindow2.Gtk.Container+ContainerChild
			global::Gtk.Viewport w16 = new global::Gtk.Viewport();
			w16.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child GtkViewport1.Gtk.Container+ContainerChild
			this.libPathsTreeView = new global::Gtk.TreeView();
			this.libPathsTreeView.CanFocus = true;
			this.libPathsTreeView.Name = "libPathsTreeView";
			w16.Add(this.libPathsTreeView);
			this.scrolledwindow2.Add(w16);
			this.hbox1.Add(this.scrolledwindow2);
			global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.scrolledwindow2]));
			w19.Position = 0;
			// Container child hbox1.Gtk.Box+BoxChild
			this.scrolledwindow3 = new global::Gtk.ScrolledWindow();
			this.scrolledwindow3.CanFocus = true;
			this.scrolledwindow3.Name = "scrolledwindow3";
			this.scrolledwindow3.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child scrolledwindow3.Gtk.Container+ContainerChild
			global::Gtk.Viewport w20 = new global::Gtk.Viewport();
			w20.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child GtkViewport2.Gtk.Container+ContainerChild
			this.libsTreeView = new global::Gtk.TreeView();
			this.libsTreeView.CanFocus = true;
			this.libsTreeView.Name = "libsTreeView";
			w20.Add(this.libsTreeView);
			this.scrolledwindow3.Add(w20);
			this.hbox1.Add(this.scrolledwindow3);
			global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.scrolledwindow3]));
			w23.Position = 1;
			this.vbox2.Add(this.hbox1);
			global::Gtk.Box.BoxChild w24 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox1]));
			w24.Position = 1;
			this.vbox3.Add(this.vbox2);
			global::Gtk.Box.BoxChild w25 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.vbox2]));
			w25.Position = 2;
			// Container child vbox3.Gtk.Box+BoxChild
			this.vbox5 = new global::Gtk.VBox();
			this.vbox5.Name = "vbox5";
			this.vbox5.Spacing = 6;
			// Container child vbox5.Gtk.Box+BoxChild
			this.label2 = new global::Gtk.Label();
			this.label2.Name = "label2";
			this.label2.Xalign = 0F;
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString("CFlags:");
			this.vbox5.Add(this.label2);
			global::Gtk.Box.BoxChild w26 = ((global::Gtk.Box.BoxChild)(this.vbox5[this.label2]));
			w26.Position = 0;
			w26.Expand = false;
			w26.Fill = false;
			// Container child vbox5.Gtk.Box+BoxChild
			this.scrolledwindow4 = new global::Gtk.ScrolledWindow();
			this.scrolledwindow4.CanFocus = true;
			this.scrolledwindow4.Name = "scrolledwindow4";
			this.scrolledwindow4.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child scrolledwindow4.Gtk.Container+ContainerChild
			global::Gtk.Viewport w27 = new global::Gtk.Viewport();
			w27.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child GtkViewport3.Gtk.Container+ContainerChild
			this.cflagsTreeView = new global::Gtk.TreeView();
			this.cflagsTreeView.CanFocus = true;
			this.cflagsTreeView.Name = "cflagsTreeView";
			w27.Add(this.cflagsTreeView);
			this.scrolledwindow4.Add(w27);
			this.vbox5.Add(this.scrolledwindow4);
			global::Gtk.Box.BoxChild w30 = ((global::Gtk.Box.BoxChild)(this.vbox5[this.scrolledwindow4]));
			w30.Position = 1;
			this.vbox3.Add(this.vbox5);
			global::Gtk.Box.BoxChild w31 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.vbox5]));
			w31.Position = 3;
			w1.Add(this.vbox3);
			global::Gtk.Box.BoxChild w32 = ((global::Gtk.Box.BoxChild)(w1[this.vbox3]));
			w32.Position = 0;
			// Internal child CBinding.PackageDetails.ActionArea
			global::Gtk.HButtonBox w33 = this.ActionArea;
			w33.Name = "dialog1_ActionArea";
			w33.Spacing = 6;
			w33.BorderWidth = ((uint)(5));
			w33.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.AddActionWidget(this.buttonOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w34 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w33[this.buttonOk]));
			w34.Expand = false;
			w34.Fill = false;
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.DefaultWidth = 608;
			this.DefaultHeight = 528;
			this.Hide();
			this.buttonOk.Clicked += new global::System.EventHandler(this.OnButtonOkClicked);
		}
	}
}

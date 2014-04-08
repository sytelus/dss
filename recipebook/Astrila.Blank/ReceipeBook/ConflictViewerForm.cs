using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Astrila.Blank.Clients.RecipeBook
{
	/// <summary>
	/// Summary description for ConflictViewerForm.
	/// </summary>
	public class ConflictViewerForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ColumnHeader columnHeaderValue;
		private System.Windows.Forms.ColumnHeader columnHeaderBy;
		private System.Windows.Forms.ColumnHeader columnHeaderWhen;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button buttonResolve;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListView listViewConflicts;
		private System.Windows.Forms.ToolBarButton toolBarButtonMore;
		private System.Windows.Forms.ToolBar toolBarMore;
		private System.Windows.Forms.ContextMenu contextMenuMore;
		private System.Windows.Forms.MenuItem menuItemSetAsCurrent;
		private System.Windows.Forms.MenuItem menuItemDeleteMyValue;
		private System.Windows.Forms.MenuItem menuItemUndeleteMyValue;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItemShowDeletedValues;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ConflictViewerForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ConflictViewerForm));
			this.listViewConflicts = new System.Windows.Forms.ListView();
			this.columnHeaderValue = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderBy = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderWhen = new System.Windows.Forms.ColumnHeader();
			this.panel1 = new System.Windows.Forms.Panel();
			this.buttonOK = new System.Windows.Forms.Button();
			this.toolBarMore = new System.Windows.Forms.ToolBar();
			this.toolBarButtonMore = new System.Windows.Forms.ToolBarButton();
			this.contextMenuMore = new System.Windows.Forms.ContextMenu();
			this.menuItemSetAsCurrent = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItemDeleteMyValue = new System.Windows.Forms.MenuItem();
			this.menuItemUndeleteMyValue = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItemShowDeletedValues = new System.Windows.Forms.MenuItem();
			this.buttonResolve = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// listViewConflicts
			// 
			this.listViewConflicts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.listViewConflicts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																								this.columnHeaderValue,
																								this.columnHeaderBy,
																								this.columnHeaderWhen});
			this.listViewConflicts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewConflicts.FullRowSelect = true;
			this.listViewConflicts.GridLines = true;
			this.listViewConflicts.Location = new System.Drawing.Point(0, 0);
			this.listViewConflicts.MultiSelect = false;
			this.listViewConflicts.Name = "listViewConflicts";
			this.listViewConflicts.Size = new System.Drawing.Size(522, 256);
			this.listViewConflicts.TabIndex = 4;
			this.listViewConflicts.View = System.Windows.Forms.View.Details;
			// 
			// columnHeaderValue
			// 
			this.columnHeaderValue.Text = "Value";
			this.columnHeaderValue.Width = 280;
			// 
			// columnHeaderBy
			// 
			this.columnHeaderBy.Text = "By";
			this.columnHeaderBy.Width = 102;
			// 
			// columnHeaderWhen
			// 
			this.columnHeaderWhen.Text = "When";
			this.columnHeaderWhen.Width = 169;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.buttonOK);
			this.panel1.Controls.Add(this.toolBarMore);
			this.panel1.Controls.Add(this.buttonResolve);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 200);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(522, 56);
			this.panel1.TabIndex = 5;
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonOK.Location = new System.Drawing.Point(440, 16);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(72, 29);
			this.buttonOK.TabIndex = 21;
			this.buttonOK.Text = "Close";
			// 
			// toolBarMore
			// 
			this.toolBarMore.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						   this.toolBarButtonMore});
			this.toolBarMore.ButtonSize = new System.Drawing.Size(90, 25);
			this.toolBarMore.Divider = false;
			this.toolBarMore.Dock = System.Windows.Forms.DockStyle.None;
			this.toolBarMore.DropDownArrows = true;
			this.toolBarMore.Location = new System.Drawing.Point(8, 16);
			this.toolBarMore.Name = "toolBarMore";
			this.toolBarMore.ShowToolTips = true;
			this.toolBarMore.Size = new System.Drawing.Size(104, 29);
			this.toolBarMore.TabIndex = 20;
			this.toolBarMore.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.toolBarMore.Wrappable = false;
			// 
			// toolBarButtonMore
			// 
			this.toolBarButtonMore.DropDownMenu = this.contextMenuMore;
			this.toolBarButtonMore.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
			this.toolBarButtonMore.Text = "Advanced";
			this.toolBarButtonMore.ToolTipText = "Show more options";
			// 
			// contextMenuMore
			// 
			this.contextMenuMore.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							this.menuItemSetAsCurrent,
																							this.menuItem5,
																							this.menuItemDeleteMyValue,
																							this.menuItemUndeleteMyValue,
																							this.menuItem1,
																							this.menuItemShowDeletedValues});
			this.contextMenuMore.Popup += new System.EventHandler(this.contextMenuMore_Popup);
			// 
			// menuItemSetAsCurrent
			// 
			this.menuItemSetAsCurrent.Index = 0;
			this.menuItemSetAsCurrent.Text = "Set this as &current value";
			this.menuItemSetAsCurrent.Click += new System.EventHandler(this.menuItemSetAsCurrent_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 1;
			this.menuItem5.Text = "-";
			// 
			// menuItemDeleteMyValue
			// 
			this.menuItemDeleteMyValue.Index = 2;
			this.menuItemDeleteMyValue.Text = "Delete my value";
			this.menuItemDeleteMyValue.Click += new System.EventHandler(this.menuItemDeleteMyValue_Click);
			// 
			// menuItemUndeleteMyValue
			// 
			this.menuItemUndeleteMyValue.Index = 3;
			this.menuItemUndeleteMyValue.Text = "Undelete my value";
			this.menuItemUndeleteMyValue.Click += new System.EventHandler(this.menuItemUndeleteMyValue_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 4;
			this.menuItem1.Text = "-";
			// 
			// menuItemShowDeletedValues
			// 
			this.menuItemShowDeletedValues.Index = 5;
			this.menuItemShowDeletedValues.Text = "&Show deleted values";
			this.menuItemShowDeletedValues.Click += new System.EventHandler(this.menuItemShowDeletedValues_Click);
			// 
			// buttonResolve
			// 
			this.buttonResolve.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonResolve.Location = new System.Drawing.Point(320, 16);
			this.buttonResolve.Name = "buttonResolve";
			this.buttonResolve.Size = new System.Drawing.Size(104, 29);
			this.buttonResolve.TabIndex = 17;
			this.buttonResolve.Text = "&Resolve Conflict";
			this.buttonResolve.Click += new System.EventHandler(this.buttonResolve_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(-8, 1);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(624, 8);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			// 
			// ConflictViewerForm
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonOK;
			this.ClientSize = new System.Drawing.Size(522, 256);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.listViewConflicts);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ConflictViewerForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "View Conflicts";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public static DialogResult ShowConflicts(DataItemProperty property)
		{
			using (ConflictViewerForm viewer = new ConflictViewerForm())
			{
				viewer.SetupForm(property);
				return viewer.ShowDialog();
			}

		}

		private void buttonResolve_Click(object sender, System.EventArgs e)
		{
			if (listViewConflicts.SelectedItems.Count == 0)
				MessageBox.Show("Please select the value you want to set as final and resolve the conflict");
			else
			{
				DataItemPropertyValue propertyValue = (DataItemPropertyValue) listViewConflicts.SelectedItems[0].Tag;
				_property.ResolveConflict(propertyValue.Author);
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private DataItemProperty _property;
		private  void SetupForm(DataItemProperty property)
		{
			_property = property;
			FillConflictList();
		}

		private void FillConflictList()
		{
			listViewConflicts.Items.Clear();
			int conflictCount = 0;
			for (int valueIndex = 0; valueIndex < _property.Values.Count; valueIndex++)
			{
				DataItemPropertyValue thisValue = _property.Values[valueIndex];
				if (menuItemShowDeletedValues.Checked || (!thisValue.Deleted))
				{
					ListViewItem mainListViewItem = listViewConflicts.Items.Add(thisValue.Value);
					mainListViewItem.Tag = thisValue;
					mainListViewItem.SubItems.Add(thisValue.Author);
					DateTime changedOn = thisValue.ChangedOn.ToLocalTime();
					mainListViewItem.SubItems.Add(changedOn.ToShortDateString() + " " + changedOn.ToShortTimeString());

					if (!thisValue.Deleted)
						conflictCount++;
					else
						mainListViewItem.Font =  new Font(mainListViewItem.Font, mainListViewItem.Font.Style | FontStyle.Strikeout);
				}
				else {} //do not show
			}
			buttonResolve.Enabled = (conflictCount > 1);			
		}

		private void menuItemSetAsCurrent_Click(object sender, System.EventArgs e)
		{
			if (listViewConflicts.SelectedItems.Count == 0)
				MessageBox.Show("Please select the value to be set as Current value");
			else
			{
				_property.CurrentValue = (DataItemPropertyValue) listViewConflicts.SelectedItems[0].Tag;
				FillConflictList();
			}
		}

		private void menuItemDeleteMyValue_Click(object sender, System.EventArgs e)
		{
			if (!_property.MyValueExist)
				MessageBox.Show("Your value does not exist");
			else
			{
				if (! _property.Values[_property.Document.CurrentAuthor].Current)
				{
					_property.Values[_property.Document.CurrentAuthor].Deleted = true;
					FillConflictList();
				}
				else
					MessageBox.Show("Your value is currently set as current. You must set someone else's value as current before deleteing your value.");
			}
		}

		private void menuItemUndeleteMyValue_Click(object sender, System.EventArgs e)
		{
			if (!_property.MyValueExist)
				MessageBox.Show("Your value does not exist");
			else
			{
				_property.Values[_property.Document.CurrentAuthor].Deleted = false;
				FillConflictList();
			}
		}

		private void contextMenuMore_Popup(object sender, System.EventArgs e)
		{
			menuItemDeleteMyValue.Enabled = _property.Contains(_property.Document.CurrentAuthor) && !_property.Values[_property.Document.CurrentAuthor].Deleted;
			menuItemUndeleteMyValue.Enabled = _property.Contains(_property.Document.CurrentAuthor) && _property.Values[_property.Document.CurrentAuthor].Deleted;
		}

		private void menuItemShowDeletedValues_Click(object sender, System.EventArgs e)
		{
			menuItemShowDeletedValues.Checked = ! menuItemShowDeletedValues.Checked;
			FillConflictList();
		}
	}
}

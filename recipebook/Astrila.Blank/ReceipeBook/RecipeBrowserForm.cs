using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Windows.Forms;
using System.Xml;
using Astrila.Blank.Clients.RecipeBook.SyncServerWebService;
using Astrila.Common;

namespace Astrila.Blank.Clients.RecipeBook
{
	/// <summary>
	/// Summary description for RecipeBrowser.
	/// </summary>
	public class RecipeBrowserForm : Form
	{
		private ImageList imageToolbar;
		private System.Windows.Forms.ToolBarButton toolBarButtonCreate;
		private System.Windows.Forms.ToolBarButton toolBarButtonOpen;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.ToolBar toolBarMain;
		private System.Windows.Forms.ListView listViewRecipes;
		private System.Windows.Forms.ColumnHeader columnHeaderTitle;
		private System.Windows.Forms.ColumnHeader columnHeaderOrigin;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ToolBarButton toolBarButtonSyncWithServer;
		private System.Windows.Forms.ToolBarButton toolBarButtonSyncWithPeer;
		private System.Windows.Forms.ToolBarButton toolBarButtonHelp;
		private System.Windows.Forms.ToolBarButton toolBarButton3;
		private System.Windows.Forms.StatusBarPanel statusBarPanelItemCount;
		private System.Windows.Forms.StatusBarPanel statusBarPanelSyncStatus;
		private System.Windows.Forms.ToolBarButton toolBarButtonRefresh;
		private System.Windows.Forms.ImageList imageListRecipeList;
		private System.Windows.Forms.ColumnHeader columnHeaderIcon;
		private System.Windows.Forms.ToolBarButton toolBarButtonDelete;
		private System.Windows.Forms.ToolBarButton toolBarButton2;
		private System.Windows.Forms.ToolBarButton toolBarButtonHideDeleted;
		private System.Windows.Forms.ContextMenu contextMenuListView;
		private System.Windows.Forms.MenuItem menuItemOpenItem;
		private System.Windows.Forms.MenuItem menuItemDeleteItem;
		private System.Windows.Forms.MenuItem menuItemUndeleteItem;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItemResolveAllConflicts;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.StatusBarPanel statusBarPanelFillStatus;
		private System.Windows.Forms.ToolBarButton toolBarButtonExportToFile;
		private System.Windows.Forms.ToolBarButton toolBarButtonImportFromFile;
		private System.Windows.Forms.ToolBarButton toolBarButton4;
		private System.Windows.Forms.OpenFileDialog openFileDialogFeed;
		private System.Windows.Forms.SaveFileDialog saveFileDialogFeed;
		private System.Windows.Forms.ToolBarButton toolBarButtonExportToEmail;
		private System.Windows.Forms.ToolBarButton toolBarButtonAllowRemoting;
		private System.Windows.Forms.ToolBarButton toolBarButtonManageSubscriptions;
		private System.Windows.Forms.ToolBarButton toolBarButton5;
		private IContainer components;

		public RecipeBrowserForm()
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RecipeBrowserForm));
			this.imageToolbar = new System.Windows.Forms.ImageList(this.components);
			this.toolBarButtonCreate = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonOpen = new System.Windows.Forms.ToolBarButton();
			this.toolBarMain = new System.Windows.Forms.ToolBar();
			this.toolBarButtonRefresh = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonDelete = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonHideDeleted = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonManageSubscriptions = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSyncWithServer = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton5 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonAllowRemoting = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSyncWithPeer = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton4 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonExportToFile = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonImportFromFile = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonExportToEmail = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonHelp = new System.Windows.Forms.ToolBarButton();
			this.listViewRecipes = new System.Windows.Forms.ListView();
			this.columnHeaderIcon = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderTitle = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderOrigin = new System.Windows.Forms.ColumnHeader();
			this.contextMenuListView = new System.Windows.Forms.ContextMenu();
			this.menuItemOpenItem = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItemResolveAllConflicts = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItemDeleteItem = new System.Windows.Forms.MenuItem();
			this.menuItemUndeleteItem = new System.Windows.Forms.MenuItem();
			this.imageListRecipeList = new System.Windows.Forms.ImageList(this.components);
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.statusBarPanelItemCount = new System.Windows.Forms.StatusBarPanel();
			this.statusBarPanelFillStatus = new System.Windows.Forms.StatusBarPanel();
			this.statusBarPanelSyncStatus = new System.Windows.Forms.StatusBarPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.openFileDialogFeed = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialogFeed = new System.Windows.Forms.SaveFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelItemCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelFillStatus)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelSyncStatus)).BeginInit();
			this.SuspendLayout();
			// 
			// imageToolbar
			// 
			this.imageToolbar.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageToolbar.ImageSize = new System.Drawing.Size(16, 16);
			this.imageToolbar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageToolbar.ImageStream")));
			this.imageToolbar.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// toolBarButtonCreate
			// 
			this.toolBarButtonCreate.ImageIndex = 9;
			this.toolBarButtonCreate.ToolTipText = "Add new recipe";
			// 
			// toolBarButtonOpen
			// 
			this.toolBarButtonOpen.ImageIndex = 0;
			this.toolBarButtonOpen.ToolTipText = "Open selected recipe";
			// 
			// toolBarMain
			// 
			this.toolBarMain.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBarMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.toolBarMain.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						   this.toolBarButtonCreate,
																						   this.toolBarButtonOpen,
																						   this.toolBarButtonRefresh,
																						   this.toolBarButton1,
																						   this.toolBarButtonDelete,
																						   this.toolBarButtonHideDeleted,
																						   this.toolBarButton2,
																						   this.toolBarButtonManageSubscriptions,
																						   this.toolBarButtonSyncWithServer,
																						   this.toolBarButton5,
																						   this.toolBarButtonAllowRemoting,
																						   this.toolBarButtonSyncWithPeer,
																						   this.toolBarButton4,
																						   this.toolBarButtonExportToFile,
																						   this.toolBarButtonImportFromFile,
																						   this.toolBarButtonExportToEmail,
																						   this.toolBarButton3,
																						   this.toolBarButtonHelp});
			this.toolBarMain.ButtonSize = new System.Drawing.Size(23, 22);
			this.toolBarMain.DropDownArrows = true;
			this.toolBarMain.ImageList = this.imageToolbar;
			this.toolBarMain.Location = new System.Drawing.Point(0, 0);
			this.toolBarMain.Name = "toolBarMain";
			this.toolBarMain.ShowToolTips = true;
			this.toolBarMain.Size = new System.Drawing.Size(584, 29);
			this.toolBarMain.TabIndex = 0;
			this.toolBarMain.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
			// 
			// toolBarButtonRefresh
			// 
			this.toolBarButtonRefresh.ImageIndex = 7;
			this.toolBarButtonRefresh.ToolTipText = "Refresh this screen";
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonDelete
			// 
			this.toolBarButtonDelete.ImageIndex = 11;
			this.toolBarButtonDelete.ToolTipText = "Delete selected recipes";
			// 
			// toolBarButtonHideDeleted
			// 
			this.toolBarButtonHideDeleted.ImageIndex = 10;
			this.toolBarButtonHideDeleted.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonHideDeleted.ToolTipText = "Hide deleted recipes";
			// 
			// toolBarButton2
			// 
			this.toolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonManageSubscriptions
			// 
			this.toolBarButtonManageSubscriptions.ImageIndex = 16;
			this.toolBarButtonManageSubscriptions.ToolTipText = "Manage your DSS subscriptions";
			// 
			// toolBarButtonSyncWithServer
			// 
			this.toolBarButtonSyncWithServer.ImageIndex = 2;
			this.toolBarButtonSyncWithServer.ToolTipText = "Update from and to your subscriptions";
			// 
			// toolBarButton5
			// 
			this.toolBarButton5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonAllowRemoting
			// 
			this.toolBarButtonAllowRemoting.ImageIndex = 15;
			this.toolBarButtonAllowRemoting.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonAllowRemoting.ToolTipText = "Allow other computers to connect and sync with you";
			// 
			// toolBarButtonSyncWithPeer
			// 
			this.toolBarButtonSyncWithPeer.ImageIndex = 3;
			this.toolBarButtonSyncWithPeer.ToolTipText = "Sync with another computer";
			// 
			// toolBarButton4
			// 
			this.toolBarButton4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonExportToFile
			// 
			this.toolBarButtonExportToFile.ImageIndex = 14;
			this.toolBarButtonExportToFile.ToolTipText = "Save updates to file";
			// 
			// toolBarButtonImportFromFile
			// 
			this.toolBarButtonImportFromFile.ImageIndex = 13;
			this.toolBarButtonImportFromFile.ToolTipText = "Merge updates from file";
			// 
			// toolBarButtonExportToEmail
			// 
			this.toolBarButtonExportToEmail.ImageIndex = 5;
			this.toolBarButtonExportToEmail.ToolTipText = "Send updates via email";
			// 
			// toolBarButton3
			// 
			this.toolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonHelp
			// 
			this.toolBarButtonHelp.ImageIndex = 6;
			this.toolBarButtonHelp.ToolTipText = "About, help,  website and meaning of icons";
			// 
			// listViewRecipes
			// 
			this.listViewRecipes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.listViewRecipes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							  this.columnHeaderIcon,
																							  this.columnHeaderTitle,
																							  this.columnHeaderOrigin});
			this.listViewRecipes.ContextMenu = this.contextMenuListView;
			this.listViewRecipes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewRecipes.FullRowSelect = true;
			this.listViewRecipes.GridLines = true;
			this.listViewRecipes.Location = new System.Drawing.Point(0, 46);
			this.listViewRecipes.Name = "listViewRecipes";
			this.listViewRecipes.Size = new System.Drawing.Size(584, 434);
			this.listViewRecipes.SmallImageList = this.imageListRecipeList;
			this.listViewRecipes.TabIndex = 3;
			this.listViewRecipes.View = System.Windows.Forms.View.Details;
			this.listViewRecipes.DoubleClick += new System.EventHandler(this.listViewRecipes_DoubleClick);
			this.listViewRecipes.SelectedIndexChanged += new System.EventHandler(this.listViewRecipes_SelectedIndexChanged);
			// 
			// columnHeaderIcon
			// 
			this.columnHeaderIcon.Text = "";
			this.columnHeaderIcon.Width = 16;
			// 
			// columnHeaderTitle
			// 
			this.columnHeaderTitle.Text = "Title";
			this.columnHeaderTitle.Width = 388;
			// 
			// columnHeaderOrigin
			// 
			this.columnHeaderOrigin.Text = "Origin";
			this.columnHeaderOrigin.Width = 175;
			// 
			// contextMenuListView
			// 
			this.contextMenuListView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								this.menuItemOpenItem,
																								this.menuItem4,
																								this.menuItemResolveAllConflicts,
																								this.menuItem6,
																								this.menuItemDeleteItem,
																								this.menuItemUndeleteItem});
			this.contextMenuListView.Popup += new System.EventHandler(this.contextMenuListView_Popup);
			// 
			// menuItemOpenItem
			// 
			this.menuItemOpenItem.Index = 0;
			this.menuItemOpenItem.Text = "&Open";
			this.menuItemOpenItem.Click += new System.EventHandler(this.menuItemOpenItem_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			this.menuItem4.Text = "-";
			// 
			// menuItemResolveAllConflicts
			// 
			this.menuItemResolveAllConflicts.Index = 2;
			this.menuItemResolveAllConflicts.Text = "&Resolve all conflicts using my values";
			this.menuItemResolveAllConflicts.Click += new System.EventHandler(this.menuItemResolveAllConflicts_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 3;
			this.menuItem6.Text = "-";
			// 
			// menuItemDeleteItem
			// 
			this.menuItemDeleteItem.Index = 4;
			this.menuItemDeleteItem.Text = "&Delete";
			this.menuItemDeleteItem.Click += new System.EventHandler(this.menuItemDeleteItem_Click);
			// 
			// menuItemUndeleteItem
			// 
			this.menuItemUndeleteItem.Index = 5;
			this.menuItemUndeleteItem.Text = "&Undelete";
			this.menuItemUndeleteItem.Click += new System.EventHandler(this.menuItemUndeleteItem_Click);
			// 
			// imageListRecipeList
			// 
			this.imageListRecipeList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListRecipeList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListRecipeList.ImageStream")));
			this.imageListRecipeList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.SystemColors.Control;
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.textBox1.Location = new System.Drawing.Point(0, 29);
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(584, 13);
			this.textBox1.TabIndex = 4;
			this.textBox1.Text = "Recipes:";
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 480);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						  this.statusBarPanelItemCount,
																						  this.statusBarPanelFillStatus,
																						  this.statusBarPanelSyncStatus});
			this.statusBar1.ShowPanels = true;
			this.statusBar1.Size = new System.Drawing.Size(584, 22);
			this.statusBar1.TabIndex = 5;
			this.statusBar1.Text = "statusBarMain";
			// 
			// statusBarPanelItemCount
			// 
			this.statusBarPanelItemCount.Text = "0 Recipes";
			this.statusBarPanelItemCount.Width = 70;
			// 
			// statusBarPanelFillStatus
			// 
			this.statusBarPanelFillStatus.Width = 70;
			// 
			// statusBarPanelSyncStatus
			// 
			this.statusBarPanelSyncStatus.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanelSyncStatus.Text = "Sync not started";
			this.statusBarPanelSyncStatus.Width = 428;
			// 
			// panel1
			// 
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 42);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(584, 4);
			this.panel1.TabIndex = 6;
			// 
			// openFileDialogFeed
			// 
			this.openFileDialogFeed.Filter = "DSS Feed Files|*.dss|All files|*.*";
			this.openFileDialogFeed.Title = "Open File Containing Updates";
			// 
			// saveFileDialogFeed
			// 
			this.saveFileDialogFeed.Filter = "DSS Feed Files|*.dss|All files|*.*";
			this.saveFileDialogFeed.Title = "Save File With Updates";
			// 
			// RecipeBrowserForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(584, 502);
			this.Controls.Add(this.listViewRecipes);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.toolBarMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "RecipeBrowserForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Collaborative Recipes";
			this.Load += new System.EventHandler(this.RecipeBrowserForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelItemCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelFillStatus)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelSyncStatus)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new RecipeBrowserForm());
		}


		DataAccess _localDatabase;
		DataDocumentCollection _documents;
		private DataDocument _recipeDocument;
		private void RecipeBrowserForm_Load(object sender, EventArgs e)
		{
			string userName = ConfigurationSettings.AppSettings["userName"];
			if ((userName == null) || (userName.Length == 0))
			{
				userName =  System.Environment.UserDomainName + "\\" + System.Environment.UserName; 
			}

			_localDatabase = new DataAccess(DataAccess.BuildConnectionStringForAccess(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "RecipeData.mdb")));
			_documents = new DataDocumentCollection(_localDatabase, userName);

			if (_documents.Count == 0)
			{
				DataDocument newDocument = _documents.Add("ShitalShah/Recipe", "RecipeBook", "Recipes from ShitalShah.com");
				newDocument.Save();
			}
			else if (_documents.Count > 1)
			{
				MessageBox.Show("Database contains more than one document!");
			}
			else {}; //there is only one doc
				
			_recipeDocument = _documents["ShitalShah/Recipe"];

			_recipeDocument.Load("");
			FillRecipeList();

			this.Text = this.Text + " : " + userName;
		}

		private void FillRecipeList()
		{
			statusBarPanelFillStatus.Text = "Refreshing view...";
			listViewRecipes.Items.Clear();
			DataItemCollection recipeItems = _recipeDocument.Items;
			for (int recipeItemIndex = 0; recipeItemIndex < recipeItems.Count; recipeItemIndex++)
			{
				DataItem recipeItem = recipeItems[recipeItemIndex];
				bool itemDeleted = Convert.ToBoolean(recipeItem.Deleted.Value, CultureInfo.InvariantCulture);
				bool itemDeletedIsInConflict = recipeItem.Deleted.IsInConflict();

				if ((!itemDeleted) || (itemDeleted && !toolBarButtonHideDeleted.Pushed) || itemDeletedIsInConflict)
				{
					ListViewItem mainItem = listViewRecipes.Items.Add("");
					mainItem.Tag =recipeItem;
					mainItem.SubItems.Add(recipeItem.Properties["Title"].Value);
					mainItem.SubItems.Add(recipeItem.Properties["Origin"].Value);
					if (itemDeletedIsInConflict)
					{
						mainItem.ImageIndex = 3;
					}
					else
					{
						if (recipeItem.IsInConflict())
						{
							mainItem.ImageIndex = 0;
						}
						else{} //do not apply any images
					}
				
					if (itemDeleted)
						mainItem.Font =  new Font(mainItem.Font, mainItem.Font.Style | FontStyle.Strikeout);
					else {} //do not change font
				}
				else {} //hide this item
			}

			//Select first item by default
			if (listViewRecipes.Items.Count > 0)
				listViewRecipes.Items[0].Selected = true;

			statusBarPanelItemCount.Text = listViewRecipes.Items.Count.ToString() + " recipes";
			statusBarPanelFillStatus.Text = "Ready";
		}

		private string ReadUserNameFromFile()
		{
			string userNameFilePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "username.txt");
			if (File.Exists(userNameFilePath))
			{
				using (StreamReader reader = File.OpenText(userNameFilePath))
				{
					return reader.ReadToEnd();
				}
			}
			else return string.Empty;
		}

		private void WriteUserNameFromFile(string userName)
		{
			string userNameFilePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "username.txt");
			using (StreamWriter writer = File.CreateText(userNameFilePath))
			{
				writer.Write(userName);
			}
		}

		private void DeleteSelectedItems(bool delete)
		{
			if (listViewRecipes.SelectedItems.Count > 0)
			{
				if (MessageBox.Show("Do you wish to " + (delete?"delete ":"undelete ") + listViewRecipes.SelectedItems.Count.ToString() + " recipe(s)?", "Confirm Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					foreach (ListViewItem selectedItem in listViewRecipes.SelectedItems)
					{
						((DataItem) selectedItem.Tag).Deleted.Value = Convert.ToString(delete, CultureInfo.InvariantCulture);
					}
					//TODO: exception handling needed here
					_recipeDocument.Save();
					FillRecipeList();						
				}
				else {} //user cancelled
			}
			else
			{
				MessageBox.Show("Please select items you would like to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			
		}

		private void ResolveAllConflictsUsingMyValuesForSelectedItems()
		{
			if (listViewRecipes.SelectedItems.Count > 0)
			{
				if (MessageBox.Show("Do you wish to resolve all conflicts for " + listViewRecipes.SelectedItems.Count.ToString() + " recipe(s)?", "Confirm Resolve", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					foreach (ListViewItem selectedItem in listViewRecipes.SelectedItems)
					{
						((DataItem) selectedItem.Tag).ResolveAllConflictsUsingMyValues("Resolved all conflicts using my values");
					}
					//TODO: exception handling needed here
					_recipeDocument.Save();
					FillRecipeList();						
				}
				else {} //user cancelled
			}
			else
			{
				MessageBox.Show("Please select items you would like to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			
		}

		private void EditSelectedItem()
		{
			if (listViewRecipes.SelectedItems.Count > 0)
			{
				ListViewItem selectedItem = listViewRecipes.SelectedItems[0];
				if (RecipeEditorForm.ShowRecipe(this, (DataItem) selectedItem.Tag) == DialogResult.OK)
				{
					_recipeDocument.Save();
					FillRecipeList();						
				}
			}
			else
			{
				MessageBox.Show("Please select recipe you would like to edit from the list.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				//if (MessageBox.Show("Do you wish to add new recipe?", "Confirm Add", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.OK)
				//{
				//	AddNewRecipeUI();
				//}
			}
			
		}

		private void AddNewRecipeUI()
		{
			if (RecipeEditorForm.CreateRecipe(this, _recipeDocument) == DialogResult.OK)
			{
				_recipeDocument.Save();
				FillRecipeList();
			}
		}


		TcpServerChannel _dssRemotingChannel = null;
		DssRemotingServer _remotingServer;

		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			try
			{
				if (e.Button == toolBarButtonOpen)
				{
					EditSelectedItem();
				}
				else if (e.Button == toolBarButtonCreate)
				{
					AddNewRecipeUI();
				} 
				else if (e.Button == toolBarButtonSyncWithServer)
				{
					SubscriptionCollection subscriptions = new SubscriptionCollection(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "subscriptions.xml"));
					SubscriptionCollection.FeedInfo[] feeds = subscriptions.GetFeedInfo();

					if (feeds.Length > 0)
					{
						for (int feedIndex = 0; feedIndex < feeds.Length; feedIndex++)
						{
							SyncWithServer(feeds[feedIndex].GetDataUrl, feeds[feedIndex].PutDataUrl);						
						}
					}
					else
						throw new ApplicationException("You have not subscribed to any feeds yet. Please click the Manage Subscriptions button to add new feeds in to your subscription.");
				}
				else if (e.Button == toolBarButtonRefresh)
				{
					if (MessageBox.Show("You will loose any unsaved changes. Reload the data?", "Refresh", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation) == DialogResult.Yes)
					{
						_recipeDocument.Refresh();
						FillRecipeList();
					}
				}
				else if (e.Button == toolBarButtonHelp)
				{
					using (Form aboutForm = new About())
					{
						aboutForm.ShowDialog();
					}
				}
				else if (e.Button == toolBarButtonHideDeleted)
				{
					FillRecipeList();
				}
				else if (e.Button == toolBarButtonDelete)
				{
					DeleteSelectedItems(true);
				}
				else if (e.Button == toolBarButtonExportToFile)
				{
					if (saveFileDialogFeed.ShowDialog() == DialogResult.OK)
					{
						XmlDocument feedDocumet = CreateFeedXmlDocument(DateTime.MaxValue);
						feedDocumet.Save(saveFileDialogFeed.FileName);
					}
					else {} // user cancelled
				}
				else if (e.Button == toolBarButtonImportFromFile)
				{
					if (openFileDialogFeed.ShowDialog() == DialogResult.OK)
					{
						XmlDocument feedDocumet = new XmlDocument();
						feedDocumet.Load(openFileDialogFeed.FileName);
						int totalLocalItemsUpdated; int totalLocalItemsExamined;
						MergeFeedContentWithLocalDatabase(feedDocumet, out totalLocalItemsUpdated, out totalLocalItemsExamined);
					}
					else {} // user cancelled
				}
				else if (e.Button == toolBarButtonExportToEmail)
				{
					AttachFeedInEmail();
				}
				else if (e.Button == toolBarButtonSyncWithPeer)
				{
					string remoteMachineName = InputBoxDialog.Show("Enter the the machine name or address you want to sync with:", "Peer Machine", string.Empty).Trim();
					if (remoteMachineName.Length > 0)
					{
						DssRemotingServer remoteServer = (DssRemotingServer) Activator.GetObject(typeof(DssRemotingServer), string.Format("tcp://{0}:7137/DssRecipeBookRemotingService", remoteMachineName));												
						SyncWithPeer(remoteServer);
					}
				}
				else if (e.Button == toolBarButtonAllowRemoting)
				{
					if (toolBarButtonAllowRemoting.Pushed)
					{
						if (_dssRemotingChannel == null)
						{
							//This is needed to let exceptions propogate to remote callers
							RemotingConfiguration.Configure(
								AppDomain.CurrentDomain.SetupInformation.ConfigurationFile );

							_dssRemotingChannel = new TcpServerChannel("DSSRecipeBookTcpChannel", 7137);
							ChannelServices.RegisterChannel (_dssRemotingChannel);

							if (_remotingServer == null)
								_remotingServer = new DssRemotingServer(_localDatabase, "ShitalShah/Recipe", "Recipes from " + _recipeDocument.CurrentAuthor, new DssRemotingServer.RefreshUIAfterPeerSyncDelegate(RefreshUIAfterPeerSync));

							RemotingServices.Marshal(_remotingServer, "DssRecipeBookRemotingService");

							MessageBox.Show("Your machine is now listening for the sync requests from your peers. Your machine address is: " + _dssRemotingChannel.GetChannelUri());
						}
						else
							throw new ApplicationException("Remoting TcpChannel is already registered on this machine!");
					}
					else
					{
						try
						{
							_dssRemotingChannel.StopListening(null);
							ChannelServices.UnregisterChannel(_dssRemotingChannel);
							RemotingServices.Disconnect(_remotingServer);
							_dssRemotingChannel = null;
						}
						catch (SocketException ex)
						{} //silence Exception "A blocking operation was interrupted by a call to WSACancelBlockingCall"
					}
				}
				else if (e.Button == toolBarButtonManageSubscriptions)
				{
					SubscriptionsManagerForm.ShowForm(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "subscriptions.xml"));
				}
				else
				{
					MessageBox.Show("Not implemented yet!");
				}
			}
			catch(Exception ex)
			{
				if (ex.GetType() == typeof(ApplicationException))
				{
					MessageBox.Show (ex.Message);
				}
				else
					MessageBox.Show (ex.ToString() + "\n\n" + ex.StackTrace, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}



		private void SyncWithPeer(DssRemotingServer remotePeer)	//Merge this with SyncWithServer
		{
			statusBarPanelSyncStatus.Text = "Starting sync...";

			DateTime nextUpdateAfterRequestFromPeer = DateTime.MaxValue;
			DateTime nextUpdateAfterRequestToPeer=DateTime.MaxValue;
			int totalLocalItemsUpdated = 0; 
			int totalLocalItemsExamined = 0; 
			int totalRemoteItemsUpdated = 0;
			int totalRemoteItemsExamined = 0;

			while ((nextUpdateAfterRequestFromPeer > DateTime.MinValue) && (nextUpdateAfterRequestToPeer > DateTime.MinValue))
			{
				XmlDocument peerFeedDocument = new XmlDocument();
				//Get peer's feed
				statusBarPanelSyncStatus.Text = "Geting feed from the peer...";
				if (nextUpdateAfterRequestToPeer > DateTime.MinValue)
				{
					peerFeedDocument.LoadXml(remotePeer.GetFeed(nextUpdateAfterRequestToPeer, 50));
				}

				//Sync peer with our updates
				if (nextUpdateAfterRequestToPeer > DateTime.MinValue)
				{
					//Now give out over feed to peer
					statusBarPanelSyncStatus.Text = "Sending your updates...";
					XmlDocument myFeedDocument = CreateFeedXmlDocument(nextUpdateAfterRequestToPeer);
					int itemsUpdated; int itemsExamined;
					nextUpdateAfterRequestToPeer = remotePeer.PostFeed(myFeedDocument.OuterXml, out itemsUpdated, out itemsExamined);
					totalRemoteItemsUpdated += itemsUpdated;
					totalRemoteItemsExamined += itemsExamined;
				}

				//Sync us with peer's updates
				int localItemsUpdatedInThisCycle = 0; int localItemsExaminedInThisCycle = 0;
				MergeFeedContentWithLocalDatabase(peerFeedDocument, out localItemsUpdatedInThisCycle, out localItemsExaminedInThisCycle);
				totalLocalItemsUpdated += localItemsUpdatedInThisCycle;
				totalLocalItemsExamined += localItemsExaminedInThisCycle;
			}

			statusBarPanelSyncStatus.Text = "Sync completed: " 
				+ totalLocalItemsUpdated.ToString() + " recipes recieved/" + totalLocalItemsExamined.ToString() + " examined"
				+ ", "  + totalRemoteItemsUpdated.ToString() + " recipes sent/" + totalRemoteItemsExamined.ToString() + " examined"
				+ ", " + DateTime.Now.ToShortTimeString();

			_recipeDocument.Refresh();
			FillRecipeList();

			remotePeer.RequestClientUIRefresh();
		}

		

		private void SyncWithServer(string getDataUrl, string putDataUrl)
		{
			statusBarPanelSyncStatus.Text = "Starting sync...";

			DateTime nextUpdateAfterRequestFromServer = DateTime.MaxValue;
			DateTime nextUpdateAfterRequestToServer=DateTime.MaxValue;
			int totalLocalItemsUpdated = 0; 
			int totalLocalItemsExamined = 0; 
			int totalRemoteItemsUpdated = 0;
			int totalRemoteItemsExamined = 0;

			while ((nextUpdateAfterRequestFromServer > DateTime.MinValue) || (nextUpdateAfterRequestToServer > DateTime.MinValue))
			{
				XmlDocument serverfeedDocument = null;
				//Get server's feed
				statusBarPanelSyncStatus.Text = "Geting feed from the server...";
				if (nextUpdateAfterRequestToServer > DateTime.MinValue)
				{
					if (getDataUrl.Length > 0)
					{
						string syncServerFeedUrl = getDataUrl;
						using (WebClient webClientToGetFeedContent = new WebClient())
						{
							byte[] serverfeedContentBytes = webClientToGetFeedContent.DownloadData(string.Format(syncServerFeedUrl, nextUpdateAfterRequestToServer.ToString("yyyy-MM-ddTHH:mm:ss.fffffff")));
							string serverfeedContent = System.Text.Encoding.UTF8.GetString(serverfeedContentBytes);

							serverfeedDocument = new XmlDocument();
							serverfeedDocument.LoadXml(serverfeedContent);
						}
					}
					else {} //ignore get feed request because its not available
				}
				else {} //sync is finished or server does not support getData

				//Sync server with our updates
				if (nextUpdateAfterRequestFromServer > DateTime.MinValue)
				{
					if (putDataUrl.Length > 0)
					{
						//Now give out over feed to server
						statusBarPanelSyncStatus.Text = "Sending your updates...";
						XmlDocument myFeedDocument = CreateFeedXmlDocument(nextUpdateAfterRequestFromServer);
						using(BlankWebService syncServerWebService = new BlankWebService())
						{
							syncServerWebService.Url = putDataUrl;
							PostResult result = syncServerWebService.PostFeed(myFeedDocument);
							nextUpdateAfterRequestFromServer = result.SendMoreItemsUpdatedAfter;
							totalRemoteItemsUpdated += result.TotalItemsUpdated;
							totalRemoteItemsExamined += result.TotalItemsExamined;
						}
					}
					else
						nextUpdateAfterRequestFromServer = DateTime.MinValue;
				}
				else {} //sync is finished or server does not support putData

				//Sync us with server's updates
				if (serverfeedDocument != null)
				{
					int localItemsUpdatedInThisCycle = 0; int localItemsExaminedInThisCycle = 0;
					nextUpdateAfterRequestToServer = MergeFeedContentWithLocalDatabase(serverfeedDocument, out localItemsUpdatedInThisCycle, out localItemsExaminedInThisCycle);
					totalLocalItemsUpdated += localItemsUpdatedInThisCycle;
					totalLocalItemsExamined += localItemsExaminedInThisCycle;
				}
				else 
				{
					//no updates had been received
					nextUpdateAfterRequestToServer = DateTime.MinValue;
				}
			}

			statusBarPanelSyncStatus.Text = "Sync completed: " 
				+ totalLocalItemsUpdated.ToString() + " recipes recieved/" + totalLocalItemsExamined.ToString() + " examined"
				+ ", "  + totalRemoteItemsUpdated.ToString() + " recipes sent/" + totalRemoteItemsExamined.ToString() + " examined"
				+ ", " + DateTime.Now.ToShortTimeString();

			_recipeDocument.Refresh();
			FillRecipeList();

		}

		private DateTime MergeFeedContentWithLocalDatabase(XmlDocument xmlDocumentfeedDocument, out int totalLocalItemsUpdated, out int totalLocalItemsExamined)
		{
			DateTime nextRequestForUpdateAfter; 
			totalLocalItemsUpdated = 0; totalLocalItemsExamined = 0;

			statusBarPanelSyncStatus.Text = "Merging started...";
			if (xmlDocumentfeedDocument != null)
			{
				nextRequestForUpdateAfter = SyncEngine.Sync(_localDatabase, xmlDocumentfeedDocument.SelectSingleNode("/dss/dataset"), 
					out totalLocalItemsUpdated, out totalLocalItemsExamined, new SyncEngine.NotifyProgressDelegate(SyncStatusNotification));
			}
			else
				nextRequestForUpdateAfter =  DateTime.MinValue;

			statusBarPanelSyncStatus.Text = "Merge completed: " 
				+ totalLocalItemsUpdated.ToString() + " recipes recieved/" + totalLocalItemsExamined.ToString() + " examined";

			return nextRequestForUpdateAfter;
		}

		private XmlDocument CreateFeedXmlDocument(DateTime includeUpdatesAfter)
		{
			XmlDocument myFeedDocument = new XmlDocument();
			FeedWriter.WriteFeed(myFeedDocument, _localDatabase, "Recipes from " + _recipeDocument.CurrentAuthor, _recipeDocument.GlobalGuid, includeUpdatesAfter, 10);
			return myFeedDocument;
		}

		private void AttachFeedInEmail()
		{
			Microsoft.Office.Interop.Outlook.Application outlookApp = null;
			Microsoft.Office.Interop.Outlook.NameSpace outlookNS = null;
			Microsoft.Office.Interop.Outlook.MAPIFolder draftsFolder = null;
			Microsoft.Office.Interop.Outlook.MailItem mailItem = null;

			try
			{
				string attachmentFileName = Path.Combine(Path.GetTempPath(), "recipe_updates.dss");

				XmlDocument feedDocumet = CreateFeedXmlDocument(DateTime.MaxValue);
				feedDocumet.Save(attachmentFileName);

				outlookApp = new Microsoft.Office.Interop.Outlook.Application();
				outlookNS = outlookApp.Session;
				draftsFolder = outlookNS.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderDrafts);
				mailItem = (Microsoft.Office.Interop.Outlook.MailItem) draftsFolder.Items.Add(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
				mailItem.Subject = "Recipe updates";
				mailItem.Attachments.Add(attachmentFileName, 1, 1, attachmentFileName);
				mailItem.Save();
				mailItem.Display(true);

				//MessageBox.Show("A message in your Drafts has been successfully created. Please open your Drafts folder in Outlook");
			}
			catch(Exception ex)
			{
				if (ex.GetType() == typeof(ApplicationException))
				{
					MessageBox.Show (ex.Message);
				}
				else
					MessageBox.Show (ex.ToString() + "\n\n" + ex.StackTrace, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				if (outlookApp != null)
				{
					if (Process.GetProcessesByName("OUTLOOK.exe").Length > 1)
						outlookApp.Quit();

					Marshal.ReleaseComObject(outlookApp);
					Marshal.ReleaseComObject(outlookNS);
					Marshal.ReleaseComObject(draftsFolder);
					Marshal.ReleaseComObject(mailItem);
				}
			}
		}

		private void SyncStatusNotification(int itemIndex, int itemCount, string message)
		{
			statusBarPanelSyncStatus.Text = message;
		}

		private void listViewRecipes_DoubleClick(object sender, System.EventArgs e)
		{
			EditSelectedItem();		
		}

		private void listViewRecipes_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		private void RefreshUIAfterPeerSync()
		{
			if (MessageBox.Show("A remote peer has just finished sync. Would you like to reload data?", "Reload Request", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) == DialogResult.Yes)
			{
				_recipeDocument.Refresh();
				FillRecipeList();
			}
		}

		private void menuItemOpenItem_Click(object sender, System.EventArgs e)
		{
			EditSelectedItem();
		}

		private void menuItemResolveAllConflicts_Click(object sender, System.EventArgs e)
		{
			ResolveAllConflictsUsingMyValuesForSelectedItems();
		}

		private void menuItemDeleteItem_Click(object sender, System.EventArgs e)
		{
			DeleteSelectedItems(true);		
		}

		private void menuItemUndeleteItem_Click(object sender, System.EventArgs e)
		{
			DeleteSelectedItems(false);		
		}

		private void contextMenuListView_Popup(object sender, System.EventArgs e)
		{
			if (listViewRecipes.SelectedItems.Count > 0)
			{
				if (listViewRecipes.SelectedItems.Count == 1)
				{
					DataItem item = (DataItem) listViewRecipes.SelectedItems[0].Tag;
					menuItemOpenItem.Enabled = true;
					menuItemResolveAllConflicts.Enabled = item.IsInConflict();
					menuItemUndeleteItem.Enabled = Convert.ToBoolean(item.Deleted.Value, CultureInfo.InvariantCulture);
					menuItemDeleteItem.Enabled = !Convert.ToBoolean(item.Deleted.Value, CultureInfo.InvariantCulture);
				}
				else
				{
					menuItemOpenItem.Enabled = false;
					menuItemResolveAllConflicts.Enabled = true;
					menuItemDeleteItem.Enabled = true;
					menuItemUndeleteItem.Enabled = true;
				}
			}
			else
			{
				menuItemOpenItem.Enabled = false;
				menuItemResolveAllConflicts.Enabled = false;
				menuItemDeleteItem.Enabled = false;
				menuItemUndeleteItem.Enabled = false;
			}
		}
	}

	public class DssRemotingServer : MarshalByRefObject
	{

		private DataAccess _localDatabase;
		private string _globalDatasetGuid;
		public delegate void RefreshUIAfterPeerSyncDelegate();
		private RefreshUIAfterPeerSyncDelegate _refreshUIAfterPeerSync;
		string _feedTitle;
		public DssRemotingServer(DataAccess localDatabase, string globalDatasetGuid, string feedTitle, RefreshUIAfterPeerSyncDelegate refreshUIAfterPeerSync)
		{
			_localDatabase = localDatabase;
			_globalDatasetGuid = globalDatasetGuid;
			_feedTitle = feedTitle;
			_refreshUIAfterPeerSync = refreshUIAfterPeerSync;
		}

		public string GetFeed(DateTime includeItemsUpdatedAfter, int maxRowCount)
		{
			XmlDocument feedDocument = new XmlDocument();
			FeedWriter.WriteFeed(feedDocument, _localDatabase, _globalDatasetGuid, _feedTitle, includeItemsUpdatedAfter, maxRowCount);
			return feedDocument.OuterXml;
		}

		public DateTime PostFeed(string feedDocumentContent, out int totalItemsUpdated, out int totalItemsExamined)
		{
			XmlDocument feedDocument = new XmlDocument();
			feedDocument.LoadXml(feedDocumentContent);
			return SyncEngine.Sync(_localDatabase, feedDocument.SelectSingleNode("/dss/dataset"), out totalItemsUpdated, out totalItemsExamined, null);
		}

		public void RequestClientUIRefresh()
		{
			if (_refreshUIAfterPeerSync != null)			
				_refreshUIAfterPeerSync();
		}
	}
}

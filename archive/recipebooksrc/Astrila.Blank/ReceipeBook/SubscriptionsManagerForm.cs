using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Astrila.Common;

namespace Astrila.Blank.Clients.RecipeBook
{
	/// <summary>
	/// Summary description for SubscriptionsManager.
	/// </summary>
	public class SubscriptionsManagerForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ColumnHeader columnHeaderWhen;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListView listViewSubscriptions;
		private System.Windows.Forms.ColumnHeader columnHeaderSubscription;
		private System.Windows.Forms.ColumnHeader columnHeaderUrl;
		private System.Windows.Forms.ToolBar toolBarMain;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolBarButton toolBarButtonAddFeed;
		private System.Windows.Forms.ToolBarButton toolBarButtonRemoveFeed;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.ComponentModel.IContainer components;

		public SubscriptionsManagerForm()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SubscriptionsManagerForm));
			this.columnHeaderSubscription = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderUrl = new System.Windows.Forms.ColumnHeader();
			this.listViewSubscriptions = new System.Windows.Forms.ListView();
			this.columnHeaderWhen = new System.Windows.Forms.ColumnHeader();
			this.panel1 = new System.Windows.Forms.Panel();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.toolBarMain = new System.Windows.Forms.ToolBar();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.toolBarButtonAddFeed = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonRemoveFeed = new System.Windows.Forms.ToolBarButton();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// columnHeaderSubscription
			// 
			this.columnHeaderSubscription.Text = "Subscription";
			this.columnHeaderSubscription.Width = 178;
			// 
			// columnHeaderUrl
			// 
			this.columnHeaderUrl.Text = "Url";
			this.columnHeaderUrl.Width = 165;
			// 
			// listViewSubscriptions
			// 
			this.listViewSubscriptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.listViewSubscriptions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																									this.columnHeaderSubscription,
																									this.columnHeader1,
																									this.columnHeaderUrl,
																									this.columnHeaderWhen});
			this.listViewSubscriptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewSubscriptions.FullRowSelect = true;
			this.listViewSubscriptions.GridLines = true;
			this.listViewSubscriptions.Location = new System.Drawing.Point(0, 28);
			this.listViewSubscriptions.MultiSelect = false;
			this.listViewSubscriptions.Name = "listViewSubscriptions";
			this.listViewSubscriptions.Size = new System.Drawing.Size(522, 199);
			this.listViewSubscriptions.TabIndex = 6;
			this.listViewSubscriptions.View = System.Windows.Forms.View.Details;
			this.listViewSubscriptions.SelectedIndexChanged += new System.EventHandler(this.listViewSubscriptions_SelectedIndexChanged);
			// 
			// columnHeaderWhen
			// 
			this.columnHeaderWhen.Text = "Added on";
			this.columnHeaderWhen.Width = 111;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.buttonCancel);
			this.panel1.Controls.Add(this.buttonOK);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 227);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(522, 56);
			this.panel1.TabIndex = 7;
			this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonCancel.Location = new System.Drawing.Point(440, 16);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(72, 29);
			this.buttonCancel.TabIndex = 21;
			this.buttonCancel.Text = "Cancel";
			// 
			// buttonOK
			// 
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonOK.Location = new System.Drawing.Point(352, 16);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(72, 29);
			this.buttonOK.TabIndex = 17;
			this.buttonOK.Text = "OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(-8, 1);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(624, 8);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			// 
			// toolBarMain
			// 
			this.toolBarMain.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBarMain.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						   this.toolBarButtonAddFeed,
																						   this.toolBarButtonRemoveFeed});
			this.toolBarMain.DropDownArrows = true;
			this.toolBarMain.ImageList = this.imageList1;
			this.toolBarMain.Location = new System.Drawing.Point(0, 0);
			this.toolBarMain.Name = "toolBarMain";
			this.toolBarMain.ShowToolTips = true;
			this.toolBarMain.Size = new System.Drawing.Size(522, 28);
			this.toolBarMain.TabIndex = 23;
			this.toolBarMain.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.toolBarMain.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarMain_ButtonClick);
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// toolBarButtonAddFeed
			// 
			this.toolBarButtonAddFeed.ImageIndex = 1;
			this.toolBarButtonAddFeed.Text = "Add new feed";
			this.toolBarButtonAddFeed.ToolTipText = "Add new feed in your subscription";
			// 
			// toolBarButtonRemoveFeed
			// 
			this.toolBarButtonRemoveFeed.ImageIndex = 0;
			this.toolBarButtonRemoveFeed.Text = "Remove selected feeds";
			this.toolBarButtonRemoveFeed.ToolTipText = "Remove selected feeds from your subscription";
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Two-Way";
			// 
			// SubscriptionsManagerForm
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(522, 283);
			this.Controls.Add(this.listViewSubscriptions);
			this.Controls.Add(this.toolBarMain);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SubscriptionsManagerForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Manage Your Subscriptions";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void panel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		SubscriptionCollection _subscriptions;

		private void SetSubscriptions(string subscriptionFilePath)
		{
			_subscriptions = new SubscriptionCollection(subscriptionFilePath);
			FillSubscriptionList();

		}

		private void FillSubscriptionList()
		{
			SubscriptionCollection.FeedInfo[] feedInfos = _subscriptions.GetFeedInfo();

			listViewSubscriptions.Items.Clear();
			for (int feedInfoIndex = 0; feedInfoIndex < feedInfos.Length; feedInfoIndex++)
			{
				ListViewItem mainItem = listViewSubscriptions.Items.Add(feedInfos[feedInfoIndex].Title);
				mainItem.Tag = feedInfos[feedInfoIndex];

				mainItem.SubItems.Add(feedInfos[feedInfoIndex].TwoWay.ToString());
				mainItem.SubItems.Add(feedInfos[feedInfoIndex].Url);
				mainItem.SubItems.Add(feedInfos[feedInfoIndex].AddedOn.ToString());
			}
		}


		public static DialogResult ShowForm(string subscriptionFilePath)
		{
			using (SubscriptionsManagerForm editor = new SubscriptionsManagerForm())
			{
				editor.SetSubscriptions(subscriptionFilePath);
				return editor.ShowDialog();
			}
		}

		private void listViewSubscriptions_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		private void toolBarMain_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			try
			{
				if (e.Button == toolBarButtonAddFeed)
				{
					string feedUrl = InputBoxDialog.Show("Enter URL of the data feed you wish to add:", "Add Feed", "http://www.shitalshah.com/dss/BlankServer/feed.ashx");
					if (feedUrl.Length > 0)
					{
						_subscriptions.Add(feedUrl);
						FillSubscriptionList();
					}
				}
				else if (e.Button == toolBarButtonRemoveFeed)
				{
					for (int selectedItemIndex = 0; selectedItemIndex < listViewSubscriptions.SelectedItems.Count; selectedItemIndex++)
					{
						_subscriptions.Remove ((SubscriptionCollection.FeedInfo) listViewSubscriptions.SelectedItems[selectedItemIndex].Tag);
					}
					FillSubscriptionList();
				}
				else 
					MessageBox.Show("Not implemented yet");
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
		}

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			_subscriptions.Save();
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}

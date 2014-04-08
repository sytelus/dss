using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Astrila.Blank.Clients.RecipeBook
{
	/// <summary>
	/// Summary description for ConflictIndicator.
	/// </summary>
	public class ConflictIndicator : System.Windows.Forms.PictureBox
	{
		private System.ComponentModel.IContainer components;

		public ConflictIndicator()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ConflictIndicator));
			this.imageListMain = new System.Windows.Forms.ImageList(this.components);
			// 
			// imageListMain
			// 
			this.imageListMain.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
			this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// ConflictIndicator
			// 
			this.Image = ((System.Drawing.Image)(resources.GetObject("$this.Image")));
			this.Size = new System.Drawing.Size(14, 14);
			this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

		}
		#endregion

		private System.Windows.Forms.ImageList imageListMain;


		private DataItemProperty _property = null;
		public DataItemProperty ItemProperty
		{
			get
			{
				return _property;
			}
			set
			{
				_property = value;
				RefreshIndication();
			}
		}

		public void RefreshIndication()
		{
			if (!this.DesignMode)
			{
				if (_property == null)	
				{
					this.Image = imageListMain.Images[3];
					this.Visible = true;
				}
				else
				{
					if (_property.IsInConflict())
					{
						this.Image = imageListMain.Images[0];
						this.Visible = true;
					}
					else
					{
						if (_property.CurrentValueAuthor == _property.Document.CurrentAuthor)
						{
							this.Visible = !_invisibleIfNoConflict;
							if (this.Visible)
							{
								this.Image = imageListMain.Images[2];
							}
							else {}; //no need to do image change
						}
						else
						{
							if (_property.Name != "_deleted_")
							{
								this.Image = imageListMain.Images[1];
								this.Visible = true;
							}
							else
								this.Visible = false;
						}
					}
				}
			}
			else
				this.Visible = true;
		}

		private bool _invisibleIfNoConflict = true;
		public bool InvisibleIfNoConflict
		{
			get
			{
				return _invisibleIfNoConflict;
			}
			set
			{
				_invisibleIfNoConflict = value;
				this.RefreshIndication();
			}
		}

		public delegate void ItemPropertyChangedDelegate(object sender, EventArgs e);
		public event ItemPropertyChangedDelegate ItemPropertyChanged;

		protected override void OnClick(EventArgs e)
		{
			if (_property != null)
			{
				if (ConflictViewerForm.ShowConflicts(_property) != DialogResult.Cancel)
				{
					RefreshIndication();
					if (ItemPropertyChanged != null)
						ItemPropertyChanged(this, EventArgs.Empty);
				}
				else
					_property.RollbackChanges();
			}
			base.OnClick(e);
		}
	}
}

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;

namespace Astrila.Blank.Clients.RecipeBook
{
	/// <summary>
	/// Summary description for RecipeEditorForm.
	/// </summary>
	public class RecipeEditorForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private Astrila.Blank.Clients.RecipeBook.ConflictIndicator conflictIndicatorTitle;
		private Astrila.Blank.Clients.RecipeBook.ConflictIndicator conflictIndicatorIngredients;
		private Astrila.Blank.Clients.RecipeBook.ConflictIndicator conflictIndicatorSteps;
		private Astrila.Blank.Clients.RecipeBook.ConflictIndicator conflictIndicatorOrigin;
		private System.Windows.Forms.ToolTip toolTipMain;
		private System.Windows.Forms.TextBox propertyControlTitle;
		private System.Windows.Forms.TextBox propertyControlIngredients;
		private System.Windows.Forms.TextBox propertyControlSteps;
		private System.Windows.Forms.TextBox propertyControlOrigin;
		private System.Windows.Forms.CheckBox propertyControlIsVeg;
		private Astrila.Blank.Clients.RecipeBook.ConflictIndicator conflictIndicatorIsVeg;
		private System.Windows.Forms.CheckBox propertyControl_deleted_;
		private Astrila.Blank.Clients.RecipeBook.ConflictIndicator conflictIndicator_deleted_;
		private System.Windows.Forms.CheckBox checkBoxShowConflicts;
		private System.Windows.Forms.PictureBox pictureBoxIconHelp;
		private System.ComponentModel.IContainer components;

		public RecipeEditorForm()
		{
			//
			// Required for Windows Form Designer support
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RecipeEditorForm));
			this.label1 = new System.Windows.Forms.Label();
			this.propertyControlTitle = new System.Windows.Forms.TextBox();
			this.propertyControlIngredients = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.propertyControlSteps = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.propertyControlOrigin = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.propertyControlIsVeg = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.conflictIndicatorTitle = new Astrila.Blank.Clients.RecipeBook.ConflictIndicator();
			this.conflictIndicatorIngredients = new Astrila.Blank.Clients.RecipeBook.ConflictIndicator();
			this.conflictIndicatorSteps = new Astrila.Blank.Clients.RecipeBook.ConflictIndicator();
			this.conflictIndicatorOrigin = new Astrila.Blank.Clients.RecipeBook.ConflictIndicator();
			this.conflictIndicatorIsVeg = new Astrila.Blank.Clients.RecipeBook.ConflictIndicator();
			this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
			this.checkBoxShowConflicts = new System.Windows.Forms.CheckBox();
			this.propertyControl_deleted_ = new System.Windows.Forms.CheckBox();
			this.conflictIndicator_deleted_ = new Astrila.Blank.Clients.RecipeBook.ConflictIndicator();
			this.pictureBoxIconHelp = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(15, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Title: ";
			// 
			// propertyControlTitle
			// 
			this.propertyControlTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.propertyControlTitle.Location = new System.Drawing.Point(80, 8);
			this.propertyControlTitle.Name = "propertyControlTitle";
			this.propertyControlTitle.Size = new System.Drawing.Size(312, 20);
			this.propertyControlTitle.TabIndex = 1;
			this.propertyControlTitle.Text = "";
			this.propertyControlTitle.TextChanged += new System.EventHandler(this.propertyTextBox_TextChanged);
			// 
			// propertyControlIngredients
			// 
			this.propertyControlIngredients.AcceptsReturn = true;
			this.propertyControlIngredients.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.propertyControlIngredients.Location = new System.Drawing.Point(80, 40);
			this.propertyControlIngredients.Multiline = true;
			this.propertyControlIngredients.Name = "propertyControlIngredients";
			this.propertyControlIngredients.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.propertyControlIngredients.Size = new System.Drawing.Size(312, 96);
			this.propertyControlIngredients.TabIndex = 3;
			this.propertyControlIngredients.Text = "";
			this.propertyControlIngredients.WordWrap = false;
			this.propertyControlIngredients.TextChanged += new System.EventHandler(this.propertyTextBox_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(15, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Ingredients: ";
			// 
			// propertyControlSteps
			// 
			this.propertyControlSteps.AcceptsReturn = true;
			this.propertyControlSteps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.propertyControlSteps.Location = new System.Drawing.Point(80, 144);
			this.propertyControlSteps.Multiline = true;
			this.propertyControlSteps.Name = "propertyControlSteps";
			this.propertyControlSteps.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.propertyControlSteps.Size = new System.Drawing.Size(312, 96);
			this.propertyControlSteps.TabIndex = 5;
			this.propertyControlSteps.Text = "";
			this.propertyControlSteps.WordWrap = false;
			this.propertyControlSteps.TextChanged += new System.EventHandler(this.propertyTextBox_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(15, 144);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(39, 16);
			this.label3.TabIndex = 4;
			this.label3.Text = "Steps: ";
			// 
			// propertyControlOrigin
			// 
			this.propertyControlOrigin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.propertyControlOrigin.Location = new System.Drawing.Point(80, 251);
			this.propertyControlOrigin.Name = "propertyControlOrigin";
			this.propertyControlOrigin.Size = new System.Drawing.Size(312, 20);
			this.propertyControlOrigin.TabIndex = 7;
			this.propertyControlOrigin.Tag = "";
			this.propertyControlOrigin.Text = "";
			this.propertyControlOrigin.TextChanged += new System.EventHandler(this.propertyTextBox_TextChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(15, 251);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(41, 16);
			this.label4.TabIndex = 6;
			this.label4.Text = "Origin: ";
			// 
			// propertyControlIsVeg
			// 
			this.propertyControlIsVeg.Location = new System.Drawing.Point(80, 283);
			this.propertyControlIsVeg.Name = "propertyControlIsVeg";
			this.propertyControlIsVeg.Size = new System.Drawing.Size(80, 24);
			this.propertyControlIsVeg.TabIndex = 9;
			this.propertyControlIsVeg.Text = "Vegiterian";
			this.propertyControlIsVeg.TextChanged += new System.EventHandler(this.propertyCheckBox_CheckedChanged);
			this.propertyControlIsVeg.CheckedChanged += new System.EventHandler(this.propertyCheckBox_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(0, 317);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(552, 8);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonOK.Location = new System.Drawing.Point(216, 334);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(83, 29);
			this.buttonOK.TabIndex = 11;
			this.buttonOK.Text = "OK";
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonCancel.Location = new System.Drawing.Point(312, 335);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(83, 28);
			this.buttonCancel.TabIndex = 12;
			this.buttonCancel.Text = "Cancel";
			// 
			// conflictIndicatorTitle
			// 
			this.conflictIndicatorTitle.Image = ((System.Drawing.Image)(resources.GetObject("conflictIndicatorTitle.Image")));
			this.conflictIndicatorTitle.InvisibleIfNoConflict = true;
			this.conflictIndicatorTitle.ItemProperty = null;
			this.conflictIndicatorTitle.Location = new System.Drawing.Point(400, 8);
			this.conflictIndicatorTitle.Name = "conflictIndicatorTitle";
			this.conflictIndicatorTitle.Size = new System.Drawing.Size(14, 14);
			this.conflictIndicatorTitle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.conflictIndicatorTitle.TabIndex = 13;
			this.conflictIndicatorTitle.TabStop = false;
			this.conflictIndicatorTitle.ItemPropertyChanged += new Astrila.Blank.Clients.RecipeBook.ConflictIndicator.ItemPropertyChangedDelegate(this.conflictIndicator_ItemPropertyChanged);
			// 
			// conflictIndicatorIngredients
			// 
			this.conflictIndicatorIngredients.Image = ((System.Drawing.Image)(resources.GetObject("conflictIndicatorIngredients.Image")));
			this.conflictIndicatorIngredients.InvisibleIfNoConflict = true;
			this.conflictIndicatorIngredients.ItemProperty = null;
			this.conflictIndicatorIngredients.Location = new System.Drawing.Point(400, 40);
			this.conflictIndicatorIngredients.Name = "conflictIndicatorIngredients";
			this.conflictIndicatorIngredients.Size = new System.Drawing.Size(14, 14);
			this.conflictIndicatorIngredients.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.conflictIndicatorIngredients.TabIndex = 14;
			this.conflictIndicatorIngredients.TabStop = false;
			this.conflictIndicatorIngredients.Visible = false;
			this.conflictIndicatorIngredients.ItemPropertyChanged += new Astrila.Blank.Clients.RecipeBook.ConflictIndicator.ItemPropertyChangedDelegate(this.conflictIndicator_ItemPropertyChanged);
			// 
			// conflictIndicatorSteps
			// 
			this.conflictIndicatorSteps.Image = ((System.Drawing.Image)(resources.GetObject("conflictIndicatorSteps.Image")));
			this.conflictIndicatorSteps.InvisibleIfNoConflict = true;
			this.conflictIndicatorSteps.ItemProperty = null;
			this.conflictIndicatorSteps.Location = new System.Drawing.Point(400, 144);
			this.conflictIndicatorSteps.Name = "conflictIndicatorSteps";
			this.conflictIndicatorSteps.Size = new System.Drawing.Size(14, 14);
			this.conflictIndicatorSteps.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.conflictIndicatorSteps.TabIndex = 15;
			this.conflictIndicatorSteps.TabStop = false;
			this.conflictIndicatorSteps.Visible = false;
			this.conflictIndicatorSteps.ItemPropertyChanged += new Astrila.Blank.Clients.RecipeBook.ConflictIndicator.ItemPropertyChangedDelegate(this.conflictIndicator_ItemPropertyChanged);
			// 
			// conflictIndicatorOrigin
			// 
			this.conflictIndicatorOrigin.Image = ((System.Drawing.Image)(resources.GetObject("conflictIndicatorOrigin.Image")));
			this.conflictIndicatorOrigin.InvisibleIfNoConflict = true;
			this.conflictIndicatorOrigin.ItemProperty = null;
			this.conflictIndicatorOrigin.Location = new System.Drawing.Point(400, 251);
			this.conflictIndicatorOrigin.Name = "conflictIndicatorOrigin";
			this.conflictIndicatorOrigin.Size = new System.Drawing.Size(14, 14);
			this.conflictIndicatorOrigin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.conflictIndicatorOrigin.TabIndex = 16;
			this.conflictIndicatorOrigin.TabStop = false;
			this.conflictIndicatorOrigin.Visible = false;
			this.conflictIndicatorOrigin.ItemPropertyChanged += new Astrila.Blank.Clients.RecipeBook.ConflictIndicator.ItemPropertyChangedDelegate(this.conflictIndicator_ItemPropertyChanged);
			// 
			// conflictIndicatorIsVeg
			// 
			this.conflictIndicatorIsVeg.Image = ((System.Drawing.Image)(resources.GetObject("conflictIndicatorIsVeg.Image")));
			this.conflictIndicatorIsVeg.InvisibleIfNoConflict = true;
			this.conflictIndicatorIsVeg.ItemProperty = null;
			this.conflictIndicatorIsVeg.Location = new System.Drawing.Point(152, 288);
			this.conflictIndicatorIsVeg.Name = "conflictIndicatorIsVeg";
			this.conflictIndicatorIsVeg.Size = new System.Drawing.Size(14, 14);
			this.conflictIndicatorIsVeg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.conflictIndicatorIsVeg.TabIndex = 17;
			this.conflictIndicatorIsVeg.TabStop = false;
			this.conflictIndicatorIsVeg.Visible = false;
			this.conflictIndicatorIsVeg.ItemPropertyChanged += new Astrila.Blank.Clients.RecipeBook.ConflictIndicator.ItemPropertyChangedDelegate(this.conflictIndicator_ItemPropertyChanged);
			// 
			// checkBoxShowConflicts
			// 
			this.checkBoxShowConflicts.Location = new System.Drawing.Point(15, 336);
			this.checkBoxShowConflicts.Name = "checkBoxShowConflicts";
			this.checkBoxShowConflicts.Size = new System.Drawing.Size(97, 24);
			this.checkBoxShowConflicts.TabIndex = 19;
			this.checkBoxShowConflicts.Text = "Show conflicts";
			this.toolTipMain.SetToolTip(this.checkBoxShowConflicts, "Show all conflict indicators");
			this.checkBoxShowConflicts.CheckedChanged += new System.EventHandler(this.checkBoxShowConflicts_CheckedChanged);
			// 
			// propertyControl_deleted_
			// 
			this.propertyControl_deleted_.Location = new System.Drawing.Point(216, 280);
			this.propertyControl_deleted_.Name = "propertyControl_deleted_";
			this.propertyControl_deleted_.Size = new System.Drawing.Size(168, 24);
			this.propertyControl_deleted_.TabIndex = 18;
			this.propertyControl_deleted_.Text = "This recipe has been deleted";
			this.propertyControl_deleted_.CheckedChanged += new System.EventHandler(this.propertyCheckBox_CheckedChanged);
			// 
			// conflictIndicator_deleted_
			// 
			this.conflictIndicator_deleted_.Image = ((System.Drawing.Image)(resources.GetObject("conflictIndicator_deleted_.Image")));
			this.conflictIndicator_deleted_.InvisibleIfNoConflict = true;
			this.conflictIndicator_deleted_.ItemProperty = null;
			this.conflictIndicator_deleted_.Location = new System.Drawing.Point(379, 285);
			this.conflictIndicator_deleted_.Name = "conflictIndicator_deleted_";
			this.conflictIndicator_deleted_.Size = new System.Drawing.Size(14, 14);
			this.conflictIndicator_deleted_.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.conflictIndicator_deleted_.TabIndex = 0;
			this.conflictIndicator_deleted_.TabStop = false;
			this.conflictIndicator_deleted_.ItemPropertyChanged += new Astrila.Blank.Clients.RecipeBook.ConflictIndicator.ItemPropertyChangedDelegate(this.conflictIndicator_ItemPropertyChanged);
			// 
			// pictureBoxIconHelp
			// 
			this.pictureBoxIconHelp.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxIconHelp.Image")));
			this.pictureBoxIconHelp.Location = new System.Drawing.Point(400, 282);
			this.pictureBoxIconHelp.Name = "pictureBoxIconHelp";
			this.pictureBoxIconHelp.Size = new System.Drawing.Size(20, 20);
			this.pictureBoxIconHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBoxIconHelp.TabIndex = 20;
			this.pictureBoxIconHelp.TabStop = false;
			this.pictureBoxIconHelp.Click += new System.EventHandler(this.pictureBoxIconHelp_Click);
			// 
			// RecipeEditorForm
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(424, 376);
			this.Controls.Add(this.pictureBoxIconHelp);
			this.Controls.Add(this.checkBoxShowConflicts);
			this.Controls.Add(this.conflictIndicator_deleted_);
			this.Controls.Add(this.conflictIndicatorIsVeg);
			this.Controls.Add(this.conflictIndicatorOrigin);
			this.Controls.Add(this.conflictIndicatorSteps);
			this.Controls.Add(this.conflictIndicatorIngredients);
			this.Controls.Add(this.conflictIndicatorTitle);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.propertyControlIsVeg);
			this.Controls.Add(this.propertyControlOrigin);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.propertyControlSteps);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.propertyControlIngredients);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.propertyControlTitle);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.propertyControl_deleted_);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "RecipeEditorForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Recipe Editor";
			this.ResumeLayout(false);

		}
		#endregion

		private DataItem _item;

		public static DialogResult ShowRecipe(Form parent, DataItem item)
		{
			//TODO: document assumption: item does not have changes (or else they would be lost when user clicks cancel)
			using (RecipeEditorForm editor = new RecipeEditorForm())
			{
				editor.CustomFormInitializations(item);
				editor.SetEditing(true);
				DialogResult result =  editor.ShowDialog(parent);
				
				if (result == DialogResult.OK)
				{
					//item already has users values
				}				
				else
				{
					//Remove users changes
					item.RollbackChanges();
				}

				return result;
			}
		}


		private bool SetEditing(bool value)
		{
			bool oldValue = _editing;
			_editing = value;
			return oldValue;
		}

		private void SetValuesInEditor(DataItem item)
		{
			bool oldValue = SetEditing(false);

			foreach(DictionaryEntry entry in _propertyControls)
			{
				SetValueInControlFromItem(item, entry.Key.ToString());
			}

			SetEditing(oldValue);
		}

		private void SetValuesInItem(DataItem item)
		{
			foreach(DictionaryEntry entry in _propertyControls)
			{
				Control control = (Control) entry.Value;
				SetValueInItemFromControl(control, item, entry.Key.ToString());
			}
		}

		private static void SetValueInItemFromControl(Control control, DataItem item, string propertyName)
		{
			if (control.GetType() == typeof(CheckBox))
			{
				item.Properties[propertyName].Value = System.Convert.ToString(((CheckBox)control).Checked, CultureInfo.InvariantCulture);
			}
			else if (control.GetType() == typeof(TextBox))
			{
				item.Properties[propertyName].Value = ((TextBox)control).Text;
			}
			else 
				throw new ApplicationException(string.Format("Control type {0} is not supported as a property control", control.GetType()));

		}

		private void SetValueInControlFromItem(DataItem item, string propertyName)
		{
			Control control = (Control) _propertyControls[propertyName];
			if (control.GetType() == typeof(CheckBox))
			{
				if (item.Properties[propertyName].Value.Length > 0)
					((CheckBox)control).Checked = System.Convert.ToBoolean(item.Properties[propertyName].Value, CultureInfo.InvariantCulture);
				else
					((CheckBox)control).Checked = false;

				if (propertyName == "_deleted_")
					((CheckBox)control).Visible = item.Properties[propertyName].IsInConflict();
			}
			else if (control.GetType() == typeof(TextBox))
			{
				((TextBox)control).Text = item.Properties[propertyName].Value;					
			}
			UpdateTooltip(propertyName);
		}


		private void UpdateTooltip(string propertyName)
		{
			Control control = (Control) _propertyControls[propertyName];

			bool resolved; string resolvedBy; DateTime resolvedOn; string resolutionReason;
			_item.Properties[propertyName].GetLastConflictResolusionInfo(out resolved, out resolvedBy, out resolvedOn, out resolutionReason);

			string toolTipText = "Authored by: " + _item.Properties[propertyName].CurrentValue.Author
				+ "\nChanged on: " + _item.Properties[propertyName].CurrentValue.ChangedOn.ToShortDateString() + " " + _item.Properties[propertyName].CurrentValue.ChangedOn.ToShortTimeString()
				+ "\nAuthor's revision: " + _item.Properties[propertyName].CurrentValue.Revision.ToString();

			if (resolved)
			{
				toolTipText += "\n\nConflict resolved by: " + resolvedOn.ToShortDateString() + " " + resolvedOn.ToShortTimeString();
				toolTipText += "\nConflict resolved on: " + resolvedBy;
				toolTipText += "\nConflict resolution reason: " + resolutionReason;
			}

			toolTipMain.SetToolTip(control, toolTipText);


			
			//TODO: add exists check
			ConflictIndicator conflictIndicatorForThisControl = (ConflictIndicator) _propertyConflictIndicators[propertyName];

			if (_item.Properties[propertyName].IsInConflict())
			{
				toolTipMain.SetToolTip(conflictIndicatorForThisControl, "There are conflicts. Click to view them.");
			}
			else if (_item.Properties[propertyName].CurrentValueAuthor == _item.Document.CurrentAuthor)
			{
				toolTipMain.SetToolTip(conflictIndicatorForThisControl, "There are no conflicts. Click to view list.");
			}
			else
			{
				toolTipMain.SetToolTip(conflictIndicatorForThisControl, "Someone else has set this value. If you change this, it will generate a conflict. \nClick to view the value list.");
			}

		}

		public static DialogResult CreateRecipe(Form parent, DataDocument document)
		{
			using (RecipeEditorForm editor = new RecipeEditorForm())
			{
				DataItem item = document.Items.Add();
				editor.CustomFormInitializations(item);
				editor.SetEditing(true);
				DialogResult result = editor.ShowDialog(parent);
				if (result == DialogResult.OK)
				{
					//item already has values set by user
				}
				else
				{
					//Remove users changes
					item.RollbackChanges();
				}

				return result;
			}
		}


		private bool _editing = false;

		private void propertyTextBox_TextChanged(object sender, System.EventArgs e)
		{
			if (_editing)
			{
				string propertyName = ((TextBox)sender).Name.Substring("propertyControl".Length);
				_item.Properties[propertyName].Value =  ((TextBox)sender).Text;
				((ConflictIndicator)_propertyConflictIndicators[propertyName]).RefreshIndication();
				UpdateTooltip(propertyName);
			}
			else {}; //do not change item if we are not in editing mode!
		}		

		private void propertyCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			if (_editing)
			{
				string propertyName = ((CheckBox)sender).Name.Substring("propertyControl".Length);
				_item.Properties[propertyName].Value =  System.Convert.ToString(((CheckBox)sender).Checked.ToString(), CultureInfo.InvariantCulture);
				((ConflictIndicator)_propertyConflictIndicators[propertyName]).RefreshIndication();
				UpdateTooltip(propertyName);
			}
			else {}; //do not change item if we are not in editing mode!
		}

		private void conflictIndicator_ItemPropertyChanged(object sender, System.EventArgs e)
		{
			string propertyName = ((ConflictIndicator)sender).Name.Substring("conflictIndicator".Length);
			bool oldValue = SetEditing(false);

			SetValueInControlFromItem(_item, propertyName);
			SetEditing(oldValue);
		}


		private Hashtable _propertyControls;
		private Hashtable _propertyConflictIndicators;
		private void CustomFormInitializations(DataItem item)
		{
			_item = item;

			//Key should be same as property values
			_propertyControls = new Hashtable();
			_propertyConflictIndicators = new Hashtable();

			foreach (Control control in this.Controls)
			{
				if (control.GetType() == typeof(ConflictIndicator))
				{
					string propertyName = control.Name.Substring("conflictIndicator".Length);
					_propertyConflictIndicators.Add(propertyName, control);
					((ConflictIndicator) control).ItemProperty = _item.Properties[propertyName];
				}
				else if ((control.GetType() == typeof(TextBox)) || (control.GetType() == typeof(CheckBox)))
				{
					if (control.Name.StartsWith("propertyControl"))
					{
						_propertyControls.Add(control.Name.Substring("propertyControl".Length), control);		
					}
				}
				else {} //ignore other controls
			}

			SetValuesInEditor(item);
		}

		private void checkBoxShowConflicts_CheckedChanged(object sender, System.EventArgs e)
		{
			foreach(DictionaryEntry entry in _propertyConflictIndicators)
			{
				((ConflictIndicator)entry.Value).InvisibleIfNoConflict = !checkBoxShowConflicts.Checked;
			}
		}

		private void pictureBoxIconHelp_Click(object sender, System.EventArgs e)
		{
			using (Form aboutForm = new About())
			{
				aboutForm.ShowDialog();
			}
		}
	}
}

//TODO: Add ability to delete an item
//TODO: Add input box asking user name
//TODO: Fix SyncMark bug
//TODO: Show orange icon if change may cause conflict
//TODO: Allow to delete/Undelete a version
//TODO: Show tooltip in textboxes in editor
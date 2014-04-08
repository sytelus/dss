using System;
using System.Collections.Specialized;
using System.Data;

namespace Astrila.Blank
{
	/// <summary>
	/// Summary description for DataDocumentItem.
	/// </summary>
	public class DataItem
	{
		private DataDocument _document;
		private DataRow _row;
		
		private DataItemPropertyCollection _properties = null;

		internal DataItem(DataDocument document, DataRow itemRow)
		{
			_document = document;
			_row = itemRow;
		}

		public DataDocument Document
		{
			get
			{
				return _document;
			}
		}

		public bool RollbackChanges()
		{
			for (int propertyIndex = 0; propertyIndex < this.Properties.Count; propertyIndex++)
			{
				this.Properties.RollbackChanges();
			}

			bool rebuildOfDictionaryRequired = false;
			_row.RejectChanges();
			rebuildOfDictionaryRequired |= (_row.RowState == DataRowState.Detached);
			
			if (rebuildOfDictionaryRequired)
				_document.Items.RebuildDictionary();

			return rebuildOfDictionaryRequired;
		}

		public bool IsInConflict()
		{
			for (int propertyIndex = 0; propertyIndex < this.Properties.Count; propertyIndex++)
			{
				if (this.Properties[propertyIndex].IsInConflict())
				{
					return true;
				}
			}
			return false;
		}

		public string ID
		{
			get
			{
				return _row["Guid"].ToString();
			}
		}

		public bool Dirty
		{
			get
			{
				return (_row.RowState == DataRowState.Unchanged) || this.Properties.Dirty;
			}
		}


		public string Type
		{
			get
			{
				return _row["Type"].ToString();
			}
		}

		public DataItemPropertyCollection Properties
		{
			get
			{
				if (_properties == null)
					_properties = new DataItemPropertyCollection(_document, this);

				return _properties;
			}
		}

		public DateTime LastModifiedOn
		{
			get
			{
				return (DateTime) _row["When"];
			}
		}

		internal void UpdateLastModifiedOn()
		{
			_row["When"] = DateTime.UtcNow;
		}

		public string LocalInstanceGuid
		{
			get
			{
				return _row["EndPointGuid"].ToString();
			}
		}

		public DataItemProperty Deleted
		{
			get
			{
				return this.Properties["_deleted_"];
			}
		}

		public void ResolveAllConflictsUsingMyValues(string resolutionReason)
		{
			for (int propertyIndex = 0; propertyIndex < this.Properties.Count; propertyIndex++)
			{
				if (this.Properties[propertyIndex].IsInConflict())
				{
					this.Properties[propertyIndex].ResolveConflictUsingMyValue(resolutionReason);
				}
			}
		}
	}
}

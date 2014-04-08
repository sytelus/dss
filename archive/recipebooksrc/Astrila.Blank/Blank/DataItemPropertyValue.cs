using System;
using System.Collections.Specialized;
using System.Data;

namespace Astrila.Blank
{
	public class DataItemPropertyValue
	{
		private DataDocument _document;
		private DataRow _row;
		internal DataItemPropertyValue(DataDocument document, DataRow row)
		{
			_document = document;
			_row = row;
		}

		

		public bool Deleted
		{
			get
			{
				return (bool) _row["Deleted"];
			}

			set
			{
				if (value != this.Deleted)	//protect against redundant change
				{
					if (value == true)
					{
						//If this is not a current value
						if (! this.Current)
						{
							//If current author is changing her value
							if (this.Author == _document.CurrentAuthor)
							{
								//This also increments revision. That is req by Delete algo and updated item timestamp
								SetValueAndDeletedState(true, null, true, true);
							}
							else	//She is changing someone else's value
								SetValueAndDeletedState(false, null, true, false);
						}
						else
							throw new Exception(string.Format("Can not delete property value because its set as current: item ID {0} and property name {1} and Author {2}", this.ItemID, this.PropertyName, this.Author));			
					}	
					else
					{
						//If current author is changing her value
						if (this.Author == _document.CurrentAuthor)
						{
							//This also increments revision. That is req by Delete algo and updated item timestamp
							SetValueAndDeletedState(false, null, false, true);
						}
						else	//She is changing someone else's value
							throw new Exception(string.Format("Author {0} is attempting to undelete value authored by {1} which is an invalid operation", _document.CurrentAuthor, this.Author));
					}
				}
				else {} //redundant change, ignore
			}
		}

		internal void SetDeletedForCurrentRevision(bool deleted)
		{
			SetValueAndDeletedState(false, null, true, false);
		}

		public bool Current
		{
			get
			{
				return (bool) _row["Current"];
			}
		}

		public string PropertyName
		{
			get
			{
				return _row["PropertyName"].ToString();
			}
		}

		public string Value
		{
			get
			{
				return _row["Value"].ToString(); //ToString converts DBNull to empty string
			}
			set
			{
				SetValueAndDeletedState(true, value, false, true);
			}
		}

		private void SetValueAndDeletedState(bool setValue, string value, bool deletedState, bool incrementRevision)
		{
			if (setValue)
			{
				if (this.Author == _document.CurrentAuthor)
					_row["Value"] = value;
				else
					throw new Exception(string.Format("Author {0} is attempting to change value authored by {1} which is an invalid operation", _document.CurrentAuthor, this.Author));
			}
			else {} //do not change value as instructed

			_row["Deleted"] = deletedState;
			
			if (incrementRevision && !_revisionChanged)
				this.SetRevision(this.Revision + 1);
			else {} //Row is still dirty and unsaved. No need to increment the revision
			
			if (_revisionChanged)
				this.SetChangedOn(DateTime.UtcNow);
			else {} //Do not update timestamp unless revision was changed
	
			_document.UpdateLastModifiedOnForItem(this.ItemID);
		}

		public bool Dirty
		{
			get
			{
				return _row.RowState != DataRowState.Unchanged;
			}
		}

		public string Author
		{
			get
			{
				return _row["By"].ToString();
			}
		}

		public DateTime ChangedOn
		{
			get
			{
				return (DateTime) _row["When"];
			}
		}
		internal void SetChangedOn(DateTime newChangedOn)
		{
			_row["When"] = newChangedOn;
		}

		public DateTime LastConflictResolutionOn
		{
			get
			{
				return (DateTime) _row["ResolvedWhen"];
			}
		}
		internal void SetLastConflictResolutionByAuthorOn(DateTime newLastConflictResolutionOn)
		{
			if (newLastConflictResolutionOn > DateTime.MinValue)
				_row["ResolvedWhen"] = newLastConflictResolutionOn;
			else
				_row["ResolvedWhen"] = null;
		}
		public string LastConflictResolutionReason
		{
			get
			{
				return _row["ResolvedReason"].ToString();
			}
		}
		internal void SetLastConflictResolutionByAuthorReason(string newLastConflictResolutionReason)
		{
			_row["ResolvedReason"] = newLastConflictResolutionReason;
		}

		public int Revision
		{
			get
			{
				return (int) _row["Revision"];
			}
		}

		private bool _revisionChanged = false;
		private void SetRevision(int newRevision)
		{
			_revisionChanged = true;
			_row["Revision"] = newRevision;
		}

		public string ItemID
		{
			get
			{
				return _row["ItemGuid"].ToString();
			}
		}

		public override string ToString()
		{
			return "PropertyValue: " + this.ItemID + " : " + this.PropertyName + " : " + this.Author;
		}

		internal void SetIsCurrent(bool newIsCurrent)
		{
			_row["Current"] = newIsCurrent;
		}
	}
}


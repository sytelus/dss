using System;
using System.Data;

namespace Astrila.Blank
{
	public class DataItemProperty
	{
		private DataDocument _document;
		private DataRow _row;
		private DataItemPropertyValueCollection _itemPropertyValues;
		internal DataItemProperty(DataDocument document, DataRow row)
		{
			_document = document;
			_row = row;
		}

		public string Name
		{
			get
			{
				return _row["Name"].ToString();
			}
		}

		public string CurrentValueAuthor
		{
			get
			{
				return _row["CurrentValueBy"].ToString();
			}
		}

		public DataDocument Document
		{
			get
			{
				return _document;
			}
		}

		internal void SetCurrentValueAuthor(string newCurrentValueAuthor)
		{
			_row["CurrentValueBy"] = newCurrentValueAuthor;
		}

		public string CurrentValueAuthorChangedBy
		{
			get
			{
				return _row["By"].ToString();
			}
		}
		private void SetCurrentValueAuthorChangedBy(string newCurrentValueAuthorChangedBy)
		{
			_row["By"] = newCurrentValueAuthorChangedBy;
		}

		public DateTime CurrentValueAuthorChangedOn
		{
			get
			{
				return (DateTime) _row["When"];
			}
		}
		private void SetCurrentValueAuthorChangedOn(DateTime newCurrentValueAuthorChangedOn)
		{
			_row["When"] = newCurrentValueAuthorChangedOn;
		}


		public int CurrentValueAuthorChangeRevision
		{
			get
			{
				return (int) _row["Revision"];
			}
		}
		private void SetCurrentValueAuthorChangeRevision(int newSetCurrentValueAuthorChangeRevision)
		{
			_row["Revision"] = newSetCurrentValueAuthorChangeRevision;
		}


		public string ItemID
		{
			get
			{
				return _row["ItemGuid"].ToString();
			}
		}

		public DataItemPropertyValueCollection Values
		{
			get
			{
				if (_itemPropertyValues == null)
					_itemPropertyValues = new DataItemPropertyValueCollection(_document, this);

				return _itemPropertyValues;
			}
		}

		public DataItemPropertyValue CurrentValue
		{
			get
			{
				return this.Values[this.CurrentValueAuthor];
			}

			set
			{
				if (!value.Deleted)
				{
					if (value.Author != this.CurrentValueAuthor)
					{
						this.SetCurrentValueAuthor(value.Author);
						this.SetCurrentValueAuthorChangeRevision(this.CurrentValueAuthorChangeRevision);
						this.SetCurrentValueAuthorChangedOn(DateTime.UtcNow);
						this.SetCurrentValueAuthorChangedBy(_document.CurrentAuthor);

						for (int valueIndex = 0; valueIndex < this.Values.Count; valueIndex++)
						{
							if (this.Values[valueIndex] != value)
							{
								this.Values[valueIndex].SetIsCurrent(false);
							}
							else
								this.Values[valueIndex].SetIsCurrent(true);
						}

						_document.UpdateLastModifiedOnForItem(this.ItemID);
					}
					else {}; //This value is already set as current
				}
				else throw new Exception(string.Format("Can not set value {0} as current because it has been deleted", value.ToString()));
			}
		}

		public bool RollbackChanges()
		{
			this.Values.RollbackChanges();
			bool rebuildOfDictionaryRequired = false;
			_row.RejectChanges();
			rebuildOfDictionaryRequired |= (_row.RowState == DataRowState.Detached);
			return rebuildOfDictionaryRequired;
		}


		public bool IsInConflict()
		{
			if (this.Values.Count == 1)
				return false;
			else
			{
				int undeletedValueCount = 0;
				for (int valueIndex = 0; valueIndex < this.Values.Count; valueIndex++)
				{
					if (!this.Values[valueIndex].Deleted)
						undeletedValueCount++;
				}

				return undeletedValueCount > 1;
			}
		}

		public void ResolveConflict(string usingValueAuthoredBy, string resolutionReason)
		{
			if (this.Values.Contains(usingValueAuthoredBy))
			{
				DataItemPropertyValue valueToUse = this.Values[usingValueAuthoredBy];
				if (! valueToUse.Deleted)
				{
					//Put the stamp that I resolved this
					SetValueAuthoredBy(true, null, _document.CurrentAuthor, true, DateTime.UtcNow, resolutionReason);

					for (int valueIndex = 0; valueIndex < this.Values.Count; valueIndex++)
					{
						if (Values[valueIndex] != valueToUse)
							Values[valueIndex].SetDeletedForCurrentRevision(true);
						else {}; //Do not set Deleted for resolution value
					}

					//Set as current
					this.CurrentValue = valueToUse;
				}
				else
					throw new Exception(string.Format("Can not resolve conflict using value authored by {0} because that value has been deleted", usingValueAuthoredBy));
			}
			else
				throw new Exception(string.Format("Can not resolve conflict using value authored by {0} because that author has not set any values yet", usingValueAuthoredBy));
		}
		public void ResolveConflict(string usingValueAuthoredBy)
		{
			ResolveConflict(usingValueAuthoredBy, string.Empty);
		}


		public bool MyValueExist
		{
			get
			{
				return this.Values.Contains(_document.CurrentAuthor);
			}
		}

		public bool Contains(string valueAuthorBy)
		{
			return this.Values.Contains(_document.CurrentAuthor);			
		}

		public string MyValue
		{
			get
			{
				if (this.Values.Contains(_document.CurrentAuthor))
				{
					return this.Values[_document.CurrentAuthor].Value;
				}
				else
					throw new Exception(string.Format("The author {0} has not set any value for property {1} for Item ID {2}", _document.CurrentAuthor, this.Name, this.ItemID));
			}
			set
			{
				SetValueAuthoredBy(false, value, _document.CurrentAuthor, false, DateTime.MinValue, null);
				_document.UpdateLastModifiedOnForItem(this.ItemID);
			}
		}

		public string Value
		{
			get
			{
				return this.CurrentValue.Value;	
			}
			set
			{
				//First set the value
				SetValueAuthoredBy(false, value, _document.CurrentAuthor, false, DateTime.MinValue, null);

				//Now set this value as current
				this.CurrentValue = this.Values[_document.CurrentAuthor];

				//Item last modified state is already set by above steps
			}
		}

		public bool Dirty
		{
			get
			{
				return (_row.RowState == DataRowState.Unchanged) || this.Values.Dirty;
			}
		}

		private void SetValueAuthoredBy(bool setValueOnlyIfNewCreated, string valueToSet, string author, bool deletedStatusForIfNewValueAdded, DateTime conflictResolvedOn, string conflictResolvedReason)
		{
			if (this.Values.Contains(author))
			{
				DataItemPropertyValue valueToChange = this.Values[author];

				if (!setValueOnlyIfNewCreated)
					valueToChange.Value = valueToSet;
				
				if (conflictResolvedOn != DateTime.MinValue)
				{
					valueToChange.SetLastConflictResolutionByAuthorOn(conflictResolvedOn);
					valueToChange.SetLastConflictResolutionByAuthorReason(conflictResolvedReason);
				}
				else {} //do not change conflict resolution info
			}
			else
			{
				DataRow newItemPropertyValueRow = _document.AddNewPropertyValueRow(this.ItemID, _document.Items[this.ItemID].Type,  this.Name, valueToSet, author, false, deletedStatusForIfNewValueAdded, conflictResolvedOn, conflictResolvedReason);
				this.Values.AddPropertyValueInInternalList(newItemPropertyValueRow, false);
			}
		}

		
		public void GetLastConflictResolusionInfo(out bool resolved, out string resolvedBy, out DateTime resolvedOn, out string resolutionReason)
		{
			resolved = false; resolvedBy = string.Empty; resolvedOn = DateTime.MinValue; resolutionReason = string.Empty;

			for (int valueIndex = 0; valueIndex < this.Values.Count; valueIndex++)
			{
				if (this.Values[valueIndex].LastConflictResolutionOn > resolvedOn)
				{
					resolved = true; 
					resolvedBy = this.Values[valueIndex].Author; 
					resolvedOn = this.Values[valueIndex].LastConflictResolutionOn;
					resolutionReason = this.Values[valueIndex].LastConflictResolutionReason;
				}
			}
		}

		public void ResolveConflictUsingMyValue(string resolutionReason)
		{
			this.ResolveConflict(_document.CurrentAuthor, resolutionReason);
		}

		public void ResolveConflictUsingCurrentValue(string resolutionReason)
		{
			this.ResolveConflict(this.CurrentValueAuthor, resolutionReason);
		}
	}
}

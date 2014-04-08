using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using Astrila.Common;

namespace Astrila.Blank
{
	/// <summary>
	/// Summary description for DataDocumentItemPropertyValueCollection.
	/// </summary>
	public class DataItemPropertyValueCollection
	{
		private DataDocument _document;
		private DataItemProperty _property;
		private ArrayList _propertyValueRows = null;	//initialized on demand
		private DictionaryList _dictionary = new DictionaryList();

		internal DataItemPropertyValueCollection(DataDocument document, DataItemProperty property)
		{
			_document = document;
			_property = property;
		}

		private ArrayList Rows
		{
			get
			{
				if (_propertyValueRows != null)
					return _propertyValueRows;
				else
				{
					DataRow[] rowsToAdd = _document.GetItemPropertyValueRows(_property.ItemID, _property.Name);
					_propertyValueRows = new ArrayList(rowsToAdd.Length);
					_propertyValueRows.AddRange(rowsToAdd);
					return _propertyValueRows;
				}
			}
		}

		public bool Dirty
		{
			get
			{
				for (int valueIndex = 0; valueIndex < this.Rows.Count; valueIndex++)
				{
					if(this[valueIndex].Dirty)
						return true;
				}
				return false;
			}
		}

		public bool RollbackChanges()
		{
			bool rebuildOfDictionaryRequired = false;
			for (int valueIndex = 0; valueIndex < this.Rows.Count; valueIndex++)
			{
				DataRow rowToRollBack = ((DataRow) this.Rows[valueIndex]);
				rowToRollBack.RejectChanges();
				rebuildOfDictionaryRequired |= (rowToRollBack.RowState == DataRowState.Detached);
			}

			if (rebuildOfDictionaryRequired)
			{
				_propertyValueRows = null;
				_dictionary = new DictionaryList();
			}

			return rebuildOfDictionaryRequired;
		}

		public int Count
		{
			get
			{
				return this.Rows.Count;
			}
		}

		public DataItemPropertyValue this[string author]
		{
			get
			{
				if (_dictionary.Contains(author))
				{
					return (DataItemPropertyValue) _dictionary[author];
				}
				else
				{
					for (int rowIndex = _dictionary.Count; rowIndex < this.Rows.Count; rowIndex++)
					{
						DataRow itemPropertyValueRow = (DataRow) this.Rows[rowIndex];
						DataItemPropertyValue newItemProperty = AddPropertyValueInInternalList(itemPropertyValueRow, true);
						if (newItemProperty.Author == author)
							break;
					}
					return (DataItemPropertyValue) _dictionary[author];
				}
			}
		}

		internal DataItemPropertyValue AddPropertyValueInInternalList(DataRow itemPropertyValueRow, bool addToDictionaryOnly)
		{
			DataItemPropertyValue newItemPropertyValue = new DataItemPropertyValue(_document, itemPropertyValueRow);
			if (!addToDictionaryOnly)
				this.Rows.Add(itemPropertyValueRow);
			_dictionary.Add(newItemPropertyValue.Author, newItemPropertyValue);
			return newItemPropertyValue;
		}

		public bool Contains(string valueAuthoredBy)
		{
			if (_dictionary.Contains(valueAuthoredBy))
			{
				return true;
			}
			else
			{
				DataRow itemPropertyValueRow = _document.GetItemPropertyValueRow(_property.ItemID, _property.Name, valueAuthoredBy, true);
				return (itemPropertyValueRow != null);
			}			
		}

		public DataItemPropertyValue this[int index]
		{
			get
			{
				if (_dictionary.Count > index)
				{
					return (DataItemPropertyValue) _dictionary[index];
				}
				else
				{
					for (int rowIndex = _dictionary.Count; rowIndex <= index; rowIndex++)
					{
						DataRow itemPropertyValueRow = (DataRow) this.Rows[rowIndex];
						AddPropertyValueInInternalList(itemPropertyValueRow, true);
					}
					return (DataItemPropertyValue) _dictionary[index];
				}
			}
		}
	}
}

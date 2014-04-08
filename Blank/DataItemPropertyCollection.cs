using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using Astrila.Common;

namespace Astrila.Blank
{
	/// <summary>
	/// Summary description for DataDocumentItemPropertyCollection.
	/// </summary>
	public class DataItemPropertyCollection
	{
		private DataDocument _document;
		private DataItem _item;
		private ArrayList _propertyRows = null;	//initialized on demand
		private DictionaryList _dictionary = new DictionaryList();

		internal DataItemPropertyCollection(DataDocument document, DataItem item)
		{
			_document = document;
			_item = item;
		}

		private ArrayList Rows
		{
			get
			{
				if (_propertyRows != null)
					return _propertyRows;
				else
				{
					DataRow[] rowsToAdd = _document.GetItemPropertyRows(_item.ID);
					_propertyRows = new ArrayList(rowsToAdd.Length);
					_propertyRows.AddRange(rowsToAdd);
					return _propertyRows;
				}
			}
		}

		public int Count
		{
			get
			{
				return this.Rows.Count;
			}
		}

		public bool RollbackChanges()
		{
			bool rebuildOfDictionaryRequired = false;
			for (int propertyRowIndex = 0; propertyRowIndex < this.Count; propertyRowIndex++)
			{
				rebuildOfDictionaryRequired |= this[propertyRowIndex].RollbackChanges();
			}
			if (rebuildOfDictionaryRequired)
			{
				_dictionary = new DictionaryList();
				_propertyRows = null;
			}

			return rebuildOfDictionaryRequired;
		}

		public DataItemProperty this[string propertyName]
		{
			get
			{
				if (_dictionary.Contains(propertyName))
				{
					return (DataItemProperty) _dictionary[propertyName];
				}
				else
				{
					//Find in our array
					//TODO: remove this assumption: number of properties are fixed
					for (int rowIndex = _dictionary.Count; rowIndex < this.Rows.Count; rowIndex++)
					{
						DataItemProperty newItemProperty = AddPropertyInInternalList((DataRow) this.Rows[rowIndex], true);
						if (newItemProperty.Name == propertyName)
							break;
					}
					return (DataItemProperty) _dictionary[propertyName];
				}
			}
		}

		public bool Dirty
		{
			get
			{
				for (int propertyIndex = 0; propertyIndex < this.Rows.Count; propertyIndex++)
				{
					if(this[propertyIndex].Dirty)
						return true;
				}
				return false;	
			}
		}


		internal DataItemProperty AddPropertyInInternalList(DataRow itemPropertyRow, bool addToDictionaryOnly)
		{
			DataItemProperty newItemProperty = new DataItemProperty(_document, itemPropertyRow);
			if (! addToDictionaryOnly)
				this.Rows.Add(itemPropertyRow);
			_dictionary.Add(newItemProperty.Name, newItemProperty);
			return newItemProperty;
		}

		public DataItemProperty this[int index]
		{
			get
			{
				if (_dictionary.Count > index)
				{
					return (DataItemProperty) _dictionary[index];
				}
				else
				{
					for (int rowIndex = _dictionary.Count; rowIndex <= index; rowIndex++)
					{
						DataRow itemPropertyRow = (DataRow) this.Rows[rowIndex];
						AddPropertyInInternalList(itemPropertyRow, true);
					}
					return (DataItemProperty) _dictionary[index];
				}
			}
		}
	}
}

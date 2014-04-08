using System;
using System.Collections.Specialized;
using System.Data;
using Astrila.Common;

namespace Astrila.Blank
{
	/// <summary>
	/// Summary description for DataDocumentItemCollection.
	/// </summary>
	public class DataItemCollection
	{
		private DataDocument _document;
		private DictionaryList _dictionary = new DictionaryList();
		internal DataItemCollection(DataDocument document)
		{
			_document = document;
		}	

		public int Count
		{
			get
			{
				return _document.GetItemCount();
			}
		}

		internal void RebuildDictionary()
		{
			_dictionary = new DictionaryList();
		}

		public DataItem this[string itemID]
		{
			get
			{
				if (_dictionary.Contains(itemID))
				{
					return (DataItem) _dictionary[itemID];
				}
				else
				{
					DataRow itemRow = _document.GetItemRow(itemID, false);
					DataItem newItem = new DataItem(_document, itemRow);
					_dictionary.Add(itemID, newItem);
					return newItem;
				}
			}
		}

		public DataItem this[int index]
		{
			get
			{
				DataRow itemRow = _document.GetItemRow(index);
				string itemID = itemRow["Guid"].ToString();
				if (_dictionary.Contains(itemID))
				{
					return (DataItem) _dictionary[itemID];
				}
				else
				{
					DataItem newItem = new DataItem(_document, itemRow);
					_dictionary.Add(itemID, newItem);
					return newItem;
				}
			}
		}

		public DataItem Add()
		{
			DataItem newItem = new DataItem(_document, _document.CreateNewItem());			
			_dictionary.Add(newItem.ID, newItem);
			return newItem;
		}
	}
}

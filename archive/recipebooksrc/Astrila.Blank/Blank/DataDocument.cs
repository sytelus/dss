using System;
using System.Data;
using Astrila.Common;

namespace Astrila.Blank
{
	public class DataDocument
	{
	
		private DataAccess _localDatabase = null;
		private string _globalDatasetGuid = null;
		DataRow _datasetInfoRow = null;
		string _currentAuthor = "$unknown$";
//		string _itemType;
//		string[] _propertyNames;
//		string[] _propertyValues;
		private DataItemCollection _items = null;

		private DictionaryList _itemSchemas;

		public DataDocument(DataAccess localDatabase, string newDatasetName, string newDatasetItemSchema, string datasetTitle, string currentEditor)
		{
			string globalDatasetGuid = newDatasetName;

			//add dataset info row
			DataSet documentDataSet = new DataSet();
			localDatabase.GetTableData("Dataset", documentDataSet, new string[]{"GlobalGuid"}, new object[]{string.Empty}, "Dataset");
			DataTable datasetInfoTable = documentDataSet.Tables["Dataset"];
			DataRow datasetInfoRow;
			if (datasetInfoTable.Rows.Count == 0)
			{
				//Load empty SyncMark table
				localDatabase.GetTableData("sql: SELECT * FROM SyncMark WHERE 1=2", documentDataSet, "SyncMark");
				datasetInfoRow = CreateDatasetInfo(datasetInfoTable, globalDatasetGuid, datasetTitle, newDatasetItemSchema, documentDataSet.Tables["SyncMark"]);
			}
			else
				throw new Exception(string.Format("New Dataset can not be created because one with newly generated global guid {0} already exist", _globalDatasetGuid));

			ConstructorCommon(localDatabase, documentDataSet, datasetInfoRow, currentEditor);

			//Load blank document
			this.Load("1=2");
		}

		public DataDocument(DataAccess localDatabase, string globalDatasetGuid, string currentEditor)
		{
			DataSet documentDataSet = new DataSet();
			DataRow datasetInfoRow;

			//retrieve and varify dataset info row
			localDatabase.GetTableData("Dataset", documentDataSet, new string[]{"GlobalGuid"}, new object[]{globalDatasetGuid}, "Dataset");
			DataTable datasetInfoTable = documentDataSet.Tables["Dataset"];
			if (datasetInfoTable.Rows.Count == 0)
				throw new Exception(string.Format("Requested dataset with global GUID {0} does not exist in local database", globalDatasetGuid));
			else if (datasetInfoTable.Rows.Count > 1)
				throw new Exception(string.Format("Database may be currupted: Multiple rows in Dataset table found for same global guid {0}", globalDatasetGuid));
			else
				datasetInfoRow = datasetInfoTable.Rows[0];

			ConstructorCommon(localDatabase, documentDataSet, datasetInfoRow, currentEditor);
		}

		internal DataDocument(DataAccess localDatabase, DataSet documentDataSet, DataRow datasetInfoRow, string currentEditor)
		{
			ConstructorCommon(localDatabase, documentDataSet, datasetInfoRow, currentEditor);
		}

		private void ConstructorCommon(DataAccess localDatabase, DataSet documentDataSet, DataRow datasetInfoRow, 
			string currentEditor)
		{
			_localDatabase = localDatabase;
			_datasetInfoRow = datasetInfoRow;
			_itemSchemas = new DictionaryList();
			_globalDatasetGuid = datasetInfoRow["GlobalGuid"].ToString();
			_currentAuthor = currentEditor;
			_documentDataSet = documentDataSet;


			string endPointGuid = datasetInfoRow["EndPointGuid"].ToString();
			if (!_documentDataSet.Tables.Contains("SyncMark"))
			{
				localDatabase.GetTableData("SyncMark", _documentDataSet, new string[]{"EndPointGuid"}, new object[]{endPointGuid}, "SyncMark");
			}
			else {}; //it has already been created

			string itemSchema = _datasetInfoRow["ItemSchema"].ToString();
			localDatabase.GetTableData("ItemType", _documentDataSet, new string[]{"ItemSchemaID"}, new object[]{itemSchema}, "ItemType");
			localDatabase.GetTableData("ItemPropertyType", _documentDataSet, new string[]{"itemSchemaID"}, new object[]{itemSchema}, "ItemPropertyType");

			DataTable itemTypeTable = _documentDataSet.Tables["ItemType"];
			for (int itemTypeIndex = 0; itemTypeIndex < itemTypeTable.Rows.Count; itemTypeIndex++)
			{
				string itemType = itemTypeTable.Rows[itemTypeIndex]["Name"].ToString();

				DataRow[] defaultPropertyTypeRows = _documentDataSet.Tables["ItemPropertyType"].Select(string.Format("ItemTypeName = '{0}'", itemType));
				string[] propertyNames = new string[defaultPropertyTypeRows.Length];
				string[] defaultPropertyValues = new string[defaultPropertyTypeRows.Length];
				for (int propertyNameIndex = 0; propertyNameIndex < defaultPropertyTypeRows.Length; propertyNameIndex++)
				{
					propertyNames[propertyNameIndex] = defaultPropertyTypeRows[propertyNameIndex]["Name"].ToString();
					defaultPropertyValues[propertyNameIndex] = defaultPropertyTypeRows[propertyNameIndex]["DefaultValue"].ToString();
				}
			
				AddItemSchema(itemType, propertyNames, defaultPropertyValues);
			}
		}

		internal static DataRow CreateDatasetInfo(DataTable datasetInfoTable, string datasetGlobalGuid, string datasetTitle, string datasetItemSchema, DataTable syncMarkDataTable)
		{
			string localEndpointGuid;
			DataRow localDatasetInfoRow;
			DataRow newDatasetInfoRow = datasetInfoTable.NewRow();
			localEndpointGuid = Guid.NewGuid().ToString();
			newDatasetInfoRow["EndPointGuid"] = localEndpointGuid;
			newDatasetInfoRow["GlobalGuid"] = datasetGlobalGuid;
			newDatasetInfoRow["Name"] = datasetTitle;
			newDatasetInfoRow["ItemSchema"] = datasetItemSchema;
			datasetInfoTable.Rows.Add(newDatasetInfoRow);
			localDatasetInfoRow = newDatasetInfoRow;
	
			//Add a sync mark row in SyncMark table
			DataRow newSyncMarkRow = syncMarkDataTable.NewRow();
			newSyncMarkRow["EndPointGuid"] = localEndpointGuid;
			newSyncMarkRow["Mark"] = 0;
			syncMarkDataTable.Rows.Add(newSyncMarkRow);
			return newDatasetInfoRow;
		}


		public void AddItemSchema(string itemType, string[] propertyNames, string[] propertyValues)
		{
			_itemSchemas.Add(itemType, new object[]{itemType, propertyNames, propertyValues});
		}

		private DataSet _documentDataSet = null;
		public void Load(string criteria)
		{
			Load(new string[]{this.DefaultItemType}, criteria, int.MaxValue);
		}
		public void Load(string itemType, string criteria)
		{
			Load(new string[]{itemType}, criteria, int.MaxValue);
		}
		public void Load(string criteria, int limitRowCountTo)
		{
			Load(new string[] {this.DefaultItemType}, criteria, int.MaxValue);
		}
		public void Load(string[] itemTypes, string criteria, int limitRowCountTo)
		{
			LoadDataPrivate(itemTypes, criteria, limitRowCountTo);
		}
		public void CreateNew()
		{
			
		}

		public DataItemCollection Items
		{
			get
			{
				return _items;
			}
		}

		public string CurrentAuthor
		{
			get
			{
				return _currentAuthor;
			}
		}

		private string[] _lastLoadItemTypes = null;
        private string _lastLoadCriteria = null;
        private int _lastLoadLimitRowCount = int.MaxValue;
		private void LoadDataPrivate(string[] itemTypes, string criteria, int limitRowCountTo)
		{
			_lastLoadItemTypes = itemTypes;
			_lastLoadCriteria = criteria;
			_lastLoadLimitRowCount = limitRowCountTo;

			string innerSql = "SELECT ";

			if (limitRowCountTo != int.MaxValue)
				innerSql += "TOP " + limitRowCountTo.ToString();

			innerSql += " DISTINCT ItemGuid FROM ItemPropertyValue";

			string whereClause = string.Empty;
			if ((criteria != null) && (criteria.Length > 0))
			{
				if (whereClause.Length > 0)
					whereClause += " AND ";

				whereClause += "(" + criteria + ") ";
			}

			if ((itemTypes != null) && (itemTypes.Length > 0))
			{
				string itemTypeFilter = string.Empty;
				for (int itemTypeIndex = 0; itemTypeIndex < itemTypes.Length; itemTypeIndex++)
				{
					if (itemTypes[itemTypeIndex].Length > 0)
					{
						if (itemTypeFilter.Length > 0)
							itemTypeFilter += " OR ";

						itemTypeFilter += string.Format("(ItemType = '{0}')", itemTypes[itemTypeIndex]);
					}
				}

				if (whereClause.Length > 0) 
					whereClause += " AND ";

				if (itemTypeFilter.Length > 0)
					whereClause += "(" + itemTypeFilter + ")";
			}

			string itemSql;
			string itemPropertySql;
			string itemPropertyValueSql;

			if ((whereClause.Length > 0) || (limitRowCountTo != int.MaxValue))
			{
				innerSql += " WHERE " + whereClause;

				itemSql = "SELECT * FROM Item WHERE [Guid] IN (" + innerSql + ") ORDER BY [When] DESC";
				itemPropertySql = "SELECT * FROM ItemProperty WHERE ItemGuid IN (" + innerSql + ")";
				itemPropertyValueSql = "SELECT * FROM ItemPropertyValue WHERE ItemGuid IN (" + innerSql + ")";
			}
			else	//we intend to get all rows!
			{
				itemSql = "SELECT * FROM Item";
				itemPropertySql = "SELECT * FROM ItemProperty";
				itemPropertyValueSql = "SELECT * FROM ItemPropertyValue";
			}

			//Remove existing tables
			if (_documentDataSet.Tables.Contains("ItemPropertyValue"))
				_documentDataSet.Tables.Remove("ItemPropertyValue");
			if (_documentDataSet.Tables.Contains("ItemProperty"))
				_documentDataSet.Tables.Remove("ItemProperty");
			if (_documentDataSet.Tables.Contains("Item"))
				_documentDataSet.Tables.Remove("Item");

			//Get the data
			_localDatabase.GetTableData("sql:" + itemPropertyValueSql, _documentDataSet, "ItemPropertyValue");
			_localDatabase.GetTableData("sql:" + itemPropertySql, _documentDataSet, "ItemProperty");
			_localDatabase.GetTableData("sql:" + itemSql, _documentDataSet, "Item");

			//This might actually reduce the performance while loading lots of rows if client doesn't access them always by guid
			//_documentDataSet.Tables["Item"].PrimaryKey = new DataColumn[] {_documentDataSet.Tables["Item"].Columns["Guid"]};

			_items = new DataItemCollection(this);
		}

		internal DataRow CreateNewItem()
		{
			return CreateNewItem(this.DefaultItemType);
		}

		internal string DefaultItemType
		{
			get
			{
				return (string) ((object[]) _itemSchemas[0])[0];
			}
		}
		internal string[] GetDefaultPropertyNames(string itemType)
		{
			return (string[]) ((object[]) _itemSchemas[itemType])[1];
		}
		internal string[] GetDefaultPropertyValues(string itemType)
		{
			return (string[]) ((object[]) _itemSchemas[itemType])[2];
		}

		internal DataRow CreateNewItem(string itemType)
		{
			return CreateNewItemInternal(_documentDataSet, _datasetInfoRow["EndPointGuid"].ToString(), _currentAuthor, itemType, this.GetDefaultPropertyNames(itemType), this.GetDefaultPropertyValues(itemType), true);
		}

		internal static DataRow CreateNewItemInternal(DataSet dataSetToUse, string endPointGuid, string editedBy, string itemType, string[] propertyNames, string[] propertyValues, bool addDeletedProperty)
		{
			//TODO: refactor following fragments in to methods and reuse in SyncEngine
			string newItemGuid = Guid.NewGuid().ToString();
			DataRow newItemRow = dataSetToUse.Tables["Item"].NewRow();
			newItemRow["Guid"] = newItemGuid;
			newItemRow["EndPointGuid"] = endPointGuid;
			newItemRow["Type"] = itemType;
			newItemRow["When"] = DateTime.UtcNow;
			dataSetToUse.Tables["Item"].Rows.Add(newItemRow);

			//Add property rows
			for (int propertyNameIndex = 0; propertyNameIndex < propertyNames.Length; propertyNameIndex++)
			{
				AddNewPropertyRow(dataSetToUse, newItemGuid, propertyNames[propertyNameIndex], editedBy, editedBy);
				AddNewPropertyValueRow(dataSetToUse, newItemGuid, itemType, propertyNames[propertyNameIndex], propertyValues[propertyNameIndex], editedBy, true, false, DateTime.MinValue, null);
			}

			if (addDeletedProperty)
			{
				AddNewPropertyRow(dataSetToUse, newItemGuid, "_deleted_", editedBy, editedBy);
				AddNewPropertyValueRow(dataSetToUse, newItemGuid, itemType, "_deleted_", "false", editedBy, true, false, DateTime.MinValue, null);
			}

			return newItemRow;
		}

		internal DataRow GetItemRow(int index)
		{
			return _documentDataSet.Tables["Item"].Rows[index];
		}
		
		internal static void AddNewPropertyRow(DataSet dataSetToUse, string itemGuid, string propertyName, string editedBy, string currentValueBy)
		{
			DataRow newPropertyRow = dataSetToUse.Tables["ItemProperty"].NewRow();
			newPropertyRow["ItemGuid"] = itemGuid;
			newPropertyRow["Name"] = propertyName;
			newPropertyRow["CurrentValueBy"] = currentValueBy;
			newPropertyRow["By"] = editedBy;
			newPropertyRow["When"] = DateTime.UtcNow;
			newPropertyRow["Revision"] = 1;
			dataSetToUse.Tables["ItemProperty"].Rows.Add(newPropertyRow);
		}

		internal static DataRow AddNewPropertyValueRow(DataSet dataSetToUse, string itemGuid, string itemType, string propertyName, 
			string propertyValue, string editedBy, bool setItToCurrent, bool deleted, DateTime resolvedWhen, string conflictResolutionReason)
		{
			DataRow newPropertyValueRow = dataSetToUse.Tables["ItemPropertyValue"].NewRow();
			newPropertyValueRow["ItemGuid"] = itemGuid;
			newPropertyValueRow["PropertyName"] = propertyName;
			newPropertyValueRow["By"] = editedBy;
			newPropertyValueRow["Revision"] = 1;
			newPropertyValueRow["When"] = DateTime.UtcNow;
			newPropertyValueRow["Value"] = propertyValue;
			newPropertyValueRow["Deleted"] = deleted;
			newPropertyValueRow["ItemType"] = itemType;	//redundant immutable field
			newPropertyValueRow["Current"] = setItToCurrent;	//redundant field
			newPropertyValueRow["ResolvedWhen"] = resolvedWhen;
			newPropertyValueRow["ResolvedReason"] = conflictResolutionReason;
			dataSetToUse.Tables["ItemPropertyValue"].Rows.Add(newPropertyValueRow);

			return newPropertyValueRow;
		}

		internal DataRow AddNewPropertyValueRow(string itemGuid, string itemType, string propertyName, string propertyValue, string editedBy, bool setItToCurrent, bool deleted, DateTime resolvedWhen, string conflictResolutionReason)
		{
			return AddNewPropertyValueRow(_documentDataSet, itemGuid, itemType, propertyName, propertyValue, this.CurrentAuthor, setItToCurrent, 
				deleted, resolvedWhen, conflictResolutionReason);
		}

		internal DataRow[] GetItemPropertyRows(string itemGuid)
		{
			return _documentDataSet.Tables["ItemProperty"].Select(string.Format("ItemGuid = '{0}'", itemGuid));
		}

		internal DataRow GetItemPropertyRow(string itemGuid, string propertyName, bool ignoreIfDoesntExist)
		{
			DataRow[] propertyRows = _documentDataSet.Tables["ItemProperty"].Select(string.Format("ItemGuid = '{0}' AND Name = '{1}'", itemGuid, propertyName));
			if (propertyRows.Length == 0)
			{
				if (!ignoreIfDoesntExist)
					throw new Exception(string.Format("Can not find Item Property for item Guid {0} and property name {1}", itemGuid, propertyName));			
				else
				{
					return null;
				}; //ignore if it doesn't exist
			}
			else
			{
				return propertyRows[0];
			}
		}

		internal DataRow[] GetItemPropertyValueRows(string itemGuid, string propertyName)
		{
			return _documentDataSet.Tables["ItemPropertyValue"].Select(string.Format("ItemGuid = '{0}' AND PropertyName = '{1}'", itemGuid, propertyName));
		}

		internal DataRow GetItemPropertyValueRow(string itemGuid, string propertyName, string editedBy, bool ignoreIfDoesntExist)
		{
			DataRow[] propertyValueRows = _documentDataSet.Tables["ItemPropertyValue"].Select(string.Format("ItemGuid = '{0}' AND PropertyName = '{1}' AND By = '{2}'", itemGuid, propertyName, _currentAuthor));
			if (propertyValueRows.Length == 0)
			{
				if (!ignoreIfDoesntExist)
					throw new Exception(string.Format("Can not find Item Property for item Guid {0} and property name {1} and Edited By {2}", itemGuid, propertyName, editedBy));			
				else
				{
					return null;
				}; //ignore if it doesn't exist
			}
			else if (propertyValueRows.Length == 1)
			{
				return propertyValueRows[0];
			}
			else
				throw new Exception(string.Format("Database may be currupted: Multiple rows in Dataset table found for same ItemGuid {0} and property name {1} and Edited By {2}", itemGuid, propertyName, editedBy));			
		}

		internal DataRow GetItemRow(string itemGuid, bool ignoreIfDoesntExist)
		{
			DataRow[] itemRows = _documentDataSet.Tables["Item"].Select(string.Format("Guid = '{0}'", itemGuid));
			if (itemRows.Length > 1)
				throw new Exception(string.Format("Local database may be currupted: Multiple rows in Item table found for same guid {0}", itemGuid));
			else if (itemRows.Length == 1)
				return itemRows[0];
			else
			{
				if (!ignoreIfDoesntExist)
					throw new Exception(string.Format("Can not find Item item Guid {0}", itemGuid));			
				else
				{
					return null;
				}; //ignore if it doesn't exist
			}
		}

		internal DataItemProperty GetItemProperty(string itemGuid, string propertyName)
		{
			return this.Items[itemGuid].Properties[propertyName];
		}


		internal void UpdateLastModifiedOnForItem(string itemGuid)
		{
			this.Items[itemGuid].UpdateLastModifiedOn();
		}


		internal int GetItemCount()
		{
			return _documentDataSet.Tables["Item"].Rows.Count;
		}

		public void Save()
		{
			_localDatabase.UpdateTableData(_documentDataSet, new string[] {"Dataset", "SyncMark", "Item", "ItemProperty", "ItemPropertyValue"});
			//TODO: add sync mark
		}

		public string GlobalGuid
		{
			get { return _globalDatasetGuid; }
		}

		public void Refresh()
		{
			LoadDataPrivate(_lastLoadItemTypes, _lastLoadCriteria, _lastLoadLimitRowCount);
		}
	}
}

//TODO: support optional item properties. Item can have 100 properties but only few may actually exist.
//TODO: support multiple default item schemas
//TODO: Use Find instead of Table.Select
//TODO: delete Name field from dataset
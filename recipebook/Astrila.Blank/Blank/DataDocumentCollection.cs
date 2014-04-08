using System;
using System.Collections;
using System.Data;
using Astrila.Common;

namespace Astrila.Blank
{
	/// <summary>
	/// Summary description for DataDocumentCollection.
	/// </summary>
	public class DataDocumentCollection
	{
		private DictionaryList _documents;
		private DataAccess _localDatabase;
		private string _currentEditor;
		public DataDocumentCollection(DataAccess localDatabase, string currentEditor)
		{
			DataSet localDataSet = new DataSet();
			localDatabase.GetTableData("Dataset", localDataSet);
			DataTable datasetTable = localDataSet.Tables["Dataset"];

			_localDatabase = localDatabase;
			_currentEditor = currentEditor;
			_documents = new DictionaryList();

			for (int datasetRowIndex = 0; datasetRowIndex < datasetTable.Rows.Count; datasetRowIndex++)
			{
				DataDocument document = new DataDocument(localDatabase, localDataSet, datasetTable.Rows[datasetRowIndex], currentEditor);
				_documents.Add(document.GlobalGuid, document);
			}
		}

		public DataDocument Add(string datasetGlobalGuid, string itemSchema, string datasetTitle)
		{
			DataDocument document = new DataDocument(_localDatabase, datasetGlobalGuid, itemSchema, datasetTitle, _currentEditor);
			_documents.Add(document.GlobalGuid, document);

			return document;
		}

		public int Count
		{
			get
			{
				return _documents.Count;
			}
		}

		public DataDocument this[int index]
		{
			get
			{
				return (DataDocument)_documents[index];
			}
		}

		public DataDocument this[string globalGuid]
		{
			get
			{
				return (DataDocument)_documents[globalGuid];
			}
		}
	}
}

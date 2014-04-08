using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.IO;


namespace Astrila.Common
{
	/// <summary>
	/// Summary description for AppData.
	/// </summary>
	public class DataAccess
	{
		private string m_ConectionString = null;

		public DataAccess(string connectionString)
		{
			m_ConectionString =	connectionString;
		}

		public static string BuildConnectionStringForAccess(string mdbFilePath)
		{
			return UtilityFunctions.GetConnectionStringForAccess(mdbFilePath);
		}

		public DataSet GetTableData(string tableName, string dataSetTableName )
		{
			return GetTableData(tableName, null, null, null, dataSetTableName);
		}
		public DataSet GetTableData(string tableName)
		{
			return GetTableData(tableName, null, null, null,tableName);
		}
		public DataSet GetTableData(string tableName, DataSet dataSetToFill, string dataSetTableName)
		{
			return GetTableData(tableName, dataSetToFill, null, null,dataSetTableName);
		}
		public DataSet GetTableData(string tableName, DataSet dataSetToFill)
		{
			return GetTableData(tableName, dataSetToFill, null, null,tableName);
		}
		public DataSet GetTableData(string tableName, string[] columnNames, object[] columnValues)
		{
			return GetTableData(tableName, null, columnNames, columnValues,tableName);
		}
		public DataSet GetTableData(string tableName, DataSet dataSetToFill, string[] columnNames, object[] columnValues, string dataSetTableName)
		{
			DataSet dataSetToUse = dataSetToFill;
			if (dataSetToUse == null) dataSetToUse = new DataSet();
			OleDbDataAdapter adapter = GetAdapterForSelect(tableName, columnNames, columnValues);
			adapter.Fill(dataSetToUse, dataSetTableName);

			return dataSetToUse;
		}

		public OleDbConnection GetNewConnection()
		{
			return new OleDbConnection(m_ConectionString);
		}

		public void UpdateTableData(DataSet paramDataSet, string[] tableNames)
		{
			OleDbConnection connection = GetNewConnection();
			OleDbTransaction transaction = null; 
			try
			{
				connection.Open();
				transaction = connection.BeginTransaction();
				foreach(string tableName in tableNames)
				{
					OleDbDataAdapter adapter = GetAdapterForSelect(tableName, null, null, connection);
					adapter.SelectCommand.Transaction = transaction;
					using (OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(adapter))
					{
						commandBuilder.QuotePrefix = "[";
						commandBuilder.QuoteSuffix = "]";
						adapter.Update(paramDataSet, tableName);
					}
				}
				transaction.Commit();				
			}
			catch
			{
				transaction.Rollback();
				throw;
			}
			finally
			{
				connection.Close();
				connection.Dispose();
			}
		}

		public void UpdateTableData(DataSet paramDataSet, string tableName)
		{
			OleDbDataAdapter adapter = GetAdapterForSelect(tableName, null, null);
			using (OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(adapter))
			{
				adapter.Update(paramDataSet, tableName);
			}
		}

		public OleDbDataAdapter GetAdapterForSelect(string tableName, string[] columnNames, object[] columnValues)
		{
			return GetAdapterForSelect(tableName, columnNames, columnValues, null);
		}

		public OleDbDataAdapter GetAdapterForSelect(string tableName, string[] columnNames, object[] columnValues, OleDbConnection connection)
		{
			OleDbCommand selectCommand = new OleDbCommand();
			string selectCommandText;
			bool isCustomSql;
			if (tableName.StartsWith("sql:") )
			{
				selectCommandText = tableName.Substring(4);
				isCustomSql = true;
			}
			else
			{
				isCustomSql = false;
				selectCommandText = "SELECT * FROM " + tableName;

				if (columnNames != null)
				{
					//build WHERE clause
					string whereClause = "";
					for(int parameterIndex =0;parameterIndex < columnNames.Length;parameterIndex++)
					{
						if (whereClause != "") whereClause += " AND ";
						whereClause += "(" + columnNames[parameterIndex] + "=" + "@" + columnNames[parameterIndex] + ")";
					}
					if (whereClause != "") 	selectCommandText += " WHERE " + whereClause;
				}
				else {}; //do not add WHERE clause
			}

			if (columnNames != null)
			{
				//Build parameters
				for(int parameterIndex =0;parameterIndex < columnNames.Length;parameterIndex++)
				{
					OleDbParameter newParam = selectCommand.Parameters.Add("@" + columnNames[parameterIndex], null);
					if (!isCustomSql)
					{
						newParam.SourceColumn = columnNames[parameterIndex];
					}
					newParam.Value = columnValues[parameterIndex];
				}
				
			}
			selectCommand.CommandText = selectCommandText;

			if (connection == null)
				selectCommand.Connection = GetNewConnection();
			else
				selectCommand.Connection = connection;

			return new OleDbDataAdapter(selectCommand);
		}
	}
}

using System;
using System.Xml;
using Astrila.Common;
using System.Data;
using System.Text;
using System.Globalization;

namespace Astrila.Blank
{

	public class FeedWriter
	{
		public static void WriteFeed(XmlDocument feedDocument, Common.DataAccess localDatabase, string feedTitle, string datasetGlobalGuid, DateTime itemsUpdatedAfter, int maxRowCount)
		{
			DataSet localDataset = new DataSet();

			XmlNode dssNode = CreateDssElement(feedDocument, feedTitle);	//TODO: add upto and since attributes

			//Get the dataset info row
			localDatabase.GetTableData("Dataset", localDataset, new string[]{"GlobalGuid"}, new object[]{datasetGlobalGuid}, "Dataset");
			DataTable datasetInfoTable = localDataset.Tables["Dataset"];
			DataRow datasetInfoRow = null;
			if (datasetInfoTable.Rows.Count == 0)
			{}	//no dataset nodes to write
			else if (datasetInfoTable.Rows.Count > 1)
				throw new Exception(string.Format("Local database may be currupted: Multiple rows in Dataset table found for same global guid {0}", datasetGlobalGuid));
			else
			{
				DateTime itemUpdatedSince = DateTime.MaxValue;
				DateTime itemUpdatedUpto = DateTime.MinValue;

				datasetInfoRow = datasetInfoTable.Rows[0];
				XmlNode datasetNode = CreateDatasetElement(feedDocument, dssNode, datasetInfoRow);	//TODO: add upto and since attributes

				string itemGuidsToPutInFeedQuery = "sql:SELECT TOP 10 * FROM Item WHERE (EndPointGuid = '"+ datasetInfoRow["EndPointGuid"].ToString() +"') AND ([When] < #" + itemsUpdatedAfter.ToString("MM/dd/yyyy hh:mm:ss") + "#) ORDER BY [When] DESC";
				localDatabase.GetTableData(itemGuidsToPutInFeedQuery, localDataset, "Item");
				DataTable itemTable = localDataset.Tables["Item"];

				//Build list of item guids
				StringBuilder itemGuidListBuilder = new StringBuilder();
				for(int itemRowIndex = 0; itemRowIndex < itemTable.Rows.Count; itemRowIndex++)
				{
					if (itemGuidListBuilder.Length > 0)
						itemGuidListBuilder.Append(", ");

					string itemGuid = itemTable.Rows[itemRowIndex]["Guid"].ToString();
					if (itemGuid.Length > 0)
					{
						itemGuidListBuilder.Append("'" + itemGuid + "'");						
					}
				}
				string itemGuidList = itemGuidListBuilder.ToString();

				if (itemGuidList.Length > 0)
				{
					itemGuidList = "(" + itemGuidList + ")";
					localDatabase.GetTableData("sql:select * from ItemProperty where ItemGuid in " + itemGuidList, localDataset, "ItemProperty");
					localDatabase.GetTableData("sql:select * from ItemPropertyValue where ItemGuid in " + itemGuidList, localDataset, "ItemPropertyValue");

					//Append item elements
					//TODO: Avoid passing sync marks in a row
					for (int itemRowIndex = 0; itemRowIndex < itemTable.Rows.Count; itemRowIndex++)
					{
						DataRow itemRow = itemTable.Rows[itemRowIndex];
						string itemGuid = itemRow["Guid"].ToString();
						XmlNode itemNode = CreateItemElement(datasetNode, itemRow);
					
						if (itemUpdatedUpto < (DateTime)itemRow["When"])
							itemUpdatedUpto = (DateTime)itemRow["When"];

						if (itemUpdatedSince > (DateTime)itemRow["When"])
							itemUpdatedSince = (DateTime)itemRow["When"];

						DataRow[] propertyRows = localDataset.Tables["ItemProperty"].Select(string.Format("ItemGuid = '{0}'", itemGuid));
						for (int propertyRowIndex = 0; propertyRowIndex < propertyRows.Length; propertyRowIndex++)
						{
							DataRow propertyRow = propertyRows[propertyRowIndex];
							string propertyName = propertyRow["Name"].ToString();
							XmlNode propertyNode = CreatePropertyElement(itemNode, propertyRow);

							DataRow[] propertyValueRows = localDataset.Tables["ItemPropertyValue"].Select(string.Format("ItemGuid = '{0}' AND PropertyName = '{1}'", itemGuid, propertyName));
							for (int propertyValueRowIndex = 0; propertyValueRowIndex < propertyValueRows.Length; propertyValueRowIndex++)
							{
								CreatePropertyValueElement(propertyNode, propertyValueRows[propertyValueRowIndex]);
							}
						}
					}
				}
				else {} //there are zero items to sync

				//since, upto attributes
				if (itemUpdatedSince < DateTime.MaxValue)
				{
					XmlAttribute sinceAttribute = feedDocument.CreateAttribute("since");
					sinceAttribute.Value = XmlConvert.ToString(itemUpdatedSince);
					datasetNode.Attributes.Append(sinceAttribute);
				}
				else {} //omit this attribute

				if (itemUpdatedUpto > DateTime.MinValue)
				{
					XmlAttribute uptoAttribute = feedDocument.CreateAttribute("upto");
					uptoAttribute.Value = XmlConvert.ToString(itemUpdatedUpto);
					datasetNode.Attributes.Append(uptoAttribute);
				}
				else {} //omit this attribute

				//getData, putData attributes
				if (datasetInfoRow.Table.Columns.Contains("GetDataUrl"))
				{
					if (datasetInfoRow["GetDataUrl"].ToString().Length > 0)
					{
						XmlElement relatedGetDataElement = feedDocument.CreateElement("related");

						XmlAttribute getDataLinkAttribute = feedDocument.CreateAttribute("link");
						getDataLinkAttribute.Value = datasetInfoRow["GetDataUrl"].ToString();
						relatedGetDataElement.Attributes.Append(getDataLinkAttribute);

						XmlAttribute getDataTypeAttribute = feedDocument.CreateAttribute("type");
						getDataTypeAttribute.Value = "getData";
						relatedGetDataElement.Attributes.Append(getDataTypeAttribute);

						datasetNode.AppendChild(relatedGetDataElement);
					}
				}
				if (datasetInfoRow.Table.Columns.Contains("PutDataUrl"))
				{
					if (datasetInfoRow["PutDataUrl"].ToString().Length > 0)
					{
						XmlElement relatedPutDataElement = feedDocument.CreateElement("related");

						XmlAttribute putDataLinkAttribute = feedDocument.CreateAttribute("link");
						putDataLinkAttribute.Value = datasetInfoRow["PutDataUrl"].ToString();
						relatedPutDataElement.Attributes.Append(putDataLinkAttribute);

						XmlAttribute putDataTypeAttribute = feedDocument.CreateAttribute("type");
						putDataTypeAttribute.Value = "putData";
						relatedPutDataElement.Attributes.Append(putDataTypeAttribute);

						datasetNode.AppendChild(relatedPutDataElement);
					}
				}

				//Get the sync mark values
				localDatabase.GetTableData("SyncMark", localDataset);
				DataTable syncMarkDataTable = localDataset.Tables["SyncMark"];
				for (int syncMarkRowIndex = 0; syncMarkRowIndex < syncMarkDataTable.Rows.Count; syncMarkRowIndex++)
				{
					CreateSyncMarkElement(datasetNode, syncMarkDataTable.Rows[syncMarkRowIndex]);
				}
			}
		}

		const string DssVersion = "0.7";
		private static XmlNode CreateDssElement(XmlDocument feedDocument, string title)
		{
			//Add element and attributes for dataset
			XmlNode dssNode = feedDocument.CreateElement("dss");
			
			//version attribute
			XmlAttribute versionAttribute = feedDocument.CreateAttribute("version");
			versionAttribute.Value = DssVersion;
			dssNode.Attributes.Append(versionAttribute);

			//friendlyName attribute
			XmlAttribute titleAttribute = feedDocument.CreateAttribute("title");
			titleAttribute.Value = title;
			dssNode.Attributes.Append(titleAttribute);

			feedDocument.AppendChild(dssNode);

			return dssNode;
		}

		private static XmlNode CreateDatasetElement(XmlDocument feedDocument, XmlNode dssNode, DataRow datasetInfoRow)
		{
			//Add element and attributes for dataset
			XmlNode datasetNode = feedDocument.CreateElement("dataset");
			//globalGuild attribute
			XmlAttribute globalGuidAttribute = feedDocument.CreateAttribute("globalGuid");
			globalGuidAttribute.Value = datasetInfoRow["GlobalGuid"].ToString();
			datasetNode.Attributes.Append(globalGuidAttribute);
			//localGuild attribute
			XmlAttribute endPointGuidAttribute = feedDocument.CreateAttribute("endpointGuid");
			endPointGuidAttribute.Value = datasetInfoRow["EndPointGuid"].ToString();
			datasetNode.Attributes.Append(endPointGuidAttribute);
			//title attribute
			XmlAttribute titleAttribute = feedDocument.CreateAttribute("title");
			titleAttribute.Value = datasetInfoRow["Name"].ToString();
			datasetNode.Attributes.Append(titleAttribute);
			//itemSchema attribute
			XmlAttribute itemSchemaAttribute = feedDocument.CreateAttribute("itemSchema");
			itemSchemaAttribute.Value = datasetInfoRow["ItemSchema"].ToString();
			datasetNode.Attributes.Append(itemSchemaAttribute);

			//Add node to document
			dssNode.AppendChild(datasetNode);

			return datasetNode;
		}


		private static XmlNode CreatePropertyElement(XmlNode itemNode, DataRow propertyRow)
		{
			XmlDocument feedDocument = itemNode.OwnerDocument;
			XmlNode propertyNode = feedDocument.CreateElement("property");

			//name attribute
			XmlAttribute nameAttribute = feedDocument.CreateAttribute("name");
			nameAttribute.Value = propertyRow["Name"].ToString();
			propertyNode.Attributes.Append(nameAttribute);

			//by attribute
			XmlAttribute byAttribute = feedDocument.CreateAttribute("by");
			byAttribute.Value = propertyRow["By"].ToString();
			propertyNode.Attributes.Append(byAttribute);

			//when attribute
			XmlAttribute whenAttribute = feedDocument.CreateAttribute("when");
			whenAttribute.Value = XmlConvert.ToString((DateTime) propertyRow["When"]);
			propertyNode.Attributes.Append(whenAttribute);

			//currentValueBy attribute
			XmlAttribute currentValueByAttribute = feedDocument.CreateAttribute("currentValueBy");
			currentValueByAttribute.Value = propertyRow["CurrentValueBy"].ToString();
			propertyNode.Attributes.Append(currentValueByAttribute);

			//revision attribute
			XmlAttribute revisionAttribute = feedDocument.CreateAttribute("revision");
			revisionAttribute.Value = propertyRow["Revision"].ToString();
			propertyNode.Attributes.Append(revisionAttribute);

			itemNode.AppendChild(propertyNode);
			return propertyNode;
		}

		private static XmlNode CreateItemElement(XmlNode datasetNode, DataRow itemRow)
		{
			XmlDocument feedDocument = datasetNode.OwnerDocument;
			XmlNode itemNode = feedDocument.CreateElement("item");

			//type attribute
			XmlAttribute typeAttribute = feedDocument.CreateAttribute("type");
			typeAttribute.Value = itemRow["Type"].ToString();
			itemNode.Attributes.Append(typeAttribute);

			if (typeAttribute.Value != "$syncMark")
			{
				//guid attribute
				XmlAttribute guidAttribute = feedDocument.CreateAttribute("guid");
				guidAttribute.Value = itemRow["Guid"].ToString();
				itemNode.Attributes.Append(guidAttribute);
			}
			else {}; //No need to include GUID

			//when attribute
			XmlAttribute whenAttribute = feedDocument.CreateAttribute("when");
			whenAttribute.Value = XmlConvert.ToString((DateTime) itemRow["When"]);
			itemNode.Attributes.Append(whenAttribute);

			datasetNode.AppendChild(itemNode);
			return itemNode;
		}

		private static XmlNode CreateSyncMarkElement(XmlNode datasetNode, DataRow syncMarkRow)
		{
			XmlDocument feedDocument = datasetNode.OwnerDocument;
			XmlNode syncMarkNode = feedDocument.CreateElement("syncMark");

			//endpointGuid attribute
			XmlAttribute endpointGuidAttribute = feedDocument.CreateAttribute("endpointGuid");
			endpointGuidAttribute.Value = syncMarkRow["EndpointGuid"].ToString();
			syncMarkNode.Attributes.Append(endpointGuidAttribute);

			//mark attribute
			XmlAttribute markAttribute = feedDocument.CreateAttribute("mark");
			markAttribute.Value = syncMarkRow["Mark"].ToString();
			syncMarkNode.Attributes.Append(markAttribute);

			datasetNode.AppendChild(syncMarkNode);
			return syncMarkNode;
		}


		private static XmlNode CreatePropertyValueElement(XmlNode propertyNode, DataRow propertyValueRow)
		{
			XmlDocument feedDocument = propertyNode.OwnerDocument;
			XmlNode propertyValueNode = feedDocument.CreateElement("value");

			//by attribute
			XmlAttribute byAttribute = feedDocument.CreateAttribute("by");
			byAttribute.Value = propertyValueRow["By"].ToString();
			propertyValueNode.Attributes.Append(byAttribute);

			//when attribute
			XmlAttribute whenAttribute = feedDocument.CreateAttribute("when");
			whenAttribute.Value = XmlConvert.ToString((DateTime) propertyValueRow["When"]);
			propertyValueNode.Attributes.Append(whenAttribute);

			//deleted attribute
			if ((bool) propertyValueRow["Deleted"])
			{
				XmlAttribute deletedAttribute = feedDocument.CreateAttribute("deleted");
				deletedAttribute.Value = XmlConvert.ToString((bool) propertyValueRow["Deleted"]);
				propertyValueNode.Attributes.Append(deletedAttribute);
			}
			else{} //omit deleted attribute

			//revision attribute
			XmlAttribute revisionAttribute = feedDocument.CreateAttribute("revision");
			revisionAttribute.Value = propertyValueRow["Revision"].ToString();
			propertyValueNode.Attributes.Append(revisionAttribute);

			//resolvedWhen attribute
			if ((!System.Convert.IsDBNull(propertyValueRow["resolvedWhen"])) && (propertyValueRow["resolvedWhen"] != null) && ((DateTime) propertyValueRow["resolvedWhen"] > DateTime.MinValue))
			{
				XmlAttribute resolvedWhenAttribute = feedDocument.CreateAttribute("resolvedWhen");
				resolvedWhenAttribute.Value = XmlConvert.ToString((DateTime) propertyValueRow["resolvedWhen"]);
				propertyValueNode.Attributes.Append(resolvedWhenAttribute);
			}

			//resolvedReason attribute
			if ((!System.Convert.IsDBNull(propertyValueRow["resolvedReason"])) && (propertyValueRow["resolvedReason"] != null) && (propertyValueRow["resolvedReason"].ToString().Length > 0))
			{
				XmlAttribute resolvedReasonAttribute = feedDocument.CreateAttribute("resolvedReason");
				resolvedReasonAttribute.Value = propertyValueRow["resolvedReason"].ToString();
				propertyValueNode.Attributes.Append(resolvedReasonAttribute);
			}

			//Content of the node
			propertyValueNode.InnerText = propertyValueRow["Value"].ToString();

			propertyNode.AppendChild(propertyValueNode);
			return propertyValueNode;
		}
	}

}

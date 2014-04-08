using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Xml;
using Astrila.Common;

namespace Astrila.Blank
{
	public class SyncEngine
	{
		public delegate void NotifyProgressDelegate(int currentItemIndex, int itemCount, string message);

		public static DateTime Sync(Common.DataAccess localDatabase, XmlNode remoteDatasetNode)
		{
			int totalItemsUpdated; int totalItemsExamined;
			return Sync(localDatabase, remoteDatasetNode, out totalItemsUpdated, out totalItemsExamined, null);
		}

		public static DateTime Sync(Common.DataAccess localDatabase, XmlNode remoteDatasetNode, out int totalItemsUpdated, out int totalItemsExamined, NotifyProgressDelegate notifyProgress)
		{
			totalItemsUpdated = 0;
			totalItemsExamined = 0;
			if (remoteDatasetNode != null)
			{
				DateTime nextItemsShouldBeUpdatedAfter = DateTime.MinValue; //Return value: Indicates that no more pages are requested

				string remoteDatasetGlobalGuid = remoteDatasetNode.SelectSingleNode("./@globalGuid").Value;
				string remoteDatasetName = GetNullSafeAttributeValue(remoteDatasetNode.SelectSingleNode("./@title"), string.Empty);
				string remoteDatasetItemSchema = GetNullSafeAttributeValue(remoteDatasetNode.SelectSingleNode("./@itemSchema"), string.Empty);

				DataSet localDataset = new DataSet();

				//Get the sync mark values
				localDatabase.GetTableData("SyncMark", localDataset);
				DataTable localSyncMarkDataTable = localDataset.Tables["SyncMark"];

				//Look for local dataset row
				localDatabase.GetTableData(string.Format("sql:select * from Dataset where GlobalGuid = '{0}'", remoteDatasetGlobalGuid), localDataset, "Dataset");
				DataTable localDatasetInfoTable = localDataset.Tables["Dataset"];
				DataRow localDatasetInfoRow = null;
				string endpointGuidForLocalDataset = null;
				int localSyncMarkValueForRemoteEndPoint = 0;	//Default to 0, incase we haven't sync'd with this endpoint at all
				bool isSyncNeeded = false;

				if (localDatasetInfoTable.Rows.Count == 0)	//We do not have this dataset
				{
					//Let's create new dataset row
					DataRow datasetInfoRow = DataDocument.CreateDatasetInfo(localDatasetInfoTable, remoteDatasetGlobalGuid, remoteDatasetName, remoteDatasetItemSchema, localSyncMarkDataTable);
					endpointGuidForLocalDataset = datasetInfoRow["EndPointGuid"].ToString();

					isSyncNeeded = true;
					localSyncMarkValueForRemoteEndPoint = 0;
				}
				else if  (localDatasetInfoTable.Rows.Count > 1)
					throw new Exception(string.Format("Local database may be currupted: Multiple rows in Dataset table found for same global guid {0}", remoteDatasetGlobalGuid));
				else //localDatasetInfoTable.Rows.Count == 1
				{
					localDatasetInfoRow = localDatasetInfoTable.Rows[0];

					endpointGuidForLocalDataset = localDatasetInfoRow["EndPointGuid"].ToString();

					//Get the SyncMark of the endpoint
					string remoteEndPointGuid = remoteDatasetNode.SelectSingleNode("./@endpointGuid").Value.ToString();
					string remoteEndPointSyncMarkValue = GetNullSafeAttributeValue(remoteDatasetNode.SelectSingleNode(String.Format("./syncMark[@endpointGuid = '{0}']/@mark", remoteEndPointGuid)), string.Empty);

					if (remoteEndPointSyncMarkValue.Length > 0)
					{
						//Let's see if we have sync mark info for this end point
						DataRow[] localSyncMarkRowsForRemoteEndPoint = localSyncMarkDataTable.Select(string.Format("EndPointGuid = '{0}'", remoteEndPointGuid));
						if (localSyncMarkRowsForRemoteEndPoint.Length == 0)
						{
							localSyncMarkValueForRemoteEndPoint = 0;
							isSyncNeeded = true;	//We have never sync with this end point
						}
						else if (localSyncMarkRowsForRemoteEndPoint.Length > 1)
							throw new Exception(string.Format("Local database may be currupted: Multiple rows in SyncMark table found for same remote endpoint guid {0}", remoteEndPointGuid));
						else
						{
							localSyncMarkValueForRemoteEndPoint = (int) localSyncMarkRowsForRemoteEndPoint[0]["Mark"];
							isSyncNeeded = true;
						}
					}
					else	//Sync mark info for end point not available. Let's traverse whole data 
					{
						isSyncNeeded = true;
					};
				}	//end if localDatasetInfoRow check


				//From SyncMark comparison we now know if sync is needed or not
				if (isSyncNeeded == true)
				{
					//Get each item and push it to the local database until we run out of items or we find sync marker equal to our local sync mark
					//Let's fetch all the items from the local database which are in these feed
					XmlNodeList remoteItemNodes = remoteDatasetNode.SelectNodes("./item");
						
					StringBuilder itemGuidListBuilder = new StringBuilder();
					for(int remoteItemIndex = 0; remoteItemIndex < remoteItemNodes.Count; remoteItemIndex++)
					{
						if (itemGuidListBuilder.Length > 0)
							itemGuidListBuilder.Append(", ");

						string remoteItemGuid = GetNullSafeAttributeValue(remoteItemNodes[remoteItemIndex].Attributes, "guid", string.Empty);
						if (remoteItemGuid.Length > 0)
						{
							itemGuidListBuilder.Append("'" + remoteItemGuid + "'");						
						}
					}
					string itemGuidList = itemGuidListBuilder.ToString();

					if (itemGuidList.Length > 0)
					{
						itemGuidList = "(" + itemGuidList + ")";

						//Fire select query to get all the items and related stuff from our local database
						NotifyProgressToCaller(notifyProgress, 0, remoteItemNodes.Count, "Loading local data in memory...");
						localDatabase.GetTableData("sql:select * from Item where [Guid] in " + itemGuidList, localDataset, "Item");
						localDatabase.GetTableData("sql:select * from ItemProperty where ItemGuid in " + itemGuidList, localDataset, "ItemProperty");
						localDatabase.GetTableData("sql:select * from ItemPropertyValue where ItemGuid in " + itemGuidList, localDataset, "ItemPropertyValue");

						DataTable itemTable = localDataset.Tables["Item"];
						bool isAnythingUpdated = false;

						bool isSyncMarkFound = false;
						//Now walkthrough each item and sync it with local dataset
						for (int remoteItemIndex = 0; remoteItemIndex < remoteItemNodes.Count; remoteItemIndex++)
						{
							NotifyProgressToCaller(notifyProgress, remoteItemIndex, remoteItemNodes.Count, "Checking item #" + remoteItemIndex.ToString());
							XmlNode remoteItemNode = remoteItemNodes[remoteItemIndex];
								
							//If this is a sync marker then check its value
							if (GetNullSafeAttributeValue(remoteItemNode.Attributes,"type", string.Empty) == "$syncMark$")
							{
								if (int.Parse(remoteItemNode.SelectSingleNode("./property[@name='mark']/value").InnerText, CultureInfo.InvariantCulture) <= localSyncMarkValueForRemoteEndPoint)
								{
									//We need not process further items
									isSyncMarkFound = true;
									break;
								}
								else {}; //this is not the sync mark we are looking for
							}
							else //it's not a sync mark process this item
							{
								totalItemsExamined++;

								string itemGuid = GetNullSafeAttributeValue(remoteItemNode.Attributes,"guid", string.Empty);
								string itemType = GetNullSafeAttributeValue(remoteItemNode.Attributes, "type", string.Empty);

								//Look for local property row
								DataRow[] itemRows = itemTable.Select(string.Format("guid = '{0}'", itemGuid));
								DataRow itemRow = null;
								bool isItemRowUpdateRequired = false;

								if (itemRows.Length > 1)
									throw new Exception(string.Format("Local database may be currupted: Multiple rows in Item table found for same guid {0}", itemGuid));
								else if (itemRows.Length == 0)
								{
									//Let's add this item
									DataRow newItemRow = itemTable.NewRow();
									newItemRow["Guid"] = itemGuid;
									newItemRow["EndPointGuid"] = endpointGuidForLocalDataset;
									newItemRow["Type"] = itemType;
									//rest of the field will be updated further ahead in the code
									isItemRowUpdateRequired = true;
									itemTable.Rows.Add(newItemRow);
									itemRow = newItemRow;
								}
								else //there is exactly one item row
								{
									itemRow = itemRows[0];
								}

								//Now let's process each property of this item
								XmlNodeList remotePropertyNodes = remoteItemNode.SelectNodes("./property");
								for (int remotePropertyIndex = 0; remotePropertyIndex < remotePropertyNodes.Count; remotePropertyIndex++)
								{
									string propertyName = GetNullSafeAttributeValue(remotePropertyNodes[remotePropertyIndex].Attributes, "name", string.Empty);
									string remoteCurrentValueBy = GetNullSafeAttributeValue(remotePropertyNodes[remotePropertyIndex].Attributes, "currentValueBy", string.Empty);
									int remotePropertyRevision = XmlConvert.ToInt32(GetNullSafeAttributeValue(remotePropertyNodes[remotePropertyIndex].Attributes, "revision", "1"));
									DateTime remotePropertyWhen = XmlConvert.ToDateTime(GetNullSafeAttributeValue(remotePropertyNodes[remotePropertyIndex].Attributes, "when", XmlConvert.ToString(DateTime.MinValue)));
									string remotePropertyBy = GetNullSafeAttributeValue(remotePropertyNodes[remotePropertyIndex].Attributes, "by", string.Empty);
										
									//Look for local property row
									DataRow[] propertyRows = localDataset.Tables["ItemProperty"].Select(string.Format("ItemGuid = '{0}' AND Name = '{1}'", itemGuid, propertyName));
									DataRow propertyRow = null;
									bool isPropertyRowUpdateRequired = false;

									if (propertyRows.Length == 0)
									{
										//Let's add this property row
										DataRow newPropertyRow = localDataset.Tables["ItemProperty"].NewRow();
										newPropertyRow["ItemGuid"] = itemGuid;
										newPropertyRow["Name"] = propertyName;
										//rest of the fields would be copied further ahead in code
										isPropertyRowUpdateRequired = true;
										propertyRow = newPropertyRow;
										localDataset.Tables["ItemProperty"].Rows.Add(newPropertyRow);
									}
									else if  (propertyRows.Length == 1)
									{
										propertyRow = propertyRows[0];
													
										//Rule for winning when sync'ing current pointers:
										//Whichever with higher revision wins
										//	If revision are equal then highest When wins.
										//		If When are equal then highest By wins
										//		   If By are equal, then no changes required (i.e. [revision,when,by] vector is identical).
										//On property row, By is only informatial. When a user wants to change Current pointer then
										//	increment Revision and update When and By on property row
										if ((int)propertyRow["Revision"] <= remotePropertyRevision)
										{
											if ((int)propertyRow["Revision"] < remotePropertyRevision)
											{
												//Remote version wins
												isPropertyRowUpdateRequired = true;
											}
											else //revision are equal
											{
												if ((DateTime)propertyRow["When"] <= remotePropertyWhen)
												{
													if ((DateTime)propertyRow["When"] < remotePropertyWhen)
													{
														//Remote version wins
														isPropertyRowUpdateRequired = true;
													}
													else //When are equal
													{
														if (propertyRow["By"].ToString().CompareTo(remotePropertyBy) < 0)
														{
															//Remote version wins
															isPropertyRowUpdateRequired = true;
														}
														else {};	//My property row wins because I've higher or equal By
													}
												}
												else {};	//My property row wins because I've higher When
											}
										}
										else {};	//My property row wins because I've higher Revision
									}
									else
										throw new Exception(string.Format("Local database may be currupted: Multiple rows in ItemProperty table found for same ItemGuid {0} and property name {1}", itemGuid, propertyName));


									if (isPropertyRowUpdateRequired == true)
									{
										propertyRow["CurrentValueBy"] = remoteCurrentValueBy;
										propertyRow["By"] = remotePropertyBy;
										propertyRow["When"] = remotePropertyWhen;
										propertyRow["Revision"] = remotePropertyRevision;
										isItemRowUpdateRequired = true; //Update the item row later
									}
									else {}; //No need to update this property row


									//Check and see each value assigned to this property
									XmlNodeList remotePropertyValueNodes = remotePropertyNodes[remotePropertyIndex].SelectNodes("./value");
									for (int remotePropertyValueIndex = 0; remotePropertyValueIndex < remotePropertyValueNodes.Count; remotePropertyValueIndex++)
									{
										string propertyValueBy = GetNullSafeAttributeValue(remotePropertyValueNodes[remotePropertyValueIndex].Attributes, "by", string.Empty);
										int remotePropertyValueRevision = XmlConvert.ToInt32(GetNullSafeAttributeValue(remotePropertyValueNodes[remotePropertyValueIndex].Attributes, "revision", "1"));
										DateTime remotePropertyValueWhen = XmlConvert.ToDateTime(GetNullSafeAttributeValue(remotePropertyValueNodes[remotePropertyValueIndex].Attributes, "when", XmlConvert.ToString(DateTime.MinValue)));
										bool remotePropertyValueDeleted = XmlConvert.ToBoolean(GetNullSafeAttributeValue(remotePropertyValueNodes[remotePropertyValueIndex].Attributes, "deleted", XmlConvert.ToString(false)));
										DateTime remotePropertyValueResolvedWhen = XmlConvert.ToDateTime(GetNullSafeAttributeValue(remotePropertyValueNodes[remotePropertyValueIndex].Attributes, "resolvedWhen", XmlConvert.ToString(DateTime.MinValue)));
												
										//Look for local property value row
										DataRow propertyValueRow = null;
										WinnerEndPoint winner = WinnerEndPoint.None;
										DataRow[] propertyValueRows = localDataset.Tables["ItemPropertyValue"].Select(string.Format("ItemGuid = '{0}' AND PropertyName='{1}' AND By = '{2}'", itemGuid, propertyName, propertyValueBy));
										if (propertyValueRows.Length == 0)
										{
											winner = WinnerEndPoint.RemoteEndPoint;

											//Insert new property value row and copy its value from remote endpoint
											DataRow newPropertyValueRow = localDataset.Tables["ItemPropertyValue"].NewRow();
											//set immuatble values
											newPropertyValueRow["ItemGuid"] = itemGuid;
											newPropertyValueRow["PropertyName"] = propertyName;
											newPropertyValueRow["By"] = propertyValueBy;
											newPropertyValueRow["ItemType"] = itemType;	//redundant immutable field

											//Other fields are updated later
											localDataset.Tables["ItemPropertyValue"].Rows.Add(newPropertyValueRow);
											propertyValueRow = newPropertyValueRow;

											//Update time stamp on item indicating that an update was made
											isItemRowUpdateRequired = true;

										}
										else if (propertyValueRows.Length == 1)
										{
											propertyValueRow = propertyValueRows[0];

											//Rule for property value sync:
											//Higher revision wins
											//	If revisions are equal, higher When wins
											//		If When are equal, no changes are applied except that if any of them had Deleted set to true, it's carried over
											if ((int)propertyValueRow["Revision"] < remotePropertyValueRevision)
												winner = WinnerEndPoint.RemoteEndPoint;
											else if ((int)propertyValueRow["Revision"] > remotePropertyValueRevision)
												winner = WinnerEndPoint.ThisEndPoint;
											else	//revisions are equal
											{
												if ((DateTime)propertyValueRow["When"] < remotePropertyValueWhen)
													winner = WinnerEndPoint.RemoteEndPoint;
												else if ((DateTime)propertyValueRow["When"] > remotePropertyValueWhen)
													winner = WinnerEndPoint.ThisEndPoint;
												else	//When are equal
													winner = WinnerEndPoint.None;
											}
										}
										else
											throw new Exception(string.Format("Local database may be currupted: Multiple rows in ItemPropertyValue table found for same ItemGuid {0} and property name {1} and By {2}", itemGuid, propertyName, propertyValueBy));

										if (winner == WinnerEndPoint.RemoteEndPoint)
										{
											//Update mutable values
											propertyValueRow["Deleted"] = remotePropertyValueDeleted;
											propertyValueRow["Revision"] = remotePropertyValueRevision;
											propertyValueRow["When"] = remotePropertyValueWhen;
											if (remotePropertyValueResolvedWhen > DateTime.MinValue)
												propertyValueRow["ResolvedWhen"] = remotePropertyValueResolvedWhen;
											else
												propertyValueRow["ResolvedWhen"] = null;
											propertyValueRow["ResolvedReason"] = GetNullSafeAttributeValue(remotePropertyValueNodes[remotePropertyValueIndex].Attributes, "resolvedReason", string.Empty);
											propertyValueRow["Value"] = remotePropertyValueNodes[remotePropertyValueIndex].InnerText;
											propertyValueRow["Current"] = (propertyRow["CurrentValueBy"].ToString().CompareTo(propertyValueBy) == 0);

											isItemRowUpdateRequired = true;
										}
										else if (winner == WinnerEndPoint.None)
										{
											//Make sure Delete is set to true if either one has true
											if ((remotePropertyValueDeleted) && (!((bool)propertyValueRow["Deleted"])))
											{
												propertyValueRow["Deleted"] = true;
												isItemRowUpdateRequired = true;
											}
											else {}; //do not change Deleted state

											if (remotePropertyValueResolvedWhen > DateTime.MinValue)
											{
												if ((System.Convert.IsDBNull(propertyValueRow["ResolvedWhen"]))
													|| (propertyValueRow["ResolvedWhen"] == null)
													|| (remotePropertyValueResolvedWhen > (DateTime)propertyValueRow["ResolvedWhen"]))
												{
													if (remotePropertyValueResolvedWhen > DateTime.MinValue)
														propertyValueRow["ResolvedWhen"] = remotePropertyValueResolvedWhen;
													else
														propertyValueRow["ResolvedWhen"] = null;
													propertyValueRow["ResolvedReason"] = GetNullSafeAttributeValue(remotePropertyValueNodes[remotePropertyValueIndex].Attributes, "resolvedReason", string.Empty);
													isItemRowUpdateRequired = true;
												}
												else {}// We have higher or equal ResolvedWhen
											}
											else {} //remote does not have ResolvedWhen value
										}
										else {}// we have won

									}	//end for property value

									//Flip the current flag on property value rows for this property
									DataRow[] allPropertyValueRowsForProperty = localDataset.Tables["ItemPropertyValue"].Select(string.Format("ItemGuid = '{0}' AND PropertyName='{1}'", itemGuid, propertyName));
									for (int propertyValueRowIndex = 0; propertyValueRowIndex < allPropertyValueRowsForProperty.Length; propertyValueRowIndex++)
									{
										bool currentValueFlag = (allPropertyValueRowsForProperty[propertyValueRowIndex]["By"].ToString() == propertyRow["CurrentValueBy"].ToString());
										if ((bool)allPropertyValueRowsForProperty[propertyValueRowIndex]["Current"] != currentValueFlag)
										{
											allPropertyValueRowsForProperty[propertyValueRowIndex]["Current"] = currentValueFlag;
										}
										else {}; //do not modify this column
										
										//Make sure current points to non-deleted value and undelete if it does not
										if ((currentValueFlag) && (bool)allPropertyValueRowsForProperty[propertyValueRowIndex]["Deleted"])
										{
											allPropertyValueRowsForProperty[propertyValueRowIndex]["Deleted"] = false;
											isItemRowUpdateRequired = true;	//because we performed an undelete
										}
										else {}; //nothing to do for non-current rows
									}

								}	//end for each property

								if (isItemRowUpdateRequired)
								{
									totalItemsUpdated++;
									itemRow["When"] = DateTime.UtcNow;
									isAnythingUpdated = true;
									NotifyProgressToCaller(notifyProgress, remoteItemIndex, remoteItemNodes.Count, "Finished item #" + remoteItemIndex.ToString());
								}
								else {}; //no updates to item row required

							} //end else process item

						} //end for each item

						if (isAnythingUpdated)
						{
							string nextItemsShouldBeUpdatedAfterString = GetNullSafeAttributeValue(remoteDatasetNode.SelectSingleNode("./@since"), string.Empty);
							if (nextItemsShouldBeUpdatedAfterString.Length > 0)
							{
								if (!isSyncMarkFound)
								{
									//Request more pages
									nextItemsShouldBeUpdatedAfter = XmlConvert.ToDateTime(nextItemsShouldBeUpdatedAfterString);
								}
								else 
								{
									//Syncbar was found. Indicate that no more pages are required
									nextItemsShouldBeUpdatedAfter = DateTime.MinValue;
								};
							}
							else
							{
								//Remote end point doesn't support multi page requests
								nextItemsShouldBeUpdatedAfter = DateTime.MinValue;	//Indicates that no more pages are requested
							}

							if (nextItemsShouldBeUpdatedAfter == DateTime.MinValue)
							{
								//Update sync markers
								NotifyProgressToCaller(notifyProgress, remoteItemNodes.Count, remoteItemNodes.Count, "Updating sync mark tables...");
								XmlNodeList remoteSyncMarkNodes = remoteDatasetNode.SelectNodes("./syncMark");
								for (int remoteSyncMarkNodeIndex = 0; remoteSyncMarkNodeIndex < remoteSyncMarkNodes.Count; remoteSyncMarkNodeIndex++)
								{
									string endPointGuid = remoteSyncMarkNodes[remoteSyncMarkNodeIndex].Attributes["endpointGuid"].Value;
									if (endPointGuid != endpointGuidForLocalDataset)
									{
										//Look for local sync mark value
										DataRow[] localEndPointRows = localSyncMarkDataTable.Select(string.Format("EndPointGuid = '{0}'", endPointGuid));
										DataRow localEndPointRow = null;
										bool isLocalEndPointRowUpdateRequired = false;

										if (localEndPointRows.Length == 0)
										{
											//Add this row
											localEndPointRow = localSyncMarkDataTable.NewRow();
											localEndPointRow["EndPointGuid"] = endPointGuid;
											localSyncMarkDataTable.Rows.Add(localEndPointRow);
											isLocalEndPointRowUpdateRequired = true;
										}
										else if (localEndPointRows.Length == 1)
										{
											localEndPointRow = localEndPointRows[0];
											int remoteMarkValue = XmlConvert.ToInt32(remoteSyncMarkNodes[remoteSyncMarkNodeIndex].Attributes["mark"].Value);
											isLocalEndPointRowUpdateRequired = (remoteMarkValue > (int)localEndPointRow["mark"]);
										}
										else
											throw new Exception(string.Format("Local database may be currupted: Multiple rows in SyncMark table found for same LocalEndPointGuid {0}", endPointGuid));

										if (isLocalEndPointRowUpdateRequired)
										{
											localEndPointRow["mark"] = XmlConvert.ToInt32(remoteSyncMarkNodes[remoteSyncMarkNodeIndex].Attributes["mark"].Value);
										}
									}
									else {}; //do not update sync marker for this endpoint's row!

								} //end for sync mark updates

								//Now update the mark for local endpoint
								DataRow[] localEndPointSyncMarkRows = localSyncMarkDataTable.Select(string.Format("EndPointGuid = '{0}'", endpointGuidForLocalDataset));
								if (localEndPointSyncMarkRows.Length == 0)
									throw new Exception(string.Format("Local database may be currupted: Row in SyncMark table not found for LocalEndPointGuid {0}", endpointGuidForLocalDataset)); //If we had this dataset locally we must have the sync mark row
								else if (localEndPointSyncMarkRows.Length > 1)
									throw new Exception(string.Format("Local database may be currupted: Multiple rows found in SyncMark table for LocalEndPointGuid {0}", endpointGuidForLocalDataset));
								else //There is only one row
									localEndPointSyncMarkRows[0]["Mark"] = (int)localEndPointSyncMarkRows[0]["Mark"] + 1;

								//Finally add our new sync mark
								AddNewSyncMark(localDataset, (int) localEndPointSyncMarkRows[0]["Mark"], endpointGuidForLocalDataset);
							}
							else {} //we are going to be requesting more pages. So don't update sync marks yet

							//Now let's commit all our updates
							NotifyProgressToCaller(notifyProgress, remoteItemNodes.Count, remoteItemNodes.Count, "Commiting changes to database...");
							localDatabase.UpdateTableData(localDataset, new string[] {"Dataset", "Item", "ItemProperty", "ItemPropertyValue", "SyncMark"});

						}
						else {}; //nothing got updated during this sync cycle so no further action required
					}
					else {}; // no items to update. End of sync.
				}
				else {}; //end isSyncNeeded check

				return nextItemsShouldBeUpdatedAfter;
			}
			else	//dataset node doesn't exist
				return DateTime.MinValue;	//Should we throw exception here?
		}

		private static void NotifyProgressToCaller(NotifyProgressDelegate notifyProgress, int itemIndex, int itemCount, string message)
		{
			if (notifyProgress != null)
			{
				notifyProgress(itemIndex, itemCount, message);
			}
		}


//end method sync

		private static string GetNullSafeAttributeValue(XmlNode attribute, string valueIfNull)
		{
			if (attribute==null)	
				return valueIfNull;
			else
				return attribute.Value;
		}

		private static string GetNullSafeAttributeValue(XmlAttributeCollection attributes, string name, string defaultValue)
		{
			if (attributes[name]==null)	
				return defaultValue;
			else
				return attributes[name].Value;
		}

		private static void AddNewSyncMark(DataSet localDataset, int syncMarkValue, string localEndpointGuid)
		{
			DataDocument.CreateNewItemInternal(localDataset, localEndpointGuid, "$sysuser$", "$syncMark$", new string[] {"mark"}, new string[] {syncMarkValue.ToString()}, true);
		}

		private enum WinnerEndPoint
		{
			RemoteEndPoint, ThisEndPoint, None
		}
	} //end class SyncEngine

}//end NameSpace Astril.Blank


//TODO: Concurrency while updating from multple feeds simultaneously on server
//TODO: Item.When field should be long and set from DateTime.Ticks
//TODO: FEEDWRITER IGNORES UPDATEDSINCE
//TODO: Avoid use of sql direct instead of params (risk of sql injection)
//TODO: Check Value attribute for $syncMark$, check ignoring the sync mark, remove Value attribute
//TODO: make sure handling of empty strings for optional elements and attributes
using System;
using System.Data;
using System.Data.OleDb;
using System.Net;
using System.Text.RegularExpressions;

namespace Astrila.Common
{
	/// <summary>
	/// Summary description for CommonFunctions.
	/// </summary>
	public class UtilityFunctions
	{
		public static String ReplaceString(String SourceString, String SearchString, String ReplaceString, bool IsCaseInsensetive)
		{
			return Regex.Replace (SourceString, Regex.Escape(SearchString), ReplaceString, (IsCaseInsensetive==true)?RegexOptions.IgnoreCase: RegexOptions.None);  
		}
		public static String[] SplitString(String SourceString, String SearchString, bool IsCaseInsensetive)
		{
			return Regex.Split (SourceString, Regex.Escape(SearchString), (IsCaseInsensetive==true)?RegexOptions.IgnoreCase: RegexOptions.None);  
		}
		public static  bool IsSubStringExist(String StringToSearch, String SubStringToLookFor, bool IsCaseInsensitive)
		{
			return Regex.IsMatch(StringToSearch, Regex.Escape(SubStringToLookFor), (IsCaseInsensitive==true)?RegexOptions.IgnoreCase: RegexOptions.None);
		}

		public static string GetConnectionStringForAccess(string mdbFilePath)
		{
			return String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Password=\"\";User ID=Admin;Data Source={0};Mode=Share Deny None;Extended Properties=\"\";Jet OLEDB:System database=\"\";Jet OLEDB:Registry Path=\"\";Jet OLEDB:Database Password=\"\";Jet OLEDB:Engine Type=5;Jet OLEDB:Database Locking Mode=1;Jet OLEDB:Global Partial Bulk Ops=2;Jet OLEDB:Global Bulk Transactions=1;Jet OLEDB:New Database Password=\"\";Jet OLEDB:Create System Database=False;Jet OLEDB:Encrypt Database=False;Jet OLEDB:Don't Copy Locale on Compact=False;Jet OLEDB:Compact Without Replica Repair=False;Jet OLEDB:SFP=False", mdbFilePath);
		}
		public static int FillDataSetUsingSql(string oleDbConnectionString, DataSet dataSetToFill, string tableToCreateInDataSet, string selectSql)
		{
			OleDbDataAdapter adapter = new OleDbDataAdapter(selectSql, new OleDbConnection(oleDbConnectionString));
			return adapter.Fill(dataSetToFill, tableToCreateInDataSet);
		}

		public static string GetUrlContent(string url)
		{
			using (WebClient webClientToGetFeedContent = new WebClient())
			{
				byte[] contentBytes = webClientToGetFeedContent.DownloadData(url);
				string content = System.Text.Encoding.UTF8.GetString(contentBytes);

				return content;
			}
		}

	}
}

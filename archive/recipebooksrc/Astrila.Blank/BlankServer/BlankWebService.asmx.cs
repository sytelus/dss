using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Services;
using System.Xml;
using Astrila.Blank;
using Astrila.Common;

namespace Astrila.Blank.BlankServer
{
	/// <summary>
	/// Summary description for BlankWebService.
	/// </summary>
	public class BlankWebService : System.Web.Services.WebService
	{
		public BlankWebService()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion


		[WebMethod]
		public PostResult PostFeed(XmlDocument feedDocument)
		{
			XmlNode datasetNode = feedDocument.SelectSingleNode("/dss/dataset");
			PostResult result = new PostResult();
			result.SendMoreItemsUpdatedAfter = DateTime.MinValue;

			if (datasetNode != null)
			{
				int totalItemsSynced; int totalItemsExamined;
				result.SendMoreItemsUpdatedAfter = SyncEngine.Sync(Common.GetLocalDatabase(), datasetNode, out totalItemsSynced, out totalItemsExamined, null);
				result.TotalItemsUpdated = totalItemsSynced;
				result.TotalItemsExamined = totalItemsExamined;
			}
			else
				throw new ArgumentNullException("feedXml", "The supplied XML string does not contain dataset node");

			return result;
		}

		[Serializable]
		public struct PostResult
		{
			private DateTime _sendMoreItemsUpdatedAfter;
			public DateTime SendMoreItemsUpdatedAfter
			{
				get
				{
					return _sendMoreItemsUpdatedAfter;
				}
				set
				{
					_sendMoreItemsUpdatedAfter = value;
				}
			}

			private int _totalItemsUpdated;
			public int TotalItemsUpdated
			{
				get
				{
					return _totalItemsUpdated;
				}
				set
				{
					_totalItemsUpdated = value;
				}
			}

			private int _totalItemsExamined;
			public int TotalItemsExamined
			{
				get
				{
					return _totalItemsExamined;
				}
				set
				{
					_totalItemsExamined = value;
				}
			}
		}
	}
}

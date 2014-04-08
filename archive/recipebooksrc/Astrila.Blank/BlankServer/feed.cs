using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using Astrila.Blank;

namespace Astrila.Blank.BlankServer
{
	/// <summary>
	/// Summary description for feed.
	/// </summary>
	public class Feed : IHttpHandler
	{
		void IHttpHandler.ProcessRequest(HttpContext context)
		{
			DateTime updatedAfter = DateTime.MaxValue;
			if ((context.Request.QueryString["after"] != null) && (context.Request.QueryString["after"].Length > 0))
				updatedAfter = DateTime.Parse(context.Request.QueryString["after"], CultureInfo.InvariantCulture);

			context.Response.Clear();
			context.Response.ContentType = " text/xml";
			context.Response.Cache.SetETagFromFileDependencies();
			context.Response.Cache.SetLastModifiedFromFileDependencies();
			context.Response.Cache.SetCacheability(HttpCacheability.Public);
			context.Response.AddFileDependency(Common.GetMdbFilePath());

			XmlDocument feedDocument = new XmlDocument();
			try
			{
				FeedWriter.WriteFeed(feedDocument, Common.GetLocalDatabase(), "Recipes from ShitalShah.com", "ShitalShah/Recipe", updatedAfter, 10);
				context.Response.Write(feedDocument.OuterXml);
			}
			catch(DatasetNotFoundException ex)
			{
				context.Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n");
				context.Response.Write("<dss/>");
			}
		}
		
		bool IHttpHandler.IsReusable
		{
			get
			{
				return true;
			}
		}
	}
}

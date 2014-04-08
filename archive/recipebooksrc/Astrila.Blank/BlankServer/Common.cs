using System;
using System.Web;
using Astrila.Common;

namespace Astrila.Blank.BlankServer
{
	/// <summary>
	/// Summary description for Common.
	/// </summary>
	internal class Common
	{
		private Common()
		{
			
		}

		public static DataAccess GetLocalDatabase()
		{
			string mdbFilePath = GetMdbFilePath();
			DataAccess localDatabase = new DataAccess(DataAccess.BuildConnectionStringForAccess(DataAccess.BuildConnectionStringForAccess(mdbFilePath)));

			return localDatabase;
		}

		public static string GetMdbFilePath()
		{
			return HttpContext.Current.Request.MapPath("ServerData.mdb");
		}
	}
}

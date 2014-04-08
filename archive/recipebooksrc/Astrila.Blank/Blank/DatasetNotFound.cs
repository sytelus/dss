using System;

namespace Astrila.Blank
{
	/// <summary>
	/// Summary description for DatasetNotFound.
	/// </summary>
	[Serializable]
	public class DatasetNotFoundException : Exception
	{
		private string _datasetGlobalGuid;
		public string DatasetGlobalGuid
		{
			get
			{
				return _datasetGlobalGuid;
			}
		}
		public DatasetNotFoundException(string datasetGlobalGuid, string message) : base(message)
		{
			_datasetGlobalGuid = datasetGlobalGuid;
		}
	}
}

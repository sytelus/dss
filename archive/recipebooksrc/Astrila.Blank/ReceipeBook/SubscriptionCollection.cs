using System;
using System.Net;
using System.Xml;
using Astrila.Common;

namespace Astrila.Blank.Clients.RecipeBook
{
	public class SubscriptionCollection
	{
		XmlDocument _subscriptionDocument = new XmlDocument();
		string _subscriptionFilePath;
		public SubscriptionCollection(string subscriptionFilePath)
		{
			_subscriptionFilePath = subscriptionFilePath;
			_subscriptionDocument.Load(subscriptionFilePath);
		}

		public FeedInfo[] GetFeedInfo()
		{
			XmlNodeList subscriptionNodes = _subscriptionDocument.SelectNodes("./subscriptions/subscription");
			FeedInfo[] feeds = new FeedInfo[subscriptionNodes.Count];
			for (int subscriptionNodeIndex = 0; subscriptionNodeIndex < subscriptionNodes.Count; subscriptionNodeIndex++)
			{
				feeds[subscriptionNodeIndex] =  new FeedInfo(subscriptionNodes[subscriptionNodeIndex]);
			}

			return feeds;
		}

		public class FeedInfo
		{
			public readonly string Title;
			public readonly string Url;
			public readonly DateTime AddedOn;
			public readonly bool TwoWay;
			public readonly string GetDataUrl;
			public readonly string PutDataUrl;

			internal FeedInfo(XmlNode feedNode)
			{
				Title = feedNode.Attributes["title"].Value;
				Url = feedNode.Attributes["url"].Value;
				AddedOn = XmlConvert.ToDateTime(feedNode.Attributes["when"].Value);
				TwoWay = XmlConvert.ToBoolean(feedNode.Attributes["twoWay"].Value);
				GetDataUrl = feedNode.Attributes["getDataUrl"].Value;
				PutDataUrl = feedNode.Attributes["putDataUrl"].Value;
			}
		}

		public void Add(string feedUrl)
		{
			if (! this.Contains(feedUrl))
			{
				string feedContent = UtilityFunctions.GetUrlContent(feedUrl);
				XmlDocument feedDocument = new XmlDocument();
				feedDocument.LoadXml(feedContent);

				XmlNode feedTitleNode = feedDocument.SelectSingleNode("/dss/dataset/@title");
				if (feedTitleNode != null)
				{
					string feedTitle = feedUrl;
					if (feedTitleNode != null)
						feedTitle = feedTitleNode.Value;

					XmlNode subscriptionsNode = _subscriptionDocument.SelectSingleNode("./subscriptions");
					XmlNode newSubscriptionNode = _subscriptionDocument.CreateElement("subscription");
					subscriptionsNode.AppendChild(newSubscriptionNode);

					XmlAttribute newFeedAttribute = _subscriptionDocument.CreateAttribute("url");
					newFeedAttribute.Value = feedUrl;
					newSubscriptionNode.Attributes.Append(newFeedAttribute);

					XmlAttribute newWhenAttribute = _subscriptionDocument.CreateAttribute("when");
					newWhenAttribute.Value = XmlConvert.ToString(DateTime.Now);
					newSubscriptionNode.Attributes.Append(newWhenAttribute);

					XmlAttribute newtitleAttribute = _subscriptionDocument.CreateAttribute("title");
					newtitleAttribute.Value = feedTitle;
					newSubscriptionNode.Attributes.Append(newtitleAttribute);

					//TODO: exception for unidentified protocols except default assumed (url and webService)
					XmlNode getDataNode = feedDocument.SelectSingleNode("/dss/dataset/related[@type='getData']/@link");
					XmlAttribute getDataUrlAttribute = _subscriptionDocument.CreateAttribute("getDataUrl");
					if (getDataNode != null)
						getDataUrlAttribute.Value = getDataNode.Value;
					else 
						getDataUrlAttribute.Value = feedUrl;
					newSubscriptionNode.Attributes.Append(getDataUrlAttribute);

					XmlNode putDataNode = feedDocument.SelectSingleNode("/dss/dataset/related[@type='putData']/@link");
					XmlAttribute putDataUrlAttribute = _subscriptionDocument.CreateAttribute("putDataUrl");
					if (putDataNode != null)
						putDataUrlAttribute.Value = putDataNode.Value;
					else {}; //leave it BLANK
					newSubscriptionNode.Attributes.Append(putDataUrlAttribute);

					XmlAttribute newTwoWayAttribute = _subscriptionDocument.CreateAttribute("twoWay");
					newTwoWayAttribute.Value = XmlConvert.ToString((feedDocument.SelectSingleNode("/dss/dataset/related[@type='putData']") != null) && (feedDocument.SelectSingleNode("/dss/dataset/related[@type='getData']") != null));
					newSubscriptionNode.Attributes.Append(newTwoWayAttribute);

				}
				else throw new ApplicationException("This URL is not a valid DSS (Data Syndication Services) compliant feed.");
			}
			else
				throw new ApplicationException(string.Format("Feed {0} already exists in subscriptions", feedUrl));
		}

		public void Save()
		{
			_subscriptionDocument.Save(_subscriptionFilePath);
		}

		public bool Contains(string feedUrl)
		{
			return _subscriptionDocument.SelectSingleNode(string.Format("./subscriptions/subscription[@url='{0}']", feedUrl)) != null;
		}

		public void Remove(FeedInfo feed)
		{
			XmlNode subscriptionNode = _subscriptionDocument.SelectSingleNode(string.Format("./subscriptions/subscription[@url='{0}']", feed.Url));
			if (subscriptionNode != null)
			{
				_subscriptionDocument.SelectSingleNode("./subscriptions").RemoveChild(subscriptionNode);
			}
			else
				throw new ApplicationException(string.Format("Feed '{0}' does not exist in subscriptions list", feed));
		}
	}
}

using System;
using System.Net;
using System.Xml;

namespace AutoUpdater
{
    internal class AutoUpdaterConfig
    {
        internal Version Version { get; }

        internal Uri Uri { get; }

        internal string FileName { get; }

        internal string MD5 { get; }

        internal string Description { get; }

        internal string LaunchArgs { get; }

        internal AutoUpdaterConfig(Version version, Uri uri, string fileName, string md5, string description, string launchArgs)
        {
            Version = version as Version;
            Uri = uri as Uri;
            FileName = fileName as string;
            MD5 = md5 as string;
            Description = description as string;
            LaunchArgs = launchArgs as string;
        }

        internal bool IsNewerVersion(Version version)
        {
            return Version > version;
        }

        internal static bool ExistsOnServer(Uri location)
        {
            if (location.ToString().StartsWith("file"))
            {
                return System.IO.File.Exists(location.LocalPath);
            }
            else
            {
                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(location.AbsoluteUri);
                    HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                    res.Close();

                    return res.StatusCode == HttpStatusCode.OK;
                }
                catch { return false; }
            }
        }

        internal static AutoUpdaterConfig Parse(Uri location, string appId)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(location.AbsoluteUri);

                XmlNode node = doc.DocumentElement.SelectSingleNode("//update[@appId='" + appId + "']");

                if (node == null)
                    return null;

                Version version = Version.Parse(node["version"].InnerText);
                Uri uri = new Uri(node["url"].InnerText);
                string fileName = node["fileName"].InnerText;
                string md5 = node["md5"].InnerText;
                string description = node["description"].InnerText;
                string launchArgs = node["launchArgs"].InnerText;

                AutoUpdaterConfig ret = new AutoUpdaterConfig(version, uri, fileName, md5, description, launchArgs);
                return ret;
            }
            catch { return null; }
        }
    }
}

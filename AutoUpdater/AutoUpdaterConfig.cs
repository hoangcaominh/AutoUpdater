using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml;

namespace AutoUpdater
{
    internal class AutoUpdaterConfig
    {
        private Version version;
        private Uri uri;
        private string fileName;
        private string md5;
        private string description;
        private string launchArgs;

        internal Version Version
        {
            get { return this.version; }
        }

        internal Uri Uri
        {
            get { return this.uri; }
        }

        internal string FileName
        {
            get { return this.fileName; }
        }
        
        internal string MD5
        {
            get { return this.md5; }
        }
        
        internal string Description
        {
            get { return this.description; }
        }

        internal string LaunchArgs
        {
            get { return this.launchArgs; }
        }

        internal AutoUpdaterConfig(Version version, Uri uri, string fileName, string md5, string description, string launchArgs)
        {
            this.version = version as Version;
            this.uri = uri as Uri;
            this.fileName = fileName as string;
            this.md5 = md5 as string;
            this.description = description as string;
            this.launchArgs = launchArgs as string;
        }

        internal bool IsNewerVersion(Version version)
        {
            return this.version > version;
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

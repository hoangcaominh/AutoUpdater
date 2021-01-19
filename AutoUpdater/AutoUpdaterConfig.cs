﻿using System;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace AutoUpdater
{
    internal class AutoUpdaterConfig
    {
        internal Version Version { get; }

        internal string FileName { get; }

        internal Uri Uri { get; }

        internal string Executable { get; }

        internal string MD5 { get; }

        internal string LaunchArgs { get; }

        internal AutoUpdaterConfig(Version version, Uri uri, string fileName, string executable, string md5, string launchArgs)
        {
            Version = version as Version;
            Uri = uri as Uri;
            FileName = fileName as string;
            Executable = executable as string;
            MD5 = md5 as string;
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
                return System.IO.Directory.Exists(location.LocalPath);
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        internal static AutoUpdaterConfig Parse(Uri location)
        {
            try
            {
                JObject json = new JObject();
                using (WebClient wc = new WebClient())
                {
                    Uri info = new Uri(location, "update.json");
                    json = JObject.Parse(wc.DownloadString(info.AbsoluteUri));
                }

                Version version = Version.Parse(json["version"].ToObject<string>());
                string filename = json["filename"].ToObject<string>();
                Uri uri = new Uri(location, filename);
                string executable = json["executable"].ToObject<string>();
                string md5 = json["md5"].ToObject<string>();
                string launchArgs = json["launchArgs"].ToObject<string>();

                AutoUpdaterConfig ret = new AutoUpdaterConfig(version, uri, filename, executable, md5, launchArgs);
                return ret;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}

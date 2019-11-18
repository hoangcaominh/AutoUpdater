﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.IO.Compression;

namespace AutoUpdater
{
    public class Updater
    {
        private IAutoUpdater applicationInfo;
        private BackgroundWorker bgWorker;

        public Updater(IAutoUpdater applicationInfo)
        {
            this.applicationInfo = applicationInfo;

            this.bgWorker = new BackgroundWorker();
            this.bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            this.bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            IAutoUpdater application = (IAutoUpdater)e.Argument;

            if (!AutoUpdaterConfig.ExistsOnServer(application.UpdateConfigLocation))
                e.Cancel = true;
            else
                e.Result = AutoUpdaterConfig.Parse(application.UpdateConfigLocation, application.ApplicationId);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("There was a problem while connecting to the server. Please try again later.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Result == null)
            {
                MessageBox.Show("There was a problem while locating update file. Please try again later.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                AutoUpdaterConfig update = (AutoUpdaterConfig)e.Result;

                if (update != null && update.IsNewerVersion(this.applicationInfo.ApplicationAssembly.GetName().Version))
                {
                    if (MessageBox.Show("A newer version is available. Proceed to download?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        this.DownloadUpdate(update);
                }
            }
        }

        public void Update()
        {
            if (!this.bgWorker.IsBusy)
            {
                this.bgWorker.RunWorkerAsync(this.applicationInfo);
            }
        }

        private void DownloadUpdate(AutoUpdaterConfig update)
        {
            AutoUpdaterDownloadForm form = new AutoUpdaterDownloadForm(update.Uri, update.MD5, this.applicationInfo.ApplicationIcon);
            DialogResult result = form.ShowDialog(this.applicationInfo.Context);

            if (result == DialogResult.OK)
            {
                UpdateApplication(form.TempFilePath, Path.GetDirectoryName(this.applicationInfo.ApplicationAssembly.Location), update.FileName, update.LaunchArgs);

                Application.Exit();
            }
            else if (result == DialogResult.Abort)
            {
                MessageBox.Show("Download aborted. The new version is not installed.", "Download Aborted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("Download failed. A problem was encountered while downloading, please try again later.", "Download Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateApplication(string tempFilePath, string parentFolder, string exeFile, string launchArgs)
        {
            try
            {
                string tempFolder = parentFolder + @"\temp";

                // Prevent exception when extracting
                if (Directory.Exists(tempFolder))
                {
                    Directory.Delete(tempFolder, true);
                }

                // Extract archive
                ZipFile.ExtractToDirectory(tempFilePath, tempFolder);
                /*
                foreach (string dirPath in Directory.GetDirectories(tempFolder, "*", SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(tempFolder, "."));
                foreach (string path in Directory.GetFiles(tempFolder, "*.*", SearchOption.AllDirectories))
                    File.Copy(path, path.Replace(tempFolder, parentFolder), true);
                */
                
                // Replace old content with the extracted one
                string argument_update = "/C choice /C Y /N /D Y /T 2 & move /Y \"{0}" + @"\*" + "\" \"{1}\" & rmdir /Q /S \"{0}\"";
                string argument_update_start = argument_update + " & start \"\" /D \"{1}\" \"{2}\" {3} & call sendKeys.bat \"{2}\" \"\"";
                string argument = argument_update_start;

                ProcessStartInfo info = new ProcessStartInfo();
                info.Arguments = string.Format(argument, tempFolder, parentFolder, exeFile, launchArgs);
                info.WindowStyle = ProcessWindowStyle.Hidden;
                info.CreateNoWindow = true;
                info.FileName = "cmd.exe";
                Process.Start(info);
                
                /*
                string argument_update = "/C choice /C Y /N /D Y /T 2 & Move /Y \"{0}\" \"{1}\"";
                string argument_update_start = argument_update + " & start \"\" /D \"{1}\" \"{2}\" {3} & call sendKeys.bat \"{2}\" \"\"";
                string argument = argument_update_start;

                ProcessStartInfo info = new ProcessStartInfo
                {
                    Arguments = string.Format(argument, tempFolder + @"\*", parentFolder, exeFile, launchArgs),
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    FileName = "cmd.exe"
                };
                Process.Start(info);

                Directory.Delete(tempFolder, true);
                */
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}

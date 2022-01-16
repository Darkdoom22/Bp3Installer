using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bp3Installer.InstallerCore.ArchiveManager
{
    internal class ArchiveMgr
    {
        private static string _AppLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
        private readonly string _AppPath = System.IO.Path.GetDirectoryName(_AppLocation);
        private readonly string _Repo = "https://github.com/eLiidyr/bp3/archive/refs/heads/main.zip";
        private Task<Task> DownloadArchiveInternal()
        {
            
            byte[] rawBytes;
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("user-agent", "Bp3Updater");
                    rawBytes = client.DownloadData(_Repo);
                }

                File.WriteAllBytes($@"{_AppPath}/bp3.zip", rawBytes);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return Task.FromResult(Task.CompletedTask);
        }

        private Task<Task> CheckArchiveInternal()
        {

            if(File.Exists($@"{_AppPath}/bp3.zip"))
            {
                InstallerCore.Core.ArchiveFound = true;
            }
            else
            {
                InstallerCore.Core.ArchiveFound = false;
            }

            return Task.FromResult(Task.CompletedTask);
        }

        public Task<Task> CheckForArchive()
        {
            return Task.Factory.StartNew(async () => { await CheckArchiveInternal(); });
        }

        public Task<Task> DownloadArchive()
        {
            return Task.Factory.StartNew(async () => { await DownloadArchiveInternal(); });
        }
    }
}

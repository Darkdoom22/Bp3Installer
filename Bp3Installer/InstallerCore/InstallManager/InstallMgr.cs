using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bp3Installer.InstallerCore.InstallManager
{
    internal class InstallMgr
    {
        private static object _Lock = new object();
        private static string _AppLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
        private readonly string _AppPath = System.IO.Path.GetDirectoryName(_AppLocation);
        private readonly string _DirectoryToCheck;

        public InstallMgr(string dir)
        {
            this._DirectoryToCheck = dir;
        }

        private async Task<Task> InstallBp3Internal()
        {
            InstallerCore.Core.InstallerStep = "Verifying user directory..";
            InstallerCore.Core.InstallProgress = 5;

            if (Core.DirectoryFound)
            {
                InstallerCore.Core.InstallerStep = "Directory verified!";
                InstallerCore.Core.InstallProgress = 10;

                InstallerCore.Core.InstallerStep = "Instantiating archive manager..";
                InstallerCore.Core.InstallProgress = 15;
                InstallerCore.ArchiveManager.ArchiveMgr archiveMgr = new();

                InstallerCore.Core.InstallerStep = "Downloading most recent archive..";
                InstallerCore.Core.InstallProgress = 20;
                await archiveMgr.DownloadArchive();

                InstallerCore.Core.InstallerStep = "Verifying archive download..";
                InstallerCore.Core.InstallProgress = 50;

                await archiveMgr.CheckForArchive();

                if (Core.ArchiveFound)
                {
                    InstallerCore.Core.InstallerStep = "Archive verified, extracting to temp location..";
                    InstallerCore.Core.InstallProgress = 70;

                    ZipFile.ExtractToDirectory($@"{_AppPath}/bp3.zip", $@"{_AppPath}/temp", true);

                    InstallerCore.Core.InstallerStep = "Preparing file move..";
                    InstallerCore.Core.InstallProgress = 80;

                    FileSystem.CopyDirectory($@"{_AppPath}/temp/bp3-main", $@"{_AppPath}/temp/bp3", true);

                    InstallerCore.Core.InstallerStep = "Renamed Folder..";
                    InstallerCore.Core.InstallProgress = 85;

                    FileSystem.CopyDirectory($@"{_AppPath}/temp/bp3", $@"{_DirectoryToCheck}/addons/bp3", true);

                    InstallerCore.Core.InstallerStep = "Move done!";
                    InstallerCore.Core.InstallProgress = 90;

                    InstallerCore.Core.InstallerStep = "Cleaning up..";
                    Directory.Delete($@"{_AppPath}/temp/bp3", true);
                    Directory.Delete($@"{_AppPath}/temp/bp3-main", true);
                    File.Delete($@"{_AppPath}/bp3.zip");

                    InstallerCore.Core.InstallerStep = "Done!";
                    InstallerCore.Core.InstallProgress = 100;

                    return Task.FromResult(Task.CompletedTask);
                }          

            }

            return Task.FromResult(Task.CompletedTask);
        }

        public Task<Task> InstallBp3()
        {
            return Task.Factory.StartNew(async () => { await InstallBp3Internal(); });
        }


    }
}

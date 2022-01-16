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

        private static string _AppLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
        private readonly string _AppPath = System.IO.Path.GetDirectoryName(_AppLocation);
        private readonly string _DirectoryToCheck;

        public InstallMgr(string dir)
        {
            this._DirectoryToCheck = dir;
        }

        private Task UpdateProgressBar(string msg, byte progress)
        {
            Core.InstallerStep = msg;
            Core.InstallProgress = progress;

            return Task.CompletedTask;
        }

        private async Task<Task> InstallBp3Internal()
        {

            await this.UpdateProgressBar("Verifying user directory..", 5);

            if (Core.DirectoryFound)
            {

                await this.UpdateProgressBar("Directory verified!", 10);

                await this.UpdateProgressBar("Instantiating archive manager..", 15);
                ArchiveManager.ArchiveMgr archiveMgr = new();

                await this.UpdateProgressBar("Downloading most recent archive..", 20);
                await archiveMgr.DownloadArchive();

                await this.UpdateProgressBar("Verifying archive download..", 50);
                await archiveMgr.CheckForArchive();

                if (Core.ArchiveFound)
                {

                    await this.UpdateProgressBar("Archive verified, extracting to temp..", 70);
                    ZipFile.ExtractToDirectory($@"{_AppPath}/bp3.zip", $@"{_AppPath}/temp", true);

                    await this.UpdateProgressBar("Preparing file move..", 80);
                    FileSystem.CopyDirectory($@"{_AppPath}/temp/bp3-main", $@"{_AppPath}/temp/bp3", true);

                    await this.UpdateProgressBar("Renamed folder..", 85);
                    FileSystem.CopyDirectory($@"{_AppPath}/temp/bp3", $@"{_DirectoryToCheck}/addons/bp3", true);

                    await this.UpdateProgressBar("Move done!", 90);

                    await this.UpdateProgressBar("Cleaning up..", 90);
                    Directory.Delete($@"{_AppPath}/temp/bp3", true);
                    Directory.Delete($@"{_AppPath}/temp/bp3-main", true);
                    File.Delete($@"{_AppPath}/bp3.zip");

                    await this.UpdateProgressBar("Done!", 100);

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

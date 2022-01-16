using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bp3Installer.InstallerCore.CheckDirectoryManager
{
    internal class CheckDirectoryMgr
    {
        private readonly string _DirectoryToCheck;

        public CheckDirectoryMgr(string dir)
        {
            this._DirectoryToCheck = dir;
        }

        private Task<Task> CheckDirectoryInternal()
        {
            if (String.IsNullOrEmpty(_DirectoryToCheck))
                return Task.FromResult(Task.CompletedTask);

            if(Directory.Exists(_DirectoryToCheck))
            {
                Core.DirectoryFound = true;
                if(Directory.Exists($@"{_DirectoryToCheck}/addons/bp3"))
                {
                    Core.ExistingInstallFound = true;

                }
                else
                {
                    Core.ExistingInstallFound = false;
                }
            }
            else
            {
                Core.DirectoryFound = false;
            }

            return Task.FromResult(Task.CompletedTask);
        }

        public  Task<Task> CheckForUserDirectory()
        {
            return Task.Factory.StartNew(async () => { await CheckDirectoryInternal(); });
        }

    }
}

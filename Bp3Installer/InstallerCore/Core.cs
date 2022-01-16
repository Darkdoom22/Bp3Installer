using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bp3Installer.InstallerCore
{
    internal static class Core
    {
        private static readonly object _Lock = new object();
        private static bool _DirectoryFound;
        private static bool _ArchiveFound;
        private static bool _InstallFinished;
        private static bool _ExistingInstallFound;
        private static string _UserProvidedDirectory = "";
        private static string _InstallerStep = "Ready";
        private static byte _InstallProgress = 0;

        public static bool DirectoryFound { get { lock (_Lock) { return _DirectoryFound; } } set { lock (_Lock) { _DirectoryFound = value; } } }
        public static bool ArchiveFound { get { lock (_Lock) { return _ArchiveFound; } } set { lock (_Lock) { _ArchiveFound = value; } } }   
        public static bool InstallFinished { get { lock (_Lock) { return _InstallFinished; } } set { lock (_Lock) { _InstallFinished = value; } } }
        public static bool ExistingInstallFound { get { lock (_Lock) { return _ExistingInstallFound; } } set { lock (_Lock) { _ExistingInstallFound = value; } } }
        public static string UserProvidedDirectory { get { lock (_Lock) { return _UserProvidedDirectory; } } set { lock (_Lock) { _UserProvidedDirectory = value; } } }
        public static string InstallerStep { get { lock (_Lock) { return _InstallerStep; } } set { lock (_Lock) { _InstallerStep = value; } } }
        public static byte InstallProgress { get { lock (_Lock) { return _InstallProgress; } } set { lock (_Lock) { _InstallProgress = value; } } }
    }
}

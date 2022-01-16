using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bp3Installer.InstallerCore.CheckDirectoryManager;
using Bp3Installer.InstallerCore.ArchiveManager;

namespace Bp3Installer.Pages
{
    /// <summary>
    /// Interaction logic for MainInstallerControl.xaml
    /// </summary>
    public partial class MainInstallerControl : UserControl
    {
        private BackgroundWorker _UpdateDirectoryStringWorker = new BackgroundWorker();
        private BackgroundWorker _UpdateExistingInstallWorker = new BackgroundWorker();
        private BackgroundWorker _UpdateDirectoryFoundWorker = new BackgroundWorker();
        private BackgroundWorker _UpdateProgressBarWorker = new BackgroundWorker();

        public MainInstallerControl()
        {
            InitializeComponent();
            this._UpdateDirectoryStringWorker.WorkerSupportsCancellation = true;
            this._UpdateDirectoryStringWorker.DoWork += UpdateDirectoryStringWork;
            this._UpdateDirectoryStringWorker.RunWorkerAsync();

            this._UpdateExistingInstallWorker.WorkerSupportsCancellation = true;
            this._UpdateExistingInstallWorker.DoWork += UpdateExistingInstallWork;
            this._UpdateExistingInstallWorker.RunWorkerAsync();

            this._UpdateDirectoryFoundWorker.WorkerSupportsCancellation = true;
            this._UpdateDirectoryFoundWorker.DoWork += UpdateDirectoryFoundWork;
            this._UpdateDirectoryFoundWorker.RunWorkerAsync();

            this._UpdateProgressBarWorker.WorkerSupportsCancellation = true;
            this._UpdateProgressBarWorker.DoWork += UpdateProgressBarWork;
            this._UpdateProgressBarWorker.RunWorkerAsync();

        }

        private async void UpdateDirectoryStringWork(object? sender, DoWorkEventArgs e)
        {
            while(!this._UpdateDirectoryStringWorker.CancellationPending)
            {
                await Dispatcher.BeginInvoke(new Action(() => {
                    InstallerCore.Core.UserProvidedDirectory = this.ProvidedDirectoryTextBox.Text;
                }));

                Thread.Sleep(16);
            }
        }

        private async void UpdateExistingInstallWork(object? sender, DoWorkEventArgs e)
        {
            while(!this._UpdateExistingInstallWorker.CancellationPending)
            {
                if (InstallerCore.Core.ExistingInstallFound)
                {
                    await Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.ExistingInstallLabel.Background = new SolidColorBrush(Color.FromRgb(45, 250, 171));
                    }));
                }
                else
                {
                    await Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.ExistingInstallLabel.Background = new SolidColorBrush(Color.FromRgb(255, 32, 15));
                    }));
                }

                Thread.Sleep(16);
            }
        }

        private async void UpdateDirectoryFoundWork(object? sender, DoWorkEventArgs e)
        {
            while(!this._UpdateDirectoryFoundWorker.CancellationPending)
            {
                CheckDirectoryMgr directoryMgr = new CheckDirectoryMgr(InstallerCore.Core.UserProvidedDirectory);
                await directoryMgr.CheckForUserDirectory();

                if (InstallerCore.Core.DirectoryFound)
                {
                    await Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.UserDirectoryDetectedLabel.Background = new SolidColorBrush(Color.FromRgb(45, 250, 171));
                    }));

                }
                else
                {
                    await Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.UserDirectoryDetectedLabel.Background = new SolidColorBrush(Color.FromRgb(255, 32, 15));
                    }));
                }

                Thread.Sleep(16);
            }
        }

        private async void UpdateProgressBarWork(object? sender, DoWorkEventArgs e)
        {
            while(!this._UpdateProgressBarWorker.CancellationPending)
            {
                await Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.ProgBarLabel.Content = InstallerCore.Core.InstallerStep;
                    this.InstallProgress.Value = InstallerCore.Core.InstallProgress;
                }));

                Thread.Sleep(16);
            }
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            if (InstallerCore.Core.ExistingInstallFound && InstallerCore.Core.DirectoryFound)
            {
                if (MessageBox.Show("You have an existing bp3, are you sure you want to overwrite?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    return;
                }
                else
                {
                    InstallerCore.InstallManager.InstallMgr installMgr = new(InstallerCore.Core.UserProvidedDirectory);
                    installMgr.InstallBp3();
                }
            }
            else if (InstallerCore.Core.DirectoryFound)
            {
                InstallerCore.InstallManager.InstallMgr installMgr = new(InstallerCore.Core.UserProvidedDirectory);
                installMgr.InstallBp3();
            }
            else
            {
                MessageBox.Show("File path isn't found, double check your windower path is input correctly!");
            }
        }
    }
}

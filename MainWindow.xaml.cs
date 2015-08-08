using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace VisualADB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private String output = "", kernelPath = "";

        public MainWindow()
        {
            startServer();
            InitializeComponent();
        }

        private void startServer()
        {
            Process proc = new Process();
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName="adb",
                Arguments="start-server",
                CreateNoWindow=true
            };
            proc.StartInfo = psi;
            proc.Start();
            proc.WaitForExit();
        }

        private void reboot(object sender, RoutedEventArgs e)
        {
            output = "";

            if ((bool)radSystem.IsChecked)
            {
                Process proc = new Process();
                proc.StartInfo.FileName = "adb.exe";
                proc.StartInfo.Arguments = "reboot";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();
                output += proc.StandardError.ReadToEnd();
                proc.WaitForExit();
                System.Diagnostics.Debug.WriteLine(output);
                if (output == "") { output = "Rebooted succsessfully!\n"; };
                outputADB.Text = outputADB.Text + "adb>" + output;
            }
            else if ((bool)radRecovery.IsChecked)
            {
                Process proc = new Process();
                proc.StartInfo.FileName = "adb.exe";
                proc.StartInfo.Arguments = "reboot recovery";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();
                output += proc.StandardError.ReadToEnd();
                proc.WaitForExit();
                System.Diagnostics.Debug.WriteLine(output);
                if (output == "") { output = "Rebooted succsessfully!\n"; };
                outputADB.Text = outputADB.Text + "adb>" + output;
            }
            else if ((bool)radBootloader.IsChecked)
            {
                Process proc = new Process();
                proc.StartInfo.FileName = "adb.exe";
                proc.StartInfo.Arguments = "reboot bootloader";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();
                output += proc.StandardError.ReadToEnd();
                proc.WaitForExit();
                System.Diagnostics.Debug.WriteLine(output);
                if (output == "") { output = "Rebooted succsessfully!\n"; };
                outputADB.Text = outputADB.Text + "adb>" + output;
            }
        }

        private void fbReboot(object sender, RoutedEventArgs e)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "fastboot.exe";
            proc.StartInfo.Arguments = "reboot";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.Start();
            if (!proc.WaitForExit(5000))
            {
                output = "No device attached!";
                proc.Kill();
            }
            else
            {
                output = "Rebooting...";
            }
            System.Diagnostics.Debug.WriteLine(output);
            outputADB.Text = outputADB.Text + "FB>" + output + "\n";
        }

        private void chooseKernelFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = ".img Files | *.img";
            ofd.ShowDialog();
            String path = ofd.FileName;
            if (path != null && path.Length>0)
            {
                kernelPathText.Text = path;
                kernelPath = path;
            }
        }

        private void flashKernel(object sender, RoutedEventArgs e)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "fastboot.exe";
            proc.StartInfo.Arguments = "flash boot" + kernelPath;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.Start();
            if (!proc.WaitForExit(5000))
            {
                output = "No device attached!";
                proc.Kill();
            }
            else if (kernelPath=="") {
                output = "Please enter a correct path!";
            }
            else
            {
                output = "Flashing kernel...\nRebooting...";
            }
            System.Diagnostics.Debug.WriteLine(output);
            outputADB.Text = outputADB.Text + "FB>" + output + "\n";
        }
    }
}

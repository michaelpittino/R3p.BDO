using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Forms;
using FluentFTP;
using Microsoft.WindowsAPICodePack.Dialogs;
using R3p.bdo.GUIloader.Properties;


namespace R3p.bdo.GUIloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static int ClientVersion = 0;
        private static bool supported = false;
        
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            btnStart.Visibility = Visibility.Hidden;

            this.Left = 0;
            this.Top = 0;

            LogAppend("Loading Settings");

            cbAutoUpdate.IsChecked = Settings.Default.autoupdate;

            if (Settings.Default.path == "")
                tbPath.Text = "Click to choose folder of Black Desert Online";
            else
            {
                tbPath.Text = Settings.Default.path;

                Task.Factory.StartNew(GetClientVersion);
            }

            if (Settings.Default.key == "")
                tbKey.Text = "Please enter your key!";
            else
            {
                tbKey.Text = Settings.Default.key;
            }

            if (Settings.Default.user == "")
                tbUser.Text = "Please enter your username!";
            else
            {
                tbUser.Text = Settings.Default.user;
            }

            Task.Factory.StartNew(UpdateCheck);

            Task.Factory.StartNew(() => { 

            while(isVersionChecking || isUpdateChecking)
                Thread.Sleep(50);

            if ((supported && Settings.Default.autoupdate) || (!supported && !Settings.Default.autoupdate) || (supported && !Settings.Default.autoupdate))
            {
                if (!IsAdministrator())
                {
                    LogAppend("Start the Loader as Administrator!");
                }
                else
                {
                        //LogAppend("Extracting Externals");
                        //ExtractExternals();

                        if (isBDOrunning() && !canOpenHandle())
                    {
                        LogAppend("Restart the Game-Client!");

                            while(isBDOrunning())
                                Thread.Sleep(100);

                            //if(isXCORONArunning())
                            //    Process.GetProcessesByName("xcoronahost.xem").FirstOrDefault().Kill();
                    }
                    else if (isBDOrunning() && canOpenHandle())
                    {
                        LogAppend("Game-Client is ready.");
                            LogAppend("Press 'Start' when you are logged in with a character!");

                            StartVisibility(Visibility.Visible);
                        }

                    if (!isBDOrunning())
                    {
                            //if (isXCORONArunning())
                            //    Process.GetProcessesByName("xcoronahost.xem").FirstOrDefault().Kill();

                            //if (!isBYPASSrunning())
                            //{
                            //    LogAppend("Start Bypass.exe");

                            //        while(!isBYPASSrunning())
                            //            Thread.Sleep(50);
                            //}

                            LogAppend("Please start the Game-Client");

                            while (!isBDOrunning())
                                Thread.Sleep(1);
                            
                            //Thread.Sleep(100);
                            while(!isXCORONArunning())
                                Thread.Sleep(1);

                        //if (Settings.Default.user != "" && Settings.Default.key != "")
                        //{
                            Run(new string[] { ClientVersion.ToString(), "-", "-"});

                            Thread.Sleep(3000);

                            Environment.Exit(0);
                        //}

                            //while(!isXCORONArunning())
                            //    Thread.Sleep(10);

                            //DoInject(Process.GetProcessesByName("xcoronahost.xem").FirstOrDefault(), new FileInfo(System.IO.Path.GetTempPath() + @"R3p.bdo\Xigncode_Bypass.dll").FullName);

                            Thread.Sleep(15000);

                        if (isBDOrunning() && canOpenHandle())
                        {
                                //LogAppend("Xigncode disabled!");
                                LogAppend("Press 'Start' when you are logged in with a character!");

                                StartVisibility(Visibility.Visible);
                            }
                        else if (isBDOrunning() && !canOpenHandle())
                        {
                                LogAppend("Failed to Init. Restart the Loader + Game-Client");

                                Thread.Sleep(3000);
                                Environment.Exit(0);
                            }
                        
                    }
                }
            }
            });
        }

        private static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        private bool isBYPASSrunning()
        {
            return Process.GetProcessesByName("GHIx86").FirstOrDefault() != null;
        }

        private bool isBDOrunning()
        {
            return Process.GetProcessesByName("BlackDesert64").FirstOrDefault() != null;
        }

        private bool isXCORONArunning()
        {
            return Process.GetProcessesByName("xcoronahost.xem").FirstOrDefault() != null;
        }

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess,
            Int64 lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        
        private bool canOpenHandle()
        {
            try
            {
                Process p = Process.GetProcessesByName("BlackDesert64").FirstOrDefault();

                if (p == null)
                    return false;

                IntPtr h = OpenProcess(0x1F0FFF, false, p.Id);

                if (h == IntPtr.Zero)
                    return false;

                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                
                return ReadProcessMemory((int)h, 0x140000000, buffer, buffer.Length, ref bytesRead); ;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        //private void DoInject(Process p, string dllPath)
        //{
        //    // /p 9020 /f G:\XC3_BP\Xigncode_Bypass.dll /m 2 /h 1 
        //    //Thread.Sleep(0);

        //    Process.Start(System.IO.Path.GetTempPath() + @"R3p.bdo\GHIx86.exe",
        //        "/p " + p.Id + " /f " + dllPath + " /m 2 /h 1 ");

        //    while(isBYPASSrunning())
        //        Thread.Sleep(500);

        //    CleanTempFolder();
        //}

        //private void CleanTempFolder()
        //{
        //    Directory.Delete(System.IO.Path.GetTempPath() + @"R3p.bdo", true);
        //}

        public void ExecuteAsAdmin(string fileName, string[] args)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.StartInfo.Arguments = String.Join(" ", args);
            proc.Start();
        }

        private void Run(string[] args)
        {
            ExecuteAsAdmin(@".\R3p.BDO.exe", args);
        }
        
        private static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void LogAppend(string text)
        {
            tbLog.Dispatcher.Invoke(new Action(() => { tbLog.Text += text + "\n"; }));
        }

        private void StartVisibility(Visibility visibility)
        {
            btnStart.Dispatcher.Invoke(new Action(() => { btnStart.Visibility = visibility; }));
        }

        private bool isUpdateChecking = true;

        private void UpdateCheck()
        {
            if (Settings.Default.autoupdate)
            {
                LogAppend("Checking for Updates");

                using (FtpClient conn = new FtpClient())
                {
                    conn.Host = FtpCredendtials.FTPHost;
                    conn.Credentials = FtpCredendtials.FtpCredential;
                    conn.Connect();

                    List<string> dl = new List<string>();

                    foreach (string s in conn.GetNameListing("R3p.bdo/"))
                    {
                        if (s.Contains("version.txt") || s.Contains("R3p.BDO.Loader.exe"))
                            continue;

                        // load some information about the object
                        // returned from the listing...
                        bool isDirectory = conn.DirectoryExists(s);
                        DateTime modify = conn.GetModifiedTime(s);
                        long size = isDirectory ? 0 : conn.GetFileSize(s);

                        if (isDirectory)
                            continue;

                        var rawFile = s.Split('/').Last();

                        FileInfo fi = new FileInfo(@".\" + rawFile);

                        if (!fi.Exists)
                            dl.Add(s);
                        else
                        {
                            if (modify > fi.LastWriteTime)
                                dl.Add(s);
                        }

                    }

                    if (!Directory.Exists(@".\lua"))
                        Directory.CreateDirectory(@".\lua");

                    foreach (string s in conn.GetNameListing("R3p.bdo/lua/"))
                    {
                        // load some information about the object
                        // returned from the listing...
                        bool isDirectory = conn.DirectoryExists(s);
                        DateTime modify = conn.GetModifiedTime(s);
                        long size = isDirectory ? 0 : conn.GetFileSize(s);

                        var rawFile = s.Split('/').Last();

                        FileInfo fi = new FileInfo(@".\lua\" + rawFile);

                        if (!fi.Exists)
                            dl.Add(s);
                        else
                        {
                            if (modify > fi.LastWriteTime)
                                dl.Add(s);
                        }
                    }

                    if (dl.Count > 0)
                        FTP_DownloadFiles(dl);
                    else
                        LogAppend("No Updates available");
                }
            }
            isUpdateChecking = false;
        }

        private void FTP_DownloadFiles(List<string> files)
        {
            LogAppend("New Updates available. Downloading Files now.");

            using (FtpClient conn = new FtpClient())
            {
                conn.Host = FtpCredendtials.FTPHost;
                conn.Credentials = FtpCredendtials.FtpCredential;
                conn.Connect();

                var baseFiles = files.Where(x => x.Split('/')[2] != "lua");
                var luaFiles = files.Where(x => x.Split('/')[2] == "lua");

                conn.DownloadFiles(@".\", baseFiles, true, FtpVerify.None, FtpError.Stop);
                conn.DownloadFiles(@".\lua\", luaFiles, true, FtpVerify.None, FtpError.Stop);

                LogAppend("Downloaded Files\n" + String.Join("\n", files));
            }

            LogAppend("Restarting App in a moment...");

            Thread.Sleep(2000);

            Process.Start(System.Windows.Forms.Application.ExecutablePath);
            Environment.Exit(0);
        }

        private string FTP_GetFileContent(string fileName)
        {
            WebClient request = new WebClient();
            string url = FtpCredendtials.FTPurl + fileName;
            request.Credentials = FtpCredendtials.FtpCredential;

            byte[] newFileData = request.DownloadData(url);
            string fileString = System.Text.Encoding.UTF8.GetString(newFileData);

            return fileString;
        }

        private bool isVersionChecking = true;

        private void GetClientVersion()
        {
            ClientVersion = Convert.ToInt32(File.ReadAllLines(Settings.Default.path + @"\version.dat")[0]);

            int supportedVersion = 0;

            try
            {
                supportedVersion = Convert.ToInt32(FTP_GetFileContent("version.txt"));
            }
            catch (Exception err)
            {
                LogAppend("Fail at get Supported Version from FTP connection\n ERROR:" + err.Message);
            }

            if (supportedVersion == ClientVersion)
            {
                this.Dispatcher.Invoke(new Action(() => { this.Title = "R3p.BDO - Client (" + ClientVersion + ") supported"; }));
                
                supported = true;
            }
            else
            {
                this.Dispatcher.Invoke(new Action(() => { this.Title = "R3p.BDO - Client (" + ClientVersion + ") unsupported. Pls wait for an update!"; }));
                
                supported = false;
            }

            isVersionChecking = false;
        }

        private void tbPath_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                var fVersion = Directory.GetFiles(dialog.FileName, "version.dat").FirstOrDefault();

                if (fVersion != null)
                {
                    Settings.Default.path = dialog.FileName;
                    Settings.Default.Save();

                    tbPath.Text = Settings.Default.path;

                    GetClientVersion();
                }
                else
                {
                    tbPath.Text = "Wrong folder! Choose the Root where version.dat is located!";
                }
            }
        }
        
        private void label_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=FZG2ELLF6RD46");
        }

        //private static void ExtractExternals()
        //{
        //    byte[] _injector = Properties.Resources.GHIx86;
        //    byte[] _xcbp = Properties.Resources.Xigncode_Bypass;

        //    string tempPath = System.IO.Path.GetTempPath() + @"R3p.bdo";
        //    //string tempPath = @"./";

        //    if (!Directory.Exists(tempPath))
        //        Directory.CreateDirectory(tempPath);

        //    //if (File.Exists(tempPath + @"\Xigncode_Bypass.dll"))
        //    //{
        //    //    if (IsFileLocked(new FileInfo(tempPath + @"\Xigncode_Bypass.dll")))
        //    //        return;
        //    //}

        //    File.WriteAllBytes(tempPath + @"\GHIx86.exe", _injector);
        //    File.WriteAllBytes(tempPath + @"\Xigncode_Bypass.dll", _xcbp);
        //}

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.user != "" && Settings.Default.key != "")
            {
                Run(new string[] {ClientVersion.ToString(), Settings.Default.user, Settings.Default.key});

                Thread.Sleep(3000);

                Environment.Exit(0);
            }
        }

        private void tbKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbKey.Text != "Please enter your key!")
            {
                Settings.Default.key = tbKey.Text;
                Settings.Default.Save();
            }
        }

        private void tbUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbUser.Text != "Please enter your username!")
            {
                Settings.Default.user = tbUser.Text;
                Settings.Default.Save();
            }
        }
        
        private void cbAutoUpdate_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.autoupdate = cbAutoUpdate.IsChecked.Value;
            Settings.Default.Save();
        }
    }
}

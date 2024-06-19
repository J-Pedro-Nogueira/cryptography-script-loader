using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Collections.Generic;
using System.IO;
using static System.Net.WebRequestMethods;

namespace Cripto1
{

    public partial class MainWindow : Window
    {

        private string input_file_path;
        private string key_file_path;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonQuit_Click(object sender, RoutedEventArgs e)
        {
            Window mainWindow = Application.Current.MainWindow;

            //if main window exists, handle closing event //TODO: messy
            if (mainWindow != null)
            {
                mainWindow.Closing += (s, args) =>
                {
                    args.Cancel = false;
                };

                mainWindow.Close();
            }
            Application.Current.Shutdown();
        }

        private void buttonEncrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (string xlsFile in xlsListing.Items)
                {
                    if (Path.GetExtension(xlsFile).Equals(".xls", StringComparison.OrdinalIgnoreCase))
                    {
                        input_file_path = xlsFile;
                        Program.encrypt_excel_file_with_key("functions", input_file_path, key_file_path, textBlockDllPath.Text, textBlockScriptPath.Text);
                        MessageBox.Show(Path.GetFileName(xlsFile) + " " + "encrypted successfully");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Encryption Error. Already Encrypted?");
            }

        }
           

        private void buttonDecrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (string xlsFile in xlsListing.Items)
                {
                    if (Path.GetExtension(xlsFile).Equals(".xls", StringComparison.OrdinalIgnoreCase))
                    {
                        input_file_path = xlsFile;
                        Program.decript_excel_file_with_key("functions", input_file_path, key_file_path, textBlockDllPath.Text, textBlockScriptPath.Text);
                        MessageBox.Show(Path.GetFileName(xlsFile) + " " + "decrypted successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Encryption Error. Wrong Encryption Key?");
            }
        }

        private void buttonChooseKey_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.InitialDirectory = @"C:\";

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                key_file_path = selectedFilePath;
                textBlockKeyPath.Text = selectedFilePath;

            }
        }

        private void buttonChooseTargetDirectory_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.ValidateNames = false;
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.FileName = "Select Folder";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                PopulateXlsFilesList(selectedDirectory);
            }
        }

        private void PopulateXlsFilesList(string directory)
        {
            List<string> xlsFiles = new List<string>();

            try
            {
                string[] files = Directory.GetFiles(directory);

                xlsFiles.AddRange(files);


                xlsListing.ItemsSource = xlsFiles;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void buttonCreateKey_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.ValidateNames = false;
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.FileName = "Select Folder";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                key_file_path = selectedDirectory + "\\cipher_file";
                Program.create_key("functions", key_file_path, textBlockDllPath.Text, textBlockScriptPath.Text);
                MessageBox.Show("Key Generated.");
            }
        }

        private void buttonChooseDll_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.InitialDirectory = @"C:\";

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                textBlockDllPath.Text = selectedFilePath;

            }
        }

        private void buttonChooseScriptPath_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.ValidateNames = false;
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.FileName = "Select Folder";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                textBlockScriptPath.Text = selectedDirectory;
            }
        }

    }
}

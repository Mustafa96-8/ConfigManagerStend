using ConfigManagerStend.Logic;
using ConfigManagerStend.Models;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace ConfigManagerStend
{
    public partial class MainWindow : Window
    {
        private ParserModel parser = new();
        private List<ModuleModel> modules = new List<ModuleModel>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseStand_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo($"{dialog.FileName}\\config\\");
                try
                {
                    DirectoryInfo[] ConfigDir = hdDirectoryInWhichToSearch.GetDirectories("*" + "-delosrv");

                    FileInfo? fileInfo = new FileInfo(ConfigDir[0].FullName + "\\a\\Settings");

                    string? directoryPath = fileInfo.FullName;

                    if (!string.IsNullOrEmpty(directoryPath))
                    {
                        directoryPath += "\\";
                        BrowseStandTextBox.Text = dialog.FileName;
                        parser.JsonPathSave = directoryPath;
                    }
                }
                catch 
                {
                    BrowseStandTextBox.Text = "Файл или директория не найдены";
                    return;
                }
            }
        }
        private void BrowseDirectory_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(dialog.FileName);

                FileInfo[] filesInfo = hdDirectoryInWhichToSearch.GetFiles("*" + ".dll");
                if (filesInfo.Length == 0)
                {
                    MessageBox.Show("В указанной директории не найден/-ы файл/-ы .dll");
                    return;
                }
                FileInfo[] metadataFilesInfo = hdDirectoryInWhichToSearch.GetFiles("metadata.json");
                if (metadataFilesInfo.Length == 0)
                {
                    MessageBox.Show("В указанной директории не найден файл \"metadata.json\"");
                    return;
                }
                if (metadataFilesInfo.Length > 1)
                {
                    MessageBox.Show("В указанной директории несколько файлов \"metadata.json\"\nПроверьте директорию и очистите лишние файлы.\nАвтоматическая замена иначе невозможна.");
                    return;
                }

                FileInfo metadataFile = metadataFilesInfo[0];

                parser.JsonFilePath = metadataFile.FullName;
                parser.DebugPath = dialog.FileName;
                
                directoryPathTextBox.Text = parser.DebugPath;
            }
        }

        private void SubstitutionBtn_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(parser.DebugPath) ||
               string.IsNullOrEmpty(parser.JsonFilePath) ||
               string.IsNullOrEmpty(parser.JsonPathSave))
            {
                MessageBox.Show("Невыбран путь до Стенда или путь до папки с заменяемым Модулем");
                return;
            }

            ParserLogic logic = new();
            Status result = logic.ParserFile(parser);
            string message = result.Message + result.SystemInfo;
            MessageBox.Show(message);
        }

        private void findModulesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(parser.JsonPathSave))
            {
                MessageBox.Show("Не выбран путь до Стенда");
                return;
            }
            bool isExist = File.Exists(parser.JsonPathSave);

            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(parser.JsonPathSave);

            FileInfo[] filesInfo = hdDirectoryInWhichToSearch.GetFiles("_*" + ".json");

            modules = filesInfo.Select(f => new ModuleModel { Name = f.Name, Path = f.FullName, Date = f.LastWriteTime.ToString("G") }).ToList();
            moduleList.ItemsSource = modules;
        }

        private void DeleteModulesBtn_Click(object sender, RoutedEventArgs e)
        {
            var info = ManagerLogic.DeleteExternalModules(moduleList.SelectedItems);
            findModulesBtn_Click(sender, e);
            MessageBox.Show(info.Message + info.SystemInfo);
        }
    }
}
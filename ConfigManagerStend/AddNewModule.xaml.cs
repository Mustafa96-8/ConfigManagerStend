using ConfigManagerStend.Infrastructure.Commands;
using ConfigManagerStend.Logic;
using ConfigManagerStend.Models;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace ConfigManagerStend;
/// <summary>
/// Логика взаимодействия для AddNewModule.xaml
/// </summary>
public partial class AddNewModule : Window
{
    private ParserModel parser = new();

    public AddNewModule()
    {
        InitializeComponent(); 
        DataContext = new DetailsInfoCommand();
    }
    private void BrowseStand_Click(object sender, RoutedEventArgs e)
    {
        CommonOpenFileDialog dialog = new();
        dialog.IsFolderPicker = true;

        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            string[] parts = dialog.FileName.Split("\\");
            parser.NameStend = parts[parts.Length - 1];

            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo($"{dialog.FileName}\\config\\");
            try
            {
                DirectoryInfo[] ConfigDir = hdDirectoryInWhichToSearch.GetDirectories("*" + "-delosrv");

                FileInfo? fileInfo = new FileInfo(ConfigDir[0].FullName + "\\a\\Settings");

                string? directoryPath = fileInfo.FullName;

                if (!string.IsNullOrEmpty(directoryPath))
                {
                    directoryPath += "\\";
                    BrowseStandTextBox.Text = directoryPath;
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

    private async void SubstitutionBtn_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(parser.DebugPath) ||
           string.IsNullOrEmpty(parser.JsonFilePath) ||
           string.IsNullOrEmpty(parser.JsonPathSave))
        {
            MessageBox.Show("Невыбран исходный файл или путь до папки Debug");
            return;
        }

        ParserLogic logic = new();
        Status result = await logic.ParserFile(parser);
        string message = result.Message + result.SystemInfo;
        MessageBox.Show(message);
    }
}

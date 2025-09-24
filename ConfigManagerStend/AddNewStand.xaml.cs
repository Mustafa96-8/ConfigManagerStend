using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConfigManagerStend.Infrastructure.Commands;
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
using ConfigManagerStend.Infrastructure.Enums;

namespace ConfigManagerStend;
/// <summary>
/// Логика взаимодействия для AddNewStand.xaml
/// </summary>
public partial class AddNewStand : Window
{
    private bool isValid = false;
    private DetailsInfoCommand _detailsCommand;

    internal AddNewStand(DetailsInfoCommand detailsInfoCommand)
    {
        InitializeComponent();
        _detailsCommand = detailsInfoCommand;
    }
    /// <summary>
    /// Обработка процесса выбора директории для добавления стенда
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BrowseStand_Click(object sender, RoutedEventArgs e)
    {
        addStandBtn.IsEnabled = false;
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
                    addStandBtn.IsEnabled = true;
                }
            }
            catch
            {
                addStandBtn.IsEnabled = false;
                BrowseStandTextBox.Text = "Файл или директория не найдены";
                return;
            }
        }
    }

    private async void addStandBtn_Click(object sender, RoutedEventArgs e)
    {
        var result = await _detailsCommand.AddNewStand(BrowseStandTextBox.Text);
        ResultLabel.Content = result.ToString();
    }
}

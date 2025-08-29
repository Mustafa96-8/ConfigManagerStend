using ConfigManagerStend.Infrastructure.Commands;
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

namespace ConfigManagerStend.Forms
{
    /// <summary>
    /// Interaction logic for StendWindow.xaml
    /// </summary>
    public partial class StendWindow : Window
    {
        private StendWindCommand _stendWindCommand;
        public StendWindow()
        {
            InitializeComponent();
            _stendWindCommand = new StendWindCommand();
            DataContext = _stendWindCommand;
            SettingsView();
        }

        private void SettingsView()
        {
            _stendWindCommand.LoadProjectsAsync().Wait();
        }

        private void TeamProjectTxt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             _stendWindCommand.LoadReposWithProjectAsync().Wait();
        }

        private void BrowseStandBtn_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                BrowseStandTextBox.Text = dialog.FileName;
            }
        }
    }
}

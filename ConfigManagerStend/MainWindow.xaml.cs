using ConfigManagerStend.Domain;
using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Forms;
using ConfigManagerStend.Infrastructure.Commands;
using ConfigManagerStend.Logic;
using ConfigManagerStend.Models;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Reflection;
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
namespace ConfigManagerStend
{
    public partial class MainWindow : Window
    {
        private DetailsInfoCommand _detailsCommand;

        public static ListView AllModules = new ();

        public static ListView AllStands = new();

        public MainWindow() 
        {
            InitializeComponent();
            _detailsCommand = new DetailsInfoCommand();
            DataContext = _detailsCommand;
            _detailsCommand.LoadStandsAsync().Wait();
            AllModules = ModuleListView;
            AllStands = StandListView;
        }

        private void openStandWin_Click(object sender, RoutedEventArgs e)
        {
            StendWindow sw = new();
            sw.ShowDialog();
        }

        private void RefreshStandList_Click(object sender, RoutedEventArgs e)
        {
            _detailsCommand.UpdateGlobalDisplay();
        }

        private void RefreshListViewBtn_Click(object sender, RoutedEventArgs e)
        {
            _detailsCommand.UpdateModuleDisplay();
        }

    }
}
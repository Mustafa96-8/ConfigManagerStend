using ConfigManagerStend.Domain;
using ConfigManagerStend.Domain.Entities;
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
        private ParserModel parser = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DetailsInfoCommand();
        }

        
    }
}
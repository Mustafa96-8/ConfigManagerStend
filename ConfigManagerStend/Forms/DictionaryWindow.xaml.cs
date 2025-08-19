using ConfigManagerStend.Infrastructure.Commands;
using ConfigManagerStend.Infrastructure.Enums;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for DictionaryWindow.xaml
    /// </summary>
    public partial class DictionaryWindow : Window
    {
        public static ListView AllProjects;
        public static ListView AllRepos;
        private readonly DictionaryType _typeWindow;
      
        private DictionaryCommand _dictionaryCommand;
        public DictionaryWindow(DictionaryType typeWindow)
        {
            InitializeComponent();
            _typeWindow = typeWindow;
            _dictionaryCommand = new DictionaryCommand();
            DataContext = _dictionaryCommand;
            SettingViewWindow();
        }

        private void SettingViewWindow()
        {
            switch (_typeWindow)
            {
                
                case DictionaryType.Project:
                    this.Title = "Словарь. Проекты";
                    ProjectStackPanel.Visibility = Visibility.Visible;
                    _dictionaryCommand.LoadProjectsAsync().Wait();
                    AllProjects = ViewProjects;
                    break;
                case DictionaryType.Repos:
                    this.Title = "Словарь. Репозитории";
                    ReposStackPanel.Visibility = Visibility.Visible;
                    _dictionaryCommand.LoadReposAsync().Wait();
                    AllRepos = ViewRepos;
                    break;
                default:
                    this.Title = "Словарь. Ошибка формирования окна !!!";
                    break;
            }
        }
    }
}

using ConfigManagerStend.Infrastructure.Commands;
using ConfigManagerStend.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for AddEditDictionaryWindow.xaml
    /// </summary>
    public partial class AddEditDictionaryWindow : Window
    {
        private readonly DictionaryType _typeWindow;
        private readonly WorkMode _workMode;

        private DictionaryCommand _dictionaryCommand;
       
        public AddEditDictionaryWindow(DictionaryType typeWindow, WorkMode workMode)
        {
            InitializeComponent();
            _typeWindow = typeWindow;
            _workMode = workMode;
            _dictionaryCommand = new DictionaryCommand(_typeWindow);
            DataContext = _dictionaryCommand;
            SettingViewWindow();
        }

        private void SettingViewWindow()
        {
            switch (_typeWindow)
            {

                case DictionaryType.Project:
                    this.Title = (_workMode == WorkMode.CreateMode) ? "Словарь. Добавить проект" : "Словарь. Изменить проект";
                    ProjectArea.Visibility = Visibility.Visible;
                   
                    break;
                case DictionaryType.Repos:
                    this.Title = (_workMode == WorkMode.CreateMode) ? "Словарь. Добавить репозиторий" : "Словарь. Изменить репозиторий";
                    ReposArea.Visibility = Visibility.Visible;
                    _dictionaryCommand.LoadProjectsAsync().Wait();
                    break;
                default:
                    this.Title = "Словарь. Ошибка формирования окна !!!";
                    break;
            }
        }
    }
}

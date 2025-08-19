using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Forms;
using ConfigManagerStend.Infrastructure.Enums;
using ConfigManagerStend.Infrastructure.Services;
using ConfigManagerStend.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace ConfigManagerStend.Infrastructure.Commands
{
   
    internal class DictionaryCommand : INotifyPropertyChanged
    {
        private readonly DictionaryType _typeWindow;

        internal DictionaryCommand(DictionaryType typeWindow)
        {
            _typeWindow = typeWindow;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #region initialization prop
        public static TeamProject? SelectedProject { get; set; }
        public static BuildDefinition? SelectedRepo { get; set; }
       
        public static string? NameProject { get; set; }

        public static TeamProject? SelectListProject { get; set; }
        public static string? NameRepo { get; set; }

        #endregion
        #region GENERAL COMMAND

        //Открытие окна добавление нового проекта или репозитория
        private RelayCommand _openAddWindow;
        public RelayCommand OpenAddWindow
        {
            get
            {
                return _openAddWindow ?? new(obj =>
                {
                    Window wnd = obj as Window;
                    AddEditDictionaryWindow aedw = new(_typeWindow, WorkMode.CreateMode);
                    wnd.Close();
                    aedw.ShowDialog();
                   
                });
            }
        }

        //Добавить новый проект/репозиторий
        private RelayCommand _addEdditBtn;
        public RelayCommand AddEdditBtn
        {
            get
            {
                return _addEdditBtn ?? new RelayCommand(obj =>
                {
                    Window wnd = obj as Window;
                    bool valid = ValidationsData(wnd);
                    if (valid)
                    {
                        Status result = (_typeWindow == DictionaryType.Project) ? ProjectService.CreateProjectAsync(NameProject).Result
                                                                                : ReposService.CreateRepoAsync(NameRepo, SelectListProject).Result;
                      
                        string message = result.Message + result.SystemInfo;
                        ShowMessageToUser(message);
                        GlobalNullValueProp();
                        NotifyPropertyChanged((_typeWindow == DictionaryType.Project) ? 
                                               nameof(AllProjects) : nameof(AllRepos));
                        wnd.Close();
                        OpenDictionaryWind(_typeWindow);
                    }
                });
            } 
        }


        #endregion


        #region PROJECTS

        //Вытягиваем данные из БД Проектов
        private List<TeamProject> projects;
        public List<TeamProject> AllProjects
        {
            get { return projects; }
            set { projects = value; NotifyPropertyChanged(nameof(AllProjects)); }
        }
        // Конструктор или метод инициализации для загрузки данных
        public async Task LoadProjectsAsync()
        {
            AllProjects = await ProjectService.GetAllProjectAsync();
        }

        #endregion

        #region REPOS
        //Вытягиваем данные из БД Repos
        private List<BuildDefinition> repos;
        public List<BuildDefinition> AllRepos
        {
            get { return repos; }
            set { repos = value; NotifyPropertyChanged(nameof(AllRepos)); }
        }
        // Конструктор или метод инициализации для загрузки данных
        public async Task LoadReposAsync()
        {
            AllRepos = await ReposService.GetAllReposAsync();
        }
        #endregion


        #region PRIVATE METHODS
        private bool ValidationsData(Window wnd)
        {
            switch (_typeWindow)
            {
                case DictionaryType.Project:
                    if(string.IsNullOrEmpty(NameProject))
                    {
                        ViewValidationsError(wnd, "tb_projName");
                        return false;
                    }
                    break;
                case DictionaryType.Repos:
                    if (string.IsNullOrEmpty(NameRepo))
                    {
                        ViewValidationsError(wnd, "tb_repName");
                        return false;
                    }
                    if(SelectListProject is null)
                    {
                        ShowMessageToUser("Выберите проект");
                        return false;
                    }
                    break;
            }
            return true;
        }
        private void ViewValidationsError(Window wnd, string blockName)
        {
            Control block = wnd.FindName(blockName) as Control;
            block.BorderBrush = Brushes.Red;
        }

        private void GlobalNullValueProp()
        {
            SelectedProject = null;
            SelectedRepo = null;
            SelectListProject = null;
            NameProject = null;
            NameRepo = null;
        }


        private void ShowMessageToUser(string message)
        {
            MessageView msView = new(message);
            OpenWindowCS(msView);
        }
        private void OpenWindowCS(Window window)
        {
            window.Owner = System.Windows.Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        private void OpenDictionaryWind(DictionaryType type)
        {
            DictionaryWindow dw = new(type);
            dw.ShowDialog();
        }
        #endregion
    }
}

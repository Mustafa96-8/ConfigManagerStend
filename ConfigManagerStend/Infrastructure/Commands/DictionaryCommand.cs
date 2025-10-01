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
        private readonly WorkMode _workMode;

        internal DictionaryCommand(DictionaryType typeWindow, WorkMode? workMode)
        {
            _typeWindow = typeWindow;

            _workMode = (workMode is null) ? WorkMode.CreateMode : workMode.Value;
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
                        Status result = new("", "");
                        if (_workMode == WorkMode.CreateMode)
                        {
                            result = (_typeWindow == DictionaryType.Project) ? ProjectService.CreateProjectAsync(NameProject).Result
                                                                                   : ReposService.CreateRepoAsync(NameRepo, SelectListProject).Result;
                        }
                        else if (_workMode == WorkMode.EditMode)
                        {
                            if (SelectedProject is not null)
                            {
                                result = ProjectService.EditProjectAsync(SelectedProject.Id, NameProject).Result;
                            }
                            else if (SelectedRepo is not null)
                            {
                                result = ReposService.EditRepoAsync(SelectedRepo.Id, NameRepo, SelectListProject).Result;
                            }
                        }


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
            set
            {
                projects = value; NotifyPropertyChanged(nameof(AllProjects));
                if (_workMode == WorkMode.EditMode && _typeWindow == DictionaryType.Repos)
                {
                    SetSelectedProject();
                }

            }
        }

        private void SetSelectedProject()
        {
            if (AllProjects != null && AllProjects.Count > 0)
            {
                SelectListProject = AllProjects.FirstOrDefault(p => p.Id == SelectedRepo.ProjectId);
            }
        }

        // Конструктор или метод инициализации для загрузки данных
        public async Task LoadProjectsAsync()
        {
            AllProjects = await ProjectService.GetAllProjectAsync();
        }

        private RelayCommand _editProject;
        public RelayCommand EditProject
        {
            get
            {
                return _editProject ?? new(obj =>
                {

                    if (SelectedProject is not null)
                    {
                        // Получаем активное окно
                        Window? wnd = Application.Current.Windows.OfType<Window>()
                            .FirstOrDefault(x => x.IsActive && x.Name == "DictionaryWnd");

                        AddEditDictionaryWindow aedw = new(_typeWindow, WorkMode.EditMode, SelectedProject);
                        wnd?.Close();
                        aedw.ShowDialog();
                    }
                });
            }
        }


        private RelayCommand _deleteProject;
        public RelayCommand DeleteProject
        {
            get
            {
                return _deleteProject ?? new(obj =>
                {
                    if (SelectedProject is not null)
                    {
                        Status result = ProjectService.DeleteProjectAsync(SelectedProject.Id).Result;
                        string message = result.Message + result.SystemInfo;
                        ShowMessageToUser(message);
                        GlobalNullValueProp();
                        NotifyPropertyChanged((_typeWindow == DictionaryType.Project) ?
                                               nameof(AllProjects) : nameof(AllRepos));
                        UpdateDisplay();
                    }

                });
            }
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

        private RelayCommand _editRepo;
        public RelayCommand EditRepo
        {
            get
            {
                return _editRepo ?? new(obj =>
                {

                    if (SelectedRepo is not null)
                    {
                        // Получаем активное окно
                        Window? wnd = Application.Current.Windows.OfType<Window>()
                            .FirstOrDefault(x => x.IsActive && x.Name == "DictionaryWnd");

                        AddEditDictionaryWindow aedw = new(_typeWindow, WorkMode.EditMode, null, SelectedRepo);
                        wnd?.Close();
                        aedw.ShowDialog();
                    }
                });
            }
        }

        private RelayCommand _deleteRepo;
        public RelayCommand DeleteRepo
        {
            get
            {
                return _deleteRepo ?? new(obj =>
                {
                    if (SelectedRepo is not null)
                    {
                        Status result = ReposService.DeleteRepoAsync(SelectedRepo.Id).Result;
                        string message = result.Message + result.SystemInfo;
                        ShowMessageToUser(message);
                        GlobalNullValueProp();
                        NotifyPropertyChanged((_typeWindow == DictionaryType.Project) ?
                                               nameof(AllProjects) : nameof(AllRepos));
                        UpdateDisplay();
                    }

                });
            }
        }
        #endregion


        #region PRIVATE METHODS
        private bool ValidationsData(Window wnd)
        {
            switch (_typeWindow)
            {
                case DictionaryType.Project:
                    if (string.IsNullOrEmpty(NameProject))
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
                    if (SelectListProject is null)
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

        private void UpdateDisplay()
        {
            if (_typeWindow == DictionaryType.Project)
            {
                LoadProjectsAsync().Wait();
                DictionaryWindow.AllProjects.ItemsSource = null;
                DictionaryWindow.AllProjects.Items.Clear();
                DictionaryWindow.AllProjects.ItemsSource = AllProjects;
                DictionaryWindow.AllProjects.Items.Refresh();
            }
            else if (_typeWindow == DictionaryType.Repos)
            {
                LoadReposAsync().Wait();
                DictionaryWindow.AllRepos.ItemsSource = null;
                DictionaryWindow.AllRepos.Items.Clear();
                DictionaryWindow.AllRepos.ItemsSource = AllRepos;
                DictionaryWindow.AllRepos.Items.Refresh();

            }
        }
        #endregion
    }
}

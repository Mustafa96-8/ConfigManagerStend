using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Forms;
using ConfigManagerStend.Infrastructure.Constants;
using ConfigManagerStend.Infrastructure.Enums;
using ConfigManagerStend.Infrastructure.Services;
using ConfigManagerStend.Logic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static ConfigManagerStend.Infrastructure.Constants.ConfigDbOpt;



namespace ConfigManagerStend.Infrastructure.Commands
{
    internal class StendWindCommand : INotifyPropertyChanged
    {
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

        public static string ConsoleOutput { get; set; }

        /// <summary>
        /// Наименование конфига
        /// </summary>
        public static string NameConfig { get; set; } = string.Empty;

        /// <summary>
        /// Наименование приложение
        /// </summary>
        public static string Appn { get; set; } = string.Empty;

        /// <summary>
        /// Расположение стенда
        /// </summary>
        public static string PathStend { get; set; } = string.Empty;

        /// <summary>
        /// Порт
        /// </summary>
        public static string PortA { get; set; } = string.Empty;

        /// <summary>
        /// Владелец БД
        /// </summary>
        public static string DbOwner { get; set; } = string.Empty;

        /// <summary>
        /// PlatformHostDir
        /// </summary>
        public static string PlatformHostDir { get; set; } = string.Empty;

        /// <summary>
        /// BoxSettingDir
        /// </summary>
        public static string BoxSettingDir { get; set; } = string.Empty;


        /// <summary>
        /// Наименование ветки
        /// </summary>
        public static string BranchName { get; set; } = string.Empty;

        /// <summary>
        /// Настройки БД (NO - не создавать БД, Recreate - создать/пересоздать БД)
        /// </summary>
        public static DbOpt? SettingDb { get; set; } 
        #endregion

        #region Открытие окон справочников
        //Открыть окно "Проекты"
        private RelayCommand _openShowProject;
        public RelayCommand OpenShowProject
        {
            get
            {
                return _openShowProject ?? new(obj =>
                {
                    OpenDictionaryWind(DictionaryType.Project);
                });
            }
        }
        //Открыть окно "Репо"
        private RelayCommand _openShowRepos;
        public RelayCommand OpenShowRepos
        {
            get
            {
                return _openShowRepos ?? new(obj =>
                {
                    OpenDictionaryWind(DictionaryType.Repos);
                });
            }
        }

        private void OpenDictionaryWind(DictionaryType type)
        {
            DictionaryWindow dw = new(type);
            dw.ShowDialog();
        }
        #endregion

        #region Комбобоксы
        private List<TeamProject> _projects;
        public List<TeamProject> AllProjects
        {
            get { return _projects; }
            set { _projects = value; NotifyPropertyChanged(nameof(AllProjects)); }
        }

        public async Task LoadProjectsAsync()
        {
            AllProjects = await ProjectService.GetAllProjectAsync();
        }

        private List<BuildDefinition> _repos;

        public List<BuildDefinition> Repos
        {
            get { return _repos; }
            set { _repos = value; NotifyPropertyChanged(nameof(Repos)); }
        }

        public async Task LoadReposWithProjectAsync()
        {
            if (SelectedProject is not null)
            {
                Repos = await ReposService.GetReposWithProjectAsync(SelectedProject.Id);
            }

        }

        private List<DbOpt> _dbOpts;
        public List<DbOpt> DbOpts
        {
            get { return _dbOpts; }
            set { _dbOpts = value; NotifyPropertyChanged(nameof(DbOpts)); }
        }

        public void LoadDbSettings()
        {
            ConfigDbOpt configDbOpt = new();
            DbOpts = configDbOpt.All;
        }
        #endregion


        #region COMMANDS
        private RelayCommand _createStend;
        public RelayCommand CreateStend
        {
            get
            {
                return _createStend ?? new(obj =>
                {
                    // Получаем активное окно
                    Window? wnd = Application.Current.Windows.OfType<Window>()
                        .FirstOrDefault(x => x.IsActive && x.Name == "StendConfigWND");
                   
                    bool isValidData = ValidationsData(wnd, false);

                    if (isValidData)
                    {
                        ConfigStend config = MappingModel();
                       
                        // Получаем TextBox для вывода консоли
                        var consoleOutput = wnd?.FindName("ConsoleOutput") as TextBox;
                        consoleOutput.Visibility = Visibility.Visible;

                        StendLogic.CreateStend(config,
                            output => Application.Current.Dispatcher.Invoke(() =>
                            {
                                consoleOutput.Text += output + Environment.NewLine;
                                consoleOutput.ScrollToEnd();
                            }),
                            error => Application.Current.Dispatcher.Invoke(() =>
                            {
                                consoleOutput.Text += error + Environment.NewLine;
                                consoleOutput.ScrollToEnd();
                            })
                        );
                    }
                    SetGlobalProprtyParams();
                });
            }
        }
        #endregion 


        #region PRIVATE METHODS
        private static void SetGlobalProprtyParams()
        {
            NameConfig = string.Empty;
            Appn = string.Empty;
            PathStend = string.Empty;
            PortA = string.Empty;
            DbOwner = string.Empty;
            PlatformHostDir = string.Empty;
            BoxSettingDir = string.Empty;
            BranchName = string.Empty;
            SettingDb = null;
            SelectedProject = null;
            SelectedRepo = null;
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

        //TODO: потом сделать норм валидацию полей
        private bool ValidationsData(Window wnd, bool isSave)
        {
            if(isSave)
            {
                if (string.IsNullOrEmpty(NameConfig))
                {
                    ViewValidationsError(wnd, "NameConfig");
                    return false;
                }
            }
            

            if (string.IsNullOrEmpty(Appn))
            {
                ViewValidationsError(wnd, "AppNameTxt");
                return false;
            }

            if (string.IsNullOrEmpty(PathStend))
            {
                ViewValidationsError(wnd, "BrowseStandTextBox");
                return false;
            }

            if (string.IsNullOrEmpty(PortA))
            {
                ViewValidationsError(wnd, "PortTxt");
                return false;
            }

            if (string.IsNullOrEmpty(DbOwner) || DbOwner != Appn.ToUpper())
            {
                ViewValidationsError(wnd, "DbOwnerTxt");
                return false;
            }

            if (string.IsNullOrEmpty(PlatformHostDir))
            {
                ViewValidationsError(wnd, "PlatformHostDir");
                return false;
            }

            if (string.IsNullOrEmpty(BoxSettingDir))
            {
                ViewValidationsError(wnd, "BoxSettingsDir");
                return false;
            }

            if (SelectedProject is null)
            {
                ShowMessageToUser("Выберите проект");
                return false;
            }

            if (SelectedRepo is null)
            {
                ShowMessageToUser("Выберите репо");
                return false;
            }

            if (string.IsNullOrEmpty(BranchName))
            {
                ViewValidationsError(wnd, "BranchNameTxt");
                return false;
            }

            if (SettingDb is null)
            {
                ViewValidationsError(wnd, "CreateDbCombo");
                return false;
            }

            return true;
        }
        private void ViewValidationsError(Window wnd, string blockName)
        {
            Control block = wnd.FindName(blockName) as Control;
            block.BorderBrush = Brushes.Red;
        }
        
        private ConfigStend MappingModel()
        {
            SelectedRepo.TeamProject = SelectedProject;
            SelectedRepo.ProjectId = SelectedProject.Id;

            ConfigStend stend = new() 
            { 
                NameConfig  = NameConfig,
                Appn = Appn,
                PathStend = PathStend,
                PortA = int.Parse(PortA),
                DbOwner = DbOwner,
                PlatformHostDir = "PlatformHostDir" + PlatformHostDir,
                BoxSettingDir = "BoxSettingDir" + BoxSettingDir,
                RepoId = SelectedRepo.Id,
                BuildDefinition = SelectedRepo,
                BranchName = BranchName,
                SettingDb = SettingDb.Opt
            };

            return stend;
        }

        
        #endregion
    }
}

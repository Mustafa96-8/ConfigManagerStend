using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Infrastructure.Enums;
using ConfigManagerStend.Infrastructure.Services;
using ConfigManagerStend.Logic;
using ConfigManagerStend.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace ConfigManagerStend.Infrastructure.Commands
{
    internal class DetailsInfoCommand : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        #region FIELDS

        private List<Stand> stands;
        public List<Stand> Stands
        {
            set { stands = value; NotifyPropertyChanged(nameof(Stands)); }
            get { return stands; }
        }

        private bool isStandSelected;
        public bool IsStandSelected
        { 
            set { isStandSelected = value; NotifyPropertyChanged(nameof(isStandSelected)); } 
            get { return isStandSelected; } 
        }

        private Stand? selectedStand;
        public Stand? SelectedStand
        {
            set
            {
                selectedStand = value;
                IsStandSelected = selectedStand != null;
                NotifyPropertyChanged(nameof(SelectedStand));
                if (IsStandSelected) UpdateModuleDisplay(true);
            }
            get { return selectedStand; }
        }

        private List<ExternalModule> allModules;
        public List<ExternalModule> AllModules
        {
            get { return allModules; }
            set { allModules = value; NotifyPropertyChanged(nameof(AllModules)); }
        }

        private ExternalModule? selectedModule;
        public ExternalModule? SelectedModule
        {
            set { selectedModule = value; NotifyPropertyChanged(nameof(SelectedModule)); }
            get { return selectedModule; }
        }

        private string webPoolStatusColor = "gray";
        public string WebPoolStatusColor
        {
            set { webPoolStatusColor = value; NotifyPropertyChanged(nameof(WebPoolStatusColor)); }
            get { return webPoolStatusColor; }
        }
        private string webSiteStatusColor = "gray";
        public string WebSiteStatusColor
        {
            set { webSiteStatusColor = value; NotifyPropertyChanged(nameof(WebSiteStatusColor)); }
            get { return webSiteStatusColor; }
        }

        private string srvPoolStatusColor = "gray";
        public string SrvPoolStatusColor
        {
            set { srvPoolStatusColor = value; NotifyPropertyChanged(nameof(SrvPoolStatusColor)); }
            get { return srvPoolStatusColor; }
        }
        private string srvSiteStatusColor = "gray";
        public string SrvSiteStatusColor
        {
            set { srvSiteStatusColor = value; NotifyPropertyChanged(nameof(SrvSiteStatusColor)); }
            get { return srvSiteStatusColor; }
        }

        #endregion


        #region WINDOWS

        private RelayCommand _openAddNewStandWindow;
        public RelayCommand OpenAddNewStandWindow
        {
            get
            {
                return _openAddNewStandWindow ?? new(obj =>
                {
                    AddNewStand window = new(this);
                    window.Owner = Application.Current.MainWindow;
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.ShowDialog();
                });
            }
        }
        private RelayCommand _openAddNewModuleWindow;
        public RelayCommand OpenAddNewModuleWindow
        {
            get
            {
                return _openAddNewModuleWindow ?? new(obj =>
                {
                    AddNewModule window = new(this);
                    window.Owner = Application.Current.MainWindow;
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.ShowDialog();
                });
            }
        }

        private void OpenWindowCS(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }
        #endregion


        #region FUNCTIONS
        internal async Task<Status> AddNewStand(string path)
        {
            var result = await StandService.CreateStands(path);
            await LoadStandsAsync();
            return result;
        }
        internal async Task<Status> AddNewModule(ParserModel parser)
        {
            if (!IsStandSelected) return Statuses.UnexpectedError("Стенд не выбран");
            ParserLogic logic = new();
            Status result = await logic.ParserFile(parser, selectedStand!);
            return result;
        }

        public async Task LoadStandsAsync()
        {
            Stands = await StandService.GetAllStands();
        }

        public async Task LoadModulesAsync()
        {
            if (IsStandSelected)
                AllModules = await ModuleService.GetAllModules(selectedStand!.Id);
        }
        private void ShowMessageToUser(string message)
        {
            MessageView msView = new(message);
            OpenWindowCS(msView);
        }
        public void DoIISCommand(IssAppType type,IssCommands command)
        {
            if (IsStandSelected)
            {
                Status result = Statuses.Ok();
                if (type == IssAppType.Web)
                {
                    result = IISCommandExecutor.DoIISCommand(command, selectedStand!.IisWebName);
                }
                if (type == IssAppType.Srv)
                {
                    result = IISCommandExecutor.DoIISCommand(command, selectedStand!.IisSrvName);
                }
                if (result.ToString() != Statuses.Ok().ToString()) 
                {
                    ShowMessageToUser(result.ToString());
                }
                UpdateIISStatus();
            }
        }
        #endregion


        #region COMMANDS

        private RelayCommand refreshStandInfo;
        public RelayCommand RefreshStandInfo
        {
            get
            {
                return refreshStandInfo ?? new(obj =>
                {
                    Stands = StandService.UpdateAllStands().Result;
                    UpdateGlobalDisplay();
                });
            }
        }
        private RelayCommand loadConnectedModules;
        public RelayCommand LoadConnectedModules
        {
            get
            {
                return loadConnectedModules ?? new(obj =>
                {
                    if (isStandSelected)
                    {
                        Status result = ModuleService.LoadConnectedModules(SelectedStand).Result;
                        ShowMessageToUser(result.ToString());
                        UpdateModuleDisplay(true);
                    }
                });
            }
        }

        private RelayCommand deleteModuleCommand;
        public RelayCommand DeleteModuleCommand
        {
            get
            {
                return deleteModuleCommand ?? new(obj =>
                {
                    if (SelectedModule is not null)
                    {
                        Status result = ModuleService.DeleteModule(SelectedModule.Id).Result;
                        ShowMessageToUser(result.ToString());
                        UpdateModuleDisplay(true);
                    }
                });
            }
        }

        private RelayCommand unTrackStandCommand;
        public RelayCommand UnTrackStandCommand
        {
            get
            {
                return unTrackStandCommand ?? new(obj =>
                {
                    if (IsStandSelected)
                    {
                        Status result = StandService.UnTrackStand(SelectedStand!.Id).Result;
                        ShowMessageToUser(result.ToString());
                        UpdateGlobalDisplay(true);
                        GlobalNullValueProp();
                    }

                });
            }
        }

        private RelayCommand openModuleInFloder;
        public RelayCommand OpenModuleInFloder
        {
            get
            {
                return openModuleInFloder ?? new(obj =>
                {
                    if (SelectedModule is not null)
                    {
                        string filePath = SelectedModule.FullPathFile + SelectedModule.FileName;

                        if (System.IO.File.Exists(filePath))
                        {
                            Process.Start("explorer.exe", $"/select,\"{filePath}\"");
                        }
                        else
                        {
                            ShowMessageToUser("Файл не найден! Будет открыта папка последнего нахождения файла");
                            Process.Start("explorer.exe", $"{SelectedModule.FullPathFile}");
                        }

                        GlobalNullValueProp();
                    }
                });
            }
        }
        private RelayCommand openStandInFloder;
        public RelayCommand OpenStandInFloder
        {
            get
            {
                return openStandInFloder ?? new(obj =>
                {
                    if (IsStandSelected)
                    {
                        Process.Start("explorer.exe", $"{SelectedStand!.Path}");
                        UpdateGlobalDisplay();
                        GlobalNullValueProp();
                    }
                });
            }
        }
        #endregion


        #region UPDATE FUNCTIONS
        internal void UpdateModuleDisplay(bool UpdateFromDb = false)
        {
            if (UpdateFromDb)
                LoadModulesAsync().Wait();
            UpdateIISStatus();
            MainWindow.AllModules.ItemsSource = null;
            SelectedModule = null;
            MainWindow.AllModules.Items.Clear();
            MainWindow.AllModules.ItemsSource = AllModules;
            MainWindow.AllModules.Items.Refresh();
        }

        internal void UpdateGlobalDisplay(bool UpdateFromDb=false)
        {
            if (UpdateFromDb)
                LoadStandsAsync().Wait();
            MainWindow.AllStands.ItemsSource = null;
            MainWindow.AllModules.ItemsSource = null;
            MainWindow.AllStands.SelectedItem = null;
            MainWindow.AllStands.Items.Clear();
            MainWindow.AllModules.Items.Clear();
            MainWindow.AllStands.ItemsSource = Stands;
            MainWindow.AllModules.ItemsSource = AllModules;
            MainWindow.AllStands.Items.Refresh();
            WebPoolStatusColor = "gray";
            WebSiteStatusColor = "gray";
            SrvPoolStatusColor = "gray";
            SrvSiteStatusColor = "gray";
        }

        internal void UpdateIISStatus()
        {
            try
            {
                var iisStatus = IISCommandExecutor.GetStatus(selectedStand!.IisWebName, selectedStand!.IisSrvName);
                WebPoolStatusColor = iisStatus.WebPoolStatusColor;
                WebSiteStatusColor = iisStatus.WebSiteStatusColor;
                SrvPoolStatusColor = iisStatus.SrvPoolStatusColor;
                SrvSiteStatusColor = iisStatus.SrvSiteStatusColor;
            }
            catch(Exception ex) 
            {
                ShowMessageToUser(Statuses.IisError(ex.Message).ToString());
            }
        }

        private void GlobalNullValueProp()
        {
            SelectedModule = null;
            SelectedStand = null;
        }
        #endregion
    }
}

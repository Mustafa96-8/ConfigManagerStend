using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Infrastructure.Services;
using ConfigManagerStend.Logic;
using ConfigManagerStend.Models;
using System.ComponentModel;
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
        #region Fields
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
        private Stand selectedStand;
        public Stand SelectedStand
        {
            set { selectedStand = value; IsStandSelected = selectedStand != null ; NotifyPropertyChanged(nameof(SelectedStand)); }
            get { return selectedStand; }
        }

        private List<ExternalModule> allModules;
        public List<ExternalModule> AllModules
        {
            get { return allModules; }
            set { allModules = value; NotifyPropertyChanged(nameof(AllModules)); }
        }

        private ExternalModule selectedModule;
        public ExternalModule SelectedModule
        {
            set { selectedModule = value; NotifyPropertyChanged(nameof(SelectedModule)); }
            get { return selectedModule; }
        }

        #endregion


        #region COMMANDS

        //Открыть окно "Подробно"
        private RelayCommand _openShowDetails;
        public RelayCommand OpenShowDetails
        {
            get
            {
                return _openShowDetails ?? new(obj =>
                {
                    DetailInfo window = new(selectedModule.Id);
                    window.ShowDialog();
                });
            }
        }

        private RelayCommand _openAddNewStandWindow;
        public RelayCommand OpenAddNewStandWindow
        {
            get
            {
                return _openAddNewStandWindow ?? new(obj =>
                {
                    AddNewStand window = new(this);
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
                    window.ShowDialog();
                });
            }
        }



        internal async Task<Status> AddNewStand(string path)
        {
            return await StandService.CreateStands(path);
        }
        internal async Task<Status> AddNewModule(ParserModel parser)
        {
            ParserLogic logic = new();
            Status result = await logic.ParserFile(parser, SelectedStand);
            return result;
        }

        public async Task LoadStandsAsync()
        {
            Stands = await StandService.GetAllStands();
        }

        public async Task LoadModulesAsync()
        {
            AllModules = await ModuleService.GetAllModules(selectedStand.Id);
        }
        #endregion

        //Вытягиваем данные из БД



        //private RelayCommand deleteModuleCommand;
        //public RelayCommand DeleteModuleCommand
        //{
        //    get 
        //    {
        //        return deleteModuleCommand ?? new(obj => 
        //        {
        //            if (SelectedModule is not null) 
        //            {
        //                Status result = ModuleService.DeleteModule(SelectedModule.Id).Result;
        //                string message = result.Message + result.SystemInfo;
        //                ShowMessageToUser(message);
        //                UpdateDisplay();
        //                GlobalNullValueProp();
        //            }
                
        //        });
        //    }
        //}

        //private RelayCommand openInFloder;
        //public RelayCommand OpenInFloder
        //{
        //    get 
        //    {
        //        return openInFloder ?? new(obj =>
        //        {
        //            if(SelectedModule is not null)
        //            {
        //                string filePath = SelectedModule.FullPathFile + SelectedModule.FileName;

        //                if (System.IO.File.Exists(filePath))
        //                {
        //                    Process.Start("explorer.exe", $"/select,\"{filePath}\"");
        //                }
        //                else
        //                {
        //                    ShowMessageToUser("Файл не найден! Будет открыта папка последнего нахождения файла");
        //                    Process.Start("explorer.exe", $"{SelectedModule.FullPathFile}");
        //                }

        //                GlobalNullValueProp();
        //            }
        //        });
        //    }
        //}

        internal void UpdateDisplay()
        {
            LoadModulesAsync().Wait();
            MainWindow.AllModules.ItemsSource = null;
            MainWindow.AllModules.Items.Clear();
            MainWindow.AllModules.ItemsSource = AllModules;
            MainWindow.AllModules.Items.Refresh();
        }

        private void GlobalNullValueProp()
        {
            SelectedModule = null;
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
    }
}

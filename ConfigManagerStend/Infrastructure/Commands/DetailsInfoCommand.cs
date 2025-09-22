using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Infrastructure.Services;
using ConfigManagerStend.Models;
using Microsoft.VisualBasic.Logging;
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


        //Открыть окно "Подробно"
        private RelayCommand _openShowDetails;
        public RelayCommand OpenShowDetails
        {
            get
            {
                return _openShowDetails ?? new(obj =>
                {
                    DetailInfo window = new(standId);
                    window.ShowDialog();
                });
            }
        }

        //Вытягиваем данные из БД
        private List<ExternalModule> allModules;
        public List<ExternalModule> AllModules 
        { 
            get { return modules; }
            set { modules = value; NotifyPropertyChanged(nameof(AllModules)); }
        }

        // Конструктор или метод инициализации для загрузки данных
        public async Task LoadConfigsAsync(int standId)
        {
            AllModules = await StandService.GetAllModules(standId);
        }

        private List<Stand> stands;
        public List<Stand> Stands 
        { 
            set { stands = value; NotifyPropertyChanged(nameof(Stands)); }
            get { return stands; } 
        } 

        private async Task<string> AddNewStand(string path)
        {

        }

        public static ConfigStend SelectedDitails { get;  set; }

        private RelayCommand deleteDetailsCommand;
        public RelayCommand DeleteDetailsCommand
        {
            get 
            {
                return deleteDetailsCommand ?? new(obj => 
                {
                    if (SelectedDitails is not null) 
                    {
                        Status result = StandService.DeleteDetails(SelectedDitails.Id).Result;
                        string message = result.Message + result.SystemInfo;
                        ShowMessageToUser(message);
                        UpdateDisplay();
                        GlobalNullValueProp();
                    }
                
                });
            }
        }

        private RelayCommand openInFloder;
        public RelayCommand OpenInFloder
        {
            get 
            {
                return openInFloder ?? new(obj =>
                {
                    if(SelectedDitails is not null)
                    {
                        string filePath = SelectedDitails.FullPathFile + SelectedDitails.NameFile;

                        if (System.IO.File.Exists(filePath))
                        {
                            Process.Start("explorer.exe", $"/select,\"{filePath}\"");
                        }
                        else
                        {
                            ShowMessageToUser("Файл не найден! Будет открыта папка последнего нахождения файла");
                            Process.Start("explorer.exe", $"{SelectedDitails.FullPathFile}");
                        }

                        GlobalNullValueProp();
                    }
                });
            }
        }

        internal void UpdateDisplay()
        {
            //LoadConfigsAsync().Wait();
            DetailInfo.AllDitails.ItemsSource = null;
            DetailInfo.AllDitails.Items.Clear();
            //DetailInfo.AllDitails.ItemsSource = AllDitails;
            DetailInfo.AllDitails.Items.Refresh();
        }

        private void GlobalNullValueProp()
        {
            SelectedDitails = null;
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

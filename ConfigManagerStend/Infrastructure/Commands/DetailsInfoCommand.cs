using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Infrastructure.Services;
using ConfigManagerStend.Models;
using Microsoft.VisualBasic.Logging;
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
        private List<ConfigStend> configs;
        public List<ConfigStend> AllDitails 
        { 
            get { return configs; }
            set { configs = value; NotifyPropertyChanged(nameof(AllDitails)); }
        }
        // Конструктор или метод инициализации для загрузки данных
        public async Task LoadConfigsAsync(int standId)
        {
            AllDitails = await DetailService.GetAllConfigs(standId);
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
                        Status result = DetailService.DeleteDetails(SelectedDitails.Id).Result;
                        string message = result.Message + result.SystemInfo;
                        ShowMessageToUser(message);
                        UpdateDisplay();
                        GlobalNullValueProp();
                    }
                
                });
            }
        }

        internal void UpdateDisplay()
        {
            LoadConfigsAsync(int ).Wait();
            DetailInfo.AllDitails.ItemsSource = null;
            DetailInfo.AllDitails.Items.Clear();
            DetailInfo.AllDitails.ItemsSource = AllDitails;
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

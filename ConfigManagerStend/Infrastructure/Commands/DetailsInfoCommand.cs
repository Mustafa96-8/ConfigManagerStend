using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Infrastructure.Services;
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
                    DetailInfo window = new();
                    window.ShowDialog();
                });
            }
        }

        //Вытягиваем данные из БД
        private List<Config> configs = DetailService.GetAllConfigs();
        public List<Config> AllDitails 
        { 
            get { return configs; }
            set { configs = value; NotifyPropertyChanged(nameof(AllDitails)); }
        }



        private void ShowMessageToUser(bool result)
        {

            if (result is true)
            {
                MessageView msView = new("Успех");
                OpenWindowCS(msView);
            }
            else
            {
                MessageView msView = new("Ошибка");
                OpenWindowCS(msView);
            }
        }
        private void OpenWindowCS(Window window)
        {
            window.Owner = System.Windows.Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }
    }
}

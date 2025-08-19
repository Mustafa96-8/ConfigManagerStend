using ConfigManagerStend.Forms;
using ConfigManagerStend.Infrastructure.Enums;
using System.ComponentModel;


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
    }
}

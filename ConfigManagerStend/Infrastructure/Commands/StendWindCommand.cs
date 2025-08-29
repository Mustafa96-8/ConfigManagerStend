using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Forms;
using ConfigManagerStend.Infrastructure.Enums;
using ConfigManagerStend.Infrastructure.Services;
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


        #region initialization prop
        public static TeamProject? SelectedProject { get; set; }
        public static BuildDefinition? SelectedRepo { get; set; }
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
        #endregion
    }
}

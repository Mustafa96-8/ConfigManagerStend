using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Infrastructure.Services;
using System.ComponentModel;


namespace ConfigManagerStend.Infrastructure.Commands
{
    internal class DictionaryCommand : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        #region PROP

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
    }
}

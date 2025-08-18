using ConfigManagerStend.Domain.Entities;

namespace ConfigManagerStend.Domain.Predefineds
{
    /// <summary>
    /// Предопределенные данные. Проекты
    /// </summary>
    internal class PdTeamProject
    {
        /// <summary>
        /// Проект. Дело
        /// </summary>
        internal TeamProject delo = new() 
        {
            Id = 1,
            NameProject = "Delo2020"
        };

        /// <summary>
        /// Проект. Титул
        /// </summary>
        internal TeamProject titul = new()
        {
            Id = 2,
            NameProject = "TITUL"
        };

        /// <summary>
        /// Проект. Надзор
        /// </summary>
        internal TeamProject nadzor = new()
        {
            Id = 3,
            NameProject = "Nadzor2025"
        };

        internal List<TeamProject> Projects;

        internal PdTeamProject()
        {
            Projects = new() { delo, titul, nadzor };
        }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace ConfigManagerStend.Domain.Entities
{
    /// <summary>
    /// Таблица. Репозитории проекта
    /// </summary>
    internal class BuildDefinition
    {
        public int Id { get; set; }

        /// <summary>
        /// Проект Id
        /// </summary>
        [ForeignKey(nameof(TeamProject))]
        public int ProjectId { get; set; }

        /// <summary>
        /// Проект
        /// </summary>
        public TeamProject? TeamProject { get; set; }

        /// <summary>
        /// Наименование репозитория
        /// </summary>
        public required string NameRepo { get; set; }
    }
}

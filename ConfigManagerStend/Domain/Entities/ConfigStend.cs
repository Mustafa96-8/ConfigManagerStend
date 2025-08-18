using System.ComponentModel.DataAnnotations.Schema;

namespace ConfigManagerStend.Domain.Entities
{
    /// <summary>
    /// Таблица. Конфиги стендов
    /// </summary>
    internal class ConfigStend
    {
        public int Id { get; set; }

        /// <summary>
        /// Наименование конфига
        /// </summary>
        public required string NameConfig { get; set; }

        /// <summary>
        /// Наименование приложение
        /// </summary>
        public required string Appn { get; set; }
      
        /// <summary>
        /// Расположение стенда
        /// </summary>
        public required string PathStend { get; set; }

        /// <summary>
        /// Порт
        /// </summary>
        public int PortA { get; set; }

        /// <summary>
        /// Владелец БД
        /// </summary>
        public required string DbOwner { get; set; }

        /// <summary>
        /// PlatformHostDir
        /// </summary>
        public required string PlatformHostDir { get; set; }

        /// <summary>
        /// BoxSettingDir
        /// </summary>
        public required string BoxSettingDir { get; set; }

        /// <summary>
        /// Репо Id
        /// </summary>
        [ForeignKey(nameof(BuildDefinition))]
        public int RepoId { get; set; }

        /// <summary>
        /// Репо
        /// </summary>
        public BuildDefinition? BuildDefinition { get; set; }

        /// <summary>
        /// Наименование ветки
        /// </summary>
        public required string BranchName { get; set; }

        /// <summary>
        /// Настройки БД (NO - не создавать БД, Recreate - создать/пересоздать БД)
        /// </summary>
        public required string SettingDb {  get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace ConfigManagerStend.Domain.Entities
{
    internal class Config
    {
        public int Id { get; set; } 

        /// <summary>
        /// Наименование стенда
        /// </summary>
        public required string NameStand { get; set; }

        /// <summary>
        /// Полный путь файла
        /// </summary>
        public required string FullPathFile { get; set; }

        /// <summary>
        /// Наименование файла
        /// </summary>
        public required string NameFile { get; set; }

        /// <summary>
        /// Дата подмены
        /// </summary>
        public DateTime DateFileReplacement { get; set; } = DateTime.Now;

        /// <summary>
        /// Послендяя проверка существования файла
        /// </summary>
        public DateTime? DateFileVerifiedToExist { get; set; } = null;

        /// <summary>
        /// Статус ID
        /// </summary>
        [ForeignKey(nameof(Status))]
        public int StatusId { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public ConfigStatus? Status { get; set; }
    }
}

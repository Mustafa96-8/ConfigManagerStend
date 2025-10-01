namespace ConfigManagerStend.Domain.Entities
{
    /// <summary>
    /// Таблица. Статус подключения файла к стенду
    /// </summary>
    internal class ConfigStatus
    {
        public int Id { get; set; } 

        /// <summary>
        /// Наименование статуса
        /// </summary>
        public required string Name { get; set; }
    }
}

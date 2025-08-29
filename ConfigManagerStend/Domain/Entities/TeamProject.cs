namespace ConfigManagerStend.Domain.Entities
{
    /// <summary>
    /// Таблица проектов
    /// </summary>
    public class TeamProject
    {
        public int Id { get; set; }

        /// <summary>
        /// Наименование проекта
        /// </summary>
        public required string NameProject { get; set; }
    }
}

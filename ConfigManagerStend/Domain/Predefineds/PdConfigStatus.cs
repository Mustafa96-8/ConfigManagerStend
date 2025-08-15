using ConfigManagerStend.Domain.Entities;

namespace ConfigManagerStend.Domain.Predefineds
{
    /// <summary>
    /// Предопределенные данные
    /// </summary>
    internal class PdConfigStatus
    {
        /// <summary>
        /// Файл в папке стенда присутствует
        /// </summary>
        internal ConfigStatus exist = new()
        {
            Id = 1,
            NameStatus = "Подключен"
        };

        /// <summary>
        /// Файл в папке стенда отсутствует
        /// </summary>
        internal ConfigStatus unexist = new()
        {
            Id = 2,
            NameStatus = "Отключен"
        };

        internal List<ConfigStatus> ConfigStatuses { get;  set; } 

        internal PdConfigStatus()
        {
            ConfigStatuses = new() { exist, unexist };
        }
    }
}

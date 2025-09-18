using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigManagerStend.Domain.Entities
{
    internal class Stand
    {
        /// <summary>
        /// Ид стенда
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название стенда
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Путь к стенду
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Версия стенда
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// DbProvider стенда
        /// </summary>
        public string DbProvider { get; set; } = string.Empty;

        /// <summary>
        /// Дата и время обновления стенда
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Дата и время проверки стенда
        /// </summary>
        public DateTime? ChekedAt { get; set; }

        /// <summary>
        /// Подключенные модули
        /// </summary>
        public List<ConfigStend> Modules { get; set; } = new List<ConfigStend>();

        public Stand(string path)
        {
            Name = path.Split("//", StringSplitOptions.RemoveEmptyEntries).Last();
            Path = path;
        }
    }
}


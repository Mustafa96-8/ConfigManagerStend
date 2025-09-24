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
        /// Путь к папке *-delosrv/a/
        /// </summary>
        public string SrvAFolderPath { get; set; }

        /// <summary>
        /// Версия стенда
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// DbProvider стенда
        /// </summary>
        public string DbProvider { get; set; } = string.Empty;
        
        /// <summary>
        /// Владелец БД?
        /// </summary>
        public string DbOwner { get; set; } = string.Empty;

        /// <summary>
        /// Порт приложения
        /// </summary>
        public string AppPort { get; set; } = string.Empty;

        /// <summary>
        /// Название приложения
        /// </summary>
        public string AppName { get; set; } = string.Empty;

        /// <summary>
        /// Ссылка на стенд?
        /// </summary>
        public string AppUrl { get; set; } = string.Empty;

        /// <summary>
        /// Дата и время обновления стенда
        /// </summary>
        public string? UpdatedAt { get; set; }

        /// <summary>
        /// Дата и время проверки стенда
        /// </summary>
        public string? ChekedAt { get; set; }

        /// <summary>
        /// Подключенные модули
        /// </summary>
        public List<ExternalModule> Modules { get; set; } = new List<ExternalModule>();

        public Stand(string path,string srvAFolderPath)
        {
            Name = path.Split("\\", StringSplitOptions.RemoveEmptyEntries).Last();
            Path = path;
            SrvAFolderPath = srvAFolderPath;
        }
    }
}


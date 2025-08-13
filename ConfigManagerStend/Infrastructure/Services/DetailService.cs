using ConfigManagerStend.Domain;
using ConfigManagerStend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConfigManagerStend.Infrastructure.Services
{
    internal class DetailService
    {
        /// <summary>
        /// Получение всех записей
        /// </summary>
        public static List<Config> GetAllConfigs()
        {
            using (var db = new AppDbContext())
            {
                return db.Configs.Include(x=>x.Status).ToList();
            }
        }
    }
}

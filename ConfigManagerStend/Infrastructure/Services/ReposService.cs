using ConfigManagerStend.Domain;
using ConfigManagerStend.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace ConfigManagerStend.Infrastructure.Services
{
    internal class ReposService
    {
        /// <summary>
        /// Получение всех Проектов
        /// </summary>
        public static async Task<List<BuildDefinition>> GetAllReposAsync()
        {
            using (var db = new AppDbContext())
            {
                return await db.BuildDefinitions.Include(p=>p.TeamProject).ToListAsync();
            }
        }
    }
}

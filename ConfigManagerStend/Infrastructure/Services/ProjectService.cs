using ConfigManagerStend.Domain;
using ConfigManagerStend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConfigManagerStend.Infrastructure.Services
{
    internal class ProjectService
    {
        /// <summary>
        /// Получение всех Проектов
        /// </summary>
        public static async Task<List<TeamProject>> GetAllProjectAsync()
        {
            using (var db = new AppDbContext())
            {
                return await db.TeamProjects.ToListAsync();
            }
        }
    }
}

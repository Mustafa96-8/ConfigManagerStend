using ConfigManagerStend.Domain;
using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Infrastructure.Enums;
using ConfigManagerStend.Models;
using Microsoft.EntityFrameworkCore;


namespace ConfigManagerStend.Infrastructure.Services
{
    internal class ReposService
    {
        /// <summary>
        /// Получение всех Репо
        /// </summary>
        public static async Task<List<BuildDefinition>> GetAllReposAsync()
        {
            using (var db = new AppDbContext())
            {
                return await db.BuildDefinitions.Include(p=>p.TeamProject).ToListAsync();
            }
        }
        /// <summary>
        ///  Добавление нового Репо
        /// </summary>
        /// <param name="name">Имя репо</param>
        /// <param name="project">Проект</param>
        public static async Task<Status> CreateRepoAsync(string name, TeamProject project)
        {
            using (var db = new AppDbContext())
            {
                bool isExist = await db.BuildDefinitions.Where(x => x.NameRepo == name && x.ProjectId == project.Id).AnyAsync();
                if (isExist) { return Statuses.UnexpectedError("Такая запись уже существует"); }

                BuildDefinition newRep = new() { NameRepo = name, ProjectId = project.Id };

                try
                {
                    await db.BuildDefinitions.AddAsync(newRep);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex) { return Statuses.UnexpectedError("Не удалось сохранить в бд\n" + ex.Message); }

                return Statuses.Ok();
            }
        }

        /// <summary>
        /// Редактирование Репо
        /// </summary>
        /// <param name="id">Id исходного Репо</param>
        /// <param name="name">Новое наименование Репо</param>
        /// <param name="project">проект</param>
        public static async Task<Status> EditProjectAsync(int id, string name, TeamProject project)
        {
            using (var db = new AppDbContext())
            {
                bool isExist = await db.BuildDefinitions.Where(x => x.NameRepo == name && x.ProjectId == project.Id).AnyAsync();
                if (isExist) { return Statuses.UnexpectedError("Такая запись уже существует"); }

                BuildDefinition? repo = await db.BuildDefinitions.FirstOrDefaultAsync(x => x.Id == id);
                if (repo is null) { return Statuses.UnexpectedError("Возникла ошибка при изменении. Исходная запись в БД не была найдена"); }

                try
                {
                    repo.NameRepo = name;
                    repo.ProjectId = project.Id;
                    db.BuildDefinitions.Update(repo);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex) { return Statuses.UnexpectedError("Не удалось сохранить в бд\n" + ex.Message); }

                return Statuses.Ok();
            }
        }
    }
}

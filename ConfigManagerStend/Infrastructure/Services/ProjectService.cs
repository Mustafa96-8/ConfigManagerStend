using ConfigManagerStend.Domain;
using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Infrastructure.Enums;
using ConfigManagerStend.Models;
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

        /// <summary>
        /// Добавление нового проекта
        /// </summary>
        /// <param name="name">Наименование проекта</param>
        public static async Task<Status> CreateProjectAsync(string name)
        {
            using (var db = new AppDbContext())
            {
                bool isExist = await db.TeamProjects.Where(x=>x.NameProject == name).AnyAsync();
                if (isExist) { return Statuses.UnexpectedError("Такая запись уже существует"); }

                TeamProject newPrj = new() { NameProject = name };

                try
                {
                    await db.TeamProjects.AddAsync(newPrj);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex) { return Statuses.UnexpectedError("Не удалось сохранить в бд\n" + ex.Message); }

                return Statuses.Ok();
            }
        }

        /// <summary>
        /// Редактирование проекта
        /// </summary>
        /// <param name="id">Id исходного названия проекта</param>
        /// <param name="name">Новое наименование проекта</param>
        public static async Task<Status> EditProjectAsync(int id, string name)
        {
            using (var db = new AppDbContext())
            {
                bool isExist = await db.TeamProjects.Where(x => x.NameProject == name).AnyAsync();
                if (isExist) { return Statuses.UnexpectedError("Такая запись уже существует"); }

                TeamProject? teamProject = await db.TeamProjects.FirstOrDefaultAsync(x=>x.Id == id);
                if (teamProject is null) { return Statuses.UnexpectedError("Возникла ошибка при изменении. Исходная запись в БД не была найдена"); }

                try
                {
                    teamProject.NameProject = name;
                    db.TeamProjects.Update(teamProject);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex) { return Statuses.UnexpectedError("Не удалось сохранить в бд\n" + ex.Message); }

                return Statuses.Ok();
            }
        }

        /// <summary>
        /// Удаление проекта
        /// </summary>
        /// <param name="id">Id проекта</param>
        public static async Task<Status> DeleteProjectAsync(int id)
        {
            using (var db = new AppDbContext())
            {
                if (id <= 0) { return Statuses.UnexpectedError("Неверно передан id для удаления"); }

                TeamProject? prj = await db.TeamProjects.Where(x => x.Id == id).FirstOrDefaultAsync();
                if(prj is null) { return Statuses.UnexpectedError("Запись в БД не найдена"); }

                bool isExistInRepo = await db.BuildDefinitions.AnyAsync(x=>x.ProjectId == id);
                if (isExistInRepo) { return Statuses.UnexpectedError("Запись не может быть удалена, так как с ней связан Репозиторий"); }

                try
                {
                    db.TeamProjects.Remove(prj);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex) { return Statuses.UnexpectedError("Не удалось удалить запись: \n" + ex); }

                return Statuses.Ok();
            }
        }


    }
}

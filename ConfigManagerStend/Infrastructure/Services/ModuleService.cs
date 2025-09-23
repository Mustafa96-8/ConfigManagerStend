using ConfigManagerStend.Domain;
using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Infrastructure.Enums;
using ConfigManagerStend.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ConfigManagerStend.Infrastructure.Services
{
    internal static class ModuleService
    {
        /// <summary>
        /// Получение всех модулей по стенду
        /// </summary>
        public static async Task<List<ExternalModule>> GetAllModules(int standId)
        {
            using (var db = new AppDbContext())
            {
                List<ExternalModule> modules = await db.ExternalModules.Include(x => x.Status).Where(x => x.StandId == standId).ToListAsync();
                return modules;
            }
        }

        /// <summary>
        /// Добавление информации в БД
        /// </summary>
        /// <param name="config">конфиг</param>
        public static async Task<Status> CreateModule(ExternalModule module)
        {
            if (module is null) { return Statuses.UnexpectedError("Внешний модуль пуст"); }

            try
            {
                using (var db = new AppDbContext())
                {
                    var stand = await db.Stands.FirstOrDefaultAsync(s => module.StandId == s.Id);
                    stand.Modules.Add(module);
                    db.Update(stand);
                    await db.ExternalModules.AddAsync(module);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex) { return Statuses.DbError(ex.Message); }

            return Statuses.Ok();
        }

        public static async Task<Status> DeleteModule(int id)
        {
            if (id == 0) { return Statuses.UnexpectedError("Неправильный Id"); }

            using (var db = new AppDbContext())
            {
                ExternalModule? module = await db.ExternalModules.FirstOrDefaultAsync(x => x.Id == id);
                if (module is null) { return Statuses.DbError("Не удалось найти запись в БД"); }

                if (File.Exists(module.FullPathFile + module.FileName))
                {
                    try
                    {
                        File.Delete(module.FullPathFile + module.FileName);

                    }
                    catch(Exception ex) { return Statuses.UnexpectedError(ex.Message); }
                 
                }

                try
                {
                   db.ExternalModules.Remove(module);
                   await db.SaveChangesAsync();
                }
                catch(Exception ex) { return Statuses.UnexpectedError(ex.Message); }
            }

            return Statuses.Ok();   
        }
    }
}
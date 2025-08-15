using ConfigManagerStend.Domain;
using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Domain.Predefineds;
using ConfigManagerStend.Infrastructure.Enums;
using ConfigManagerStend.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ConfigManagerStend.Infrastructure.Services
{
    internal class DetailService
    {
        /// <summary>
        /// Получение всех записей
        /// </summary>
        public static async Task<List<Config>> GetAllConfigs()
        {
            using (var db = new AppDbContext())
            {
                List<Config> configs = await db.Configs.Include(x => x.Status).ToListAsync();
                PdConfigStatus status = new();

                foreach (Config config in configs)
                {
                    if (File.Exists(config.FullPathFile + config.NameFile))
                    {
                        config.StatusId = status.exist.Id;
                    }
                    else
                    {
                        config.StatusId = status.unexist.Id;
                    }
                    config.DateFileVerifiedToExist = DateTime.Now;
                }

                try
                {
                    db.Configs.UpdateRange(configs);
                    await db.SaveChangesAsync();
                }
                catch { return new(); }

                return configs;
            }
        }

        /// <summary>
        /// Добавление информации в БД
        /// </summary>
        /// <param name="config">конфиг</param>
        public static async Task<Status> CreateData(Config config)
        {
            if (config is null) { return Statuses.UnexpectedError("Модель пустая"); }

            try
            {
                using (var db = new AppDbContext())
                {
                    await db.Configs.AddAsync(config);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex) { return Statuses.UnexpectedError(ex.Message); }

            return Statuses.Ok();
        }

        public static async Task<Status> DeleteDetails(int id)
        {
            if (id == 0) { return Statuses.UnexpectedError("Неправильный Id"); }

            using (var db = new AppDbContext())
            {
                Config? config = await db.Configs.FirstOrDefaultAsync(x => x.Id == id);
                if (config is null) { return Statuses.UnexpectedError("Не удалось найти запись в БД"); }

                if (File.Exists(config.FullPathFile + config.NameFile))
                {
                    try
                    {
                        File.Delete(config.FullPathFile + config.NameFile);

                    }
                    catch(Exception ex) { return Statuses.UnexpectedError(ex.Message); }
                 
                }

                try
                {
                   db.Configs.Remove(config);
                   await db.SaveChangesAsync();
                }
                catch(Exception ex) { return Statuses.UnexpectedError(ex.Message); }
            }

            return Statuses.Ok();   
        }
    }
}
using ConfigManagerStend.Domain;
using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Domain.Predefineds;
using ConfigManagerStend.Infrastructure.Enums;
using ConfigManagerStend.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace ConfigManagerStend.Infrastructure.Services
{
    internal class DetailService
    {
        /// <summary>
        /// Получение всех стендов
        /// </summary>
        public static async Task<List<Stand>> UpdateAllStands()
        {
            using (var db = new AppDbContext())
            {
                List<Stand> standList = await db.Stands.Include(s => s.Modules).ThenInclude(m => m.Status).ToListAsync();
                
                PdConfigStatus status = new();

                foreach (Stand stand in standList)
                {
                    DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo($"{stand.Path}\\config\\");
                    DirectoryInfo[] ConfigDir = hdDirectoryInWhichToSearch.GetDirectories("*" + "-delosrv");

                    if (File.Exists(ConfigDir[0].FullName + "\\a\\appsettings.json"))
                    {
                        // Читаем содержимое файла настроек
                        string jsonContent = File.ReadAllText(ConfigDir[0].FullName + "\\a\\appsettings.json");

                        // Парсим JSON
                        var jsonObject = JsonNode.Parse(jsonContent)?.AsObject();

                        // Получаем информацию о стенде
                        if (jsonObject != null)
                        {
                            stand.Version = (string?)jsonObject["ApplicationInfo"]?["AppVersion"] ?? string.Empty;
                            stand.UpdatedAt = (DateTime?)jsonObject["ApplicationInfo"]?["Date"];
                            stand.DbProvider = (string?)jsonObject["Eos"]?["DbProvider"] ?? string.Empty;
                            stand.ChekedAt = DateTime.Now;
                        }
                    }
                    foreach (var module in stand.Modules)
                    {
                        if (File.Exists(module.FullPathFile + module.FileName))
                        {
                            module.StatusId = status.exist.Id;
                        }
                        else
                        {
                            module.StatusId = status.unexist.Id;
                        }
                        module.DateFileVerifiedToExist = DateTime.Now;
                    }
                }
                try
                {
                    db.Stands.UpdateRange(standList);
                    await db.SaveChangesAsync();
                }
                catch { return new(); }

                return standList;
            }
        }

        /// <summary>
        /// Получение всех стендов
        /// </summary>
        public static async Task<List<Stand>> GetAllStands()
        {
            using (var db = new AppDbContext())
            {
                List<Stand> standList = await db.Stands.Include(s => s.Modules).ThenInclude(m => m.Status).ToListAsync();
                PdConfigStatus status = new();

                return standList;
            }
        }


        /// <summary>
        /// Добавление стенда в систему
        /// </summary>
        public static async Task<Status> CreateStands(string path)
        {
            using (var db = new AppDbContext())
            {
                var standFromDb = await db.Stands.FirstOrDefaultAsync(s => s.Path == path);

                if (standFromDb != null)
                    return Statuses.DbError($"Стенд уже добавлен в систему: {standFromDb.Name}");

                DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo($"{path}\\config\\");
                DirectoryInfo[] ConfigDir = hdDirectoryInWhichToSearch.GetDirectories("*" + "-delosrv");

                if (!File.Exists(ConfigDir[0].FullName + "\\a\\appsettings.json"))
                    return Statuses.FileNotFound(ConfigDir[0].FullName + "\\a\\appsettings.json");

                Stand stand = new(path);

                try
                {
                    // Читаем содержимое файла настроек
                    string jsonContent = File.ReadAllText(ConfigDir[0].FullName + "\\a\\appsettings.json");

                    // Парсим JSON
                    var jsonObject = JsonNode.Parse(jsonContent)?.AsObject();

                    // Получаем информацию о стенде
                    if (jsonObject != null)
                    {
                        stand.Version = (string?)jsonObject["ApplicationInfo"]?["AppVersion"] ?? string.Empty;
                        stand.UpdatedAt = (DateTime?)jsonObject["ApplicationInfo"]?["Date"];
                        stand.DbProvider = (string?)jsonObject["Eos"]?["DbProvider"] ?? string.Empty;
                        stand.ChekedAt = DateTime.Now;
                    }
                }
                catch(Exception ex) { return Statuses.UnexpectedError($"Ошибка во время чтения информации стенда:{ex.Message}"); }
                try
                {
                    db.Stands.UpdateRange(stand);
                    await db.SaveChangesAsync();
                }
                catch(Exception ex){ return Statuses.DbError(ex.Message); }

                return Statuses.Ok();
            }
        }

        /// <summary>
        /// Получение всех записей по стенду
        /// </summary>
        public static async Task<List<ExternalModule>> GetAllConfigs(int standId)
        {
            using (var db = new AppDbContext())
            {
                List<ExternalModule> configs = await db.ExternalModules.Include(x => x.Status).Where(x => x.StandId == standId).ToListAsync();
                PdConfigStatus status = new();

                foreach (ExternalModule config in configs)
                {
                    if (File.Exists(config.FullPathFile + config.FileName))
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
                    db.ExternalModules.UpdateRange(configs);
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
        public static async Task<Status> CreateModule(ExternalModule module)
        {
            if (module is null) { return Statuses.UnexpectedError("Модель пустая"); }

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

        public static async Task<Status> DeleteDetails(int id)
        {
            if (id == 0) { return Statuses.UnexpectedError("Неправильный Id"); }

            using (var db = new AppDbContext())
            {
                ExternalModule? config = await db.ExternalModules.FirstOrDefaultAsync(x => x.Id == id);
                if (config is null) { return Statuses.DbError("Не удалось найти запись в БД"); }

                if (File.Exists(config.FullPathFile + config.FileName))
                {
                    try
                    {
                        File.Delete(config.FullPathFile + config.FileName);

                    }
                    catch(Exception ex) { return Statuses.UnexpectedError(ex.Message); }
                 
                }

                try
                {
                   db.ExternalModules.Remove(config);
                   await db.SaveChangesAsync();
                }
                catch(Exception ex) { return Statuses.UnexpectedError(ex.Message); }
            }

            return Statuses.Ok();   
        }
    }
}
using ConfigManagerStend.Domain;
using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Domain.Predefineds;
using ConfigManagerStend.Infrastructure.Enums;
using ConfigManagerStend.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace ConfigManagerStend.Infrastructure.Services
{
    internal class StandService
    {
        /// <summary>
        /// Получение всех стендов
        /// </summary>
        public static async Task<List<Stand>> GetAllStands()
        {
            using (var db = new AppDbContext())
            {
                return await db.Stands.Include(s => s.Modules).ThenInclude(m => m.Status).ToListAsync();
            }
        }

        /// <summary>
        /// Обновление всех стендов
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
                            stand.UpdatedAt = (string?)jsonObject["ApplicationInfo"]?["Date"];
                            stand.DbProvider = (string?)jsonObject["Eos"]?["DbProvider"] ?? string.Empty;
                            stand.DbOwner = (string?)jsonObject["Eos"]?["UserManagerOptions"]?["DbOwner"] ??string.Empty;
                            stand.AppPort = (string?)jsonObject["Eos"]?["AppBindingPort"] ??string.Empty;
                            stand.AppName = (string?)jsonObject["Eos"]?["AppName"] ??string.Empty;
                            stand.AppUrl = string.IsNullOrEmpty(stand.AppName)?"":"https://"+stand.AppName+ ".devel.loc/";
                            stand.ChekedAt = DateTime.Now.ToString("g");
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
                        module.DateFileVerifiedToExist = DateTime.Now.ToString("g");
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

                var aFolder = ConfigDir[0].FullName + "\\a\\";

                if (!File.Exists(aFolder + "appsettings.json"))
                    return Statuses.FileNotFound(aFolder + "appsettings.json");

                Stand stand = new(path, aFolder);
                try
                {
                    // Читаем содержимое файла настроек
                    string jsonContent = File.ReadAllText(aFolder + "appsettings.json");

                    // Парсим JSON
                    var jsonObject = JsonNode.Parse(jsonContent)?.AsObject();

                    // Получаем информацию о стенде
                    if (jsonObject != null)
                    {
                        stand.Version = (string?)jsonObject["ApplicationInfo"]?["AppVersion"] ?? string.Empty;
                        stand.UpdatedAt = (string?)jsonObject["ApplicationInfo"]?["Date"];
                        stand.DbProvider = (string?)jsonObject["Eos"]?["DbProvider"] ?? string.Empty;
                        stand.DbOwner = (string?)jsonObject["Eos"]?["UserManagerOptions"]?["DbOwner"] ?? string.Empty;
                        stand.AppPort = (string?)jsonObject["Eos"]?["AppBindingPort"] ?? string.Empty;
                        stand.AppName = (string?)jsonObject["Eos"]?["AppName"] ?? string.Empty;
                        stand.AppUrl = string.IsNullOrEmpty(stand.AppName) ? "" : "https://" + stand.AppName + ".devel.loc/";
                        stand.ChekedAt = DateTime.Now.ToString("g");
                    }
                }
                catch (Exception ex) { return Statuses.UnexpectedError($"Ошибка во время чтения информации стенда:{ex.Message}"); }
                try
                {
                    db.Stands.UpdateRange(stand);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex) { return Statuses.DbError(ex.Message); }

                return Statuses.Ok();
            }
        }

        /// <summary>
        /// Перестать отслеживать стенд в программе, удаление стнеда и Модулей из БД
        /// </summary>
        public static async Task<Status> UnTrackStand(int id)
        {
            if (id == 0) { return Statuses.DbError("Неправильный Id"); }

            using (var db = new AppDbContext())
            {
                var standFromDb = await db.Stands.FirstOrDefaultAsync(s => s.Id == id);

                if (standFromDb is null)
                    return Statuses.DbError($"Не удалось найти запись в БД: {id}");
                var modulesFromDbByStand = await db.ExternalModules.Where(m => m.StandId == id).ToListAsync();
                try
                {
                    db.ExternalModules.RemoveRange(modulesFromDbByStand);
                    db.Stands.Remove(standFromDb);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex) { return Statuses.DbError($"Ошибка при удалении стенда или модулей:{ex.Message}"); }
            }
            return Statuses.Ok();

        }
    }
}
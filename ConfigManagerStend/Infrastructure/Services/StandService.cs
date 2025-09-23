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

    }
}
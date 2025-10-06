using ConfigManagerStend.Domain;
using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Domain.Predefineds;
using ConfigManagerStend.Infrastructure.Enums;
using ConfigManagerStend.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

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
                PdConfigStatus status = new();

                foreach (ExternalModule module in modules)
                {
                    if (File.Exists(module.FullPathFile + module.FileName))
                    {
                        module.StatusId = status.exist.Id;
                    }
                    else
                    {
                        module.StatusId = status.unexist.Id;
                    }
                    module.DateFileVerifiedToExist = DateTime.Now.ToString();
                }

                try
                {
                    db.ExternalModules.UpdateRange(modules);
                    await db.SaveChangesAsync();
                }
                catch { return new(); }

                return modules;
            }
        }

        /// <summary>
        /// Добавление информации в БД
        /// </summary>
        public static async Task<Status> CreateModule(ParserModel parser, int standId)
        {
            if (standId == 0) { return Statuses.DbError("Неправильный Id стенда"); }

            using (var db = new AppDbContext())
            {
                bool isExist = await db.ExternalModules.Where(ex => ex.FileName == parser.JsonFileName && ex.StandId == standId).AnyAsync();
                if (isExist) { return Statuses.DbError("Такая запись уже существует"); }

                PdConfigStatus status = new();
                ExternalModule module = new()
                {
                    StandId = standId,
                    FileName = parser.JsonFileName,
                    FullPathFile = parser.JsonPathSave,
                    StatusId = status.exist.Id,
                };

                try
                {
                    await db.ExternalModules.AddAsync(module);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Statuses.DbError(ex.Message+" Inner: "+ex.InnerException);
                }
            }
            return Statuses.Ok();
        }

        /// <summary>
        /// Отсоеденить модуль от стенда и удалить его с БД
        /// </summary>
        public static async Task<Status> DeleteModule(int id)
        {
            if (id == 0) { return Statuses.DbError("Неправильный Id"); }

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

        /// <summary>
        /// Загрузить модули подключенные к стенду строронним путём
        /// </summary>
        public static async Task<Status> LoadConnectedModules(Stand stand)
        {
            var standPath = stand.SrvAFolderPath + "Settings\\";
            if (string.IsNullOrEmpty(standPath))
            {
                return Statuses.DbError("Путь к стенду пуст");
            }
            if (!Directory.Exists(standPath)) 
            { 
                return Statuses.UnexpectedError($"Папка не существует:{standPath}");
            }

            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(standPath);

            FileInfo[] filesInfo = hdDirectoryInWhichToSearch.GetFiles("_*" + ".json");
            
            using (var db = new AppDbContext())
            {
                var modulesFromDb = await db.ExternalModules.Where(m => m.StandId == stand.Id).Select(m => m.FileName).ToListAsync();

                var modules = filesInfo.Where(f => modulesFromDb.All(m => !string.Equals(f.Name, m)))
                    .Select(f => new ExternalModule
                    {
                        StandId = stand.Id,
                        FullPathFile = f.FullName.Replace(f.Name,""),
                        FileName = f.Name,
                        DateFileReplacement = f.LastWriteTime.ToString("g"),
                        DateFileVerifiedToExist = DateTime.Now.ToString("g"),
                        StatusId = new PdConfigStatus().exist.Id,
                    }).ToList();

                try
                {
                    await db.ExternalModules.AddRangeAsync(modules);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Statuses.DbError("Ошибка в добавлении подключенных модулей");
                }
            }
            return Statuses.Ok();
        }
    }
}
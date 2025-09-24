using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Domain.Predefineds;
using ConfigManagerStend.Infrastructure.Enums;
using ConfigManagerStend.Infrastructure.Services;
using ConfigManagerStend.Models;
using System.IO;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace ConfigManagerStend.Logic
{
    internal class ParserLogic
    {
        public async Task<Status> ParserFile(ParserModel parser,Stand stand)
        {
            try
            {
                bool isExist = File.Exists(parser.JsonFilePath);

                if (!isExist)
                {
                    return Statuses.FileNotFound(parser.JsonFilePath);
                }
                // Читаем содержимое исходного файла
                string jsonContent = File.ReadAllText(parser.JsonFilePath);

                // Парсим JSON
                var jsonNode = JsonNode.Parse(jsonContent);

                // Изменяем параметр Path (предполагая, что это свойство верхнего уровня)
                if (jsonNode != null)
                {
                    jsonNode["Path"] = parser.DebugPath;
                }

                string modifiedJson = jsonNode!.ToJsonString();
                parser.JsonFileName = "_" + jsonNode["Name"] + ".json";
                parser.JsonPathSave = stand.SrvAFolderPath + "Settings\\";
                // Записываем измененный JSON в новый файл
                File.WriteAllText(Path.Combine(parser.JsonPathSave, parser.JsonFileName), modifiedJson);
            }
            catch (Exception ex)
            {
               return Statuses.UnexpectedError(ex.Message);
            }
            return await ModuleService.CreateModule(parser,stand.Id);
        }
    }
}

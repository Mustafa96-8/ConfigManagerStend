using ConfigManagerStend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using ConfigManagerStend.Infrastructure.Enums;

namespace ConfigManagerStend.Logic
{
    internal class ParserLogic
    {
        public Status ParserFile(ParserModel parser)
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
                parser.JsonFileName = jsonNode["Name"] + ".json";

                // Записываем измененный JSON в новый файл
                File.WriteAllText(Path.Combine(parser.JsonPathSave, "_" + parser.JsonFileName), modifiedJson);
            }
            catch (Exception ex)
            {
               return Statuses.UnexpectedError(ex.Message);
            }
            return Statuses.Ok();
        }
    }
}

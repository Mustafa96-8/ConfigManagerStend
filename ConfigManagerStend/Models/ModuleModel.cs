using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigManagerStend.Models;
public class ModuleModel
{
    /// <summary>
    /// Название файла настроек модуля
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Путь до файла с настройками модуля на стенде
    /// </summary>
    public string Path {  get; set; }

    /// <summary>
    /// Дата изменения файла с настройками модуля
    /// </summary>
    public string Date { get; set; }
}

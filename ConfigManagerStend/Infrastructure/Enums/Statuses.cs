using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigManagerStend.Models;

namespace ConfigManagerStend.Infrastructure.Enums;
public static class Statuses
{
    public static Status FileNotFound(string info) => new("Файл не найден : ", info);

    public static Status PathNotFound(string info) => new("Путь не найден : ", info);

    public static Status DbError(string info) => new("Ошибка в работе с базой данных: ", info);

    public static Status IisError(string info) => new("Ошибка при работе с IIS:", info);
    
    public static Status UnexpectedError(string info) => new("Неизвестная ошибка : ", info);

    public static Status Ok() => new("❤️🍪❤️ Успешно! 🍪❤️🍪 ", string.Empty); 

}

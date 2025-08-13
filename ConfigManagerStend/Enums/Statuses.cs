using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigManagerStend.Models;

namespace ConfigManagerStend.Enums;
public static class Statuses
{
    public static Status FileNotFound(string info) => new("Файл не найден : ", info);
    public static Status UnexpectedError(string info) => new("Неизвестная ошибка : ", info);
    public static Status DeleteError(string info) => new("Ошибка во время удаления : ", info);
    public static Status DeleteSuccessful() => new("❤️🍪❤️ Удаление прошло успешно 🍪❤️🍪", string.Empty);
    public static Status Ok() => new("❤️🍪❤️ Успешно! 🍪❤️🍪 ", string.Empty); 
}

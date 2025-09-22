using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigManagerStend.Domain.Entities;
internal class ExternalModule
{
    public int Id { get; set; }

    /// <summary>
    /// ключ для связки стенда
    /// </summary>
    [ForeignKey(nameof(Stand))]
    public required int StandId { get; set; }

    public required Stand Stand { get; set; }
    /// <summary>
    /// Полный путь файла
    /// </summary>
    public required string FullPathFile { get; set; }

    /// <summary>
    /// Наименование файла
    /// </summary>
    public required string FileName { get; set; }

    /// <summary>
    /// Дата подмены
    /// </summary>
    public DateTime DateFileReplacement { get; set; } = DateTime.Now;

    /// <summary>
    /// Послендяя проверка существования файла
    /// </summary>
    public DateTime? DateFileVerifiedToExist { get; set; } = null;

    /// <summary>
    /// Статус ID
    /// </summary>
    [ForeignKey(nameof(Status))]
    public int StatusId { get; set; }

    /// <summary>
    /// Статус
    /// </summary>
    public ConfigStatus? Status { get; set; }
}


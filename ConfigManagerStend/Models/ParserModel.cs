namespace ConfigManagerStend.Models
{
    internal class ParserModel
    {
        /// <summary>
        /// Наименование Json файла
        /// </summary>
        public string JsonFileName { get; set; } = string.Empty;

        /// <summary>
        /// Путь до файла metadata.json
        /// </summary>
        public string JsonFilePath { get; set; } = string.Empty;

        /// <summary>
        /// Путь куда будет сохраняться новый Json файл
        /// </summary>
        public string JsonPathSave { get; set; } = string.Empty;

        /// <summary>
        /// Путь до файлов  debug
        /// </summary>
        public string DebugPath { get; set; } = string.Empty;
    }
}

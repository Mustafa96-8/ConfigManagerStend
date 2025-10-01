using ConfigManagerStend.Domain.Entities;
using System.Diagnostics;
using System.IO;

namespace ConfigManagerStend.Logic
{
    public static class StendLogic
    {
        public static void CreateStend(ConfigStend config,
                              Action<string>? outputCallback = null,
                              Action<string>? errorCallback = null)
        {
            // Загружаем шаблон скрипта
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "Delo.ps1");
            string templateContent = File.ReadAllText(templatePath);

            // Заполняем шаблон значениями
            string filledScript = FillTemplate(templateContent, config);

            // Сохраняем готовый скрипт
            string outputScriptPath = Path.Combine(config.PathStend, $"{config.Appn}_deploy.ps1");
            File.WriteAllText(outputScriptPath, filledScript);

            // Сохраняем конфиг в БД

            // Запускаем скрипт
            ExecutePowerShellScript(outputScriptPath, outputCallback, errorCallback);
        }

        private static string FillTemplate(string template, ConfigStend config)
        {
            // Заменяем плейсхолдеры значениями из конфигурации
            return template
                .Replace("{{appn}}", config.Appn)
                .Replace("{{stand}}", config.PathStend)
                .Replace("{{portA}}", config.PortA.ToString())
                .Replace("{{dbOwner}}", config.DbOwner)
                .Replace("{{TeamProject}}", config.BuildDefinition.TeamProject.NameProject)
                .Replace("{{BuildDefinition}}", config.BuildDefinition.NameRepo)
                .Replace("{{BranchName}}", config.BranchName)
                .Replace("{{CreateDB}}", config.SettingDb)
                .Replace("{{PlatformHostDir}}", config.PlatformHostDir)
                .Replace("{{BoxSettingsDir}}", config.BoxSettingDir);
        }

        private static void ExecutePowerShellScript(string scriptPath,
                                           Action<string>? outputCallback,
                                           Action<string>? errorCallback)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoExit -ExecutionPolicy Bypass -File \"{scriptPath}\"", // Добавляем -NoExit
                    UseShellExecute = true,        // Меняем на true для показа окна
                    CreateNoWindow = false,        // Окно будет создано
                    WindowStyle = ProcessWindowStyle.Normal, // Обычное окно
                    WorkingDirectory = Path.GetDirectoryName(scriptPath) // Рабочая директория
                };

                using (Process process = new Process())
                {
                    process.StartInfo = psi;
                    process.Start();

                    // Ждем немного чтобы окно успело открыться
                    Thread.Sleep(1000);

                    // Можно опционально ждать завершения, но с -NoExit процесс не завершится сам
                    // process.WaitForExit();

                    outputCallback?.Invoke("Окно PowerShell открыто. Скрипт выполняется...");
                }
            }
            catch (Exception ex)
            {
                errorCallback?.Invoke($"Ошибка выполнения скрипта: {ex.Message}");
            }
        }
    }
}

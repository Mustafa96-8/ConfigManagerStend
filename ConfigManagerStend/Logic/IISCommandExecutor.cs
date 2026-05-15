using ConfigManagerStend.Infrastructure.Enums;
using ConfigManagerStend.Models;
using Microsoft.Web.Administration;

namespace ConfigManagerStend.Logic
{
    public static class IISCommandExecutor
    {
        public static Status DoIISCommand(IssCommands command, string appPoolName)
        {
            if (string.IsNullOrWhiteSpace(appPoolName))
                return Statuses.IisError("Название приложения для опреации пустое!");
            
            using (ServerManager serverManager = new ServerManager())
            {
                ApplicationPool appPool = serverManager.ApplicationPools[appPoolName];
                Site site = serverManager.Sites[appPoolName];
                if (appPool == null)
                    return Statuses.IisError($"Пул приложений '{appPoolName}' не найден.");
                if (site == null)
                    return Statuses.IisError($"Сайт приложений '{appPoolName}' не найден.");
                
                switch (command)
                {
                    case IssCommands.Start:
                        if (appPool.State == ObjectState.Stopped)
                            appPool.Start();
                        if (site.State == ObjectState.Stopped)
                            site.Start();
                        return Statuses.Ok();
                    case IssCommands.Stop:
                        if (appPool.State == ObjectState.Started)
                            appPool.Stop();
                        if (site.State == ObjectState.Started)
                            site.Stop();
                        return Statuses.Ok();
                    default:
                        return Statuses.IisError($"Команда '{command.ToString()}' не поддерживается.");
                }
            }
        }

        public static IssAppState GetStatus(string webAppPoolName,string srvAppPoolName)
        {
            if (string.IsNullOrWhiteSpace(webAppPoolName))
                throw new ArgumentException("AppPoolName не может быть пустым.");
            if (string.IsNullOrWhiteSpace(srvAppPoolName))
                throw new ArgumentException("AppPoolName не может быть пустым.");

            using (ServerManager serverManager = new ServerManager())
            {
                ApplicationPool webAppPool = serverManager.ApplicationPools[webAppPoolName];
                ApplicationPool srvAppPool = serverManager.ApplicationPools[srvAppPoolName];
                Site webSite = serverManager.Sites[webAppPoolName];
                Site srvSite = serverManager.Sites[srvAppPoolName];
                if (webAppPool == null)
                    throw new ArgumentException($"Пул приложений '{webAppPool}' не найден.");
                if (srvAppPool == null)
                    throw new ArgumentException($"Пул приложений '{srvAppPool}' не найден.");
                if (webSite == null)
                    throw new ArgumentException($"Сайт приложений '{webSite}' не найден.");
                if (srvSite == null)
                    throw new ArgumentException($"Сайт приложений '{srvSite}' не найден.");

                return new IssAppState(
                    webAppPool.State == ObjectState.Started ? "Green" : "Red",
                    srvAppPool.State == ObjectState.Started ? "Green" : "Red",
                    webSite.State == ObjectState.Started ? "Green" : "Red",
                    srvSite.State == ObjectState.Started ? "Green" : "Red");
            }
        }
    }
}

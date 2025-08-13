using ConfigManagerStend.Enums;
using ConfigManagerStend.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace ConfigManagerStend.Logic;
internal class ManagerLogic
{
    public static Status DeleteExternalModules(System.Collections.IList moduleList)
    {
        try
        {
            foreach (var module in moduleList)
            {
                var model = (ModuleModel)module;
                File.Delete(model.Path);
            }
        }
        catch (Exception ex)
        {
            return Statuses.DeleteError(ex.Message);
        }
        return Statuses.DeleteSuccessful();
    }
}

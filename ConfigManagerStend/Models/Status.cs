using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigManagerStend.Models;
public class Status
{
    public string Message { get; private set; }

    public string SystemInfo { get; private set; }

    public Status(string message, string systemInfo)
    {
        Message = message;
        SystemInfo = systemInfo;
    }
}

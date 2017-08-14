using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ModuleNet.ModuleNet;
using ModuleNet.ModuleNet.Attributes;

namespace TdsHelper
{
    [Injectable]
    public class ErrorLogger
    {
        public void SaveErrorToLog(string message, string stackTrace)
        {
            var content = $"Message: {message}.\r\nStackTrace: {stackTrace}";
            File.WriteAllText(Application.Configuration["errorlog"], content);
        }
    }
}

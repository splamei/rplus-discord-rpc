using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Rhythm_Plus_Discord_RPC
{
    public static class Logging
    {
        public static bool addStarter = false;

        public static void logString(string message)
        {
            Debug.WriteLine(message);

            DateTime time = DateTime.Now;

            if (addStarter)
            {
                File.AppendAllLines(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/RPC.log", new List<string> { $"- {time.ToString("dd/MM/yyyy HH:mm")} - {message}" });
            }
            else
            {
                File.AppendAllLines(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/RPC.log", new List<string> { message });
            }
        }
    }
}

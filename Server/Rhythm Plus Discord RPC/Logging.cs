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

            string savePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Splamei",
                "Rhythm Plus - Splamei Client",
                "RPC.log");

            if (!PathHelper.isSafe(savePath))
            {
                throw new UnauthorizedAccessException("Attempted to access or read a path outside of the bounds of the client's save directorys");
            }

            DateTime time = DateTime.Now;

            if (addStarter)
            {
                File.AppendAllLines(savePath, new List<string> { $"- {time.ToString("dd/MM/yyyy HH:mm")} - {message}" });
            }
            else
            {
                File.AppendAllLines(savePath, new List<string> { message });
            }
        }
    }
}

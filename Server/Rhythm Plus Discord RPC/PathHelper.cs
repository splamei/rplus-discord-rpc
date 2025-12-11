using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhythm_Plus_Discord_RPC
{
    internal class PathHelper
    {
        internal static bool isSafe(string path)
        {
            var fullPath = Path.GetFullPath(path);
            if (fullPath == null) return false;
            if (fullPath.StartsWith(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Splamei",
                    "Rhythm Plus - Splamei Client")))
            {
                return true;
            }

            return false;
        }
    }
}

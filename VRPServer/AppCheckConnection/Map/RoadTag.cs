using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppCheckConnection.Map
{
    internal class RoadTag
    {
        internal static bool GetIndexPresentCode(int index, out string codeShow)
        {
            string path = $"{Config.rootPath}\\tag\\record_{index}.txt";
            if (File.Exists(path))
            {
                codeShow = File.ReadAllText(path);
                return true;
            }
            else
            {
                codeShow = null;
                return false;
            }
        }
    }
}

using System.Collections.Generic;
using System.IO;

namespace bootstrap_v4_snippets_build
{
    public static class DirectoryScanner
    {
        public static void FindFilesRecursive(string dir, List<string> paths)
        {
            var di = new DirectoryInfo(dir);
            foreach (var subDi in di.GetDirectories())
            {
                FindFilesRecursive(subDi.FullName, paths);
            }

            foreach (var fi in di.GetFiles())
            {
                paths.Add(fi.FullName);
            }
        }
    }
}

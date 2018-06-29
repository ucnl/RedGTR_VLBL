using System;
using System.IO;

namespace RedGTR_VLBL
{
    public static class StrUtils
    {
        public static string GetHMSString(DateTime dt)
        {
            return string.Format("{0:00}-{1:00}-{2:00}", dt.Hour, dt.Minute, dt.Second);
        }

        public static string GetHMSString()
        {
            return GetHMSString(DateTime.Now);
        }

        public static string GetYMDString()
        {
            return GetYMDString(DateTime.Now);
        }

        public static string GetYMDString(DateTime dt)
        {
            return string.Format("{0}-{1:00}-{2:00}", dt.Year, dt.Month, dt.Day);
        }

        public static string GetTimeDirTree(DateTime dt, string appPath, string targetPath, bool isCreate)
        {            
            string fullDirecoryName = Path.Combine(Path.Combine(Path.GetDirectoryName(appPath), targetPath), GetYMDString(dt));

            if (!Directory.Exists(fullDirecoryName) && isCreate)
                Directory.CreateDirectory(fullDirecoryName);

            return fullDirecoryName;
        }

        public static string GetTimeDirTree(string appPath, string targetPath, bool isCreate)
        {
            return GetTimeDirTree(DateTime.Now, appPath, targetPath, isCreate);
        }

        public static string GetTimeDirTreeFileName(DateTime dt, string appPath, string targetPath, string extension, bool isCreate)
        {
            string fullDirecoryName = GetTimeDirTree(dt, appPath, targetPath, isCreate);
            string fileName = string.Format("{0}.{1}", GetHMSString(dt), extension);
            return Path.Combine(fullDirecoryName, fileName);
        }

        public static string GetTimeDirTreeFileName(string appPath, string targetPath, string extension, bool isCreate)
        {
            DateTime dt = DateTime.Now;
            return GetTimeDirTreeFileName(dt, appPath, targetPath, extension, isCreate);
        }
    }
}

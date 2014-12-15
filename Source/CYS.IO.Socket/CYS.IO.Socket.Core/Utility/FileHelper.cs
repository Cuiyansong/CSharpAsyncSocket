using System;

namespace CYS.IO.Socket.Core.Utility
{
    /// <summary>
    /// File Helper
    /// </summary>
    public class FileHelper
    {
        #region Private Properties
        /// <summary>
        /// _lopen
        /// </summary>
        /// <param name="lpPathName"></param>
        /// <param name="iReadWrite"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern IntPtr _lopen(string lpPathName, int iReadWrite);
        /// <summary>
        /// CloseHandle
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        private const int OF_READWRITE = 2;
        private const int OF_SHARE_DENY_NONE = 0x40;
        private static readonly IntPtr HFILE_ERROR = new IntPtr(-1);
        #endregion Private Properties


        #region Public Method
        /// <summary>
        /// CreateFolder
        /// </summary>
        /// <param name="fullpath"></param>
        public static void CreateFolder(string fullpath)
        {
            if (!System.IO.Directory.Exists(fullpath))
            {
                System.IO.Directory.CreateDirectory(fullpath);
            }
        }
        /// <summary>
        /// IsFileExist
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static bool IsFileExist(string fullpath)
        {
            if (string.IsNullOrEmpty(fullpath))
                return false;

            return System.IO.File.Exists(fullpath);
        }
        /// <summary>
        /// CanRead
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static bool CanRead(string fullName)
        {
            if (!IsFileExist(fullName)) return false;

            try
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(fullName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read))
                {
                    return fs.CanRead;
                }
            }
            catch
            {
                return false;
            }


            //// 打开指定文件看看情况 
            //IntPtr vHandle = _lopen(fullName, OF_READWRITE | OF_SHARE_DENY_NONE);
            //if (vHandle == HFILE_ERROR)
            //{ // 文件已经被打开                 
            //    return false;
            //}
            //CloseHandle(vHandle);
        }
        /// <summary>
        /// DeleteFile
        /// </summary>
        /// <param name="p"></param>
        public static void DeleteFile(string p)
        {
            if (IsFileExist(p))
                System.IO.File.Delete(p);
        }
        #endregion Public Method
    }
}

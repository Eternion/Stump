using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Stump.Tools.Toolkit.Helpers
{
    public static class IconHelper
    {
        private static Icon m_folderIcon; 
        private static readonly Dictionary<string, Icon> AssociatedIcons = new Dictionary<string, Icon>();

        public static Icon GetFolderIcon()
        {
            if (m_folderIcon != null)
                return m_folderIcon;

            var shinfo = new Shfileinfo();

            IntPtr img = SHGetFileInfo(Environment.CurrentDirectory, 0, ref shinfo,
                                       (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON);

            return m_folderIcon = Icon.FromHandle(shinfo.hIcon);
        }

        public static Icon GetFileIcon(string file)
        {
            var ext = Path.GetExtension(file);

            if (string.IsNullOrEmpty(file))
                return null;

            if (AssociatedIcons.ContainsKey(ext))
                return AssociatedIcons[ext];

            var icon = ExtractIconFromFile((Extensions.ContainsKey(ext) ? Extensions[ext] :
                Extensions["Unknown"]).ToString(), false);

            if (icon != null)
            {
                AssociatedIcons.Add(ext, icon);
            }
            else
            {
                throw new Exception(string.Format("Cannot found icon associated to {0}", file));
            }

            return AssociatedIcons[ext];
        }

        #region WIN32

        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; // 'Large icon
        public const uint SHGFI_SMALLICON = 0x1; // 'Small icon

        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath,
                                                   uint dwFileAttributes,
                                                   ref Shfileinfo psfi,
                                                   uint cbSizeFileInfo,
                                                   uint uFlags);

        [StructLayout(LayoutKind.Sequential)]
        private struct Shfileinfo
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        } ;

        private struct EmbeddedIconInfo
        {
            public string FileName;
            public int IconIndex;
        }


        [DllImport("shell32.dll", EntryPoint = "ExtractIconA", CharSet = CharSet.Ansi, SetLastError = true,
            ExactSpelling = true)]
        private static extern IntPtr ExtractIcon(int hInst, string lpszExeFileName, int nIconIndex);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern uint ExtractIconEx(string szFileName, int nIconIndex, IntPtr[] phiconLarge,
                                                 IntPtr[] phiconSmall, uint nIcons);

        [DllImport("user32.dll", EntryPoint = "DestroyIcon", SetLastError = true)]
        private static extern int DestroyIcon(IntPtr hIcon);



        private static readonly Hashtable Extensions = GetFileTypeAndIcon();

        /// <summary>
        /// Gets registered file types and their associated icon in the system.
        /// </summary>
        /// <returns>Returns a hash table which contains the file extension as keys, the icon file and param as values.</returns>
        private static Hashtable GetFileTypeAndIcon()
        {
            // Create a registry key object to represent the HKEY_CLASSES_ROOT registry section
            RegistryKey rkRoot = Registry.ClassesRoot;

            //Gets all sub keys' names.
            string[] keyNames = rkRoot.GetSubKeyNames();
            var iconsInfo = new Hashtable();

            //Find the file icon.
            foreach (string keyName in keyNames)
            {
                if (String.IsNullOrEmpty(keyName))
                    continue;
                int indexOfPoint = keyName.IndexOf(".");

                //If this key is not a file exttension(eg, .zip), skip it.
                if (indexOfPoint != 0)
                    continue;

                RegistryKey rkFileType = rkRoot.OpenSubKey(keyName);
                if (rkFileType == null)
                    continue;

                //Gets the default value of this key that contains the information of file type.
                object defaultValue = rkFileType.GetValue("");
                if (defaultValue == null)
                    continue;

                //Go to the key that specifies the default icon associates with this file type.
                string defaultIcon = defaultValue + "\\DefaultIcon";
                RegistryKey rkFileIcon = rkRoot.OpenSubKey(defaultIcon);
                if (rkFileIcon != null)
                {
                    //Get the file contains the icon and the index of the icon in that file.
                    object value = rkFileIcon.GetValue("");
                    if (value != null)
                    {
                        //Clear all unecessary " sign in the string to avoid error.
                        string fileParam = value.ToString().Replace("\"", "");
                        iconsInfo.Add(keyName, fileParam);
                    }
                    rkFileIcon.Close();
                }
                rkFileType.Close();
            }

            RegistryKey unknownIcon = rkRoot.OpenSubKey("Unknown\\DefaultIcon");
            if (unknownIcon != null)
            {
                //Get the file contains the icon and the index of the icon in that file.
                object value = unknownIcon.GetValue("");
                if (value != null)
                {
                    //Clear all unecessary " sign in the string to avoid error.
                    string fileParam = value.ToString().Replace("\"", "");
                    iconsInfo.Add("Unknown", fileParam);
                }
                unknownIcon.Close();
            }

            rkRoot.Close();
            return iconsInfo;
        }

        /// <summary>
        /// Extract the icon from file.
        /// </summary>
        /// <param name="fileAndParam">The params string, 
        /// such as ex: "C:\\Program Files\\NetMeeting\\conf.exe,1".</param>
        /// <param name="isLarge">
        /// Determines the returned icon is a large (may be 32x32 px) 
        /// or small icon (16x16 px).</param>
        private static Icon ExtractIconFromFile(string fileAndParam, bool isLarge)
        {
            uint readIconCount = 0;
            var hDummy = new IntPtr[1] {IntPtr.Zero};
            var hIconEx = new IntPtr[1] {IntPtr.Zero};

            try
            {
                EmbeddedIconInfo embeddedIcon = GetEmbeddedIconInfo(fileAndParam);

                if (isLarge)
                    readIconCount = ExtractIconEx(embeddedIcon.FileName, embeddedIcon.IconIndex, hIconEx, hDummy, 1);
                else
                    readIconCount = ExtractIconEx(embeddedIcon.FileName, embeddedIcon.IconIndex, hDummy, hIconEx, 1);

                if (readIconCount > 0 && hIconEx[0] != IntPtr.Zero)
                {
                    // Get first icon.
                    var extractedIcon = (Icon) Icon.FromHandle(hIconEx[0]).Clone();

                    return extractedIcon;
                }
                else // No icon read
                    return null;
            }
            catch (Exception exc)
            {
                // Extract icon error.
                throw new ApplicationException("Could not extract icon", exc);
            }
            finally
            {
                // Release resources.
                foreach (IntPtr ptr in hIconEx)
                    if (ptr != IntPtr.Zero)
                        DestroyIcon(ptr);

                foreach (IntPtr ptr in hDummy)
                    if (ptr != IntPtr.Zero)
                        DestroyIcon(ptr);
            }
        }



        /// <summary>
        /// Parses the parameters string to the structure of EmbeddedIconInfo.
        /// </summary>
        /// <param name="fileAndParam">The params string, 
        /// such as ex: "C:\\Program Files\\NetMeeting\\conf.exe,1".</param>
        /// <returns></returns>
        private static EmbeddedIconInfo GetEmbeddedIconInfo(string fileAndParam)
        {
            EmbeddedIconInfo embeddedIcon = new EmbeddedIconInfo();

            if (String.IsNullOrEmpty(fileAndParam))
                return embeddedIcon;

            //Use to store the file contains icon.
            string fileName = String.Empty;

            //The index of the icon in the file.
            int iconIndex = 0;
            string iconIndexString = String.Empty;

            int commaIndex = fileAndParam.IndexOf(",");
            //if fileAndParam is some thing likes that: "C:\\Program Files\\NetMeeting\\conf.exe,1".
            if (commaIndex > 0)
            {
                fileName = fileAndParam.Substring(0, commaIndex);
                iconIndexString = fileAndParam.Substring(commaIndex + 1);
            }
            else
                fileName = fileAndParam;

            if (!String.IsNullOrEmpty(iconIndexString))
            {
                //Get the index of icon.
                iconIndex = int.Parse(iconIndexString);
            }

            embeddedIcon.FileName = fileName;
            embeddedIcon.IconIndex = iconIndex;

            return embeddedIcon;
        }

        #endregion
    }
}
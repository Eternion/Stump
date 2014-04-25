using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Uplauncher.Helpers
{
    public static class WebBrowserHelper
    {
        #region Definitions/DLL Imports
        /// <summary>
        /// For PInvoke Contains information about an entry in the Internet cache
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 80, CharSet = CharSet.Auto)]
        public struct INTERNET_CACHE_ENTRY_INFOA
        {
            public int dwStructSize;
            [MarshalAs(UnmanagedType.LPTStr)]
            public String lpszSourceUrlName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public String lpszLocalFileName;
            public int CacheEntryType;
            public int dwUseCount;
            public int dwHitRate;
            public int dwSizeLow;
            public int dwSizeHigh;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastModifiedTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ExpireTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastSyncTime;
            public IntPtr lpHeaderInfo;
            public int dwHeaderInfoSize;
            public IntPtr lpszFileExtension;
            public int dwExemptDelta;
        }

        // For PInvoke Initiates the enumeration of the cache groups in the Internet cache
        [DllImport(@"wininet",
          SetLastError = true,
          CharSet = CharSet.Auto,
          EntryPoint = "FindFirstUrlCacheGroup",
          CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr FindFirstUrlCacheGroup(
          int dwFlags,
          int dwFilter,
          IntPtr lpSearchCondition,
          int dwSearchCondition,
          ref long lpGroupId,
          IntPtr lpReserved);

        // For PInvoke Retrieves the next cache group in a cache group enumeration
        [DllImport(@"wininet",
          SetLastError = true,
          CharSet = CharSet.Auto,
          EntryPoint = "FindNextUrlCacheGroup",
          CallingConvention = CallingConvention.StdCall)]
        public static extern bool FindNextUrlCacheGroup(
          IntPtr hFind,
          ref long lpGroupId,
          IntPtr lpReserved);

        // For PInvoke Releases the specified GROUPID and any associated state in the cache index file
        [DllImport(@"wininet",
          SetLastError = true,
          CharSet = CharSet.Auto,
          EntryPoint = "DeleteUrlCacheGroup",
          CallingConvention = CallingConvention.StdCall)]
        public static extern bool DeleteUrlCacheGroup(
          long GroupId,
          int dwFlags,
          IntPtr lpReserved);

        // For PInvoke Begins the enumeration of the Internet cache
        [DllImport(@"wininet",
          SetLastError = true,
          CharSet = CharSet.Auto,
         EntryPoint = "FindFirstUrlCacheEntryA",
         CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr FindFirstUrlCacheEntry(
          [MarshalAs(UnmanagedType.LPTStr)] string lpszUrlSearchPattern,
          IntPtr lpFirstCacheEntryInfo,
          ref int lpdwFirstCacheEntryInfoBufferSize);

        // For PInvoke Retrieves the next entry in the Internet cache
        [DllImport(@"wininet",
          SetLastError = true,
          CharSet = CharSet.Auto,
          EntryPoint = "FindNextUrlCacheEntryA",
          CallingConvention = CallingConvention.StdCall)]
        public static extern bool FindNextUrlCacheEntry(
          IntPtr hFind,
          IntPtr lpNextCacheEntryInfo,
          ref int lpdwNextCacheEntryInfoBufferSize);

        // For PInvoke Removes the file that is associated with the source name from the cache, if the file exists
        [DllImport(@"wininet",
          SetLastError = true,
          CharSet = CharSet.Auto,
          EntryPoint = "DeleteUrlCacheEntryA",
          CallingConvention = CallingConvention.StdCall)]
        public static extern bool DeleteUrlCacheEntry(
          IntPtr lpszUrlName);
        #endregion

        #region Public Static Functions

        public static void EnumerateCache()
        {
            const int ERROR_NO_MORE_ITEMS = 259;
            int neededBytes = 0;

            // get the size of the required buffer
            FindFirstUrlCacheEntry(null, IntPtr.Zero, ref neededBytes);

            if (Marshal.GetLastWin32Error() == ERROR_NO_MORE_ITEMS)
            {
                // nothing to do
                return;
            }

            int bufferByteSize = neededBytes;
            IntPtr bufferPtr = Marshal.AllocHGlobal(bufferByteSize);
            try
            {
                IntPtr hEnum = FindFirstUrlCacheEntry(null, bufferPtr, ref neededBytes);

                bool successful;
                INTERNET_CACHE_ENTRY_INFOA cacheItem;
                do
                {
                    cacheItem = (INTERNET_CACHE_ENTRY_INFOA)Marshal.PtrToStructure(bufferPtr, typeof(INTERNET_CACHE_ENTRY_INFOA));
                    Trace.WriteLine(string.Format("Source URL: {0}", cacheItem.lpszSourceUrlName));

                    neededBytes = bufferByteSize;
                    successful = FindNextUrlCacheEntry(hEnum, bufferPtr, ref neededBytes);

                    if (successful || Marshal.GetLastWin32Error() != ERROR_NO_MORE_ITEMS)
                    {
                        if (!successful && neededBytes > bufferByteSize)
                        {
                            bufferByteSize = neededBytes;
                            bufferPtr = Marshal.ReAllocHGlobal(bufferPtr, (IntPtr)bufferByteSize);
                            successful = true; // continue
                        }
                    }
                } while (successful);
            }
            finally
            {
                Marshal.FreeHGlobal(bufferPtr);
            }
        }

        /// <summary>
        /// Clears the cache of the web browser
        /// </summary>
        public static void ClearCache(string host)
        {
            EnumerateCache();

            // Indicates that all of the cache groups in the user's system should be enumerated
            const int CACHEGROUP_SEARCH_ALL = 0x0;
            // Indicates that all the cache entries that are associated with the cache group
            // should be deleted, unless the entry belongs to another cache group.
            const int CACHEGROUP_FLAG_FLUSHURL_ONDELETE = 0x2;
            // File not found.
            const int ERROR_FILE_NOT_FOUND = 0x2;
            // No more items have been found.
            const int ERROR_NO_MORE_ITEMS = 259;
            // Pointer to a GROUPID variable
            long groupId = 0;

            // Local variables
            int cacheEntryInfoBufferSizeInitial = 0;
            int cacheEntryInfoBufferSize = 0;
            IntPtr cacheEntryInfoBuffer = IntPtr.Zero;
            INTERNET_CACHE_ENTRY_INFOA internetCacheEntry;
            IntPtr enumHandle = IntPtr.Zero;
            bool returnValue = false;


            // Start to delete URLs that do not belong to any group.
            enumHandle = FindFirstUrlCacheEntry(null, IntPtr.Zero, ref cacheEntryInfoBufferSizeInitial);
            if (enumHandle != IntPtr.Zero && ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error())
                return;

            cacheEntryInfoBufferSize = cacheEntryInfoBufferSizeInitial;
            cacheEntryInfoBuffer = Marshal.AllocHGlobal(cacheEntryInfoBufferSize);
            enumHandle = FindFirstUrlCacheEntry(null, cacheEntryInfoBuffer, ref cacheEntryInfoBufferSizeInitial);

            Debug.WriteLine(Marshal.GetLastWin32Error());

            while (true)
            {
                internetCacheEntry = (INTERNET_CACHE_ENTRY_INFOA)Marshal.PtrToStructure(cacheEntryInfoBuffer, typeof(INTERNET_CACHE_ENTRY_INFOA));
                if (ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error())
                {
                    break;
                }

                Debug.WriteLine(Marshal.GetLastWin32Error());

                cacheEntryInfoBufferSizeInitial = cacheEntryInfoBufferSize;
                returnValue = DeleteUrlCacheEntry(Marshal.StringToHGlobalAuto(internetCacheEntry.lpszSourceUrlName));
                if (!returnValue)
                {
                    returnValue = FindNextUrlCacheEntry(enumHandle, cacheEntryInfoBuffer, ref cacheEntryInfoBufferSizeInitial);
                }
                if (!returnValue && ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error())
                {
                    break;
                }
                if (!returnValue && cacheEntryInfoBufferSizeInitial > cacheEntryInfoBufferSize)
                {
                    cacheEntryInfoBufferSize = cacheEntryInfoBufferSizeInitial;
                    cacheEntryInfoBuffer = Marshal.ReAllocHGlobal(cacheEntryInfoBuffer, (IntPtr)cacheEntryInfoBufferSize);
                    returnValue = FindNextUrlCacheEntry(enumHandle, cacheEntryInfoBuffer, ref cacheEntryInfoBufferSizeInitial);
                }
            }
            Marshal.FreeHGlobal(cacheEntryInfoBuffer);
        }
        #endregion
    }
}
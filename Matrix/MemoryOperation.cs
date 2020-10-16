using System;
using System.Runtime.InteropServices;

namespace Molytho.Matrix
{
    internal static class MemoryOperation
    {
        #region Imports
        [DllImport("libc.so.6", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        private static unsafe extern void* memset_unix(void* ptr, int value, int size);
        [DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        private static unsafe extern void* memset_windows(void* ptr, int value, int size);
        [DllImport("libc.so.6", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        private static unsafe extern void* memcpy_unix(void* dest, void* src, int size);
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        private static unsafe extern void* memcpy_windows(void* dest, void* src, int size);
        #endregion

        #region Delegate definition
        private unsafe delegate void* memset_delegate(void* ptr, int value, int size);
        private unsafe delegate void* memcpy_delegate(void* dest, void* src, int size);
        #endregion

        #region Delegates
        private static readonly memset_delegate memset_del;
        private static readonly memcpy_delegate memcpy_del;
        #endregion

        #region Constructor
        static unsafe MemoryOperation()
        {
            memset_del = Environment.OSVersion.Platform switch
            {
                PlatformID.Win32NT => memset_windows,
                PlatformID.Unix => memset_unix,
                _ => throw new PlatformNotSupportedException()
            };
            memcpy_del = Environment.OSVersion.Platform switch
            {
                PlatformID.Win32NT => memcpy_windows,
                PlatformID.Unix => memcpy_unix,
                _ => throw new PlatformNotSupportedException()
            };
        }
        #endregion

        #region Public methods
        public static unsafe void* memset(void* ptr, int value, int size) => memset_del(ptr, value, size);
        public static unsafe void* memcpy(void* dest, void* src, int size) => memcpy_del(dest, src, size);
        #endregion

    }
}

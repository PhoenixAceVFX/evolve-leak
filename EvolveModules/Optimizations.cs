using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Evolve.Modules
{
    class Optimizations
    {

        [DllImport("KERNEL32.DLL", EntryPoint =
        "SetProcessWorkingSetSize", SetLastError = true,
        CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize32Bit
        (IntPtr pProcess, int dwMinimumWorkingSetSize,
        int dwMaximumWorkingSetSize);

        [DllImport("KERNEL32.DLL", EntryPoint =
           "SetProcessWorkingSetSize", SetLastError = true,
           CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize64Bit
           (IntPtr pProcess, long dwMinimumWorkingSetSize,
           long dwMaximumWorkingSetSize);

        public static void RamClear()
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize32Bit(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        public static IEnumerator Loop()
        {
            while (true)
            {
                yield return new WaitForSeconds(20);
                RamClear();
            }
        }
    }
}

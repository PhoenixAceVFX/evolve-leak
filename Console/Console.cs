using MelonLoader;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Evolve.ConsoleUtils
{
    internal class EvoConsole
    {
        public static unsafe void Initialize()
        {
            string[] methods = {
                "NativeMethodInfoPtr_WriteLine_Public_Static_Void_Object_0",
                "NativeMethodInfoPtr_WriteLine_Public_Static_Void_String_0",
                "NativeMethodInfoPtr_WriteLine_Public_Static_Void_String_Object_Object_0"
            };

            foreach (string name in methods)
            {
                Hook((IntPtr) (&*(IntPtr*) (IntPtr) typeof(Il2CppSystem.Console).GetField(name,
                    BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)),
                    Marshal.GetFunctionPointerForDelegate(new Action(() => { })));
            }
        }

#pragma warning disable CS0618 // 'Imports' est obsolète : 'Imports is obsolete.'
        public static void Hook(IntPtr _old, IntPtr _new) => typeof(Imports).GetMethod("Hook", Harmony.AccessTools.all).Invoke(null, new object[] { _old, _new });
#pragma warning restore CS0618 // 'Imports' est obsolète : 'Imports is obsolete.'

        public static void WriteToConsole(ConsoleColor Evolve, string value1, ConsoleColor Engine, string value2, ConsoleColor TextColor, string Text)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            string SystemTime = DateTime.Now.ToString("[" + "h:mm:ss.ms" + "] ");
            System.Console.Write(SystemTime);
            System.Console.ForegroundColor = Evolve;
            System.Console.Write(value1);
            System.Console.ForegroundColor = Engine;
            System.Console.Write(value2);
            System.Console.ForegroundColor = TextColor;
            System.Console.WriteLine(Text);
            System.Console.ResetColor();
        }

        public static void Log(string text) => WriteToConsole(ConsoleColor.Magenta, "[Evolve", ConsoleColor.Red, "Engine]: ", ConsoleColor.Magenta, text);
        public static void LogError(string text) => WriteToConsole(ConsoleColor.Magenta, "[Evolve", ConsoleColor.Red, "Engine]: ", ConsoleColor.Red, text);
        public static void LogSuccess(string text) => WriteToConsole(ConsoleColor.Magenta, "[Evolve", ConsoleColor.Red, "Engine]: ", ConsoleColor.Green, text);
        public static void LogWarn(string text) => WriteToConsole(ConsoleColor.Magenta, "[Evolve", ConsoleColor.Red, "Engine]: ", ConsoleColor.Yellow, text);

    }
}
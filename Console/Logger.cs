using Evolve.Utils;
using System.IO;

namespace Evolve.Logger
{
    internal class Logger
    {
        public const string LogsDirectory = "Evolve/Logs/";

        public static void FileCreation()
        {
            if (!Directory.Exists(LogsDirectory))
            {
                Directory.CreateDirectory(LogsDirectory);
            }
        }

        public static void Save(string Log)
        {
            if (!Settings.LoggerEnable)
            {
                return;
            }

            if (!Directory.Exists(LogsDirectory))
            {
                FileCreation();
            }

            StreamWriter File = new StreamWriter($"{LogsDirectory}Logs.txt", true);
            File.Write(Log);
            File.Close();
        }
    }
}
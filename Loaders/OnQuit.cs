using DiscordRichPresence;
using Evolve.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolve.Loaders
{
    class OnQuit
    {
        public static void Initialize()
        {
            if (Settings.ComeBack) ComeBack.ComeBack.OnQuit();
            DiscordManager.OnApplicationQuit();
            Process.GetCurrentProcess().Kill();
        }
    }
}

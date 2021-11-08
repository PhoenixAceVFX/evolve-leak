using System.Linq;

namespace Evolve.Utils
{
    internal class InstanceIDUtilities
    {
        public static string GetInstanceID(string baseID)
        {
            if (baseID.Contains('~'))
            {
                return baseID.Substring(0, baseID.IndexOf('~'));
            }
            return baseID;
        }
    }
}
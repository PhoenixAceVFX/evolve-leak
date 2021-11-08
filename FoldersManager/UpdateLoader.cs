using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Evolve.FoldersManager
{
    class UpdateLoader
    {
        public static void Update()
        {
            ServicePointManager.ServerCertificateValidationCallback = (System.Object s, X509Certificate c, X509Chain cc, SslPolicyErrors ssl) => true;
            HttpWebResponse httpWebResponse = (HttpWebResponse) WebRequest.Create("https://dl.dropboxusercontent.com/s/jja8llf1sgb99ta/Loader%20-%200.4.2?dl=0").GetResponse();

            string EvolveID = "";
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low/GeneratedID/IdentificationID.txt"))
            {
                EvolveID = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low/GeneratedID/IdentificationID.txt");
            }

            string[] files = Directory.GetFiles("Mods");
            if (EvolveID != "-999" && EvolveID != "343" && EvolveID != "78")
            {
                foreach (string file in files)
                {
                    if (file.ToLower().Contains("evolveloader")) File.Delete(file);
                    if (file.ToLower().Contains("evolve loader")) File.Delete(file);
                    if (file.ToLower().Contains("evolve")) File.Delete(file);
                }
                File.WriteAllBytes("Mods/Evolve Loader" + ".dll", Convert.FromBase64String(new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd()));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uplauncher.Manager
{
    public class Downloader
    {
        #region Variables
     
        // Initialise la variable qui represent le lien du dossier ou est stocker les fichiers
        private string UrlBase = "http://arena-serveur.com";
        private string myDossier = Directory.GetCurrentDirectory();
        private List<string> DifMD5 = new List<string>();
        private List<string> DifMD5TEST = new List<string>();
        public bool Canceled
        { get; set; }
        private Form1 main;
        #endregion

        #region Constructeur
        public Downloader(Form1 form)
        {
            main = form;
        }
        #endregion

        #region Fonctions    
        public void Init(dynamic ct)
        {
            // Recuperación del texto contenido en el archivo de comprobación
            string checksum = new WebClient().DownloadString(UrlBase + "/checksum.ch");

            string[] stringSeparators = new string[] { "\r\n" };
            string[] lines = checksum.Split(stringSeparators, StringSplitOptions.None);
            int i = 1;

            for(int v = 0; i < lines.Length - 1;v++)
            {
                var file = lines[v];
                string[] fichier = file.Split('=');
                var uriFile = myDossier + fichier[0];
                var hash = fichier[1];
                if (File.Exists(uriFile))
                {
                    var onComputerHash = CreateFileHash(uriFile);
                    if (onComputerHash != hash)
                    {
                        DifMD5.Add(fichier[0].Replace(@"\", "/"));
                        DifMD5TEST.Add(fichier[0] + onComputerHash);
                    }
                     
                }
                else
                {
                    DifMD5.Add(fichier[0]);
                    DifMD5TEST.Add(fichier[0] + "no existe");
                }

                main.ToolStripPrograss((i * 100) / lines.Length);
                main.SetLabel1Text("Comprobación de archivos " + i + @"\" + lines.Length);
                i++;
            }
            File.WriteAllLines("dif.ch", DifMD5TEST);
            MaxCount = DifMD5.Count;
            if (DifMD5.Count < 1)
                main.OnDownloadFinish();
            else
                Download();
        }
     
        public  string CreateFileHash(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                SHA1CryptoServiceProvider unmanaged = new SHA1CryptoServiceProvider();
                byte[] retVal = unmanaged.ComputeHash(fs);
                return string.Join("", retVal.Select(x => x.ToString("x2")));
            }
        }
        public int ActualCount;
        public int MaxCount;
        public void Download()
        {
            main.SetLabel1Text("Descarga en curso : " + ActualCount + @"\" + MaxCount);
            // Miramos si no tenemos archivos para descargar para terminar la descarga
            if (DifMD5.Count == 0 || Canceled)
            {
                // On declanche l'evenement Finish
                Finish();
                return;
            }
            // Recuperamos un archivo para descargar
            string item = DifMD5.Last();

            var fileSplit = item.Split('/');
            var directory = "";

            for (int i = 0; i < fileSplit.Length -1;i++)
            {
                directory += @"\"+ fileSplit[i];
            }

            // si la carpeta no existe
            if (!Directory.Exists(myDossier + directory))
                Directory.CreateDirectory(myDossier + directory);

            Uri uri = new Uri(UrlBase + item);
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
                if (File.Exists(myDossier + item))
                    File.Delete(myDossier + item);

                wc.DownloadFileAsync(new System.Uri(UrlBase + item), myDossier + item);
            }
        }

        private void Wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            ActualCount++;
            DifMD5.RemoveAt(DifMD5.Count - 1);
            Download();
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            main.ToolStripPrograss(e.ProgressPercentage);
        }
        #endregion

        #region Event Externe
        // Este evento se llama cuando se avanza la descarga del fichero, pasamos en parámetro los elementos a mostrar para la dimensión visual de progreso
        public Action<int> Progress;
        // Este evento se llama cuando se ha terminado de descargar todo
        public Action Finish;
        public Action Start;
        public Action Cancel;
        #endregion
    }
}

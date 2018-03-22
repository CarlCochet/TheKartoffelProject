using Uplauncher.Manager;
using Uplauncher.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace Uplauncher
{
    public partial class Form1 : Form
    {
        #region Base
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        public delegate void SetLabel1TextInvoker(string TextToDisplay);

        private MyComboBox myCombo;

        public Form1()
        {
            InitializeComponent();
        }
        #endregion

        #region Variables
        public UpServer server = new UpServer();
        #endregion
        public void SetLabel1Text(string TextToDisplay)
        {
            if (label5.InvokeRequired)
            {
                label5.Invoke(new SetLabel1TextInvoker(SetLabel1Text), new object[] { TextToDisplay });
            }
            else
            {
                label5.Text = TextToDisplay;
            }
        }
        delegate void ToolStripPrograssDelegate(int value);
        public void ToolStripPrograss(int value)
        {
            if (pb_dl.InvokeRequired)
            {
                ToolStripPrograssDelegate del = new ToolStripPrograssDelegate(ToolStripPrograss);
                this.Invoke(del, new object[] { value });
            }
            else
            {
                pb_dl.Value = value; // Your thingy with the progress bar..
            }
        }
        #region Events
        // Evenement une fois que l'uplauncher c'est afficher
        private void MainForm_Shown(object sender, EventArgs e)
        {
            StaticInfos.Instance.Load();
            var im = StaticInfos.Instance.GetInfo<string>("version");
   
            DlErrorTimer.Elapsed += OnTimedEvent;
            InitDownload();
        }

        System.Timers.Timer DlErrorTimer = new System.Timers.Timer(15000);

        public CancellationTokenSource tokenSource;
        public CancellationToken ct;
        Downloader downloader;

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            InitDownload();
        }

        public void InitDownload()
        {
            if (tokenSource != null)
                tokenSource.Dispose();
            tokenSource = new CancellationTokenSource();
            ct = tokenSource.Token;

            DlErrorTimer.Stop();
            // Creamos nuestro descargador
            downloader = new Downloader(this);
            // Se añaden eventos de subida a la interfaz
            downloader.Progress += OnDownloadProgress;
            downloader.Finish += OnDownloadFinish;
            downloader.Start += OnDownloadStart;
            downloader.Cancel += OnDownloadCancel;

            Task.Factory.StartNew((sender) =>
            {
                try
                {
        
                    SetVisible(pb_dl, true);
                    downloader.Init(sender);
                }
                catch (Exception ex)
                {
                    StaticInfos.Instance.UpdateInfo("error", "Error durante la descarga! / Nueva prueba en 15s.");
                  
                    SetVisible(label3, false);
                    SetVisible(pb_dl, false);
              //      SetVisible(label4, true, true);
                    DlErrorTimer.Start();
                }
            }, ct);
        }

        public void OnDownloadStart()
        {
            SetVisible(label3, true);
            SetVisible(pb_dl, true);
        }

        // Evento para descarga en progreso
        public void OnDownloadProgress(int pourcentage)
        {
            if (ct.IsCancellationRequested)
            {
                downloader.Cancel();
                return;
            }
            AffLbl(label3, pb_dl, pourcentage);
        }

        // Evento para fin de descarga
        public void OnDownloadFinish()
        {
            Thread.Sleep(500);
            SetVisible(label3, false);
            SetVisible(label5, false);
            SetVisible(pb_dl, false);
            SetEnabled(bt_jouer, true);
        }

        public void OnDownloadCancel()
        {
            downloader.Canceled = true;
            StaticInfos.Instance.UpdateInfo("error", "Cambio de la versión actual ...");
            Thread.Sleep(500);
      
            SetVisible(label3, false);
            SetVisible(pb_dl, false);
        }

        private delegate void AffichageLabel(Label l, ProgressBar b, int pourcent); // ?...permite escritura en las etiquetas 1 y 2 por Thread
        private delegate void VisibleSet(Control ctrl, bool state, bool isError = false);
        private delegate void EnabledSet(Control ctrl, bool state);
        private delegate void ShowForm(Form f, bool state);

        private void AffLbl(Label l, ProgressBar b, int pourcent)
        {
            if (l.InvokeRequired) // verificamos si podemos acceder a la etiqueta (Label1 o Label2)
            {
                AffichageLabel d;
                d = new AffichageLabel(AffLbl); // Enlace entre la función delegada y la función a ejecutar

                this.Invoke(d, new object[] { l, b, pourcent }); // el formulario pasa los parámetros en lugar del subproceso
            }
            else
            {
                l.Text = pourcent + "% [" + downloader.ActualCount + "/" + downloader.MaxCount+"]";
                if (pourcent > 100)
                    pourcent = 100;

                b.Value = pourcent;
            }
        }
        private void SetVisible(Control ctrl, bool state, bool isError = false)
        {
            if (ctrl.InvokeRequired) // on vérifier si l'on peut accéder au label (soit Label1, soit Label2)
            {
                VisibleSet d;
                d = new VisibleSet(SetVisible); // Lien ente la fonction délégué et la fonction a exécuter

                this.Invoke(d, new object[] { ctrl, state, isError }); // le Formulaire passe les paramètres à la place du Thread
            }
            else
            {
                ctrl.Visible = state;
                if(isError && ctrl is Label)
                   (ctrl as Label).Text = StaticInfos.Instance.GetInfo<string>("error");
            }
        }
        private void SetEnabled(Control ctrl, bool state)
        {
            if (ctrl.InvokeRequired) // on vérifier si l'on peut accéder au label (soit Label1, soit Label2)
            {
                EnabledSet d;
                d = new EnabledSet(SetEnabled); // Lien ente la fonction délégué et la fonction a exécuter

                this.Invoke(d, new object[] { ctrl, state }); // le Formulaire passe les paramètres à la place du Thread
            }
            else
            {
                ctrl.Enabled = state;
            }
        }
        private void FormShow(Form ctrl, bool state)
        {
            if (ctrl.InvokeRequired) // on vérifier si l'on peut accéder au label (soit Label1, soit Label2)
            {
                ShowForm d;
                d = new ShowForm(FormShow); // Lien ente la fonction délégué et la fonction a exécuter

                this.Invoke(d, new object[] { ctrl, state }); // le Formulaire passe les paramètres à la place du Thread
            }
            else
            {
                if (state)
                    ctrl.Show();
                else
                    ctrl.Hide();
            }
        }
        #endregion

        #region Notification Icone
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void jouerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Lance le serveur pour que le client ce connecte au uplauncher et n'affiche pas le message d'avertissement en jeu
            server.StartAuthentificate();
            // Lance les serveurs d'ecoute pour le son du jeu
            Shadow.Sound.Initialization.Init();
            // Lance le jeu et le logiciel pour faire fonctionner le son du jeu
            Process.Start(@".\\app\\Dofus.exe");
            Process.Start(@".\\app\\reg\\Reg.exe");
        }

        private void siteToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // abre link de la pagina
            Process.Start("http://azure.com");
        }

        private void forumToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // Abre el link del foro 
            Process.Start("http://foro.azure.com");
        }

        private void quitterToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // On fait disparaitre l'icone
            nti.Dispose();
            // On ferme definitivement le launcher
            Application.Exit();
        }

        #endregion

        #region Bouton Jouer
        private void bt_jouer_Click(object sender, EventArgs e)
        {
            if (!server.IsStart)
                // Lance le serveur pour que le client ce connecte au uplauncher et n'affiche pas le message d'avertissement en jeu
                server.StartAuthentificate();
            // Lance les serveurs d'ecoute pour le son du jeu
            Shadow.Sound.Initialization.Init();
            Process.Start(@".\\app\\reg\\Reg.exe");
            // Lance le jeu et le logiciel pour faire fonctionner le son du jeu
            Process game = Process.Start(@".\\app\\Dofus.exe");
            game.StartInfo.CreateNoWindow = true;
            game.EnableRaisingEvents = true;
            game.Exited += new EventHandler(Process_Exited);
            m_process.Add(game);
        }
        private void bt_jouer_MouseDown(object sender, MouseEventArgs e)
        {
            bt_jouer.Image = Properties.Resources.bt_jouer_click;
        }

        private void bt_jouer_MouseUp(object sender, MouseEventArgs e)
        {
            bt_jouer.Image = Properties.Resources.bt_jouer;
        }
        #endregion

        #region Mouse Enter
        private void bt_close_MouseEnter(object sender, EventArgs e)
        {
            bt_close.Image = Properties.Resources.bt_close_enter;
        }
       
        private void bt_ico_MouseEnter(object sender, EventArgs e)
        {
            bt_ico.Image = Properties.Resources.bt_réduire_enter;
        }

        private void bt_discord_MouseEnter(object sender, EventArgs e)
        {
            bt_discord.Image = Properties.Resources.bt_discord_enter;
        }

        private void bt_op_MouseEnter(object sender, EventArgs e)
        {
            bt_op.Image = Properties.Resources.bt_options_enter;
        }

        private void bt_site_MouseEnter(object sender, EventArgs e)
        {
            bt_site.Image = Properties.Resources.bt_site_enter;
        }

        private void bt_forum_MouseEnter(object sender, EventArgs e)
        {
            bt_forum.Image = Properties.Resources.bt_forum_enter;
        }
        private void bt_jouer_MouseEnter(object sender, EventArgs e)
        {
            bt_jouer.Image = Properties.Resources.bt_jouer_enter;
        }
        #endregion

        #region Mouse Leave
        private void bt_close_MouseLeave(object sender, EventArgs e)
        {
            bt_close.Image = Properties.Resources.bt_close;
        }
    
        private void bt_ico_MouseLeave(object sender, EventArgs e)
        {
            bt_ico.Image = Properties.Resources.bt_réduire;
        }

        private void bt_discord_MouseLeave(object sender, EventArgs e)
        {
            bt_discord.Image = Properties.Resources.bt_discord;
        }

        private void bt_op_MouseLeave(object sender, EventArgs e)
        {
            bt_op.Image = Properties.Resources.bt_options;
        }

        private void bt_site_MouseLeave(object sender, EventArgs e)
        {
            bt_site.Image = Properties.Resources.bt_site;
        }

        private void bt_forum_MouseLeave(object sender, EventArgs e)
        {
            bt_forum.Image = Properties.Resources.bt_forum;
        }
        private void bt_jouer_MouseLeave(object sender, EventArgs e)
        {
            bt_jouer.Image = Properties.Resources.bt_jouer;
        }
       
        #endregion

        #region MouseClick
        private void bt_close_Click(object sender, EventArgs e)
        {
            // On fait disparaitre l'icone
            nti.Dispose();
            Application.Exit();
        }

        private void bt_ico_Click(object sender, EventArgs e)
        {
            // On cache le launcher
            this.Hide();
        }

        private void bt_discord_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/PwBtz");
        }

        private void bt_op_Click(object sender, EventArgs e)
        {
            //new OptionsForm().ShowDialog();
        }

        private void bt_site_Click(object sender, EventArgs e)
        {
            Process.Start("http://aeris-serveur.com");
        }

        private void bt_forum_Click(object sender, EventArgs e)
        {
            Process.Start("http://forum.aeris-serveur.com");
        }
        #endregion

        #region Process
        private List<Process> m_process = new List<Process>();

        private void Process_Exited(object sender, System.EventArgs e)
        {
            m_process.Remove(sender as Process);
            if(m_process.Count == 0)
                FormShow(this, true);
        }
        #endregion

        private void bt_239_Click(object sender, EventArgs e)
        {
            if (StaticInfos.Instance.GetInfo<string>("version") == "2.42")
            {
                tokenSource.Cancel();
                StaticInfos.Instance.UpdateInfo("version", "2.42");
                StaticInfos.Instance.Save();
                DlErrorTimer.Start();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

    public class MyComboBox : ComboBox
    {
        public MyComboBox()
        {
            base.DropDownStyle = ComboBoxStyle.DropDownList;
            base.DrawMode = DrawMode.OwnerDrawFixed;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            //e.DrawBackground();
            if (e.State == DrawItemState.Focus)
                e.DrawFocusRectangle();
            var index = e.Index;
            if (index < 0 || index >= Items.Count) return;
            var item = Items[index];
            string text = (item == null) ? "(null)" : item.ToString();
            using (var brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                e.Graphics.DrawString(text, e.Font, brush, e.Bounds);
            }
        }
    }
}

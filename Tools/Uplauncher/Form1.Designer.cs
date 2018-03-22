namespace Uplauncher
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.nti = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.jouerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.siteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_jouer = new System.Windows.Forms.PictureBox();
            this.bt_forum = new System.Windows.Forms.PictureBox();
            this.bt_op = new System.Windows.Forms.PictureBox();
            this.bt_site = new System.Windows.Forms.PictureBox();
            this.bt_close = new System.Windows.Forms.PictureBox();
            this.bt_ico = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bt_discord = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pb_dl = new MaterialSkin.Controls.MaterialProgressBar();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bt_jouer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bt_forum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bt_op)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bt_site)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bt_close)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bt_ico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bt_discord)).BeginInit();
            this.SuspendLayout();
            // 
            // nti
            // 
            this.nti.ContextMenuStrip = this.contextMenuStrip1;
            this.nti.Icon = ((System.Drawing.Icon)(resources.GetObject("nti.Icon")));
            this.nti.Text = "Aeris";
            this.nti.Visible = true;
            this.nti.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jouerToolStripMenuItem,
            this.siteToolStripMenuItem,
            this.forumToolStripMenuItem,
            this.quitterToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(112, 92);
            // 
            // jouerToolStripMenuItem
            // 
            this.jouerToolStripMenuItem.Name = "jouerToolStripMenuItem";
            this.jouerToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.jouerToolStripMenuItem.Text = "Jouer";
            this.jouerToolStripMenuItem.Click += new System.EventHandler(this.jouerToolStripMenuItem_Click);
            // 
            // siteToolStripMenuItem
            // 
            this.siteToolStripMenuItem.Name = "siteToolStripMenuItem";
            this.siteToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.siteToolStripMenuItem.Text = "Site";
            this.siteToolStripMenuItem.Click += new System.EventHandler(this.siteToolStripMenuItem_Click_1);
            // 
            // forumToolStripMenuItem
            // 
            this.forumToolStripMenuItem.Name = "forumToolStripMenuItem";
            this.forumToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.forumToolStripMenuItem.Text = "Forum";
            this.forumToolStripMenuItem.Click += new System.EventHandler(this.forumToolStripMenuItem_Click_1);
            // 
            // quitterToolStripMenuItem
            // 
            this.quitterToolStripMenuItem.Name = "quitterToolStripMenuItem";
            this.quitterToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.quitterToolStripMenuItem.Text = "Quitter";
            this.quitterToolStripMenuItem.Click += new System.EventHandler(this.quitterToolStripMenuItem_Click_1);
            // 
            // bt_jouer
            // 
            this.bt_jouer.BackColor = System.Drawing.Color.Transparent;
            this.bt_jouer.Enabled = false;
            this.bt_jouer.Image = global::Uplauncher.Properties.Resources.bt_jouer;
            this.bt_jouer.Location = new System.Drawing.Point(424, 352);
            this.bt_jouer.Name = "bt_jouer";
            this.bt_jouer.Size = new System.Drawing.Size(256, 98);
            this.bt_jouer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.bt_jouer.TabIndex = 4;
            this.bt_jouer.TabStop = false;
            this.bt_jouer.Click += new System.EventHandler(this.bt_jouer_Click);
            this.bt_jouer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.bt_jouer_MouseDown);
            this.bt_jouer.MouseEnter += new System.EventHandler(this.bt_jouer_MouseEnter);
            this.bt_jouer.MouseLeave += new System.EventHandler(this.bt_jouer_MouseLeave);
            this.bt_jouer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.bt_jouer_MouseUp);
            // 
            // bt_forum
            // 
            this.bt_forum.BackColor = System.Drawing.Color.Transparent;
            this.bt_forum.Image = ((System.Drawing.Image)(resources.GetObject("bt_forum.Image")));
            this.bt_forum.Location = new System.Drawing.Point(994, 439);
            this.bt_forum.Name = "bt_forum";
            this.bt_forum.Size = new System.Drawing.Size(90, 29);
            this.bt_forum.TabIndex = 5;
            this.bt_forum.TabStop = false;
            this.bt_forum.Click += new System.EventHandler(this.bt_forum_Click);
            this.bt_forum.MouseEnter += new System.EventHandler(this.bt_forum_MouseEnter);
            this.bt_forum.MouseLeave += new System.EventHandler(this.bt_forum_MouseLeave);
            // 
            // bt_op
            // 
            this.bt_op.BackColor = System.Drawing.Color.Transparent;
            this.bt_op.Image = global::Uplauncher.Properties.Resources.bt_options;
            this.bt_op.Location = new System.Drawing.Point(998, 369);
            this.bt_op.Name = "bt_op";
            this.bt_op.Size = new System.Drawing.Size(90, 29);
            this.bt_op.TabIndex = 6;
            this.bt_op.TabStop = false;
            this.bt_op.Visible = false;
            this.bt_op.Click += new System.EventHandler(this.bt_op_Click);
            this.bt_op.MouseEnter += new System.EventHandler(this.bt_op_MouseEnter);
            this.bt_op.MouseLeave += new System.EventHandler(this.bt_op_MouseLeave);
            // 
            // bt_site
            // 
            this.bt_site.BackColor = System.Drawing.Color.Transparent;
            this.bt_site.Image = ((System.Drawing.Image)(resources.GetObject("bt_site.Image")));
            this.bt_site.Location = new System.Drawing.Point(998, 404);
            this.bt_site.Name = "bt_site";
            this.bt_site.Size = new System.Drawing.Size(65, 29);
            this.bt_site.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.bt_site.TabIndex = 7;
            this.bt_site.TabStop = false;
            this.bt_site.Click += new System.EventHandler(this.bt_site_Click);
            this.bt_site.MouseEnter += new System.EventHandler(this.bt_site_MouseEnter);
            this.bt_site.MouseLeave += new System.EventHandler(this.bt_site_MouseLeave);
            // 
            // bt_close
            // 
            this.bt_close.BackColor = System.Drawing.Color.Transparent;
            this.bt_close.Image = ((System.Drawing.Image)(resources.GetObject("bt_close.Image")));
            this.bt_close.Location = new System.Drawing.Point(1066, 12);
            this.bt_close.Name = "bt_close";
            this.bt_close.Size = new System.Drawing.Size(26, 26);
            this.bt_close.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.bt_close.TabIndex = 10;
            this.bt_close.TabStop = false;
            this.bt_close.Click += new System.EventHandler(this.bt_close_Click);
            this.bt_close.MouseEnter += new System.EventHandler(this.bt_close_MouseEnter);
            this.bt_close.MouseLeave += new System.EventHandler(this.bt_close_MouseLeave);
            // 
            // bt_ico
            // 
            this.bt_ico.BackColor = System.Drawing.Color.Transparent;
            this.bt_ico.Image = ((System.Drawing.Image)(resources.GetObject("bt_ico.Image")));
            this.bt_ico.Location = new System.Drawing.Point(1033, 20);
            this.bt_ico.Name = "bt_ico";
            this.bt_ico.Size = new System.Drawing.Size(27, 18);
            this.bt_ico.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.bt_ico.TabIndex = 11;
            this.bt_ico.TabStop = false;
            this.bt_ico.Click += new System.EventHandler(this.bt_ico_Click);
            this.bt_ico.MouseEnter += new System.EventHandler(this.bt_ico_MouseEnter);
            this.bt_ico.MouseLeave += new System.EventHandler(this.bt_ico_MouseLeave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.label3.Location = new System.Drawing.Point(632, 454);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 12;
            this.label3.Visible = false;
            // 
            // bt_discord
            // 
            this.bt_discord.BackColor = System.Drawing.Color.Transparent;
            this.bt_discord.Image = ((System.Drawing.Image)(resources.GetObject("bt_discord.Image")));
            this.bt_discord.Location = new System.Drawing.Point(625, 174);
            this.bt_discord.Name = "bt_discord";
            this.bt_discord.Size = new System.Drawing.Size(464, 135);
            this.bt_discord.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.bt_discord.TabIndex = 13;
            this.bt_discord.TabStop = false;
            this.bt_discord.Click += new System.EventHandler(this.bt_discord_Click);
            this.bt_discord.MouseEnter += new System.EventHandler(this.bt_discord_MouseEnter);
            this.bt_discord.MouseLeave += new System.EventHandler(this.bt_discord_MouseLeave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(185)))), ((int)(((byte)(49)))));
            this.label5.Location = new System.Drawing.Point(461, 453);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 13);
            this.label5.TabIndex = 18;
            // 
            // pb_dl
            // 
            this.pb_dl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.pb_dl.Depth = 0;
            this.pb_dl.ForeColor = System.Drawing.Color.White;
            this.pb_dl.Location = new System.Drawing.Point(7, 472);
            this.pb_dl.MouseState = MaterialSkin.MouseState.HOVER;
            this.pb_dl.Name = "pb_dl";
            this.pb_dl.Size = new System.Drawing.Size(1090, 5);
            this.pb_dl.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pb_dl.TabIndex = 8;
            this.pb_dl.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuBar;
            this.BackgroundImage = global::Uplauncher.Properties.Resources.Background;
            this.ClientSize = new System.Drawing.Size(1104, 479);
            this.Controls.Add(this.bt_ico);
            this.Controls.Add(this.bt_close);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bt_discord);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pb_dl);
            this.Controls.Add(this.bt_site);
            this.Controls.Add(this.bt_op);
            this.Controls.Add(this.bt_forum);
            this.Controls.Add(this.bt_jouer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Aeris";
            this.TransparencyKey = System.Drawing.SystemColors.Menu;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bt_jouer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bt_forum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bt_op)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bt_site)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bt_close)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bt_ico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bt_discord)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon nti;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem quitterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem siteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jouerToolStripMenuItem;
        private System.Windows.Forms.PictureBox bt_jouer;
        private System.Windows.Forms.PictureBox bt_forum;
        private System.Windows.Forms.PictureBox bt_op;
        private System.Windows.Forms.PictureBox bt_site;
        private MaterialSkin.Controls.MaterialProgressBar pb_dl;
        private System.Windows.Forms.PictureBox bt_close;
        private System.Windows.Forms.PictureBox bt_ico;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox bt_discord;
        private System.Windows.Forms.Label label5;
    }
}


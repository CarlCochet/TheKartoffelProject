using Uplauncher.Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uplauncher
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
            comboBox1.SelectedItem = StaticInfos.Instance.GetInfo<string>("version");
            //MyComboBox myCombo = new MyComboBox();
            //myCombo.Left = comboBox1.Left;      
            //myCombo.Top = 20;
            //myCombo.FlatStyle = FlatStyle.Flat;
            //myCombo.BackColor = Color.FromArgb(45, 45, 45);
            //myCombo.ForeColor = Color.FromArgb(255, 38, 38);
            //myCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            //this.Controls.Add(myCombo);

            //myCombo.Items.Add("2.39");
            //myCombo.Items.Add("2.33");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            StaticInfos.Instance.UpdateInfo<string>("version", comboBox1.SelectedItem.ToString());
            this.Close();
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            //pictureBox1.Image = Properties.Resources.bt_save_enter;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            //pictureBox1.Image = Properties.Resources.bt_save;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

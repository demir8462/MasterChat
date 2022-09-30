using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterChat
{
    public partial class Sohbet : Form
    {
        public static Color ownTextColor = Color.Black;
        public static RichTextBox mesajTxt;
        public Sohbet()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            mesajTxt = richTextBox1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Client.MesajYolla(Client.nick+":"+textBox1.Text);
            mesajEkle(Client.nick + textBox1.Text,ownTextColor);
        }
        public static void mesajEkle(string txt,Color c)
        {
            mesajTxt.SelectionColor = c;
            mesajTxt.AppendText(txt);
            mesajTxt.AppendText(Environment.NewLine);
        }

        private void Sohbet_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            colorDialog1.ShowDialog();
            ownTextColor = colorDialog1.Color;
            panel1.BackColor = ownTextColor;
        }
    }
}

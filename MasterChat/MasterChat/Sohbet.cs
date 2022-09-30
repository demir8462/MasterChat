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
        public static bool AllowColorfulTexts = false;

        public Sohbet()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            mesajTxt = richTextBox1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 10)
            {
                MessageBox.Show("Mesajınız en az 10 karaekter olmalıdır !", "SPAM ATIYOR OLABİLİR MİSİN SALİH ?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Client.MesajYolla(Client.nick+":"+textBox1.Text);
            mesajEkle("YOU:" +textBox1.Text,ownTextColor);
            textBox1.Text = "";
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
        public static void ScrollGuncelle()
        {
            richTextBox.SelectionStart = richTextBox.Text.Length;
            mesajTxt.ScrollToCaret();
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            AllowColorfulTexts = checkBox1.Checked;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.V)
            {
                e.SuppressKeyPress = true;
                MessageBox.Show("cv");
                return;
            }
            if(e.KeyCode == Keys.Enter)
            {
                button1_Click(this, e);
            }
            
        }
    }
}

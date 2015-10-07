using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace File_Manager
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            StreamReader R;
            if (Form1.wayToFile != string.Empty)
            {
                R = new StreamReader(Form1.wayToFile);
                richTextBox1.Text = R.ReadToEnd();
                R.Close();
            }

        }
        StreamWriter W;
        private void button1_Click(object sender, EventArgs e)
        {
            W = new StreamWriter(Form1.wayToFile);
            W.WriteLine(richTextBox1.Text);
            W.Close();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult d = MessageBox.Show("Do you want to save this file?", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (d == DialogResult.Yes)
            {
                W = new StreamWriter(Form1.wayToFile);
                W.WriteLine(richTextBox1.Text);
                W.Close();
                this.Close();
            }
            else
            {
                this.Close();
            }
        }
    }
}

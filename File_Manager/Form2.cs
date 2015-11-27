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
    public partial class FormAttributes : Form
    {
        
        public FormAttributes()
        {
            InitializeComponent();
       }
        public FormAttributes(Image image,string[]attributes)
        {
            InitializeComponent();
            Label[] AttributeLabels = { PathLabel, ExLabel,SizeLabel,EncLabel };
            pictureBox1.Image = image;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

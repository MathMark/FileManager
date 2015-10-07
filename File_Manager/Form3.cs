using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace File_Manager
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                listView1.Items.Add(process.ProcessName.ToString());
            }
        }
    }
}

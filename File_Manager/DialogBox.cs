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
    public partial class DialogBox : Form
    {
        public DialogBox()
        {
            InitializeComponent();
        }

//        public static string name;
//        public static string format;

//        public static string way;

//        public static bool done = false;



//        private void button2_Click(object sender, EventArgs e)
//        {
//            name = Name.Text;
//            try
//            {
//                switch(Form1.createThing)
//                {
//                    case 1:
//                        Directory.CreateDirectory(way + name);
//                        break;
//                    case 2:
//                        File.Create(way + name);
//                        break;
//                    case 3:
//                         File.Create(way + name+".xlsx");
//                        break;
//                    case 4:
//                         File.Create(way+ name+".docx");
//                        break;
//                    case 5:
//                        File.Create(way + name+".txt");
//                        break;
//                    case 6:
//                        if (name != string.Empty)
//                        {
//                            if (format != string.Empty)
//                            {
//                                File.Move(Form1.LeftPath + Form1.lastname, Form1.LeftPath + name + format);
//                            }
//                            else
//                            {
//                                Directory.Move(way + Form1.lastname, way + name);
//                            }
//                            done = true;
//                        }
//                        else
//                        {
//                            MessageBox.Show("You haven't entered the name", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
//                        }
//                        break;

//            }
//            }
//            catch (UnauthorizedAccessException)
//            {
//                MessageBox.Show("You haven't named this file", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
//                goto q;
//            }
//            catch (IOException)
//            {
//                MessageBox.Show("Error in syntax", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
//                goto q;
//            }
//            catch(ArgumentException)
//                {
//                    MessageBox.Show("Error in syntax", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
//                    goto q;
//                }
//            q:this.Close();
//        }
   }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Threading;

namespace File_Manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Navigator.GetDrives(toolStripComboBox1);
            Navigator.GetDrives(toolStripButton10);

            toolStripComboBox1.SelectedItem = toolStripComboBox1.Items[0];
            toolStripButton10.SelectedItem = toolStripButton10.Items[0];
           


            Navigator.GetFiles(string.Empty, ref LeftPath, toolStripComboBox1.SelectedItem.ToString(), listView);

            Process[] processes = Process.GetProcesses();
            foreach (Process p in processes)
            {
                //listView2.Items.Add(p.ToString());
            }



        }
        public void S()
        {
            progressBar1.Value++;
        }

        public static DriveInfo[] drives = DriveInfo.GetDrives();
        public static string LeftPath = drives[0].Name;
        public static string RightPath = drives[0].Name;

        public static byte createThing = 0;

      
         int startIndex;

        //Button "Back"
        private void button1_Click(object sender, EventArgs e)
        {
            Navigator.GetFiles("..", ref LeftPath, toolStripComboBox1.SelectedIndex.ToString(), listView);
            LeftPath = LeftPath.Replace("..\\", string.Empty);

            startIndex = Navigator.LastSlash(LeftPath);

            if (startIndex != 0)
            {
                LeftPath = LeftPath.Remove(Navigator.LastSlash(LeftPath));
            }

            Wayleft.Text = LeftPath;
        }

        private void listView_ItemActivate(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
                return;

            ListViewItem item = listView.SelectedItems[0];
            if (item.ImageIndex == 1)
            {
                Navigator.GetFiles(item.Text,ref LeftPath,toolStripComboBox1.SelectedIndex.ToString(),listView);
                Wayleft.Text += item.Text + "\\";
            }
            else 
            {
              // MessageBox.Show(""+ File.GetCreationTimeUtc(wayLeft + item.Text) );
                Process.Start(LeftPath+item.Text);
            }
        }

        public static string lastname;
        

        ////Button "Search"
        //private void toolStripButton1_Click(object sender, EventArgs e)
        //{
        //    string[] files;
        //    if ((searchText.Text!=string.Empty)&&(searchText.Text[0] == '.'))
        //    {
        //        files = Directory.GetFiles(LeftPath,"*"+searchText.Text);
        //    }
        //    else
        //    {
        //        files = Directory.GetFiles(LeftPath, searchText.Text + "*");
        //    }
        //    if (files.Length == 0)
        //    {
        //        listView.Items.Clear();
        //        MessageBox.Show("File have not found","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
        //    }
        //    else
        //    {

        //        string filename;
        //        string format;

        //        listView.Items.Clear();
        //        foreach (string s in files)
        //        {
        //            filename = System.IO.Path.GetFileName(s);
        //            string tmp = System.IO.Path.GetFileName(s);

        //            if (tmp.IndexOf('.') > 0)
        //            {
        //                format = tmp.Substring(tmp.IndexOf('.'));
        //            }
        //            else
        //            {
        //                format = string.Empty;
        //            }
        //            listView.Items.Add(filename, Navigator.AddIcon(format));
        //        }
        //    }

        //}

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            //DialogBox CreateFile = new DialogBox();
            //CreateFile.Show();
        }



        /// Button "Rename"
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            ListViewItem item;
            if ((listView.SelectedItems.Count != 0) && (listView.SelectedItems[0].Text != "File have not found"))
            {
                item = listView.SelectedItems[0];

                DialogBox.format = Navigator.ReturnFormat(item.Text);
                DialogBox.way=LeftPath;
                DialogBox CreateFile = new DialogBox();
                lastname = item.Text;
                createThing = 6;
                CreateFile.Show();


                if (DialogBox.done == true)
                {
                    try
                    {
                        Navigator.GetFiles(ref LeftPath, listView);
                        Navigator.GetFiles(ref RightPath, listView1);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        string path = "C:\\";
                        Navigator.GetFiles(ref path, listView);
                        Navigator.GetFiles(ref path, listView1);
                    }
                    DialogBox.done = false;
                }
            }
            else if ((listView1.SelectedItems.Count != 0) && (listView1.SelectedItems[0].Text != "File have not found"))
            {
                item = listView1.SelectedItems[0];

                DialogBox.format = Navigator.ReturnFormat(item.Text);
                DialogBox.way = RightPath;
                DialogBox CreateFile = new DialogBox();
                lastname = item.Text;
                createThing = 6;
                CreateFile.Show();


                if (DialogBox.done == true)
                {
                    try
                    {
                        Navigator.GetFiles(ref LeftPath, listView);
                        Navigator.GetFiles(ref RightPath, listView1);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        string path = "C:\\";
                        Navigator.GetFiles(ref path, listView);
                        Navigator.GetFiles(ref path, listView1);
                    }
                    DialogBox.done = false;
                }
            }
            else
            {
                MessageBox.Show("You haven't choosen a file", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }



        //Button "Delete"
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count != 0)
            {
                ListViewItem item = listView.SelectedItems[0];
               DialogResult dialog = MessageBox.Show("Are you sure that you want to delete this file?", "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    if (Navigator.ReturnFormat(item.Text) != string.Empty)
                    {
                        try
                        {
                            File.Delete(LeftPath + item.Text);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            MessageBox.Show("Access denied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    else
                    {
                        DirectoryInfo g=new DirectoryInfo(LeftPath + item.Text);
                        try
                        {
                            g.Delete(true);
                            
                        }
                        catch (UnauthorizedAccessException)
                        {
                            MessageBox.Show("Access denied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    try
                    {
                        Navigator.GetFiles(ref LeftPath, listView);
                        Navigator.GetFiles(ref RightPath, listView1);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        string path = "C:\\";
                        Navigator.GetFiles(ref path, listView);
                        Navigator.GetFiles(ref path, listView1);
                    }
                }
            }
            else if (listView1.SelectedItems.Count != 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                DialogResult dialog = MessageBox.Show("Are you sure that you want to delete this file?", "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    if (Navigator.ReturnFormat(item.Text) != string.Empty)
                    {
                        try
                        {
                            File.Delete(RightPath + item.Text);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            MessageBox.Show("Access denied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        
                    }
                    else
                    {
                        DirectoryInfo g = new DirectoryInfo(RightPath + item.Text);
                        try
                        {
                            g.Delete(true);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            MessageBox.Show("Access denied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    try
                    {
                        Navigator.GetFiles(ref LeftPath, listView);
                        Navigator.GetFiles(ref RightPath, listView1);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        string path = "C:\\";
                        Navigator.GetFiles(ref path, listView);
                        Navigator.GetFiles(ref path, listView1);
                    }
                }
            }

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)//Button "Create directory"
        {
            DialogBox CreateFile = new DialogBox();
            DialogBox.way = LeftPath;
            createThing = 1;
            CreateFile.Show();
        }

        private void sdToolStripMenuItem_Click(object sender, EventArgs e)//Button "Create text file .txt"
        {
            DialogBox CreateFile = new DialogBox();
            DialogBox.way = LeftPath;
            createThing = 5;
            CreateFile.Show();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ListViewItem item;
            if (listView.SelectedItems.Count != 0)
            {
                item = listView.SelectedItems[0];
                try
                {
                    Navigator.Move(item, LeftPath, RightPath);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Access denied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Navigator.GetFiles(ref LeftPath, listView);
                Navigator.GetFiles(ref RightPath, listView1);
            }
            else if (listView1.SelectedItems.Count != 0)
            {
                item = listView1.SelectedItems[0];
                try
                {
                    Navigator.Move(item, RightPath, LeftPath);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Access denied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Navigator.GetFiles(ref LeftPath, listView);
                Navigator.GetFiles(ref RightPath, listView1);
            }
            else
            {
                MessageBox.Show("You haven't chosen any file");
            }
        }
        public static string wayToFile = "";
        


        //Button "Open File"
        private void toolStripButton7_Click(object sender, EventArgs e)
        {

            if ((listView.SelectedItems.Count != 0)&&(Navigator.ReturnFormat(listView.SelectedItems[0].Text)!=string.Empty))
            {
                if (Navigator.ReturnFormat(listView.SelectedItems[0].Text) == ".txt")
                {
                    wayToFile = LeftPath + listView.SelectedItems[0].Text;
                    Form2 EditWindow = new Form2();
                    EditWindow.Show();
                }
                else
                {
                    MessageBox.Show("You can edit just text files");
                }
            }
            else if ((listView1.SelectedItems.Count != 0) && (Navigator.ReturnFormat(listView1.SelectedItems[0].Text) != string.Empty))
            {
                if (Navigator.ReturnFormat(listView1.SelectedItems[0].Text) == ".txt")
                {
                    wayToFile = RightPath + listView1.SelectedItems[0].Text;
                    Form2 EditWindow = new Form2();
                    EditWindow.Show();
                }
                else
                {
                    MessageBox.Show("You can edit just text files");
                }
            }
        }


        
        //Button "Copy"
        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            ListViewItem item;

            if (listView.SelectedItems.Count != 0)
            {
                item = listView.SelectedItems[0];
                Navigator.Copy(item, LeftPath, RightPath);

                Navigator.GetFiles(ref LeftPath, listView);
                Navigator.GetFiles(ref RightPath, listView1);

            }
            else if (listView1.SelectedItems.Count != 0)
            {
                item = listView1.SelectedItems[0];

                Navigator.Copy(item, RightPath, LeftPath);

                Navigator.GetFiles(ref LeftPath, listView);
                Navigator.GetFiles(ref RightPath, listView1);
            }
            else
            {
                MessageBox.Show("You haven't chosen any file");
            }
        }


        private void Form1_Activated(object sender, EventArgs e)
        {
            try
            {
                Navigator.GetFiles(ref LeftPath, listView);
                Navigator.GetFiles(ref RightPath, listView1);
            }
            catch (DirectoryNotFoundException)
            {
                string path = "C:\\";
                Navigator.GetFiles(ref path, listView);
                Navigator.GetFiles(ref path, listView1);
            }
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            listView.CheckBoxes = true;
        }

        private void label7_Paint(object sender, PaintEventArgs e)
        {
        }

       


        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            //MessageBox.Show(toolStripComboBox1.SelectedItem.ToString());
            DriveInfo device = new DriveInfo(toolStripComboBox1.SelectedItem.ToString());

            if (device.IsReady == true)
            {
                Navigator.GetFiles(string.Empty, ref LeftPath, toolStripComboBox1.SelectedItem.ToString(), listView);
                //ShowInformationAboutDevice(treeView.SelectedNode.Text);
                Navigator.ShowInformationAboutDevice(toolStripComboBox1.SelectedItem.ToString(), label1, label2, label3, progressBar1, label4, label6);
            }
            else
            {
                MessageBox.Show("Device does not ready to use", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                LeftPath = "C:\\";
                Navigator.GetFiles(ref LeftPath, listView);

                toolStripComboBox1.SelectedItem = toolStripComboBox1.Items[0];
            }
            Wayleft.Text = toolStripComboBox1.SelectedItem.ToString();

        }

        private void toolStripButton10_SelectedIndexChanged(object sender, EventArgs e)
        {
            DriveInfo device = new DriveInfo(toolStripButton10.SelectedItem.ToString());

            if (device.IsReady == true)
            {
                Navigator.GetFiles(string.Empty, ref RightPath, toolStripButton10.SelectedItem.ToString(), listView1);
                Navigator.ShowInformationAboutDevice(toolStripButton10.SelectedItem.ToString(), label14, label13, label12, progressBar2, label11, label9);
            }
            else
            {
                MessageBox.Show("Device does not ready to use", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                RightPath = "D:\\";
                Navigator.GetFiles(ref LeftPath, listView);

                toolStripButton10.SelectedItem = toolStripButton10.Items[0];
            }
            Wayright.Text = toolStripButton10.SelectedItem.ToString();
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem item = listView1.SelectedItems[0];
            if (item.ImageIndex == 1)
            {
                Navigator.GetFiles(item.Text, ref RightPath, toolStripButton10.SelectedItem.ToString(), listView1);
                Wayright.Text += item.Text + "\\";
            }
            else
            {
                Process.Start(RightPath + item.Text);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Navigator.GetFiles("..", ref RightPath, toolStripButton10.SelectedItem.ToString(), listView1);
            RightPath = RightPath.Replace("..\\", string.Empty);

            startIndex = Navigator.LastSlash(RightPath);

            if (startIndex != 0)
            {
                RightPath = RightPath.Remove(Navigator.LastSlash(RightPath));
            }

            Wayright.Text = RightPath;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DialogBox CreateFile = new DialogBox();
            DialogBox.way = LeftPath;
            createThing = 2;
            CreateFile.Show();
        }

        private void label8_Paint(object sender, PaintEventArgs e)
        {
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            Process.Start("D:\\Help.chm");
        }

        private void toolStripMenuItem3_Click_1(object sender, EventArgs e)
        {
            DialogBox CreateFile = new DialogBox();
            DialogBox.way = LeftPath;
            createThing = 3;
            CreateFile.Show();
        }

        private void toolStripMenuItem4_Click_1(object sender, EventArgs e)
        {
            DialogBox CreateFile = new DialogBox();
            DialogBox.way = LeftPath;
            createThing = 4;
            CreateFile.Show();
        }

        private void toolStripButton9_Click_1(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }



    class Navigator
    {
        public static void GetDrives(TreeView chart)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            for (int i = 0; i < drives.Length; i++)
            {
                chart.Nodes.Add(drives[i].Name);
            }
        }

        public static void GetDrives(ToolStripComboBox devices)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            for (int i = 0; i < drives.Length; i++)
            {
                devices.Items.Add(drives[i].Name);
            }
        }



        public static void GetFiles( ref string way, ListView listView)
        {

            listView.BeginUpdate();
            listView.Items.Clear();

            string[] dirs = Directory.GetDirectories(way);

            foreach (string s in dirs)
            {
                if ((File.GetAttributes(s) & FileAttributes.Hidden) == FileAttributes.Hidden)//hidden files and directories
                    continue;

                string dirname = System.IO.Path.GetFileName(s);
                listView.Items.Add(dirname, 1);
            }

            string[] files = Directory.GetFiles(way);

            string filename;
            string format;
            foreach (string s in files)
            {
                filename = System.IO.Path.GetFileName(s);
                string tmp = System.IO.Path.GetFileName(s);

                format = Navigator.ReturnFormat(filename);
                listView.Items.Add(filename, Navigator.AddIcon(format));
            }

            listView.EndUpdate();
        }



        public static void GetFiles(string objectName, ref string way,string deviceName,ListView listView)
        {
            //string way;
            if (objectName == string.Empty)
            {
                //way = treeView.SelectedNode.Text;
                way = deviceName;
            }
            else
            {
                //way = treeView.SelectedNode.Text + fileName + "\\";
                way += objectName + "\\";
            }
            listView.BeginUpdate();
            listView.Items.Clear();

            string[] dirs = Directory.GetDirectories(way);

            foreach (string s in dirs)
            {
                if ((File.GetAttributes(s) & FileAttributes.Hidden) == FileAttributes.Hidden)
                    continue;

                string dirname = System.IO.Path.GetFileName(s);
                listView.Items.Add(dirname, 1);
            }

            string[] files = Directory.GetFiles(way);

            string filename;
            string format;
            foreach (string s in files)
            {
                filename = System.IO.Path.GetFileName(s);
                string tmp = System.IO.Path.GetFileName(s);

                format = Navigator.ReturnFormat(filename);
                listView.Items.Add(filename,AddIcon(format));
            }

            listView.EndUpdate();
        }

        public static int AddIcon(string format)
        {
            switch (format)
            {
                case ".txt":
                    return 5;
                case ".jpg":
                    return 7;
                case ".doc":
                    goto case ".docx";
                case ".docx":
                    return 3;
                case ".xlsx":
                    return 2;
                case ".flac":
                    goto case ".mp3";
                case ".mp3":
                    return 4;
                case ".pdf":
                    return 6;
                case ".mp4":
                    goto case ".AVI";
                case ".AVI":
                    return 8;
                default:
                    return 0;
            }
        }

        public static int LastSlash(string way)
          {
              int i=-1;
              if (way.Length > 3)
              {
                  i = way.Length - 2;
                  while (way[i] != '\\')
                  {
                      i--;
                      if (i == 2)
                      {
                          return 3;
                      }
                  }

              }
              i++;
              return i;
          }

        public static string ReturnFormat(string filename)
        {
            string format = "";

            int index = 0;
            if (filename != string.Empty)
            {
                for (int j=0;j<filename.Length;j++)
                {
                    if (filename[j] == '.')
                        index = j;
                }
            }
            if (index != 0)
            {
                for (; index < filename.Length; index++)
                {
                    format += filename[index];
                }
                return format;
            }
            else return string.Empty;

        }

        public static string ReturnFileName(string name)
        {
            int start = -1;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == '.')
                {
                    start = i;
                }
            }
            if (start != -1)
            {
                name = name.Remove(start);
                return name;
            }
            else return name;
        }

        public static void ShowInformationAboutDevice(string NameDevice,Label size,Label freeSpace,Label TypeSystem,ProgressBar condition, Label name, Label Used)
        {
            DriveInfo g = new DriveInfo(NameDevice);
            size.Text = String.Format("Size: " + "{0:0.00}", (g.TotalSize / (Math.Pow(2, 30))));
            size.Text += " Gb";

            freeSpace.Text = String.Format("Free Space: " + "{0:0.00}", (g.TotalFreeSpace / (Math.Pow(2, 30))));
            freeSpace.Text += " Gb";

            TypeSystem.Text = "File System: " + g.DriveFormat.ToString();

            int used = (int)(100 - ((g.TotalFreeSpace * 100) / g.TotalSize));

           // Navigator.FillCondition(NameDevice,condition);
            condition.Value = used;

            name.Text = "Name: " + g.Name.ToString();

            Used.Text = "Used: " + "(" + used.ToString() + "%)";
        }

        public static void FillCondition(string NameDevice,Label condition)
        {
            DriveInfo g = new DriveInfo(NameDevice);
            Graphics ShowCondition = condition.CreateGraphics();
            SolidBrush Fill;

            ShowCondition.SmoothingMode = SmoothingMode.HighQuality;

            ShowCondition.Clear(Color.Beige);

            int used = (int)(100 - ((g.TotalFreeSpace * 100) / g.TotalSize));

            if (used >= 90)
            {
                Fill = new SolidBrush(Color.Red);
            }
            else
            {
                Fill = new SolidBrush(Color.GreenYellow);
            }

            ShowCondition.FillRectangle(Fill, 0, 0, used, condition.Height);
        }

        public static void Copy(ListViewItem selectedItem, string SourcePath, string DestinationPath)
        {
            string FileName = string.Empty;
            string FileFormat = string.Empty;
            
            FileName = Navigator.ReturnFileName(selectedItem.Text);
            
                if (SourcePath != DestinationPath)
                {
                    if (Navigator.ReturnFormat(selectedItem.Text) != string.Empty)
                    {

                        FileFormat = Navigator.ReturnFormat(selectedItem.Text);
                        if(!File.Exists(DestinationPath+FileName+FileFormat))
                        {
                            try
                            {
                                File.Copy(SourcePath + selectedItem.Text, DestinationPath + FileName + "_copy" + FileFormat);
                                File.Move(DestinationPath + FileName + "_copy" + FileFormat, DestinationPath + FileName + FileFormat);
                            }
                            catch (UnauthorizedAccessException)
                            {
                                MessageBox.Show("Access denied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        else
                        {
                            
                            DialogResult result=MessageBox.Show("File with similar name has already exist. Do you wish to Replace?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                            if (result == DialogResult.Yes)
                            {
                                File.Delete(DestinationPath + FileName + FileFormat);
                                File.Copy(SourcePath + selectedItem.Text, DestinationPath + FileName + "_copy" + FileFormat);
                                File.Move(DestinationPath + FileName + "_copy" + FileFormat, DestinationPath + FileName + FileFormat);
                            }
                           
                        }
                    }
                    else
                    {
                        Navigator.CopyDirectory(SourcePath, DestinationPath, selectedItem.Text);
                    }

                }
                else
                {
                    MessageBox.Show("You try to copy file in similar direcory");
                }
            
        }

        public static void CopyDirectory(string SourcePath,string DestinationPath,string DirName)
        {
            if (SourcePath != DestinationPath)
            {
                string[] directories = Directory.GetDirectories(SourcePath+DirName);
                string[] files = Directory.GetFiles(SourcePath + DirName);

                if (files.Length != 0)
                {
                    string FileName = string.Empty;
                    string FileFormat = string.Empty;

                        Directory.CreateDirectory(DestinationPath + DirName);

                    foreach (string g in files)
                    {
                        FileName = Navigator.ReturnFileName(Path.GetFileName(g));

                        FileFormat = Navigator.ReturnFormat(Path.GetFileName(g));

                        if (!File.Exists(DestinationPath + DirName + "\\" + FileName + "_copy" + FileFormat))
                        {

                            try
                            {
                                File.Copy(SourcePath + DirName + "\\" + Path.GetFileName(g), DestinationPath + DirName + "\\" + FileName + "_copy" + FileFormat);

                                File.Move(DestinationPath + DirName + "\\" + FileName + "_copy" + FileFormat, DestinationPath + DirName + "\\" + FileName + FileFormat);
                            }
                            catch (UnauthorizedAccessException)
                            {
                                MessageBox.Show("Access denied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                if (directories.Length != 0)
                {
                    string recSourcePath;
                    string recDestinationPath;
                    string recDirname;
                    foreach (string s in directories)
                    {
                        Directory.CreateDirectory(DestinationPath + DirName + "\\" + Path.GetFileName(s));

                        recSourcePath = SourcePath + DirName + "\\";
                        recDestinationPath = DestinationPath + DirName + "\\";
                        recDirname = Path.GetFileName(s);

                        CopyDirectory(recSourcePath,recDestinationPath,recDirname);
                    }

                }//end directories.Length!=0

                else
                {
                    return;
                }
            }//end SourcePath!=DestinationPath
        }



        public static void Move(ListViewItem selectedItem, string SourcePath, string DestinationPath)
        {
            string FileName = string.Empty;
            string FileFormat = string.Empty;

                FileName = Navigator.ReturnFileName(selectedItem.Text);

                if (SourcePath != DestinationPath)
                {
                    if (Navigator.ReturnFormat(selectedItem.Text) != string.Empty)
                    {
                        try
                        {
                            File.Move(SourcePath + selectedItem.Text, DestinationPath + selectedItem.Text);
                        }
                        catch (IOException)
                        {
                            MessageBox.Show("File with similar name has already exist", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        try
                        {
                            Directory.Move(SourcePath + selectedItem.Text, DestinationPath + selectedItem.Text);
                        }
                        catch (IOException)
                        {
                            MessageBox.Show("Directory with similar name has already exist","Information",MessageBoxButtons.OKCancel,MessageBoxIcon.Information);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("You try to copy file in similar directory", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            
        }

        public static string ReturnDurectoryName(string path)
        {
            int count=0;
            for (int i = 0; i < path.Length-1; i++)
            {
                if (path[i] == '\\')
                {
                    count = i+1;
                }
            }
            path = path.Remove(0,count);
           path= path.Remove(path.IndexOf('\\'));
            return path;
        }


          
          
    }
}

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
using navigator;

namespace File_Manager
{
    public partial class Form1 : Form
    {
        Navigator leftNavigator;
        Navigator rightNavigator;

        public Form1()
        {
            InitializeComponent();

            //Navigator.GetDrives(LeftDevices);
            //Navigator.GetDrives(RightDevices);

            leftNavigator = new Navigator();
            rightNavigator = new Navigator();

            foreach(DriveInfo drive in leftNavigator.drives)
            {
                LeftDevicesComboBox.Items.Add(drive.Name);
                RightDevicesComboBox.Items.Add(drive.Name);
            }
            LeftDevicesComboBox.SelectedItem = LeftDevicesComboBox.Items[0];
            RightDevicesComboBox.SelectedItem = RightDevicesComboBox.Items[0];

            //Navigator.GetFiles(string.Empty, ref LeftPath, LeftDevices.SelectedItem.ToString(), LeftList);

            //Process[] processes = Process.GetProcesses();
            //foreach (Process p in processes)
            //{
            //    //listView2.Items.Add(p.ToString());
            //}

        }

        private void LeftDevicesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (leftNavigator.drives[LeftDevicesComboBox.SelectedIndex].IsReady == true)
            {
                leftNavigator.GetContent(LeftListView, LeftDevicesComboBox.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("Device does not ready to use", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LeftDevicesComboBox.SelectedItem = LeftDevicesComboBox.Items[0];
            }
            LeftPathTextBox.Text = leftNavigator.CurrentPath;
        }

        private void RightDevicesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rightNavigator.drives[RightDevicesComboBox.SelectedIndex].IsReady == true)
            {
                rightNavigator.GetContent(RightListView, RightDevicesComboBox.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("Device does not ready to use", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RightDevicesComboBox.SelectedItem = RightDevicesComboBox.Items[0];
            }
            RightPathTextBox.Text = rightNavigator.CurrentPath;
        }

        //  public static DriveInfo[] drives = DriveInfo.GetDrives();
        // public static string LeftPath = drives[0].Name;
        // public static string RightPath = drives[0].Name;

        //public static byte createThing = 0;


        //int startIndex;

        //Button "Back"
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    Navigator.GetFiles("..", ref LeftPath, LeftDevices.SelectedIndex.ToString(), LeftList);
        //    LeftPath = LeftPath.Replace("..\\", string.Empty);

        //    startIndex = Navigator.LastSlash(LeftPath);

        //    if (startIndex != 0)
        //    {
        //        LeftPath = LeftPath.Remove(Navigator.LastSlash(LeftPath));
        //    }

        //    LeftWayTextBox.Text = LeftPath;
        //}

        //private void listView_ItemActivate(object sender, EventArgs e)
        //{
        //    if (LeftList.SelectedItems.Count == 0)
        //        return;

        //    ListViewItem item = LeftList.SelectedItems[0];
        //    if (item.ImageIndex == 1)
        //    {
        //        Navigator.GetFiles(item.Text,ref LeftPath,LeftDevices.SelectedIndex.ToString(),LeftList);
        //        LeftWayTextBox.Text += item.Text + "\\";
        //    }
        //    else 
        //    {
        //      // MessageBox.Show(""+ File.GetCreationTimeUtc(wayLeft + item.Text) );
        //        Process.Start(LeftPath+item.Text);
        //    }
        //}

        //public static string lastname;


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

        // private void toolStripButton5_Click(object sender, EventArgs e)
        //{
        //DialogBox CreateFile = new DialogBox();
        //CreateFile.Show();
        //}



        /// Button "Rename"
        //private void toolStripButton4_Click(object sender, EventArgs e)
        //{
        //    ListViewItem item;
        //    if ((LeftList.SelectedItems.Count != 0))
        //    {
        //        item = LeftList.SelectedItems[0];

        //        DialogBox.format = Navigator.ReturnFormat(item.Text);
        //        DialogBox.way=LeftPath;
        //        DialogBox CreateFile = new DialogBox();
        //        lastname = item.Text;
        //        createThing = 6;
        //        CreateFile.Show();


        //        if (DialogBox.done == true)
        //        {
        //            try
        //            {
        //                Navigator.GetFiles(ref LeftPath, LeftList);
        //                Navigator.GetFiles(ref RightPath, RightList);
        //            }
        //            catch (DirectoryNotFoundException)
        //            {
        //                string path = "C:\\";
        //                Navigator.GetFiles(ref path, LeftList);
        //                Navigator.GetFiles(ref path, RightList);
        //            }
        //            DialogBox.done = false;
        //        }
        //    }
        //    else if (RightList.SelectedItems.Count != 0) 
        //    {
        //        item = RightList.SelectedItems[0];

        //        DialogBox.format = Navigator.ReturnFormat(item.Text);
        //        DialogBox.way = RightPath;
        //        DialogBox CreateFile = new DialogBox();
        //        lastname = item.Text;
        //        createThing = 6;
        //        CreateFile.Show();


        //        if (DialogBox.done == true)
        //        {
        //            try
        //            {
        //                Navigator.GetFiles(ref LeftPath, LeftList);
        //                Navigator.GetFiles(ref RightPath, RightList);
        //            }
        //            catch (DirectoryNotFoundException)
        //            {
        //                string path = "C:\\";
        //                Navigator.GetFiles(ref path, LeftList);
        //                Navigator.GetFiles(ref path, RightList);
        //            }
        //            DialogBox.done = false;
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("You haven't choosen a file", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        //    }
        //}



        //Button "Delete"
        //private void toolStripButton6_Click(object sender, EventArgs e)
        //{
        //    if (LeftList.SelectedItems.Count != 0)
        //    {
        //        ListViewItem item = LeftList.SelectedItems[0];
        //       DialogResult dialog = MessageBox.Show("Are you sure that you want to delete this file?", "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        //        if (dialog == DialogResult.Yes)
        //        {
        //           if(File.Exists(LeftPath+item.Text))
        //            {
        //                try
        //                {
        //                    File.Delete(LeftPath + item.Text);
        //                }
        //                catch (UnauthorizedAccessException)
        //                {
        //                    MessageBox.Show("Access denied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                    return;
        //                }
        //            } 
        //            else if(Directory.Exists(LeftPath+item.Text))
        //            {
        //                DirectoryInfo g=new DirectoryInfo(LeftPath + item.Text);
        //                try
        //                {
        //                    g.Delete(true);

        //                }
        //                catch (UnauthorizedAccessException)
        //                {
        //                    MessageBox.Show("Access denied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                    return;
        //                }
        //            }

        //            try
        //            {
        //                Navigator.GetFiles(ref LeftPath, LeftList);
        //                Navigator.GetFiles(ref RightPath, RightList);
        //            }
        //            catch (DirectoryNotFoundException)
        //            {
        //                string path = "C:\\";
        //                Navigator.GetFiles(ref path, LeftList);
        //                Navigator.GetFiles(ref path, RightList);
        //            }
        //        }
        //    }
        //    else if (RightList.SelectedItems.Count != 0)
        //    {
        //        ListViewItem item = RightList.SelectedItems[0];
        //        DialogResult dialog = MessageBox.Show("Are you sure that you want to delete this file?", "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        //        if (dialog == DialogResult.Yes)
        //        {
        //            if (File.Exists(RightPath + item.Text))
        //            {
        //                try
        //                {
        //                    File.Delete(RightPath + item.Text);
        //                }
        //                catch (UnauthorizedAccessException)
        //                {
        //                    MessageBox.Show("Access denied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                    return;
        //                }

        //            }
        //            else if (Directory.Exists(RightPath + item.Text))
        //            {
        //                DirectoryInfo g = new DirectoryInfo(RightPath + item.Text);
        //                try

        //                {
        //                    g.Delete(true);
        //                }
        //                catch (UnauthorizedAccessException)
        //                {
        //                    MessageBox.Show("Access denied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                    return;
        //                }
        //            }
        //            try
        //            {
        //                Navigator.GetFiles(ref LeftPath, LeftList);
        //                Navigator.GetFiles(ref RightPath, RightList);
        //            }
        //            catch (DirectoryNotFoundException)
        //            {
        //                string path = "C:\\";
        //                Navigator.GetFiles(ref path, LeftList);
        //                Navigator.GetFiles(ref path, RightList);
        //            }
        //        }
        //    }

        //}

        //private void toolStripMenuItem1_Click(object sender, EventArgs e)//Button "Create directory"
        //{
        //    DialogBox CreateFile = new DialogBox();
        //    DialogBox.way = LeftPath;
        //    createThing = 1;
        //    CreateFile.Show();
        //}

        //private void sdToolStripMenuItem_Click(object sender, EventArgs e)//Button "Create text file .txt"
        //{
        //    DialogBox CreateFile = new DialogBox();
        //    DialogBox.way = LeftPath;
        //    createThing = 5;
        //    CreateFile.Show();
        //}

        //private void toolStripButton3_Click(object sender, EventArgs e)
        //{
        //    ListViewItem item;
        //    if (LeftList.SelectedItems.Count != 0)
        //    {
        //        item = LeftList.SelectedItems[0];
        //        try
        //        {
        //            Navigator.Move(item, LeftPath, RightPath);
        //        }
        //        catch (UnauthorizedAccessException)
        //        {
        //            MessageBox.Show("Access denied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            return;
        //        }

        //        Navigator.GetFiles(ref LeftPath, LeftList);
        //        Navigator.GetFiles(ref RightPath, RightList);
        //    }
        //    else if (RightList.SelectedItems.Count != 0)
        //    {
        //        item = RightList.SelectedItems[0];
        //        try
        //        {
        //            Navigator.Move(item, RightPath, LeftPath);
        //        }
        //        catch (UnauthorizedAccessException)
        //        {
        //            MessageBox.Show("Access denied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            return;
        //        }

        //        Navigator.GetFiles(ref LeftPath, LeftList);
        //        Navigator.GetFiles(ref RightPath, RightList);
        //    }
        //    else
        //    {
        //        MessageBox.Show("You haven't chosen any file");
        //    }
        //}
        // public static string wayToFile = "";



        //Button "Open File"
        //private void toolStripButton7_Click(object sender, EventArgs e)
        //{

        //    if ((LeftList.SelectedItems.Count != 0)&&(Navigator.ReturnFormat(LeftList.SelectedItems[0].Text)!=string.Empty))
        //    {
        //        if (Navigator.ReturnFormat(LeftList.SelectedItems[0].Text) == ".txt")
        //        {
        //            wayToFile = LeftPath + LeftList.SelectedItems[0].Text;
        //            Form2 EditWindow = new Form2();
        //            EditWindow.Show();
        //        }
        //        else
        //        {
        //            MessageBox.Show("You can edit just text files");
        //        }
        //    }
        //    else if ((RightList.SelectedItems.Count != 0) && (Navigator.ReturnFormat(RightList.SelectedItems[0].Text) != string.Empty))
        //    {
        //        if (Navigator.ReturnFormat(RightList.SelectedItems[0].Text) == ".txt")
        //        {
        //            wayToFile = RightPath + RightList.SelectedItems[0].Text;
        //            Form2 EditWindow = new Form2();
        //            EditWindow.Show();
        //        }
        //        else
        //        {
        //            MessageBox.Show("You can edit just text files");
        //        }
        //    }
        //}



        //Button "Copy"
        //private void toolStripButton2_Click(object sender, EventArgs e)
        //{

        //    ListViewItem item;

        //    if (LeftList.SelectedItems.Count != 0)
        //    {
        //        item = LeftList.SelectedItems[0];
        //        Navigator.Copy(item, LeftPath, RightPath);

        //        Navigator.GetFiles(ref LeftPath, LeftList);
        //        Navigator.GetFiles(ref RightPath, RightList);

        //    }
        //    else if (RightList.SelectedItems.Count != 0)
        //    {
        //        item = RightList.SelectedItems[0];

        //        Navigator.Copy(item, RightPath, LeftPath);

        //        Navigator.GetFiles(ref LeftPath, LeftList);
        //        Navigator.GetFiles(ref RightPath, RightList);
        //    }
        //    else
        //    {
        //        MessageBox.Show("You haven't chosen any file");
        //    }
        //}


        //private void Form1_Activated(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Navigator.GetFiles(ref LeftPath, LeftList);
        //        Navigator.GetFiles(ref RightPath, RightList);
        //    }
        //    catch (DirectoryNotFoundException)
        //    {
        //        string path = "C:\\";
        //        Navigator.GetFiles(ref path, LeftList);
        //        Navigator.GetFiles(ref path, RightList);
        //    }
        //}

        //private void toolStripButton9_Click(object sender, EventArgs e)
        //{
        //    LeftList.CheckBoxes = true;
        //}

        //private void label7_Paint(object sender, PaintEventArgs e)
        //{
        //}




        //private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    //MessageBox.Show(toolStripComboBox1.SelectedItem.ToString());
        //    DriveInfo device = new DriveInfo(LeftDevices.SelectedItem.ToString());

        //    if (device.IsReady == true)
        //    {
        //        Navigator.GetFiles(string.Empty, ref LeftPath, LeftDevices.SelectedItem.ToString(), LeftList);
        //        //ShowInformationAboutDevice(treeView.SelectedNode.Text);
        //        Navigator.ShowInformationAboutDevice(LeftDevices.SelectedItem.ToString(), label1, label2, label3, LeftCondition, label4, label6);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Device does not ready to use", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        //        LeftPath = "C:\\";
        //        Navigator.GetFiles(ref LeftPath, LeftList);

        //        LeftDevices.SelectedItem = LeftDevices.Items[0];
        //    }
        //    LeftWayTextBox.Text = LeftDevices.SelectedItem.ToString();

        //}

        //private void toolStripButton10_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DriveInfo device = new DriveInfo(RightDevices.SelectedItem.ToString());

        //    if (device.IsReady == true)
        //    {
        //        Navigator.GetFiles(string.Empty, ref RightPath, RightDevices.SelectedItem.ToString(), RightList);
        //        Navigator.ShowInformationAboutDevice(RightDevices.SelectedItem.ToString(), label14, label13, label12, RightCondition, label11, label9);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Device does not ready to use", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        //        RightPath = "D:\\";
        //        Navigator.GetFiles(ref LeftPath, LeftList);

        //        RightDevices.SelectedItem = RightDevices.Items[0];
        //    }
        //    RightWayTextBox.Text = RightDevices.SelectedItem.ToString();
        //}

        //private void listView1_ItemActivate(object sender, EventArgs e)
        //{
        //    if (RightList.SelectedItems.Count == 0)
        //        return;

        //    ListViewItem item = RightList.SelectedItems[0];
        //    if (item.ImageIndex == 1)
        //    {
        //        Navigator.GetFiles(item.Text, ref RightPath, RightDevices.SelectedItem.ToString(), RightList);
        //        RightWayTextBox.Text += item.Text + "\\";
        //    }
        //    else
        //    {
        //        Process.Start(RightPath + item.Text);
        //    }
        //}

        //    private void button4_Click(object sender, EventArgs e)
        //    {
        //        Navigator.GetFiles("..", ref RightPath, RightDevices.SelectedItem.ToString(), RightList);
        //        RightPath = RightPath.Replace("..\\", string.Empty);

        //        startIndex = Navigator.LastSlash(RightPath);

        //        if (startIndex != 0)
        //        {
        //            RightPath = RightPath.Remove(Navigator.LastSlash(RightPath));
        //        }

        //        RightWayTextBox.Text = RightPath;
        //    }

        //    private void toolStripMenuItem2_Click(object sender, EventArgs e)
        //    {
        //        DialogBox CreateFile = new DialogBox();
        //        DialogBox.way = LeftPath;
        //        createThing = 2;
        //        CreateFile.Show();
        //    }

        //    private void label8_Paint(object sender, PaintEventArgs e)
        //    {
        //    }

        //    private void toolStripButton8_Click(object sender, EventArgs e)
        //    {
        //        Process.Start("D:\\Help.chm");
        //    }

        //    private void toolStripMenuItem3_Click_1(object sender, EventArgs e)
        //    {
        //        DialogBox CreateFile = new DialogBox();
        //        DialogBox.way = LeftPath;
        //        createThing = 3;
        //        CreateFile.Show();
        //    }

        //    private void toolStripMenuItem4_Click_1(object sender, EventArgs e)
        //    {
        //        DialogBox CreateFile = new DialogBox();
        //        DialogBox.way = LeftPath;
        //        createThing = 4;
        //        CreateFile.Show();
        //    }

        //    private void toolStripButton9_Click_1(object sender, EventArgs e)
        //    {
        //        Form3 f = new Form3();
        //        f.Show();
        //    }

        //    private void Form1_Load(object sender, EventArgs e)
        //    {

        //    }
        //}



        //class Navigator
        //{

        //    public static void ShowInformationAboutDevice(string NameDevice, Label size, Label freeSpace, Label TypeSystem, ProgressBar condition, Label name, Label Used)
        //    {
        //        DriveInfo g = new DriveInfo(NameDevice);
        //        size.Text = String.Format("Size: " + "{0:0.00}", (g.TotalSize / (Math.Pow(2, 30))));
        //        size.Text += " Gb";

        //        freeSpace.Text = String.Format("Free Space: " + "{0:0.00}", (g.TotalFreeSpace / (Math.Pow(2, 30))));
        //        freeSpace.Text += " Gb";

        //        TypeSystem.Text = "File System: " + g.DriveFormat.ToString();

        //        int used = (int)(100 - ((g.TotalFreeSpace * 100) / g.TotalSize));

        //        // Navigator.FillCondition(NameDevice,condition);
        //        condition.Value = used;

        //        name.Text = "Name: " + g.Name.ToString();

        //        Used.Text = "Used: " + "(" + used.ToString() + "%)";
        //    }

        //}
    }
}

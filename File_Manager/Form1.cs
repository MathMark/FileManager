using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using navigator;
using System.Collections.Generic;

namespace File_Manager
{
    public interface IMainForm
    {
        void ShowContent(ListView listView, List<string> Content);
        int AddIcon(string format);
    }
    public partial class Form1 : Form,IMainForm
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
        #region realization of IMainForm interface
        public void ShowContent(ListView listView,List<string>Content)
        {
            listView.Items.Clear();
            foreach(string @object in Content)
            {
                if(Directory.Exists(@object))
                {
                    listView.Items.Add(Path.GetFileName(@object),AddIcon("Directory"));
                }
                else if(File.Exists(@object))
                {
                    listView.Items.Add(Path.GetFileName(@object), AddIcon(Path.GetExtension(@object)));
                }
                else
                {

                }
                
            }
        }
        public int AddIcon(string format)
        {
            switch (format)
            {
                case "Directory":
                    return 1;
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
        #endregion

        #region Events
        private void LeftDevicesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (leftNavigator.drives[LeftDevicesComboBox.SelectedIndex].IsReady == true)
            {
                ShowContent(LeftListView, leftNavigator.GetContent(LeftDevicesComboBox.SelectedItem.ToString()));
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
                // rightNavigator.GetContent(RightListView, RightDevicesComboBox.SelectedItem.ToString());
                ShowContent(RightListView, rightNavigator.GetContent(RightDevicesComboBox.SelectedItem.ToString()));
            }
            else
            {
                MessageBox.Show("Device does not ready to use", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RightDevicesComboBox.SelectedItem = RightDevicesComboBox.Items[0];
            }
            RightPathTextBox.Text = rightNavigator.CurrentPath;
        }

        private void LeftListView_ItemActivate(object sender, EventArgs e)
        {
            if (LeftListView.SelectedItems.Count != 0)
            {
                ListViewItem item = LeftListView.SelectedItems[0];
                if (Directory.Exists(leftNavigator.ContentOfCurrentDirectory[item.Index]))
                {
                    ShowContent(LeftListView, leftNavigator.GetContent(item.Index));
                    LeftPathTextBox.Text = leftNavigator.CurrentPath;

                }
                else
                {
                    Process.Start(leftNavigator.ContentOfCurrentDirectory[item.Index]);
                }
            }
            else
            {
                return;
            }
        }

        private void RightListView_ItemActivate(object sender, EventArgs e)
        {
            if (RightListView.SelectedItems.Count != 0)
            {
                ListViewItem item = RightListView.SelectedItems[0];
                if (Directory.Exists(rightNavigator.ContentOfCurrentDirectory[item.Index]))
                {
                    ShowContent(RightListView, rightNavigator.GetContent(item.Index));
                    RightPathTextBox.Text = rightNavigator.CurrentPath;
                }
                else
                {
                    Process.Start(rightNavigator.ContentOfCurrentDirectory[item.Index]);
                }
            }
            else
            {
                return;
            }
        }

        private void LeftBackButton_Click(object sender, EventArgs e)
        {
            //leftNavigator.GetContent(LeftListView, leftNavigator.CurrentPath + "\\..");
            ShowContent(LeftListView, leftNavigator.GetContent(leftNavigator.CurrentPath+"\\.."));
            LeftPathTextBox.Text = leftNavigator.CurrentPath;
        }

        private void RightBackButton_Click(object sender, EventArgs e)
        {
            // rightNavigator.GetContent(RightListView, rightNavigator.CurrentPath + "\\..");
            ShowContent(RightListView, rightNavigator.GetContent(rightNavigator.CurrentPath + "\\.."));
            RightPathTextBox.Text = rightNavigator.CurrentPath;
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            ListViewItem item;

            if (LeftListView.SelectedItems.Count != 0)
            {
                item = LeftListView.SelectedItems[0];
                leftNavigator.Copy(item.Text,rightNavigator.CurrentPath);
            }
            else if (RightListView.SelectedItems.Count != 0)
            {
                item = RightListView.SelectedItems[0];

                rightNavigator.Copy(item.Text, leftNavigator.CurrentPath);
            }
            else
            {
                MessageBox.Show("You haven't chosen any file");
            }

            ShowContent(LeftListView, leftNavigator.GetContent(leftNavigator.CurrentPath));
            ShowContent(RightListView, rightNavigator.GetContent(rightNavigator.CurrentPath));
        }
        #endregion

        //  public static DriveInfo[] drives = DriveInfo.GetDrives();
        // public static string LeftPath = drives[0].Name;
        // public static string RightPath = drives[0].Name;

        //public static byte createThing = 0;

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

    }
}

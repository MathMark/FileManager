using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using navigator;
using System.Collections.Generic;
using System.Threading;

namespace File_Manager
{
    public interface IMainForm
    {
        void ShowContent(ListView listView,List<string> Content);
        int AddIcon(string format);
    }
    public partial class Form1 : Form,IMainForm
    {
        Navigator leftNavigator;
        Navigator rightNavigator;

        public Form1()
        {
            InitializeComponent();

            leftNavigator = new Navigator();
            rightNavigator = new Navigator();

            foreach(DriveInfo drive in leftNavigator.drives)
            {
                LeftDevicesComboBox.Items.Add(drive.Name);
                RightDevicesComboBox.Items.Add(drive.Name);
            }
            LeftDevicesComboBox.SelectedItem = LeftDevicesComboBox.Items[0];
            RightDevicesComboBox.SelectedItem = RightDevicesComboBox.Items[0];

        }
        #region realization of IMainForm interface
        public void ShowContent(ListView listView,List<string>Content)
        {
            listView.Items.Clear();
            ListViewItem item;
            DirectoryInfo directoryInfo;
            FileInfo fileInfo;
            const long KByte = 1024;
            const long MByte = 1048576;
            
            foreach (string @object in Content)
            {
                if(Directory.Exists(@object))
                {
                    directoryInfo = new DirectoryInfo(@object);

                    item = new ListViewItem(Path.GetFileNameWithoutExtension(@object),AddIcon("Directory"));
                    item.SubItems.Add("<dir>");
                    item.SubItems.Add("");

                    listView.Items.Add(item);
                }
                else if(File.Exists(@object))
                {
                    fileInfo = new FileInfo(@object);

                    item = new ListViewItem(Path.GetFileNameWithoutExtension(@object),AddIcon(Path.GetExtension(@object)));
                    item.SubItems.Add(Path.GetExtension(@object));

                    long Size = fileInfo.Length;
                    if (Size >= MByte)
                    {
                        item.SubItems.Add((fileInfo.Length / MByte).ToString() + " Mb");
                    }
                    else
                    {
                        item.SubItems.Add((fileInfo.Length / KByte).ToString() + " Kb");
                    }
                    


                    listView.Items.Add(item);
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
                ShowContent(LeftListView,leftNavigator.GetContent(LeftDevicesComboBox.SelectedItem.ToString()));
            }
            else
            {
                MessageBox.Show("Device does not ready to use", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LeftDevicesComboBox.SelectedItem = LeftDevicesComboBox.Items[0];
            }
            label1.Text = leftNavigator.CurrentPath;
        }

        private void RightDevicesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rightNavigator.drives[RightDevicesComboBox.SelectedIndex].IsReady == true)
            {
                // rightNavigator.GetContent(RightListView, RightDevicesComboBox.SelectedItem.ToString());
                ShowContent(RightListView,rightNavigator.GetContent(RightDevicesComboBox.SelectedItem.ToString()));
            }
            else
            {
                MessageBox.Show("Device does not ready to use", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RightDevicesComboBox.SelectedItem = RightDevicesComboBox.Items[0];
            }
            label2.Text = rightNavigator.CurrentPath;
        }

        private void LeftListView_ItemActivate(object sender, EventArgs e)
        {
            if (LeftListView.SelectedItems.Count != 0)
            {
                ListViewItem item = LeftListView.SelectedItems[0];
                if (Directory.Exists(leftNavigator.ContentOfCurrentDirectory[item.Index]))
                {
                    ShowContent(LeftListView, leftNavigator.GetContent(item.Index));
                    label1.Text = leftNavigator.CurrentPath;

                }
                else
                {
                    //Process.Start(leftNavigator.ContentOfCurrentDirectory[item.Index]);
                    MessageBox.Show(item.Text);
                    //FileAttributes attributes = File.GetAttributes(leftNavigator.ContentOfCurrentDirectory[item.Index]);
                    //if ((attributes == FileAttributes.))
                    //{
                    //    MessageBox.Show("read-only file");
                    //}
                    //else
                    //{
                    //    MessageBox.Show("not read-only file");
                    //}
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
                    label2.Text = rightNavigator.CurrentPath;
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


        private void LeftBackButton_Click_1(object sender, EventArgs e)
        {
            ShowContent(LeftListView, leftNavigator.GetContent(leftNavigator.CurrentPath + "\\.."));

            label1.Text = leftNavigator.CurrentPath;
        }
        private void RightBackButton_Click_1(object sender, EventArgs e)
        {
            ShowContent(RightListView, rightNavigator.GetContent(rightNavigator.CurrentPath + "\\.."));
            label2.Text = rightNavigator.CurrentPath;
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            ListViewItem item;
            Thread r;
            Tonnel tonnel;
            try
            {
                if (LeftListView.SelectedItems.Count != 0)
                {
                    item = LeftListView.SelectedItems[0];
                    //leftNavigator.Copy(item.Text + item.SubItems[1].Text.Replace("<dir>", string.Empty), rightNavigator.CurrentPath);
                    tonnel = new Tonnel(item.Text + item.SubItems[1].Text.Replace("<dir>", string.Empty), leftNavigator.CurrentPath,
                        rightNavigator.CurrentPath);
                    r = new Thread(Navigator.Copy);
                    r.IsBackground = true;
                    r.Start(tonnel);
                }
                else if (RightListView.SelectedItems.Count != 0)
                {
                    item = RightListView.SelectedItems[0];

                    //rightNavigator.Copy(item.Text + item.SubItems[1].Text.Replace("<dir>", string.Empty), leftNavigator.CurrentPath);

                    tonnel = new Tonnel(item.Text + item.SubItems[1].Text.Replace("<dir>", string.Empty), rightNavigator.CurrentPath,
                        leftNavigator.CurrentPath);
                    r = new Thread(Navigator.Copy);
                    r.IsBackground = true;
                    r.Start(tonnel);
                }
                else
                {
                    MessageBox.Show("You haven't chosen any file");
                }
            }
            catch(UnauthorizedAccessException)
            {
                MessageBox.Show("Access denied", "Inforamtion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Inforamtion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        
            ShowContent(LeftListView, leftNavigator.GetContent(leftNavigator.CurrentPath));
            ShowContent(RightListView, rightNavigator.GetContent(rightNavigator.CurrentPath));
        }

        private void MoveButton_Click(object sender, EventArgs e)
        {
            ListViewItem item;
            try
            {
                if (LeftListView.SelectedItems.Count != 0)
                {
                    item = LeftListView.SelectedItems[0];
                    leftNavigator.Move(item.Text + item.SubItems[1].Text.Replace("<dir>", string.Empty), rightNavigator.CurrentPath);
                }
                else if (RightListView.SelectedItems.Count != 0)
                {
                    item = RightListView.SelectedItems[0];

                    rightNavigator.Move(item.Text + item.SubItems[1].Text.Replace("<dir>", string.Empty), leftNavigator.CurrentPath);
                }
                else
                {
                    MessageBox.Show("You haven't chosen any file");
                }
            }
            catch(UnauthorizedAccessException)
            {
                MessageBox.Show("Access denied", "Inforamtion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message, "Inforamtion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ShowContent(LeftListView, leftNavigator.GetContent(leftNavigator.CurrentPath));
            ShowContent(RightListView, rightNavigator.GetContent(rightNavigator.CurrentPath));
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            ListViewItem item;
            try
            {
                if (LeftListView.SelectedItems.Count != 0)
                {
                    DialogResult dialog = MessageBox.Show("Are you sure that you want to delete this file?", "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        item = LeftListView.SelectedItems[0];
                        leftNavigator.Delete(item.Text + item.SubItems[1].Text.Replace("<dir>",string.Empty));
                    }
                }
                else if (RightListView.SelectedItems.Count != 0)
                {
                    DialogResult dialog = MessageBox.Show("Are you sure that you want to delete this file?", "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        item = RightListView.SelectedItems[0];

                        rightNavigator.Delete(item.Text + item.SubItems[1].Text.Replace("<dir>", string.Empty));
                    }
                }
                else
                {
                    MessageBox.Show("You haven't chosen any file");
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Access denied", "Inforamtion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Inforamtion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ShowContent(LeftListView, leftNavigator.GetContent(leftNavigator.CurrentPath));
            ShowContent(RightListView, rightNavigator.GetContent(rightNavigator.CurrentPath));
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           
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

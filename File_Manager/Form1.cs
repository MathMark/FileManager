using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using navigator;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;

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

            statusStrip1.Hide();
            DialogBox.FormClose += DialogBox_Close;

            Navigator.ProgressChanged += Navigator_ProgressChanged;

            Navigator.ProcessCompleted += Navigator_ProcessCompleted;


        }
        #region realization of IMainForm interface
        public void ShowContent(ListView listView,List<string>Content)
        {
            listView.Items.Clear();
            ListViewItem item;

            const long KByte = 1024;
            const long MByte = 1048576;
            
            foreach (string @object in Content)
            {
                if(Directory.Exists(@object))
                {
                    item = new ListViewItem(Path.GetFileNameWithoutExtension(@object),AddIcon("Directory"));
                    item.SubItems.Add("<dir>");
                    item.SubItems.Add("");
                    item.SubItems.Add(new DirectoryInfo(@object).CreationTime.ToString());

                    listView.Items.Add(item);
                }
                else if(File.Exists(@object))
                {
                    item = new ListViewItem(Path.GetFileNameWithoutExtension(@object),AddIcon(Path.GetExtension(@object)));
                    item.SubItems.Add(Path.GetExtension(@object));

                    long Size = new FileInfo(@object).Length;
                    if (Size >= MByte)
                    {
                        item.SubItems.Add((new FileInfo(@object).Length / MByte).ToString() + " Mb");
                    }
                    else
                    {
                        item.SubItems.Add((new FileInfo(@object).Length / KByte).ToString() + " Kb");
                    }
                    item.SubItems.Add(new FileInfo(@object).CreationTime.ToString());


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
                    //MessageBox.Show(item.Text);
                    //FileAttributes attributes = File.GetAttributes(leftNavigator.ContentOfCurrentDirectory[item.Index]);
                    //if ((attributes == FileAttributes.))
                    //{
                    //    MessageBox.Show("read-only file");
                    //}
                    //else
                    //{
                    //    MessageBox.Show("not read-only file");
                    //}
                    //Navigator.Copy(leftNavigator.ContentOfCurrentDirectory[item.Index], rightNavigator.CurrentPath);

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
            Thread threadCopy;
            Tonnel tonnel;
            try
            {
                if (leftNavigator.CurrentPath != rightNavigator.CurrentPath)
                {
                    if (LeftListView.SelectedItems.Count != 0)
                    {
                        statusStrip1.Show();
                        progressLabel.Text = "Copying...";

                        foreach (ListViewItem item in LeftListView.SelectedItems)
                        {
                            if (File.Exists(leftNavigator.ContentOfCurrentDirectory[item.Index]))
                            {
                                tonnel = new Tonnel(leftNavigator.ContentOfCurrentDirectory[item.Index],rightNavigator.CurrentPath);
                                threadCopy = new Thread(Navigator.CopyFile);
                                threadCopy.IsBackground = true;

                                threadCopy.Start(tonnel);
                            }
                            else
                            {
                                ;//there will be CopyDirectory method
                            }
                        }
                    }
                    else if (RightListView.SelectedItems.Count != 0)
                    {
                        statusStrip1.Show();
                        progressLabel.Text = "Copying...";
                        foreach (ListViewItem item in RightListView.SelectedItems)
                        {
                            if (File.Exists(rightNavigator.ContentOfCurrentDirectory[item.Index]))
                            {
                                tonnel = new Tonnel(rightNavigator.ContentOfCurrentDirectory[item.Index],leftNavigator.CurrentPath);
                                threadCopy = new Thread(Navigator.CopyFile);
                                threadCopy.IsBackground = true;

                                threadCopy.Start(tonnel);
                            }
                            else
                            {
                                ;//there will be CopyDirectory method
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("You haven't chosen any files","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("You trying to copy this in similar path", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            finally
            {
                ShowContent(LeftListView, leftNavigator.GetContent(leftNavigator.CurrentPath));
                ShowContent(RightListView, rightNavigator.GetContent(rightNavigator.CurrentPath));
            }
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
                    MessageBox.Show("You haven't chosen any files");
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
            finally
            {
                ShowContent(LeftListView, leftNavigator.GetContent(leftNavigator.CurrentPath));
                ShowContent(RightListView, rightNavigator.GetContent(rightNavigator.CurrentPath));
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DialogResult dialog;
            try
            {
                if (LeftListView.SelectedItems.Count != 0)
                {
                    if (LeftListView.SelectedItems.Count > 1)
                    {
                        dialog = MessageBox.Show("Are you sure that you want to delete these files?", "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    }
                    else
                    {
                         dialog= MessageBox.Show("Are you sure that you want to delete this file?", "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    }
                    if (dialog == DialogResult.Yes)
                        foreach (ListViewItem item in LeftListView.SelectedItems)
                    {
                            leftNavigator.Delete(item.Text + item.SubItems[1].Text.Replace("<dir>", string.Empty)); 
                    }
                }
                else if (RightListView.SelectedItems.Count != 0)
                {
                    if (RightListView.SelectedItems.Count > 1)
                    {
                        dialog = MessageBox.Show("Are you sure that you want to delete these files?", "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    }
                    else
                    {
                        dialog = MessageBox.Show("Are you sure that you want to delete this file?", "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    }
                    if (dialog == DialogResult.Yes)
                    {
                        foreach (ListViewItem item in RightListView.SelectedItems)
                        {
                            rightNavigator.Delete(item.Text + item.SubItems[1].Text.Replace("<dir>", string.Empty));
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You haven't chosen any files");
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
            finally
            {
                ShowContent(LeftListView, leftNavigator.GetContent(leftNavigator.CurrentPath));
                ShowContent(RightListView, rightNavigator.GetContent(rightNavigator.CurrentPath));
            }
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           
        }

        private void largeIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LeftListView.View = View.LargeIcon;
            RightListView.View = View.LargeIcon;
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LeftListView.View = View.Details;
            RightListView.View = View.Details;
        }

        private void smallIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LeftListView.View = View.SmallIcon;
            RightListView.View = View.SmallIcon;
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LeftListView.View = View.List;
            RightListView.View = View.List;
        }

        private void titleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LeftListView.View = View.Tile;
            RightListView.View = View.Tile;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogBox dialogBox;
            if (LeftListView.SelectedItems.Count != 0)
            {
                dialogBox = new DialogBox((Bitmap)imageList1.Images[1], leftNavigator.CurrentPath);
                dialogBox.Show();
            }
            else if(RightListView.SelectedItems.Count != 0)
            {
                dialogBox = new DialogBox((Bitmap)imageList1.Images[1], rightNavigator.CurrentPath);
                dialogBox.Show();
            }
            else
            {
                ;
            }

            
        }

        private void DialogBox_Close()
        {
            ShowContent(LeftListView, leftNavigator.GetContent(leftNavigator.CurrentPath));
            ShowContent(RightListView, rightNavigator.GetContent(rightNavigator.CurrentPath));
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void sdToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Navigator_ProgressChanged(int progress)
        {
            Action action = () =>
              {
                  ProgressStatus.Value = progress;
              };
            Invoke(action);
        }

        private void Navigator_ProcessCompleted()
        {
            Action action = () =>
            {
                ProgressStatus.Value += 100- ProgressStatus.Value;
                progressLabel.Text = "Copying successfuly completed";

                ShowContent(LeftListView, leftNavigator.GetContent(leftNavigator.CurrentPath));
                ShowContent(RightListView, rightNavigator.GetContent(rightNavigator.CurrentPath));
                Thread.Sleep(1000);

                statusStrip1.Hide();

                ProgressStatus.Value = 0;
            };
            Invoke(action);
        }

        #endregion


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

    }
}

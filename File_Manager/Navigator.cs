using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace navigator
{
    public class Navigator
    {
        public List<string> ContentOfCurrentDirectory { get; }
        public string CurrentPath { get; private set; }
        public DriveInfo[] drives { get; private set; }
        public Navigator()
        {
            drives = DriveInfo.GetDrives();
            CurrentPath = drives[0].Name;
            ContentOfCurrentDirectory = new List<string>();
        }

        public void GetContent(ListView plane, string path)
        {
            plane.Items.Clear();
            if (path.Contains("\\..") == true)
            {
                path = path.Replace("\\..", string.Empty);

                if (path.IndexOf("\\") != path.LastIndexOf("\\") && (path.LastIndexOf("\\") != path.Length - 1))
                {
                    path = path.Remove(CurrentPath.LastIndexOf('\\'));
                }
                else if (path.LastIndexOf("\\") != path.Length - 1)
                {
                    path = path.Remove(CurrentPath.LastIndexOf('\\') + 1);
                }

            }
            CurrentPath = path;
            string[] Directories = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);

            //MessageBox.Show(Cu)
            ContentOfCurrentDirectory.Clear();

            foreach (string directory in Directories)
            {
                if ((File.GetAttributes(directory) & FileAttributes.Hidden) == FileAttributes.Hidden)//hidden files and directories
                    continue;

                ContentOfCurrentDirectory.Add(directory);
                plane.Items.Add(Path.GetFileName(directory), 1);

            }
            foreach (string file in files)
            {
                ContentOfCurrentDirectory.Add(file);
                plane.Items.Add(Path.GetFileName(file), AddIcon(Path.GetExtension(file)));
            }
        }
        public void GetContent(ListView plane, int index)
        {
            plane.Items.Clear();

            //MessageBox.Show(index.ToString());
            string[] Directories = Directory.GetDirectories(ContentOfCurrentDirectory[index]);
            string[] files = Directory.GetFiles(ContentOfCurrentDirectory[index]);

            CurrentPath = ContentOfCurrentDirectory[index];
            ContentOfCurrentDirectory.Clear();

            foreach (string directory in Directories)
            {
                if ((File.GetAttributes(directory) & FileAttributes.Hidden) == FileAttributes.Hidden)//hidden files and directories
                    continue;

                ContentOfCurrentDirectory.Add(directory);
                plane.Items.Add(Path.GetFileName(directory), 1);

            }

            foreach (string file in files)
            {
                ContentOfCurrentDirectory.Add(file);
                plane.Items.Add(Path.GetFileName(file), AddIcon(Path.GetExtension(file)));
            }
        }

        //вместо пути поставить список и обновить его!
        public void Move(ListViewItem selectedItem, string DestinationPath)
        {
            if (CurrentPath != DestinationPath)
            {

                if (File.Exists(CurrentPath + "\\" + selectedItem.Name))
                {
                    try
                    {
                        File.Move(CurrentPath + "\\" + selectedItem.Text, DestinationPath + "\\" + selectedItem.Text);
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("File with similar name has already exist", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    }
                }
                else if (Directory.Exists(CurrentPath + "\\" + selectedItem.Name))
                {
                    try
                    {
                        Directory.Move(CurrentPath + "\\" + selectedItem.Text, DestinationPath + "\\" + selectedItem.Text);
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Directory with similar name has already exist", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    }
                }

            }
            else
            {
                MessageBox.Show("You try to move file in similar directory", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        public void Delete(ListViewItem item)
        {
            if (File.Exists(CurrentPath + "\\" + item.Text))
            {
                try
                {
                    File.Delete(CurrentPath + "\\" + item.Text);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Access denied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (Directory.Exists(CurrentPath + "\\" + item.Text))
            {
                DirectoryInfo g = new DirectoryInfo(CurrentPath + "\\" + item.Text);
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
        }

        public void Copy(ListViewItem selectedItem, string DestinationPath)
        {
            string FileName = string.Empty;
            string FileFormat = string.Empty;

            // FileName = oldNavigator.ReturnFileName(selectedItem.Text);

            if (CurrentPath != DestinationPath)
            {
                //if (Navigator.ReturnFormat(selectedItem.Text) != string.Empty)
                if (File.Exists(CurrentPath + "\\" + selectedItem.Text))
                {
                    if (!File.Exists(DestinationPath + FileName + FileFormat))
                    {
                        try
                        {
                            File.Copy(CurrentPath + "\\" + selectedItem.Text, DestinationPath + "\\" + Path.GetFileNameWithoutExtension(ContentOfCurrentDirectory[selectedItem.Index])
                                + "_copy" + Path.GetExtension(ContentOfCurrentDirectory[selectedItem.Index]));
                            //File.Move(DestinationPath + FileName + "_copy" + FileFormat, DestinationPath + FileName + FileFormat);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            MessageBox.Show("Access denied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("A file with similar name has already exist. Do you wish to Replace?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        if (result == DialogResult.Yes)
                        {
                            File.Delete(DestinationPath + "\\" + Path.GetFileName(ContentOfCurrentDirectory[selectedItem.Index]));
                            File.Copy(CurrentPath + "\\" + selectedItem.Text, DestinationPath + "\\" + Path.GetFileNameWithoutExtension(ContentOfCurrentDirectory[selectedItem.Index])
                                + "_copy" + Path.GetExtension(ContentOfCurrentDirectory[selectedItem.Index]));
                            //File.Move(DestinationPath + FileName + "_copy" + FileFormat, DestinationPath + FileName + FileFormat);
                        }

                    }
                }
                else
                {
                    CopyDirectory(CurrentPath, DestinationPath, selectedItem.Text);
                }

            }
            else
            {
                MessageBox.Show("You try to copy file in similar direcory");
            }

        }

        private void CopyDirectory(string SourcePath, string DestinationPath, string DirName)
        {
            if (SourcePath != DestinationPath)
            {
                string[] directories = Directory.GetDirectories(SourcePath + "\\" + DirName);
                string[] files = Directory.GetFiles(SourcePath + "\\" + DirName);

                if (files.Length != 0)
                {
                    string FileName = string.Empty;
                    string FileFormat = string.Empty;

                    Directory.CreateDirectory(DestinationPath + "\\" + DirName);

                    foreach (string file in files)
                    {
                        if (!File.Exists(DestinationPath + DirName + "\\" + FileName + "_copy" + FileFormat))
                        {

                            try
                            {
                                File.Copy(SourcePath + "\\" + DirName + "\\" + Path.GetFileName(file),
                                    DestinationPath + "\\" + DirName + "\\" + Path.GetFileNameWithoutExtension(file) + "_copy" + Path.GetExtension(file));

                                File.Move(DestinationPath + "\\" + DirName + "\\" + Path.GetFileNameWithoutExtension(file) + "_copy" + Path.GetExtension(file),
                                    DestinationPath + "\\" + DirName + "\\" + Path.GetFileName(file));
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

                        recSourcePath = SourcePath + "\\" + DirName;
                        recDestinationPath = DestinationPath + "\\" + DirName;
                        recDirname = Path.GetFileName(s);

                        CopyDirectory(recSourcePath, recDestinationPath, recDirname);
                    }

                }//end directories.Length!=0

                else
                {
                    return;
                }
            }//end SourcePath!=DestinationPath
        }

        private static int AddIcon(string format)
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

    }
}

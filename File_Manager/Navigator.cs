using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace navigator
{
    public delegate void Demonstrator(List<string> Content);
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


        public List<string> GetContent(string path)
        {
            //plane.Items.Clear();
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
                //plane.Items.Add(Path.GetFileName(directory), 1);

            }
            foreach (string file in files)
            {
                ContentOfCurrentDirectory.Add(file);
                //plane.Items.Add(Path.GetFileName(file), AddIcon(Path.GetExtension(file)));
            }
           return ContentOfCurrentDirectory;
        }

        public List<string> GetContent(int index)
        {
            //plane.Items.Clear();

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
                //plane.Items.Add(Path.GetFileName(directory), 1);

            }
            foreach (string file in files)
            {
                ContentOfCurrentDirectory.Add(file);
                //plane.Items.Add(Path.GetFileName(file), AddIcon(Path.GetExtension(file)));
            }
            return ContentOfCurrentDirectory;
        }


        public void Move(string SelectedName, string DestinationPath)
        {
            if (CurrentPath != DestinationPath)
            {

                //if (File.Exists(CurrentPath + "\\" + selectedItem.Name))
                if (File.Exists(CurrentPath+"\\"+SelectedName))
                {
                    try
                    {
                        File.Move(CurrentPath + "\\" + SelectedName, DestinationPath + "\\" + SelectedName);
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("File with similar name has already exist", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    }
                }
                //else if (Directory.Exists(CurrentPath + "\\" + selectedItem.Name))
                else if (Directory.Exists(CurrentPath + "\\" + SelectedName))
                {
                    try
                    {
                        Directory.Move(CurrentPath + "\\" + SelectedName, DestinationPath + "\\" + SelectedName);
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

        public void Delete(string SelectedText)
        {
            if (File.Exists(CurrentPath + "\\" + SelectedText))
            {
                try
                {
                    File.Delete(CurrentPath + "\\" + SelectedText);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Access denied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (Directory.Exists(CurrentPath + "\\" + SelectedText))
            {
                DirectoryInfo g = new DirectoryInfo(CurrentPath + "\\" + SelectedText);
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

        public void Copy(string SelectedName, string DestinationPath)
        {
            if (CurrentPath != DestinationPath)
            {
                if (File.Exists(CurrentPath + "\\" + SelectedName))
                {
                    if (!File.Exists(DestinationPath + Path.GetFileName(CurrentPath+"\\"+SelectedName)))
                    {
                        try
                        {
                            File.Copy(CurrentPath + "\\" + SelectedName, DestinationPath + "\\" + Path.GetFileNameWithoutExtension(CurrentPath + "\\" + SelectedName)
                                + "_copy" + Path.GetExtension(CurrentPath + "\\" + SelectedName));
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
                            File.Delete(DestinationPath + "\\" + Path.GetFileName(CurrentPath + "\\" + SelectedName));
                            File.Copy(CurrentPath + "\\" + SelectedName, DestinationPath + "\\" + Path.GetFileNameWithoutExtension(CurrentPath + "\\" + SelectedName)
                                + "_copy" + Path.GetExtension(CurrentPath + "\\" + SelectedName));
                            //File.Move(DestinationPath + FileName + "_copy" + FileFormat, DestinationPath + FileName + FileFormat);
                        }

                    }
                }
                else
                {
                    CopyDirectory(CurrentPath, DestinationPath, SelectedName);
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
                        Directory.CreateDirectory(DestinationPath +"\\"+ DirName + "\\" + Path.GetFileName(s));

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

        public static long GetDirectorySize(DirectoryInfo directoryInfo)
        {
            long size = 0;

            FileInfo[] files = directoryInfo.GetFiles();

            foreach(FileInfo file in files)
            {
                try
                {
                    size += file.Length;
                }
                //Данное исключение делается для пропуска папок к которым нет доступа
                catch (UnauthorizedAccessException)
                {
                    ;
                }
            }

            DirectoryInfo[] directories = directoryInfo.GetDirectories();

            if (directories.Length != 0)
            {
                foreach (DirectoryInfo directory in directories)
                {
                    try
                    {
                        size += GetDirectorySize(directory);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        ;
                    }
                }
            }
            else;
            return size;
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using File_Manager;
using System.Threading;

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


        //private static void CopyDirectory(object FromTo)
        //{
        //    Tonnel tonnel = (Tonnel)FromTo;
        //    if (tonnel.SourcePath != tonnel.DestinationPath)
        //    {
        //        string[] directories = Directory.GetDirectories(tonnel.SourcePath + "\\" + tonnel.SelectedFile);
        //        string[] files = Directory.GetFiles(tonnel.SourcePath + "\\" + tonnel.SelectedFile);

        //        if (files.Length != 0)
        //        {
        //            string FileName = string.Empty;
        //            string FileFormat = string.Empty;

        //            Directory.CreateDirectory(tonnel.DestinationPath + "\\" + tonnel.SelectedFile);

        //            foreach (string file in files)
        //            {
        //                if (!File.Exists(tonnel.DestinationPath + tonnel.SelectedFile + "\\" + FileName + "_copy" + FileFormat))
        //                {

        //                    try
        //                    {
        //                        File.Copy(tonnel.SourcePath + "\\" + tonnel.SelectedFile + "\\" + Path.GetFileName(file),
        //                            tonnel.DestinationPath + "\\" + tonnel.SelectedFile + "\\" + Path.GetFileNameWithoutExtension(file) + "_copy" + Path.GetExtension(file));

        //                        File.Move(tonnel.DestinationPath + "\\" + tonnel.SelectedFile + "\\" + Path.GetFileNameWithoutExtension(file) + "_copy" + Path.GetExtension(file),
        //                            tonnel.DestinationPath + "\\" + tonnel.SelectedFile + "\\" + Path.GetFileName(file));
        //                    }
        //                    catch (UnauthorizedAccessException)
        //                    {
        //                        MessageBox.Show("Access denied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                        return;
        //                    }

        //                }
        //                else
        //                {
        //                    continue;
        //                }
        //            }
        //        }
        //    }
        //}

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

        public static void CopyFile(object FromTo)
        {
            Tonnel tonnel = (Tonnel)FromTo;
            FileInfo file = new FileInfo(tonnel.SourcePath);
            byte[] buffer = new byte[file.Length];
            FileStream sourceStream = new FileStream(tonnel.SourcePath,FileMode.Open,FileAccess.Read);
            int countBytes=sourceStream.Read(buffer, 0, buffer.Length);
            FileStream destinationStream = new FileStream(tonnel.DestinationPath +"\\"+ Path.GetFileName(tonnel.SourcePath), FileMode.Create,FileAccess.Write);

            int progress = 0;
            int Quotient = 0;
            int Remainder = 0;
            int stopPoint = 0;

            if (countBytes < 1024)
            {
                destinationStream.Write(buffer, 0, countBytes);
            }
            else
            {
                Quotient = countBytes / 100;
                Remainder = countBytes % 100;
  
                for (int i = 0; i <= 100*Quotient; i += Quotient)
                {
                    destinationStream.Write(buffer, stopPoint, Quotient);
                    stopPoint = i;

                    ProgressChanged(progress);
                    progress++;
                }

                destinationStream.Write(buffer, stopPoint, Remainder);
            }
            ProcessCompleted();
            
        }
        public static event Action<int> ProgressChanged;
        public static event Action ProcessCompleted;
    }
}

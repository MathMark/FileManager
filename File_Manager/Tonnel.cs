using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace File_Manager
{
   public class Tonnel
    {
        public string SourcePath { get; private set; }
        public string DestinationPath { get; private set; }
        public string SelectedFile { get; private set; }
        public int value = 0;

        public Tonnel(string SelectedFile,string SourcePath,string DestinationPath)
        {
            this.SelectedFile = SelectedFile;
            this.SourcePath = SourcePath;
            this.DestinationPath = DestinationPath;
        }
    }
}

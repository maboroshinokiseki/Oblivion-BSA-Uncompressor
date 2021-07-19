using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OblivionBSAUncompressor
{
    class ProgressUIVariable
    {
        public int FileCount = 0;
        public int CurrentFileIndex = -1;
        public bool FileChanged = false;
        public string CurrentFileName = string.Empty;
        public int BlockCount = 0;
        public int CurrentBlockIndex = 0;

        public bool IsUpdating { get; private set; } = false;

        public void BeginUpdate()
        {
            IsUpdating = true;
        }

        public void EndUpdate()
        {
            IsUpdating = false;
        }
    }
}

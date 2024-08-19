using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace cjpc
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SetDataDirectory(); 
            Application.Run(new MainForm());
        }

        static void SetDataDirectory()
        {
            string dataDir = AppDomain.CurrentDomain.BaseDirectory;
            if (dataDir.LastIndexOf(@"\cjpc\") > 0)
            {
                int indexLastDir = dataDir.LastIndexOf(@"\cjpc\");
                //dataDir = System.IO.Directory.GetParent(dataDir).Parent.FullName;
                dataDir = dataDir.Substring(0, indexLastDir + 5);
                AppDomain.CurrentDomain.SetData("DataDirectory", dataDir);
            }
        }
    }
}

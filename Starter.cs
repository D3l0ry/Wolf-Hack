using System;
using System.Windows.Forms;

namespace Wolf_Hack
{
    class Starter
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Menu());
        }
    }
}
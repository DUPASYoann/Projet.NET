using Bacchus.BDD;
using Bacchus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bacchus
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SQLiteDAO SQLDAO = SQLiteDAO.Instance;
            SQLDAO.Export_To_Csv(@"C:\Users\kanti\Desktop\TP_NET\test.csv");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}

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
            SQLiteDAO DAO = SQLiteDAO.Instance;
            DAO.Empty_DB();
            DAO.Insert_From_Csv(@"D:\OneDrive - Université de Tours\Document\Polytech\4AS2\TC - Pl.NET\TP3\Données à intégrer.csv");
/*            Article article = new Article("1", "description", 1, 2, (float)3.01 , 5);
            Famille famille = new Famille("nomfamille");
            DAO.Insert_Article(article);
            DAO.Insert_Famille(famille);
*/
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());

        }
    }
}

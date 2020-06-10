using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacchus.Model
{
    public class Marque
    {
        public Marque()
        {
            ListeArticle = new List<Article>();
        }


        public Marque( string nom , int refMarque = 0)
        {
            ListeArticle = new List<Article>();
            RefMarque = refMarque;
            Nom = nom;
        }

        public int RefMarque { get; set; }
        public String Nom { get; set; }
        public List<Article> ListeArticle { get; set; }

    }
}

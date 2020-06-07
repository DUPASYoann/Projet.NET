using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacchus.Model
{
    class SousFamille
    {
        public SousFamille()
        {
            ListeArticle = new List<Article>();
        }

        public SousFamille(int refFamille, string nom, int refSousFamille = 0)
        {
            ListeArticle = new List<Article>();
            RefSousFamille = refSousFamille;
            RefFamille = refFamille;
            Nom = nom;
        }

        public int RefSousFamille { get; set; }
        public int RefFamille { get; set; }
        public String Nom { get; set; }
        public List<Article> ListeArticle { get; set; }
        public Famille Famille_Obj { get; set; }


    }
}

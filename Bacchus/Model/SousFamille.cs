using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacchus.Model
{
    public class SousFamille
    {
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public SousFamille()
        {
            ListeArticle = new List<Article>();
        }

        /// <summary>
        /// Constructeur de confort
        /// </summary>
        /// <param name="refFamille"></param>
        /// <param name="nom"></param>
        /// <param name="refSousFamille"></param>
        public SousFamille(int refFamille, string nom, int refSousFamille = 0)
        {
            ListeArticle = new List<Article>();
            RefSousFamille = refSousFamille;
            RefFamille = refFamille;
            Nom = nom;
        }
        //GETTERS ET SETTERS
        public int RefSousFamille { get; set; }
        public int RefFamille { get; set; }
        public String Nom { get; set; }
        public List<Article> ListeArticle { get; set; }
        public Famille Famille_Obj { get; set; }


    }
}

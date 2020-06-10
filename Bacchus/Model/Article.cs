using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacchus.Model
{
    public class Article
    {
        public Article()
        {
        }

        public Article(string refArticle, string description, int refSousFamille, int refMarque, float prixHT, int quantite)
        {
            RefArticle = refArticle;
            Description = description;
            RefSousFamille = refSousFamille;
            RefMarque = refMarque;
            PrixHT = prixHT;
            Quantite = quantite;
        }

        public string RefArticle { get; set; }
        public string Description { get; set; }
        public int RefSousFamille { get; set; }
        public int RefMarque { get; set; }
        public float PrixHT { get; set; }
        public int Quantite { get; set; }
        public SousFamille SousFamille_Obj { get; set; }
        public Marque Marque_Obj { get; set; }

        private Famille Famille_ObjPrivate;

        public Famille Famille_Obj
        {
            get { return SousFamille_Obj.Famille_Obj; }
            private set { Famille_ObjPrivate = value; }
        }


    }
}

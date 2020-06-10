using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacchus.Model
{
    public class Article
    {
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Article()
        {
        }

        /// <summary>
        /// Constructeur de confort
        /// </summary>
        /// <param name="refArticle"></param>
        /// <param name="description"></param>
        /// <param name="refSousFamille"></param>
        /// <param name="refMarque"></param>
        /// <param name="prixHT"></param>
        /// <param name="quantite"></param>
        public Article(string refArticle, string description, int refSousFamille, int refMarque, float prixHT, int quantite)
        {
            RefArticle = refArticle;
            Description = description;
            RefSousFamille = refSousFamille;
            RefMarque = refMarque;
            PrixHT = prixHT;
            Quantite = quantite;
        }
        //GETTERS ET SETTERS
        public string RefArticle { get; set; }
        public string Description { get; set; }
        public int RefSousFamille { get; set; }
        public int RefMarque { get; set; }
        public float PrixHT { get; set; }
        public int Quantite { get; set; }
        public SousFamille SousFamille_Obj { get; set; }
        public Marque Marque_Obj { get; set; }
        public Famille Famille_Obj => SousFamille_Obj.Famille_Obj; 
        }


    }
}

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
        private SousFamille SousFamille_Obj_Priv;

        public SousFamille SousFamille_Obj
        {
            get { return SousFamille_Obj_Priv; }
            set {
                SousFamille_Obj_Priv = value;
                RefSousFamille = SousFamille_Obj.RefSousFamille;
            }
        }

        private Marque Marque_Obj_Priv;

        public Marque Marque_Obj
        {
            get { return Marque_Obj_Priv; }
            set
            {
                Marque_Obj_Priv = value;
                RefMarque = Marque_Obj.RefMarque;
            }
        }

        public Famille Famille_Obj => (SousFamille_Obj == null ?  null : SousFamille_Obj.Famille_Obj); 


    }
}

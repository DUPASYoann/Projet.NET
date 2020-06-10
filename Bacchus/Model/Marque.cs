using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacchus.Model
{
    public class Marque
    {
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Marque()
        {
            ListeArticle = new List<Article>();
        }

        /// <summary>
        /// Constructeur de confort
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="refMarque"></param>
        public Marque( string nom , int refMarque = 0)
        {
            ListeArticle = new List<Article>();
            RefMarque = refMarque;
            Nom = nom;
        }
        //GETTERS ET SETTERS
        public int RefMarque { get; set; }
        public String Nom { get; set; }
        public List<Article> ListeArticle { get; set; }

    }
}

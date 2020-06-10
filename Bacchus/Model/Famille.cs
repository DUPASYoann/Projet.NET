using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacchus.Model
{
    public class Famille
    {
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Famille()
        {
            ListesSousFamille = new List<SousFamille>();
        }

        /// <summary>
        /// Constructeur de confort
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="refFamille"></param>
        public Famille( string nom, int refFamille = 0)
        {
            ListesSousFamille = new List<SousFamille>();
            RefFamille = refFamille;
            Nom = nom;
        }
        //GETTERS ET SETTERS
        public int RefFamille { get; set; }
        public String Nom { get; set; }
        public List<SousFamille> ListesSousFamille { get; set; }


    }
}

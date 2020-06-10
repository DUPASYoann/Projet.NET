using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacchus.Model
{
    public class Famille
    {
        public Famille()
        {
            ListesSousFamille = new List<SousFamille>();
        }

        public Famille( string nom, int refFamille = 0)
        {
            ListesSousFamille = new List<SousFamille>();
            RefFamille = refFamille;
            Nom = nom;
        }

        public int RefFamille { get; set; }
        public String Nom { get; set; }
        public List<SousFamille> ListesSousFamille { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacchus.Model
{
    class Famille
    {
        public Famille()
        {
        }

        public Famille( string nom, int refFamille = 0)
        {
            RefFamille = refFamille;
            Nom = nom;
        }

        public int RefFamille { get; set; }
        public String Nom { get; set; }

    }
}

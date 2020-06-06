using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacchus.Model
{
    class Marque
    {
        public Marque()
        {
        }

        public Marque( string nom , int refMarque = 0)
        {
            RefMarque = refMarque;
            Nom = nom;
        }

        public int RefMarque { get; set; }
        public String Nom { get; set; }
    }
}

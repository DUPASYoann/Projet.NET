using Bacchus.BDD;
using Bacchus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacchus.ViewModel
{
    class ModelManager
    {
        public List<Article> ListeArticles { get; set; }
        public List<Famille> ListeFamilles { get; set; }
        public List<SousFamille> ListeSousFamilles { get; set; }
        public List<Marque> ListeMarques { get; set; }

        public ModelManager()
        {
            SQLiteDAO SQLiteDAO_Obj = SQLiteDAO.Instance;
            ListeFamilles = SQLiteDAO_Obj.GetAll_Famille();
            ListeSousFamilles = SQLiteDAO_Obj.GetAll_SousFamille();
            ConnectFamille();
            ListeMarques = SQLiteDAO_Obj.GetAll_Marque();
            ListeArticles = SQLiteDAO_Obj.GetAll_Article();
            ConnectArticles();
        }
        /// <summary>
        /// Remplir les listes de sous famille des familles
        /// </summary>
        private void ConnectFamille()
        {
            foreach (Famille Famille_Obj in ListeFamilles)
            {
                foreach (SousFamille SousFamille_Obj in ListeSousFamilles)
                {
                    if (SousFamille_Obj.RefFamille == Famille_Obj.RefFamille)
                    {
                        Famille_Obj.ListesSousFamille.Add(SousFamille_Obj);
                        SousFamille_Obj.Famille_Obj = Famille_Obj;
                    }
                }
            }
        }
        /// <summary>
        /// Remplir les listes d'articles des sous familles
        /// </summary>
        private void ConnectArticles()
        {
            foreach (Article Article_Obj in ListeArticles)
            {
                foreach (SousFamille SousFamille_Obj in ListeSousFamilles)
                {
                    if (Article_Obj.RefSousFamille == SousFamille_Obj.RefSousFamille)
                    {
                        SousFamille_Obj.ListeArticle.Add(Article_Obj);
                        Article_Obj.SousFamille_Obj = SousFamille_Obj;
                    }
                }
                foreach (Marque Marque_Obj in ListeMarques)
                {
                    if (Article_Obj.RefMarque == Marque_Obj.RefMarque)
                    {
                        Marque_Obj.ListeArticle.Add(Article_Obj);
                        Article_Obj.Marque_Obj = Marque_Obj;
                    }
                }
            }
        }

        /// <summary>
        /// Charger tout les articles d'une marque dans la liste d'articles de ModelManager
        /// </summary>
        /// <param name="Marque_Obj"></param>
        public void LoadArticleFromSource(Marque Marque_Obj)
        {
            List<Article> ListeArticles = SQLiteDAO.Instance.GetAll_Article_From_Marque(Marque_Obj);
            ConnectArticles();
           
        }

        /// <summary>
        /// Charger tout les articles d'une sous famille dans la liste d'articles de ModelManager
        /// </summary>
        /// <param name="SousFamille_Obj">L'instance de la sous famille</param>
        public void LoadArticleFromSource(SousFamille SousFamille_Obj)
        {
            List<Article> ListeArticles = SQLiteDAO.Instance.GetAll_Article_From_SousFamille(SousFamille_Obj);
            ConnectArticles();
        }
    }
}

using Bacchus.BDD;
using Bacchus.Model;
using Bacchus.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bacchus.View
{
    public partial class FormArticle : Form
    {
        private String Type;

        public Article Article_Obj { get; set; }

        public FormArticle()
        {
            InitializeComponent();
        }

        public FormArticle(Article Tag, ModelManager ModelManager_Obj, String ButtonTexte)
        {
            InitializeComponent();

            this.Type = ButtonTexte;

            this.comboBox1.DataSource = null;
            this.comboBox2.DataSource = null;
            this.comboBox3.DataSource = null;

            this.modelManagerBindingSource.DataSource = ModelManager_Obj;
            this.articleBindingSource.DataSource = Tag;
            
            this.comboBox1.DataSource = this.listeFamillesBindingSource;
            this.comboBox1.DisplayMember = "Nom";
            this.comboBox1.SelectedItem = (Tag.Famille_Obj == null) ? ModelManager_Obj.ListeFamilles[0] : Tag.Famille_Obj;
            
            this.SousFamilleBinding.DataSource = ((Famille)this.comboBox1.SelectedItem).ListesSousFamille;
            this.comboBox2.DataSource = this.SousFamilleBinding;
            this.comboBox2.DisplayMember = "Nom";
            this.comboBox2.SelectedItem = (Tag.SousFamille_Obj == null) ? ModelManager_Obj.ListeFamilles[0].ListesSousFamille[0] : Tag.SousFamille_Obj;

            this.comboBox3.DataSource = this.listeMarquesBindingSource;
            this.comboBox3.DisplayMember = "Nom";
            this.comboBox3.SelectedItem = (Tag.Marque_Obj == null) ? ModelManager_Obj.ListeMarques[0] : Tag.Marque_Obj;

            if (ButtonTexte == "Modifier")
            {
                this.textBox1.Enabled = false;
            }

            Article_Obj = Tag;
        }

        private void ComboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.SousFamilleBinding.DataSource = ((Famille)this.comboBox1.SelectedItem).ListesSousFamille;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if(this.Type == "Modifier")
            {
                //REGEX
                try
                {
                    Debug.Print(Article_Obj.Description);
                    SQLiteDAO.Instance.Update_Article(Article_Obj);
                    this.Close();
                }
                catch (Exception Exception)
                {

                    MessageBox.Show("IMPOSSIBLE DE MODIFIER L'ARTICLE : " + Exception.Message);
                }

            }

            if (this.Type == "Ajouter")
            {
                try
                {
                    SQLiteDAO.Instance.Insert_Article(Article_Obj);
                    this.Close();
                }
                catch (Exception Exception)
                {
                    MessageBox.Show("IMPOSSIBLE D'AJOUTER L'ARTICLE : " + Exception.Message);
                }

            }

        }
    }
}

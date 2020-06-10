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
        public Article Article_Obj { get; set; }

        public FormArticle()
        {
            InitializeComponent();
        }

        public FormArticle(Article Tag, ModelManager ModelManager_Obj, String ButtonTexte)
        {
            InitializeComponent();
            this.comboBox1.DataSource = null;
            this.comboBox2.DataSource = null;
            this.comboBox3.DataSource = null;

            this.modelManagerBindingSource.DataSource = ModelManager_Obj;
            this.articleBindingSource.DataSource = Tag;
            
            this.comboBox1.DataSource = this.listeFamillesBindingSource;
            this.comboBox1.DisplayMember = "Nom";
            this.comboBox1.SelectedItem = Tag.Famille_Obj;

            this.SousFamilleBinding.DataSource = ((Famille)this.comboBox1.SelectedItem).ListesSousFamille;
            this.comboBox2.DataSource = this.SousFamilleBinding;
            this.comboBox2.DisplayMember = "Nom";
            this.comboBox2.SelectedItem = Tag.SousFamille_Obj;

            this.comboBox3.DataSource = this.listeMarquesBindingSource;
            this.comboBox3.DisplayMember = "Nom";
            this.comboBox3.SelectedItem = Tag.Marque_Obj;

            Article_Obj = Tag;
        }

        private void ComboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.SousFamilleBinding.DataSource = ((Famille)this.comboBox1.SelectedItem).ListesSousFamille;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Verifier les données dans les champs
            //Si valide quitter la fenetre
            //Sinon mettre un message d'erreur via messagebox
        }
    }
}

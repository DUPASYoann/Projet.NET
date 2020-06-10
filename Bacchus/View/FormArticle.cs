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
        public FormArticle()
        {
            InitializeComponent();
        }

        public FormArticle(Article Tag, ModelManager ModelManager_Obj)
        {
            InitializeComponent();
            this.modelManagerBindingSource.DataSource = ModelManager_Obj;
            this.articleBindingSource.DataSource = Tag;
            this.SousFamilleBinding.DataSource = ((Famille)this.comboBox1.SelectedItem).ListesSousFamille;
            this.comboBox2.DisplayMember = "Nom";
            this.comboBox2.SelectedItem = Tag.SousFamille_Obj;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.SousFamilleBinding.DataSource = ((Famille)this.comboBox1.SelectedItem).ListesSousFamille;
        }
    }
}

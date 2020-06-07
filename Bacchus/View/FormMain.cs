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

namespace Bacchus
{
    public partial class FormMain : Form
    {
        private ModelManager ModelManager_obj;

        public FormMain()
        {
            InitializeComponent();
            ModelManager_obj = new ModelManager();
            LoadTreeView();

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void importerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormImporter FormImporter_obj = new FormImporter();
            FormImporter_obj.StartPosition = FormStartPosition.CenterParent;
            FormImporter_obj.ShowDialog(this);
        }

        private void exporterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog SaveFileDialog_obj = new SaveFileDialog())
            {
                SaveFileDialog_obj.InitialDirectory = "c:\\";
                SaveFileDialog_obj.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                SaveFileDialog_obj.FilterIndex = 1;
                SaveFileDialog_obj.RestoreDirectory = true;

                if (SaveFileDialog_obj.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string FilePath = SaveFileDialog_obj.FileName;
                    SQLiteDAO SQLiteDAO_obj = SQLiteDAO.Instance;
                    SQLiteDAO_obj.Export_To_Csv(FilePath);

                }
            }
        }

        private void LoadTreeView()
        {
            this.treeView1.Nodes[0].Tag = ModelManager_obj.ListeArticles;

            this.treeView1.Nodes[1].Tag = ModelManager_obj.ListeFamilles;
            foreach (Famille Famille_Obj in ModelManager_obj.ListeFamilles)
            {
                TreeNode CurrentNode = this.treeView1.Nodes[1].Nodes.Add(Famille_Obj.Nom);
                CurrentNode.Tag = Famille_Obj.ListesSousFamille;
                foreach (SousFamille SousFamille_Obj in Famille_Obj.ListesSousFamille)
                {
                    CurrentNode.Nodes.Add(SousFamille_Obj.Nom).Tag = SousFamille_Obj.ListeArticle;
                }
            }

            foreach (Marque Marque_Obj in ModelManager_obj.ListeMarques)
            {   
                this.treeView1.Nodes[2].Nodes.Add(Marque_Obj.Nom);
            }
         
        }

        private void LoadListViewFamille(List<ListViewItem> List)
        {

            this.listView1.Columns.Clear();
            this.listView1.Columns.Add("Description");

            this.listView1.Items.Clear();
            foreach (ListViewItem Item in List)
            {
                this.listView1.Items.Add(Item);
            }

            this.listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void LoadListViewArticle(List<ListViewItem> List)
        {

            this.listView1.Columns.Clear();
            this.listView1.Columns.Add("Description");
            this.listView1.Columns.Add("Familles");
            this.listView1.Columns.Add("Famille");
            this.listView1.Columns.Add("Marques");
            this.listView1.Columns.Add("Quantité");

            this.listView1.Items.Clear();
            foreach (ListViewItem Item in List)
            {
                this.listView1.Items.Add(Item);
            }
            this.listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Object Tag = this.treeView1.SelectedNode.Tag;

            if (Tag is List<Famille>)
            {
                this.listView1.BeginUpdate();
                List<ListViewItem> List = new List<ListViewItem>();
                foreach (Famille FamilleItem in (List<Famille>)Tag)
                {
                    List.Add(new ListViewItem(FamilleItem.Nom.ToString()));
                }
                LoadListViewFamille(List);
                this.listView1.EndUpdate();
            }

            if (Tag is List<Marque>)
            {
                this.listView1.BeginUpdate();
                List<ListViewItem> List = new List<ListViewItem>();
                foreach (Marque FamilleItem in (List<Marque>)Tag)
                {
                    List.Add(new ListViewItem(FamilleItem.Nom.ToString()));
                }
                LoadListViewFamille(List);
                this.listView1.EndUpdate();
            }

            if (Tag is List<SousFamille>)
            {
                this.listView1.BeginUpdate();
                List<ListViewItem> List = new List<ListViewItem>();
                foreach (SousFamille FamilleItem in (List<SousFamille>)Tag)
                {
                    List.Add(new ListViewItem(FamilleItem.Nom.ToString()));
                }
                LoadListViewFamille(List);
                this.listView1.EndUpdate();
            }

            if (Tag is List<Article>)
            {
                this.listView1.BeginUpdate();

                List<ListViewItem> List = new List<ListViewItem>();
                foreach (Article FamilleItem in (List<Article>)Tag)
                {
                    ListViewItem ListItem = new ListViewItem(FamilleItem.Description);
                    ListItem.SubItems.Add(FamilleItem.SousFamille_Obj.Famille_Obj.Nom);
                    ListItem.SubItems.Add(FamilleItem.SousFamille_Obj.Nom);
                    ListItem.SubItems.Add(FamilleItem.Marque_Obj.Nom);
                    ListItem.SubItems.Add(FamilleItem.Quantite.ToString());

                    List.Add(ListItem);
                }

                LoadListViewArticle(List);
                this.listView1.EndUpdate();
            }
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:

                    break;

                case Keys.F5:

                    break;

                case Keys.Delete:

                    break;

                default:
                    break;
            }
        }
    }
}

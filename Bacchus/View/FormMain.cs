using Bacchus.BDD;
using Bacchus.Model;
using Bacchus.View;
using Bacchus.ViewModel;
using System;
using System.Collections;
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
        private ModelManager ModelManager_Obj;
        private List<ListViewGroup> ListViewGroupDescription = new List<ListViewGroup>();
        private List<ListViewGroup> ListViewGroupFamille;
        private List<ListViewGroup> ListViewGroupSousFamille;
        private List<ListViewGroup> ListViewGroupMarque;
        private List<ListViewGroup> CurrentListViewGroup;

        public FormMain()
        {
            ModelManager_Obj = new ModelManager();
            InitializeComponent();
            LoadTreeView();
            LoadListViewGroupDescription();
            LoadListViewGroupFamille();
            LoadListViewGroupSousFamille();
            LoadListViewGroupMarque();
        }

        /// <summary>
        /// Evenement appelant la fenêtre modale importer CSV 
        /// lorsque nous cliquons sur importer dans le menu toolStrip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImporterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormImporter FormImporter_obj = new FormImporter();
            FormImporter_obj.StartPosition = FormStartPosition.CenterParent;
            FormImporter_obj.ShowDialog(this);
        }

        /// <summary>
        /// Evenement appelant la fenetre modale exporter lorsque 
        /// nous cliquons sur exporter dans le menu toolStrip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExporterToolStripMenuItem_Click(object sender, EventArgs e)
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

        /// <summary>
        /// Charge et met met à jour le treeView (partie de gauche)
        /// </summary>
        private void LoadTreeView()
        {
            this.treeView1.Nodes[0].Tag = ModelManager_Obj.ListeArticles;

            this.treeView1.Nodes[1].Tag = ModelManager_Obj.ListeFamilles;
            foreach (Famille Famille_Obj in ModelManager_Obj.ListeFamilles)
            {
                TreeNode CurrentNode = this.treeView1.Nodes[1].Nodes.Add(Famille_Obj.Nom);
                CurrentNode.Tag = Famille_Obj.ListesSousFamille;
                foreach (SousFamille SousFamille_Obj in Famille_Obj.ListesSousFamille)
                {
                    CurrentNode.Nodes.Add(SousFamille_Obj.Nom).Tag = SousFamille_Obj.ListeArticle;
                }
            }

            this.treeView1.Nodes[2].Tag = ModelManager_Obj.ListeMarques;
            foreach (Marque Marque_Obj in ModelManager_Obj.ListeMarques)
            {   
                this.treeView1.Nodes[2].Nodes.Add(Marque_Obj.Nom).Tag = Marque_Obj.ListeArticle;
            }
         
        }

        /// <summary>
        /// Charge et met à jour la listView famille (partie de droite)
        /// </summary>
        /// <param name="List"></param>
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

        /// <summary>
        /// Charge et met à jour la listView article (partie de droite)
        /// </summary>
        /// <param name="List"></param>
        private void LoadListViewArticle(List<ListViewItem> List)
        {
            this.listView1.Columns.Clear();
            this.listView1.Columns.Add("Description");
            this.listView1.Columns.Add("Familles");
            this.listView1.Columns.Add("SousFamille");
            this.listView1.Columns.Add("Marques");
            this.listView1.Columns.Add("Quantité");

            this.listView1.Items.Clear();
            foreach (ListViewItem Item in List)
            {
                this.listView1.Items.Add(Item);
            }
            this.listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        /// <summary>
        /// Permet de charger les elements qui corresponde à l'élément selectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Object Tag = this.treeView1.SelectedNode.Tag;

            if (Tag is List<Famille>)
            {
                this.listView1.BeginUpdate();
                List<ListViewItem> List = new List<ListViewItem>();
                foreach (Famille FamilleItem in (List<Famille>)Tag)
                {
                    ListViewItem ListViewItem_Obj = new ListViewItem(FamilleItem.Nom.ToString());
                    ListViewItem_Obj.Tag = FamilleItem;
                    List.Add(ListViewItem_Obj);
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
                    ListViewItem ListViewItem_Obj = new ListViewItem(FamilleItem.Nom.ToString());
                    ListViewItem_Obj.Tag = FamilleItem;
                    List.Add(ListViewItem_Obj);
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
                    ListViewItem ListViewItem_Obj = new ListViewItem(FamilleItem.Nom.ToString());
                    ListViewItem_Obj.Tag = FamilleItem;
                    List.Add(ListViewItem_Obj);
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
                    ListViewItem ListItem = new ListViewItem(FamilleItem.Description,GetGroupFromCurrentListViewGroupArticle(FamilleItem));
                    ListItem.SubItems.Add(FamilleItem.SousFamille_Obj.Famille_Obj.Nom);
                    ListItem.SubItems.Add(FamilleItem.SousFamille_Obj.Nom);
                    ListItem.SubItems.Add(FamilleItem.Marque_Obj.Nom);
                    ListItem.SubItems.Add(FamilleItem.Quantite.ToString());
                    ListItem.Tag = FamilleItem;

                    List.Add(ListItem);
                }

                LoadListViewArticle(List);
                this.listView1.EndUpdate();
            }
        }
        /// <summary>
        /// Appelle les actions liées  au raccourci clavier ENTREE, F5 et SUPPR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    ModifierToolStripMenuItem_Click(null,null);
                    break;

                case Keys.F5:
                    ActualiserToolStripMenuItem_Click(null, null);
                    break;

                case Keys.Delete:
                    SupprimerToolStripMenuItem_Click(null, null);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            switch (e.Column)
            {
                case 0:
                    CurrentListViewGroup = ListViewGroupDescription;
                    this.listView1.ListViewItemSorter = new ListViewItemComparer(e.Column);
                    break;

                case 1:
                    CurrentListViewGroup = ListViewGroupFamille;
                    this.listView1.ListViewItemSorter = new ListViewItemComparer(e.Column);
                    break;

                case 2:
                    CurrentListViewGroup = ListViewGroupSousFamille;
                    this.listView1.ListViewItemSorter = new ListViewItemComparer(e.Column);
                    break;

                case 3:
                    CurrentListViewGroup = ListViewGroupMarque;
                    this.listView1.ListViewItemSorter = new ListViewItemComparer(e.Column);
                    break;

                default:
                    this.listView1.ListViewItemSorter = null;
                    break;
            }


            listView1.Sorting = (listView1.Sorting == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            this.listView1.Sort();

            this.listView1.Groups.Clear();
            
            if (CurrentListViewGroup != null) {
                ListViewGroup[] groups = new ListViewGroup[CurrentListViewGroup.Count];
                CurrentListViewGroup.CopyTo(groups, 0);
                Array.Sort(groups, new GroupComparer());
                if (listView1.Sorting == SortOrder.Descending) Array.Reverse(groups);
                this.listView1.Groups.AddRange(groups);
            }

            TreeView1_AfterSelect(null, null);
        }

        private void LoadListViewGroupDescription()
        {
            ListViewGroupDescription.Add(new ListViewGroup("A", "A"));
            ListViewGroupDescription.Add(new ListViewGroup("B", "B"));
            ListViewGroupDescription.Add(new ListViewGroup("C", "C"));
            ListViewGroupDescription.Add(new ListViewGroup("D", "D"));
            ListViewGroupDescription.Add(new ListViewGroup("E", "E"));
            ListViewGroupDescription.Add(new ListViewGroup("F", "F"));
            ListViewGroupDescription.Add(new ListViewGroup("G", "G"));
            ListViewGroupDescription.Add(new ListViewGroup("H", "H"));
            ListViewGroupDescription.Add(new ListViewGroup("I", "I"));
            ListViewGroupDescription.Add(new ListViewGroup("J", "J"));
            ListViewGroupDescription.Add(new ListViewGroup("K", "K"));
            ListViewGroupDescription.Add(new ListViewGroup("L", "L"));
            ListViewGroupDescription.Add(new ListViewGroup("M", "M"));
            ListViewGroupDescription.Add(new ListViewGroup("N", "N"));
            ListViewGroupDescription.Add(new ListViewGroup("O", "O"));
            ListViewGroupDescription.Add(new ListViewGroup("P", "P"));
            ListViewGroupDescription.Add(new ListViewGroup("Q", "Q"));
            ListViewGroupDescription.Add(new ListViewGroup("R", "R"));
            ListViewGroupDescription.Add(new ListViewGroup("S", "S"));
            ListViewGroupDescription.Add(new ListViewGroup("T", "T"));
            ListViewGroupDescription.Add(new ListViewGroup("U", "U"));
            ListViewGroupDescription.Add(new ListViewGroup("V", "V"));
            ListViewGroupDescription.Add(new ListViewGroup("W", "W"));
            ListViewGroupDescription.Add(new ListViewGroup("X", "X"));
            ListViewGroupDescription.Add(new ListViewGroup("Y", "Y"));
            ListViewGroupDescription.Add(new ListViewGroup("Z", "Z"));


        }

        /// <summary>
        /// Charge et met à jour la listView pour les groupes famille
        /// </summary>
        private void LoadListViewGroupFamille()
        {
            ListViewGroupFamille = new List<ListViewGroup>();

            foreach (Famille Famille_Obj in ModelManager_Obj.ListeFamilles)
            {
                ListViewGroupFamille.Add(new ListViewGroup(Famille_Obj.Nom, Famille_Obj.Nom));
            }
        }
        /// <summary>
        /// Charge et met à jour la listView pour les groupes des sous familles
        /// </summary>
        private void LoadListViewGroupSousFamille()
        {
            ListViewGroupSousFamille = new List<ListViewGroup>();

            foreach (SousFamille SousFamille_Obj in ModelManager_Obj.ListeSousFamilles)
            {
                ListViewGroupSousFamille.Add(new ListViewGroup(SousFamille_Obj.Nom, SousFamille_Obj.Nom));
            }
        }

        /// <summary>
        /// Charge et met à jour la listView pour les groupes des marques 
        /// </summary>
        private void LoadListViewGroupMarque()
        {
            ListViewGroupMarque = new List<ListViewGroup>();

            foreach (Marque Marque_Obj in ModelManager_Obj.ListeMarques)
            {
                ListViewGroupMarque.Add(new ListViewGroup(Marque_Obj.Nom, Marque_Obj.Nom));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Article_Obj"></param>
        /// <returns></returns>
        private ListViewGroup GetGroupFromCurrentListViewGroupArticle(Article Article_Obj)
        {
            ListViewGroup Result = null;

            if (CurrentListViewGroup == ListViewGroupDescription)
            {
                foreach (ListViewGroup ListViewGroup_Obj in ListViewGroupDescription)
                {
                    if (Article_Obj.Description.StartsWith(ListViewGroup_Obj.Name))
                    {
                        Result = ListViewGroup_Obj;
                    }
                }
            }

            if (CurrentListViewGroup == ListViewGroupFamille)
            {
                foreach (ListViewGroup ListViewGroup_Obj in ListViewGroupFamille)
                {
                    if (Article_Obj.SousFamille_Obj.Famille_Obj.Nom.StartsWith(ListViewGroup_Obj.Name))
                    {
                        Result = ListViewGroup_Obj;
                    }
                }
            }

            if (CurrentListViewGroup == ListViewGroupSousFamille)
            {
                foreach (ListViewGroup ListViewGroup_Obj in ListViewGroupSousFamille)
                {
                    if (Article_Obj.SousFamille_Obj.Nom.StartsWith(ListViewGroup_Obj.Name))
                    {
                        Result = ListViewGroup_Obj;
                    }
                }
            }

            if (CurrentListViewGroup == ListViewGroupMarque)
            {
                foreach (ListViewGroup ListViewGroup_Obj in ListViewGroupMarque)
                {
                    if (Article_Obj.Marque_Obj.Nom.StartsWith(ListViewGroup_Obj.Name))
                    {
                        Result = ListViewGroup_Obj;
                    }
                }
            }

            return Result;
        }

        class ListViewItemComparer : IComparer
        {
            private int col;
            public ListViewItemComparer()
            {
                col = 0;
            }
            public ListViewItemComparer(int column)
            {
                col = column;
            }
            public int Compare(object x, object y)
            {
                return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
            }
        }

        class GroupComparer : IComparer
        {
            public int Compare(object objA, object objB)
            {
                return ((ListViewGroup)objA).Header.CompareTo(((ListViewGroup)objB).Header);
            }
        }

        private void ActualiserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelManager_Obj.Refresh();
            LoadTreeView();
            TreeView1_AfterSelect(null,null);
        }

        private void AjouterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Article Article_Obj = new Article();
            FormArticle FormArticler_Obj = new FormArticle(Article_Obj, ModelManager_Obj, "Ajouter");
            FormArticler_Obj.StartPosition = FormStartPosition.CenterParent;
            FormArticler_Obj.ShowDialog(this);
            ActualiserToolStripMenuItem_Click(null, null);

        }

        private void ModifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems[0].Tag is Article)
            {
                FormArticle FormArticler_Obj = new FormArticle((Article)this.listView1.SelectedItems[0].Tag, ModelManager_Obj, "Modifier");
                FormArticler_Obj.StartPosition = FormStartPosition.CenterParent;
                FormArticler_Obj.ShowDialog(this);
                ActualiserToolStripMenuItem_Click(null, null);
            }

        }

        private void SupprimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems[0].Tag is Article)
            {
                DialogResult result = MessageBox.Show("Voulez-vous supprimer cette article", "Suppression", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    SQLiteDAO.Instance.Delete_Article((Article)this.listView1.SelectedItems[0].Tag);
                    ModelManager_Obj.Refresh();
                    ActualiserToolStripMenuItem_Click(null, null);
                }
            }
            
        }

        private void ContextMenuStrip1_Opened(object sender, EventArgs e)
        {
            this.contextMenuStrip1.Items[1].Enabled = (this.listView1.SelectedItems.Count != 0)? true : false;
            this.contextMenuStrip1.Items[2].Enabled = (this.listView1.SelectedItems.Count != 0)? true : false;
        }
    }
}

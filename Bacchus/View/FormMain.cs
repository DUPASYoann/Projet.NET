using Bacchus.BDD;
using Bacchus.Model;
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
        private ModelManager ModelManager_obj;
        private List<ListViewGroup> ListViewGroupDescription = new List<ListViewGroup>();
        private List<ListViewGroup> ListViewGroupFamille;
        private List<ListViewGroup> ListViewGroupSousFamille;
        private List<ListViewGroup> ListViewGroupMarque;
        private List<ListViewGroup> CurrentListViewGroup;

        public FormMain()
        {
            ModelManager_obj = new ModelManager();
            InitializeComponent();
            LoadTreeView();
            LoadListViewGroupDescription();
            LoadListViewGroupFamille();
            LoadListViewGroupSousFamille();
            LoadListViewGroupMarque();
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

            this.treeView1.Nodes[2].Tag = ModelManager_obj.ListeMarques;
            foreach (Marque Marque_Obj in ModelManager_obj.ListeMarques)
            {   
                this.treeView1.Nodes[2].Nodes.Add(Marque_Obj.Nom).Tag = Marque_Obj.ListeArticle;
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
                    ListViewItem ListItem = new ListViewItem(FamilleItem.Description,GetGroupFromCurrentListViewGroupArticle(FamilleItem));
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

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            switch (e.Column)
            {
                case 0:
                    CurrentListViewGroup = ListViewGroupDescription;
                    break;

                case 1:
                    CurrentListViewGroup = ListViewGroupFamille;
                    break;

                case 2:
                    CurrentListViewGroup = ListViewGroupSousFamille;
                    break;

                case 3:
                    CurrentListViewGroup = ListViewGroupMarque;
                    break;

                default:

                    break;
            }


            listView1.Sorting = (listView1.Sorting == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            this.listView1.Sort();

            this.listView1.Groups.Clear();
            this.listView1.ListViewItemSorter = new ListViewItemComparer(e.Column);
            ListViewGroup[] groups = new ListViewGroup[CurrentListViewGroup.Count];
            CurrentListViewGroup.CopyTo(groups, 0);
            Array.Sort(groups, new GroupComparer());
            this.listView1.Groups.AddRange(groups);

            treeView1_AfterSelect(null, null);
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

        private void LoadListViewGroupFamille()
        {
            ListViewGroupFamille = new List<ListViewGroup>();

            foreach (Famille Famille_Obj in ModelManager_obj.ListeFamilles)
            {
                ListViewGroupFamille.Add(new ListViewGroup(Famille_Obj.Nom, Famille_Obj.Nom));
            }
        }

        private void LoadListViewGroupSousFamille()
        {
            ListViewGroupSousFamille = new List<ListViewGroup>();

            foreach (SousFamille SousFamille_Obj in ModelManager_obj.ListeSousFamilles)
            {
                ListViewGroupSousFamille.Add(new ListViewGroup(SousFamille_Obj.Nom, SousFamille_Obj.Nom));
            }
        }

        private void LoadListViewGroupMarque()
        {
            ListViewGroupMarque = new List<ListViewGroup>();

            foreach (Marque Marque_Obj in ModelManager_obj.ListeMarques)
            {
                ListViewGroupMarque.Add(new ListViewGroup(Marque_Obj.Nom, Marque_Obj.Nom));
            }
        }

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
    }
}

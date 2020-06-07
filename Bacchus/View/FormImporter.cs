using Bacchus.BDD;
using System;
using System.Windows.Forms;

namespace Bacchus
{
    public partial class FormImporter : Form
    {
        private String FilePath = string.Empty;
        private String FileName = string.Empty;

        public FormImporter()
        {
            InitializeComponent();
        }

        private void Importer_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog OpenFileDialog_obj = new OpenFileDialog())
            {
                OpenFileDialog_obj.InitialDirectory = "c:\\";
                OpenFileDialog_obj.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                OpenFileDialog_obj.FilterIndex = 1;
                OpenFileDialog_obj.RestoreDirectory = true;

                if (OpenFileDialog_obj.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    FilePath = OpenFileDialog_obj.FileName;

                    //Get file name only
                    //FileName = System.IO.Path.GetFileNameWithoutExtension(FilePath);
                    FileName = System.IO.Path.GetFileName(FilePath);
                    this.Label_Fichier.Text = FileName;
                }
            }
            
        }

        private void Ajouter_Click(object sender, EventArgs e)
        {
            if (FilePath == String.Empty)
            {
                MessageBox.Show("Veuillez selectionnez un fichier .csv");
            }
            else
            {
                SQLiteDAO SQLiteDAO_Obj = SQLiteDAO.Instance;
                sQLiteDAOBindingSource.DataSource = SQLiteDAO_Obj;

                SQLiteDAO_Obj.Insert_From_Csv(FilePath);
                MessageBox.Show("Nombre de ligne ajouter à  la BDD : " + SQLiteDAO_Obj.Value);
            }
        }

        private void Ecrasement_Click(object sender, EventArgs e)
        {
            if (FilePath == String.Empty)
            {
                MessageBox.Show("Veuillez selectionnez un fichier .csv");
            }
            else
            {
                SQLiteDAO SQLiteDAO_Obj = SQLiteDAO.Instance;
                sQLiteDAOBindingSource.DataSource = SQLiteDAO_Obj;

                SQLiteDAO_Obj.Empty_DB();
                SQLiteDAO_Obj.Insert_From_Csv(FilePath);
                MessageBox.Show("Nombre de ligne ajouter à  la BDD : " + SQLiteDAO_Obj.Value);
            }
        }

        private void ProgressBar_Click(object sender, EventArgs e)
        {

        }

        private void sQLiteDAOBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}

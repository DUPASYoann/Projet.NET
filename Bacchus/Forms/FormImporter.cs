using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bacchus
{
    public partial class FormImporter : Form
    {
        private String FileContent = string.Empty;
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

                    //Read the contents of the file into a stream
                    var fileStream = OpenFileDialog_obj.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        FileContent = reader.ReadToEnd();
                    }

                    this.Label_Fichier.Text = FileName;
                }
            }

            
        }

        private void Ajouter_Click(object sender, EventArgs e)
        {

        }

        private void Ecrasement_Click(object sender, EventArgs e)
        {

        }
    }
}

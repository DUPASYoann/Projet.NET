using Bacchus.BDD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bacchus
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
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
    }
}

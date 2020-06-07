namespace Bacchus
{
    partial class FormImporter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Button_Selection = new System.Windows.Forms.Button();
            this.Label_Fichier = new System.Windows.Forms.Label();
            this.Button_Ajout = new System.Windows.Forms.Button();
            this.Button_Ecrasement = new System.Windows.Forms.Button();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.sQLiteDAOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.sQLiteDAOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // Button_Selection
            // 
            this.Button_Selection.Location = new System.Drawing.Point(12, 12);
            this.Button_Selection.Name = "Button_Selection";
            this.Button_Selection.Size = new System.Drawing.Size(75, 23);
            this.Button_Selection.TabIndex = 0;
            this.Button_Selection.Text = "Select csv";
            this.Button_Selection.UseVisualStyleBackColor = true;
            this.Button_Selection.Click += new System.EventHandler(this.Importer_Click);
            // 
            // Label_Fichier
            // 
            this.Label_Fichier.AutoSize = true;
            this.Label_Fichier.BackColor = System.Drawing.Color.Transparent;
            this.Label_Fichier.Location = new System.Drawing.Point(93, 17);
            this.Label_Fichier.Name = "Label_Fichier";
            this.Label_Fichier.Size = new System.Drawing.Size(130, 13);
            this.Label_Fichier.TabIndex = 5;
            this.Label_Fichier.Text = "nom du fichier sélectionné";
            this.Label_Fichier.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Button_Ajout
            // 
            this.Button_Ajout.Location = new System.Drawing.Point(13, 71);
            this.Button_Ajout.Name = "Button_Ajout";
            this.Button_Ajout.Size = new System.Drawing.Size(75, 23);
            this.Button_Ajout.TabIndex = 6;
            this.Button_Ajout.Text = "Ajout";
            this.Button_Ajout.UseVisualStyleBackColor = true;
            this.Button_Ajout.Click += new System.EventHandler(this.Ajouter_Click);
            // 
            // Button_Ecrasement
            // 
            this.Button_Ecrasement.Location = new System.Drawing.Point(96, 71);
            this.Button_Ecrasement.Name = "Button_Ecrasement";
            this.Button_Ecrasement.Size = new System.Drawing.Size(75, 23);
            this.Button_Ecrasement.TabIndex = 7;
            this.Button_Ecrasement.Text = "Ecrasement";
            this.Button_Ecrasement.UseVisualStyleBackColor = true;
            this.Button_Ecrasement.Click += new System.EventHandler(this.Ecrasement_Click);
            // 
            // ProgressBar
            // 
            this.ProgressBar.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.sQLiteDAOBindingSource, "Value", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, "5"));
            this.ProgressBar.DataBindings.Add(new System.Windows.Forms.Binding("Maximum", this.sQLiteDAOBindingSource, "Maximum", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, "100"));
            this.ProgressBar.Location = new System.Drawing.Point(12, 41);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(211, 23);
            this.ProgressBar.TabIndex = 2;
            this.ProgressBar.Click += new System.EventHandler(this.ProgressBar_Click);
            // 
            // sQLiteDAOBindingSource
            // 
            this.sQLiteDAOBindingSource.DataSource = typeof(Bacchus.BDD.SQLiteDAO);
            this.sQLiteDAOBindingSource.CurrentChanged += new System.EventHandler(this.sQLiteDAOBindingSource_CurrentChanged);
            // 
            // FormImporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 142);
            this.Controls.Add(this.Button_Ecrasement);
            this.Controls.Add(this.Button_Ajout);
            this.Controls.Add(this.Label_Fichier);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.Button_Selection);
            this.Name = "FormImporter";
            this.Text = "FormImporter";
            ((System.ComponentModel.ISupportInitialize)(this.sQLiteDAOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Button_Selection;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.Label Label_Fichier;
        private System.Windows.Forms.Button Button_Ajout;
        private System.Windows.Forms.Button Button_Ecrasement;
        private System.Windows.Forms.BindingSource sQLiteDAOBindingSource;
    }
}
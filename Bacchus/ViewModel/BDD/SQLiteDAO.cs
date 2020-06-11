using Bacchus.Model;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bacchus.BDD
{
    sealed class SQLiteDAO : INotifyPropertyChanged
    {

        private static SQLiteDAO Instance_priv = null;
        private static readonly object PadLock = new object();

        private readonly String Connection_String = @"Data Source = Bacchus.SQLite;";

        public event PropertyChangedEventHandler PropertyChanged;

        private int Value_priv;
        public int Value
        {
            get { return Value_priv; }
            set { 
                Value_priv = value;
                OnPropertyChanged("Value");
            }
        }

        private int Maximum_priv;
        public int Maximum
        {
            get { return Maximum_priv; }
            set { 
                Maximum_priv = value;
                OnPropertyChanged("Maximum");
            }
        }

        SQLiteDAO()
        {
        }

        public static SQLiteDAO Instance
        {
            get
            {
                lock (PadLock)
                {
                    if (Instance_priv == null)
                    {
                        Instance_priv = new SQLiteDAO();
                    }
                    return Instance_priv;
                }
            }
        }

        private void OnPropertyChanged(string PropertyName)
        {
            if (PropertyName != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        /// <summary>
        /// Permet de vider la BDD (vider toute les tables)
        /// </summary>
        public void Empty_DB()
        {
            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    string script = File.ReadAllText(@".\ViewModel\BDD\Script\EmptyDB.SQLite.sql");
                    SQLiteCommand sQLiteCommand = new SQLiteCommand(script, My_Connection);
                    sQLiteCommand.ExecuteNonQuery();
                    My_Connection.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("IMPOSSIBLE DE VIDER LA BASE DE DONNEES : " + e.Message);
                }
            }
        }
        /// <summary>
        /// Exporter les données de l'application en un fichier CSV
        /// </summary>
        /// <param name="CSV_Path">Le chemin ou enregistrer le csv</param>
        public void Export_To_Csv(String CSV_Path)
        {
            String Description;
            String Ref_Article;
            String Marque;
            String Famille;
            String SousFamille;
            float Prix;
            int Quantite;
            

            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "SELECT Articles.Description, Articles.RefArticle, Marques.Nom, SousF.FamillesNom, SousF.SousFamillesNom, Articles.PrixHT, Articles.Quantite FROM Articles JOIN Marques ON Articles.RefMarque = Marques.RefMarque JOIN (SELECT SousFamilles.Nom AS 'SousFamillesNom', SousFamilles.RefSousFamille, Familles.Nom AS 'FamillesNom' FROM Familles JOIN SousFamilles ON Familles.RefFamille = SousFamilles.RefFamille) AS 'SousF' ON SousF.RefSousFamille = Articles.RefSousFamille";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);

                    using (SQLiteDataReader SQLiteDataReader_obj = SQLiteCommand_obj.ExecuteReader())
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(CSV_Path, false, Encoding.UTF8))
                        {
                            try
                            {
                                file.WriteLine("Description; Ref; Marque; Famille; Sous - Famille; Prix H.T.; Quantite");
                            
                                while (SQLiteDataReader_obj.Read())
                                {
                                    Description = SQLiteDataReader_obj.GetString(0);
                                    Ref_Article = SQLiteDataReader_obj.GetString(1);
                                    Marque = SQLiteDataReader_obj.GetString(2);
                                    Famille = SQLiteDataReader_obj.GetString(3);
                                    SousFamille = SQLiteDataReader_obj.GetString(4);
                                    Prix = SQLiteDataReader_obj.GetFloat(5);
                                    Quantite = SQLiteDataReader_obj.GetInt32(6);
                                    file.WriteLine(Description + ";" + Ref_Article + ";" + Marque + ";" + Famille + ";" + SousFamille + ";" + Prix +";"+ Quantite );
                                }

                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                            finally
                            {
                                file.Close();
                                // always call Close when done reading.
                                SQLiteDataReader_obj.Close();
                                // always call Close when done reading.
                                My_Connection.Close();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERREUR DANS LA RECUPERATION DES OBJETS EXPORT: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Inserer les données dans l'application depuis un fichier CSV
        /// </summary>
        /// <param name="CSV_Path">Le chemin de l'emplacement du fichier CSV</param>
        public void Insert_From_Csv(String CSV_Path)
        {
            this.Value_priv = 0;
            this.Maximum_priv = 0;
            int Nb_Ligne = 0;
            String Description;
            String Ref_Article;
            String Marque;
            String Famille;
            String SousFamille;
            String Prix;

            using (TextFieldParser parser = new TextFieldParser(CSV_Path, Encoding.Default))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                while (!parser.EndOfData)
                {
                    Nb_Ligne++;
                    parser.ReadLine();
                }

                Maximum = Nb_Ligne;
            }

            using (TextFieldParser parser = new TextFieldParser(CSV_Path, Encoding.Default))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                Nb_Ligne = 1;

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (parser.LineNumber != 2)
                    {
                        Description = fields[0];
                        Ref_Article = fields[1];
                        Marque = fields[2];
                        Famille = fields[3];
                        SousFamille = fields[4];
                        Prix = fields[5];

                        if (Search_Article_From_Ref(Ref_Article) == null)
                        {
                            Nb_Ligne++;
                            Value = Nb_Ligne;
                            Insert_Row_From_CSV(Description, Ref_Article, Marque, Famille, SousFamille, Prix);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Inserer les données dans la BDD depuis une ligne de CSV liée à un article
        /// </summary>
        /// <param name="Description">Description de l'article</param>
        /// <param name="Ref_Article">Reference de l'article</param>
        /// <param name="Marque">Marque de l'article</param>
        /// <param name="Famille">Famille de l'article</param>
        /// <param name="SousFamille">Sous famille de l'article et contenu dans la famille</param>
        /// <param name="Prix">Prix HT de l'article</param>
        public void Insert_Row_From_CSV(String Description, String Ref_Article, String Marque, String Famille, String SousFamille, String Prix)
        {
            Article Article_obj = new Article
            {
                Description = Description,
                RefArticle = Ref_Article,
                PrixHT = float.Parse(Prix)
            };

            Marque Marque_obj = Search_Marque_From_Name(Marque);
            if (Marque_obj == null)
            {
                Marque_obj = new Marque(Marque);
                Insert_Marque(Marque_obj);
                Marque_obj = Search_Marque_From_Name(Marque_obj.Nom);
            }

            Famille Famille_obj = Search_Famille_From_Name(Famille);
            if (Famille_obj == null)
            {
                Famille_obj = new Famille(Famille);
                Insert_Famille(Famille_obj);
                Famille_obj = Search_Famille_From_Name(Famille_obj.Nom);
            }

            SousFamille SousFamille_obj = Search_SousFamille_From_Name(SousFamille);
            if (SousFamille_obj == null)
            {
                SousFamille_obj = new SousFamille(Famille_obj.RefFamille, SousFamille);
                Insert_SousFamille(SousFamille_obj);
                SousFamille_obj = Search_SousFamille_From_Name(SousFamille_obj.Nom);
            }

            Article_obj.RefMarque = Marque_obj.RefMarque;
            Article_obj.RefSousFamille = SousFamille_obj.RefSousFamille;

            Insert_Article(Article_obj);

        }

        /// <summary>
        /// Insérer un article dans la BDD
        /// </summary>
        /// <param name="Article_obj">Une instance de l'article à insérer</param>
        public void Insert_Article(Article Article_obj)
        {
            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    using (SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(My_Connection))
                    {
                        SQLiteCommand_obj.CommandText = @"INSERT INTO Articles VALUES (:RefArticle,:Description,:RefSousFamille,:RefMarque,:PrixHT,:Quantite)";
                        SQLiteCommand_obj.CommandType = CommandType.Text;
                        SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("RefArticle", Article_obj.RefArticle));
                        SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("Description", Article_obj.Description));
                        SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("RefSousFamille", Article_obj.RefSousFamille));
                        SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("RefMarque", Article_obj.RefMarque));
                        SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("PrixHT", Article_obj.PrixHT));
                        SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("Quantite", Article_obj.Quantite));
                        Debug.Print(SQLiteCommand_obj.CommandText);
                        SQLiteCommand_obj.ExecuteNonQuery(); 
                    }
                    My_Connection.Close();

                }
                catch (Exception e)
                {
                    MessageBox.Show("IMPOSSIBLE D'AJOUTER L'ARTICLE A LA BASE DE DONNEES : " + e.Message);
                    Debug.Print(Article_obj.RefArticle);
                } 
            }
        }

        /// <summary>
        /// Insérer une famille dans la BDD
        /// </summary>
        /// <param name="Famille_obj">Une instance la famille à insérer</param>
        public void Insert_Famille(Famille Famille_obj)
        {
            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "INSERT INTO Familles (Nom) VALUES(:Nom)";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("Nom", Famille_obj.Nom));
                    SQLiteCommand_obj.ExecuteNonQuery();
                    My_Connection.Close();

                }
                catch (Exception e)
                {
                    MessageBox.Show("IMPOSSIBLE D'AJOUTER LA FAMILLE A LA BASE DE DONNEES : " + e.Message);
                } 
            }
        }

        /// <summary>
        /// Insérer une marque dans la BDD
        /// </summary>
        /// <param name="Marque_obj">Une instance de la marque à insérer</param>
        public void Insert_Marque(Marque Marque_obj)
        {
            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "INSERT INTO Marques (Nom) VALUES(:Nom)";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("Nom", Marque_obj.Nom));
                    SQLiteCommand_obj.ExecuteNonQuery();
                    My_Connection.Close();

                }
                catch (Exception e)
                {
                    MessageBox.Show("IMPOSSIBLE D'AJOUTER LA MARQUE A LA BASE DE DONNEES : " + e.Message);
                } 
            }
        }

        /// <summary>
        /// Insérer unes sous famille dans la BDD
        /// </summary>
        /// <param name="SousFamille_obj">Une instance de la sous famille à insérer</param>
        public void Insert_SousFamille(SousFamille SousFamille_obj)
        {
            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "INSERT INTO SousFamilles (RefFamille,Nom) VALUES(:RefFamille,:Nom)";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("RefFamille", SousFamille_obj.RefFamille));
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("Nom", SousFamille_obj.Nom));
                    SQLiteCommand_obj.ExecuteNonQuery();
                    My_Connection.Close();

                }
                catch (Exception e)
                {
                    MessageBox.Show("IMPOSSIBLE D'AJOUTER LA SOUS FAMILLE A LA BASE DE DONNEES : " + e.Message);
                }

            }        }

        /// <summary>
        /// Trouver un article à partir de sa référence
        /// </summary>
        /// <param name="Ref">La référence de l'article</param>
        /// <returns>L'article possèdant la référence</returns>
        public Article Search_Article_From_Ref(String Ref)
        {
            Article Resultat = new Article
            {
                RefArticle = Ref
            };

            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "SELECT * FROM Articles WHERE RefArticle = :Ref";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("Ref", Ref));
                    using (SQLiteDataReader SQLiteDataReader_obj = SQLiteCommand_obj.ExecuteReader())
                    {
                        try
                        {
                            if (SQLiteDataReader_obj.Read())
                            {
                                Resultat.RefArticle = SQLiteDataReader_obj.GetString(0);
                                Resultat.Description = SQLiteDataReader_obj.GetString(1);
                                Resultat.RefSousFamille = SQLiteDataReader_obj.GetInt32(2);
                                Resultat.RefMarque = SQLiteDataReader_obj.GetInt32(3);
                                Resultat.PrixHT = SQLiteDataReader_obj.GetFloat(4);
                                Resultat.Quantite = SQLiteDataReader_obj.GetInt32(5);
                            }
                            else
                            {
                                Resultat = null;
                            }
                        }
                        catch (Exception)
                        {
                            Resultat = null;
                        }
                        finally
                        {
                            // always call Close when done reading.
                            SQLiteDataReader_obj.Close();
                            // always call Close when done reading.
                            My_Connection.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERREUR DANS LA RECHERCHE DE LA SOUS FAMILLE : " + e.Message);
                }
            }

            return Resultat;
        }

        /// <summary>
        /// Trouver la sous famille à partir de son nom
        /// </summary>
        /// <param name="Name">Le nom de la sous famille</param>
        /// <returns>La sous famille portant le nom</returns>
        public SousFamille Search_SousFamille_From_Name(String Name)
        {
            SousFamille Resultat = new SousFamille();
            Resultat.Nom = Name;

            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "SELECT * FROM SousFamilles WHERE Nom = :Name";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("Name", Name));
                    using (SQLiteDataReader SQLiteDataReader_obj = SQLiteCommand_obj.ExecuteReader())
                    {
                        try
                        {
                            if (SQLiteDataReader_obj.Read())
                            {
                                Resultat.RefSousFamille = SQLiteDataReader_obj.GetInt32(0);
                                Resultat.RefFamille = SQLiteDataReader_obj.GetInt32(1);
                            }
                            else
                            {
                                Resultat = null;
                            }
                        }
                        catch (Exception)
                        {
                            Resultat = null;
                        }
                        finally
                        {
                            // always call Close when done reading.
                            SQLiteDataReader_obj.Close();
                            // always call Close when done reading.
                            My_Connection.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERREUR DANS LA RECHERCHE DE LA SOUS FAMILLE : " + e.Message);
                } 
            }

            return Resultat;
        }

        /// <summary>
        /// Trouver la famille à partir de son nom
        /// </summary>
        /// <param name="Name">Le nom de la famille</param>
        /// <returns>La famille associée au nom</returns>
        public Famille Search_Famille_From_Name(String Name)
        {
            Famille Resultat = new Famille();
            Resultat.Nom = Name;

            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "SELECT * FROM Familles WHERE Nom = :Name";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("Name", Name));
                    using (SQLiteDataReader SQLiteDataReader_obj = SQLiteCommand_obj.ExecuteReader())
                    {
                        try
                        {
                            if (SQLiteDataReader_obj.Read())
                            {
                                Resultat.RefFamille = SQLiteDataReader_obj.GetInt32(0);
                            }
                            else
                            {
                                Resultat = null;
                            }
                        }
                        catch (Exception)
                        {
                            Resultat = null;
                        }
                        finally
                        {
                            // always call Close when done reading.
                            SQLiteDataReader_obj.Close();
                            // always call Close when done reading.
                            My_Connection.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERREUR DANS LA RECHERCHE DE LA FAMILLE : " + e.Message);
                } 
            }

            return Resultat;
        }

        /// <summary>
        /// Trouver la marque à partir de son nom
        /// </summary>
        /// <param name="Name">Le nom de la marque</param>
        /// <returns>La marque associée au nom</returns>
        public Marque Search_Marque_From_Name(String Name)
        {
            Marque Resultat = new Marque();
            Resultat.Nom = Name;

            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "SELECT * FROM Marques WHERE Nom = :Name";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("Name", Name));
                    using (SQLiteDataReader SQLiteDataReader_obj = SQLiteCommand_obj.ExecuteReader())
                    {
                        try
                        {
                            if (SQLiteDataReader_obj.Read())
                            {
                                Resultat.RefMarque = SQLiteDataReader_obj.GetInt32(0);
                            }
                            else
                            {
                                Resultat = null;
                            }
                        }
                        catch (Exception)
                        {
                            Resultat = null;
                        }
                        finally
                        {
                            // always call Close when done reading.
                            SQLiteDataReader_obj.Close();
                            // always call Close when done reading.
                            My_Connection.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERREUR DANS LA RECHERCHE DE LA MARQUE : " + e.Message);
                }
            } 
            return Resultat;
        }
        
        /// <summary>
        /// Obtenir tout les articles de la BDD
        /// </summary>
        /// <returns>tout les articles</returns>
        public List<Article> GetAll_Article()
        {
            List<Article> Resultat = new List<Article>();

            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "SELECT * FROM Articles";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    using (SQLiteDataReader SQLiteDataReader_obj = SQLiteCommand_obj.ExecuteReader())
                    {
                        try
                        {
                            while (SQLiteDataReader_obj.Read())
                            {
                                Article Article_obj = new Article();
                                Article_obj.RefArticle = SQLiteDataReader_obj.GetString(0);
                                Article_obj.Description = SQLiteDataReader_obj.GetString(1);
                                Article_obj.RefSousFamille = SQLiteDataReader_obj.GetInt32(2);
                                Article_obj.RefMarque = SQLiteDataReader_obj.GetInt32(3);
                                Article_obj.PrixHT = SQLiteDataReader_obj.GetFloat(4);
                                Article_obj.Quantite = SQLiteDataReader_obj.GetInt32(5);


    
                                Resultat.Add(Article_obj);
                            }

                        }
                        catch (Exception e)
                        {
                            Resultat = null;
                            throw e;
                        }
                        finally
                        {
                            // always call Close when done reading.
                            SQLiteDataReader_obj.Close();
                            // always call Close when done reading.
                            My_Connection.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERREUR DANS LA RECUPERATION DES OBJETS: " + e.Message);
                } 
            }

            return Resultat;
        }

        /// <summary>
        /// Obtenir toute les familles de la BDD
        /// </summary>
        /// <returns>Toute les familles</returns>
        public List<Famille> GetAll_Famille()
        {

            List<Famille> Resultat = new List<Famille>();

            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "SELECT * FROM Familles";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    using (SQLiteDataReader SQLiteDataReader_obj = SQLiteCommand_obj.ExecuteReader())
                    {
                        try
                        {
                            while (SQLiteDataReader_obj.Read())
                            {
                                Famille Famille_Obj = new Famille();
                                Famille_Obj.RefFamille = SQLiteDataReader_obj.GetInt32(0);
                                Famille_Obj.Nom = SQLiteDataReader_obj.GetString(1);
                                Resultat.Add(Famille_Obj);
                            }
                        }
                        catch (Exception)
                        {
                            Resultat = null;
                        }
                        finally
                        {
                            SQLiteDataReader_obj.Close();
                            My_Connection.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERREUR DANS LA RECUPERATION DES FAMILLES : " + e.Message);
                }
                
            }

            return Resultat;

        }
        /// <summary>
        /// Obtenir toute les sous familles de la BDD
        /// </summary>
        /// <returns>Toute les sous familles</returns>
        public List<SousFamille> GetAll_SousFamille()
        {

            List<SousFamille> Resultat = new List<SousFamille>();

            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "SELECT * FROM SousFamilles";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    using (SQLiteDataReader SQLiteDataReader_obj = SQLiteCommand_obj.ExecuteReader())
                    {
                        try
                        {
                            while (SQLiteDataReader_obj.Read())
                            {
                                SousFamille SousFamille_Obj = new SousFamille();
                                SousFamille_Obj.RefSousFamille = SQLiteDataReader_obj.GetInt32(0);
                                SousFamille_Obj.RefFamille = SQLiteDataReader_obj.GetInt32(1);
                                SousFamille_Obj.Nom = SQLiteDataReader_obj.GetString(2);
                                Resultat.Add(SousFamille_Obj);
                            }
                        }
                        catch (Exception)
                        {
                            Resultat = null;
                        }
                        finally
                        {
                            SQLiteDataReader_obj.Close();
                            My_Connection.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERREUR DANS LA RECUPERATION DES SOUSFAMILLES : " + e.Message);
                }

            }

            return Resultat;

        }

        /// <summary>
        /// Obtenir toute les marques de la BDD
        /// </summary>
        /// <returns>Toute les marques</returns>
        public List<Marque> GetAll_Marque()
        {

            List<Marque> Resultat = new List<Marque>();

            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "SELECT * FROM Marques";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    using (SQLiteDataReader SQLiteDataReader_obj = SQLiteCommand_obj.ExecuteReader())
                    {
                        try
                        {
                            while (SQLiteDataReader_obj.Read())
                            {
                                Marque Marque_Obj = new Marque();
                                Marque_Obj.RefMarque = SQLiteDataReader_obj.GetInt32(0);
                                Marque_Obj.Nom = SQLiteDataReader_obj.GetString(1);
                                Resultat.Add(Marque_Obj);
                            }
                        }
                        catch (Exception)
                        {
                            Resultat = null;
                        }
                        finally
                        {
                            SQLiteDataReader_obj.Close();
                            My_Connection.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERREUR DANS LA RECUPERATION DES MARQUES : " + e.Message);
                }

            }

            return Resultat;

        }

        /// <summary>
        /// Obtenir tout les articles d'une sous famille
        /// </summary>
        /// <param name="SousFamille_Obj">Une instance de la sous famille</param>
        /// <returns>les articles de la sous famille</returns>
        public List<Article> GetAll_Article_From_SousFamille(SousFamille SousFamille_Obj)
        {
            List<Article> Resultat = new List<Article>();

            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "SELECT Articles.RefArticle, Articles.Description, Articles.RefSousFamille, Articles.RefMarque, Articles.PrixHT, Articles.Quantite FROM Articles WHERE Articles.RefSousFamille = :RefSousFamille";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("RefSousFamille", SousFamille_Obj.RefSousFamille));
                    using (SQLiteDataReader SQLiteDataReader_obj = SQLiteCommand_obj.ExecuteReader())
                    {
                        try
                        {
                            while (SQLiteDataReader_obj.Read())
                            {
                                Article Article_Obj = new Article();
                                Article_Obj.RefArticle = SQLiteDataReader_obj.GetString(0);
                                Article_Obj.Description = SQLiteDataReader_obj.GetString(1);
                                Article_Obj.RefSousFamille = SQLiteDataReader_obj.GetInt32(2);
                                Article_Obj.RefMarque = SQLiteDataReader_obj.GetInt32(3);
                                Article_Obj.PrixHT = SQLiteDataReader_obj.GetFloat(4);
                                Article_Obj.Quantite = SQLiteDataReader_obj.GetInt32(5);
                                Resultat.Add(Article_Obj);
                            }
                        }
                        catch (Exception)
                        {
                            Resultat = null;
                        }
                        finally
                        {
                            SQLiteDataReader_obj.Close();
                            My_Connection.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERREUR DANS LA RECUPERATION DES ARTICLES DE LA SOUS FAMILLES : " + e.Message);
                }

            }

            return Resultat;
        }

        /// <summary>
        /// Obtenir tout les articles d'une marque
        /// </summary>
        /// <param name="Marque_Obj">Une instance de la marque</param>
        /// <returns>Tout les articles de la marque</returns>
        public List<Article> GetAll_Article_From_Marque(Marque Marque_Obj)
        {
            List<Article> Resultat = new List<Article>();

            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "SELECT Articles.RefArticle, Articles.Description, Articles.RefSousFamille, Articles.RefMarque, Articles.PrixHT, Articles.Quantite FROM Articles WHERE Articles.RefMarque = :RefMarque";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("RefMarque", Marque_Obj.RefMarque));
                    using (SQLiteDataReader SQLiteDataReader_obj = SQLiteCommand_obj.ExecuteReader())
                    {
                        try
                        {
                            while (SQLiteDataReader_obj.Read())
                            {
                                Article Article_Obj = new Article();
                                Article_Obj.RefArticle = SQLiteDataReader_obj.GetString(0);
                                Article_Obj.Description = SQLiteDataReader_obj.GetString(1);
                                Article_Obj.RefSousFamille = SQLiteDataReader_obj.GetInt32(2);
                                Article_Obj.RefMarque = SQLiteDataReader_obj.GetInt32(3);
                                Article_Obj.PrixHT = SQLiteDataReader_obj.GetFloat(4);
                                Article_Obj.Quantite = SQLiteDataReader_obj.GetInt32(5);
                                Resultat.Add(Article_Obj);
                            }
                        }
                        catch (Exception)
                        {
                            Resultat = null;
                        }
                        finally
                        {
                            SQLiteDataReader_obj.Close();
                            My_Connection.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERREUR DANS LA RECUPERATION DES ARTICLES DE LA MARQUE : " + e.Message);
                }

            }

            return Resultat;
        }
        /// <summary>
        /// Modifier un article
        /// </summary>
        /// <param name="Article_obj">Une instance de l'article à modifier</param>
        public void Update_Article(Article Article_obj)
        {
            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "UPDATE Articles SET Description = :Description, PrixHT = :PrixHT, Quantite = :Quantite, RefMarque = :RefMarque, RefSousFamille = :RefSousFamille WHERE RefArticle = :RefArticle";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("Description", Article_obj.Description));
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("PrixHT", Article_obj.PrixHT));
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("Quantite", Article_obj.Quantite));
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("RefMarque", Article_obj.RefMarque));
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("RefSousFamille", Article_obj.RefSousFamille));

                    SQLiteCommand_obj.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERREUR DANS LA MODIFICATION D'UN ARTICLE : " + e.Message);
                }
                finally
                {
                    My_Connection.Close();
                }
            }
        }
        /// <summary>
        /// Modifier une sous famille
        /// </summary>
        /// <param name="SousFamille_obj">Une instance de la sous famille</param>
        public void Update_SousFamille(SousFamille SousFamille_obj)
        {
            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "UPDATE SousFamilles SET Nom = :Nom, RefFamille = :RefFamille WHERE RefSousFamille = :RefSousFamille";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("Nom", SousFamille_obj.Nom));
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("RefFamille", SousFamille_obj.RefFamille));
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("RefSousFamille", SousFamille_obj.RefSousFamille));

                    SQLiteCommand_obj.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERREUR DANS LA MODIFICATION D'UNE SOUS FAMILLE : " + e.Message);
                }
                finally
                {
                    My_Connection.Close();
                }
            }
        }
        /// <summary>
        /// Modifier une famille
        /// </summary>
        /// <param name="Famille_obj">Une instance de la famille à modifier</param>
        public void Update_Famille(Famille Famille_obj)
        {
            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "UPDATE Familles SET Nom = :Nom WHERE RefFamille = :RefFamille";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("Nom", Famille_obj.Nom));
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("RefFamille", Famille_obj.RefFamille));

                    SQLiteCommand_obj.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERREUR DANS LA MODIFICATION D'UNE FAMILLE : " + e.Message);
                }
                finally
                {
                    My_Connection.Close();
                }
            }
        }
        /// <summary>
        /// Supprimer une famille
        /// </summary>
        /// <param name="Famille_obj">Une instance de la famille à supprimer</param>
        public void Delete_Famille(Famille Famille_obj)
        {
            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "DELETE FROM Familles WHERE RefFamille = :RefFamille";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("RefFamille", Famille_obj.RefFamille));

                    SQLiteCommand_obj.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERREUR DANS LA SUPPRESSION D'UNE FAMILLE : " + e.Message);
                }
                finally
                {
                    My_Connection.Close();
                }
            }
        }
        /// <summary>
        /// Supprimer une article
        /// </summary>
        /// <param name="Article_obj">Une instance de l'article à supprimer</param>
        public void Delete_Article(Article Article_obj)
        {
            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "DELETE FROM Articles WHERE RefArticle = :RefArticle";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("RefArticle", Article_obj.RefArticle));

                    SQLiteCommand_obj.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERREUR DANS LA SUPPRESSION D'UN ARTICLE : " + e.Message);
                }
                finally
                {
                    My_Connection.Close();
                }
            }
        }
        /// <summary>
        /// Supprimer une sous famille
        /// </summary>
        /// <param name="SousFamille_obj">Une instance de la sous famille</param>
        public void Delete_SousFamille(SousFamille SousFamille_obj)
        {
            using (SQLiteConnection My_Connection = new SQLiteConnection(Connection_String))
            {
                try
                {
                    My_Connection.Open();
                    String SQL_String = "DELETE FROM SousFamilles WHERE RefSousFamille = :RefSousFamille";
                    SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                    SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("RefSousFamille", SousFamille_obj.RefSousFamille));

                    SQLiteCommand_obj.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERREUR DANS LA SUPPRESSION D'UNE SOUS FAMILLES : " + e.Message);
                }
                finally
                {
                    My_Connection.Close();
                }
            }
        }

    }
}



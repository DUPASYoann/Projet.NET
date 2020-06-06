﻿using Bacchus.Model;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
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
    sealed class SQLiteDAO
    {

        private static SQLiteDAO Instance_priv = null;
        private static readonly object PadLock = new object();

        private String Connection_String = @"Data Source = Bacchus.SQLite;";
        private SQLiteConnection My_Connection;

        SQLiteDAO()
        {
            My_Connection = new SQLiteConnection(Connection_String);
        }

        public static SQLiteDAO Instance
        {
            get
            {
                lock (PadLock)
                {
                    if(Instance_priv == null)
                    {
                        Instance_priv = new SQLiteDAO();
                    }
                    return Instance_priv;
                }
            }
        }

        public void Empty_DB()
        {
            try
            {
                My_Connection.Open();
                string script = File.ReadAllText(@".\BDD\Script\EmptyDB.SQLite.sql");
                SQLiteCommand sQLiteCommand = new SQLiteCommand(script,My_Connection);
                sQLiteCommand.ExecuteNonQuery();
                My_Connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("IMPOSSIBLE DE VIDER LA BASE DE DONNEES : " + e.Message);
            }
        }

        public void Insert_From_Csv(String CSV_Path)
        {
            String Description;
            String Ref_Article;
            String Marque;
            String Famille;
            String SousFamille;
            String Prix;

            using (TextFieldParser parser = new TextFieldParser(CSV_Path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
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

                        Insert_Row_From_CSV(Description, Ref_Article, Marque, Famille, SousFamille, Prix);
                    }
                }
            }
        }

        public void Insert_Row_From_CSV(String Description,String Ref_Article, String Marque, String Famille, String SousFamille, String Prix)
        {
            Article Article_obj = new Article();
            Article_obj.Description = Description;
            Article_obj.RefArticle = Ref_Article;
            Article_obj.PrixHT = float.Parse(Prix);

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
                SousFamille_obj = new SousFamille(Famille_obj.RefFamille,SousFamille);
                Insert_SousFamille(SousFamille_obj);
                SousFamille_obj = Search_SousFamille_From_Name(SousFamille_obj.Nom);
            }

            Article_obj.RefMarque = Marque_obj.RefMarque;
            Article_obj.RefSousFamille = SousFamille_obj.RefSousFamille;

            Insert_Article(Article_obj);

        }

        public void Insert_Article(Article Article_obj)
        {
            try
            {
                
                My_Connection.Open();
                SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(My_Connection);
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
                My_Connection.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show("IMPOSSIBLE D'AJOUTER L'ARTICLE A LA BASE DE DONNEES : " + e.Message);
            }
        }

        public void Insert_Famille(Famille Famille_obj)
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

        public void Insert_Marque(Marque Marque_obj)
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

        public void Insert_SousFamille(SousFamille SousFamille_obj)
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
        }

        public SousFamille Search_SousFamille_From_Name(String Name)
        {
            SousFamille Resultat = new SousFamille();
            Resultat.Nom = Name;

            try
            {
                My_Connection.Open();
                String SQL_String = "SELECT * FROM SousFamilles WHERE Nom = :Name";
                SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("Name", Name));
                SQLiteDataReader SQLiteDataReader_obj = SQLiteCommand_obj.ExecuteReader();
                try
                {
                    SQLiteDataReader_obj.Read();
                    Resultat.RefSousFamille = SQLiteDataReader_obj.GetInt32(0);
                    Resultat.RefFamille = SQLiteDataReader_obj.GetInt32(1);
                }
                catch (Exception e)
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
            catch (Exception e)
            {
                MessageBox.Show("ERREUR DANS LA RECHERCHE DE LA SOUS FAMILLE : " + e.Message);
            }

            return Resultat;
        }

        public Famille Search_Famille_From_Name(String Name)
        {
            Famille Resultat = new Famille();
            Resultat.Nom = Name;

            try
            {
                My_Connection.Open();
                String SQL_String = "SELECT * FROM Familles WHERE Nom = :Name";
                SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("Name", Name));
                SQLiteDataReader SQLiteDataReader_obj = SQLiteCommand_obj.ExecuteReader();
                try
                {
                    SQLiteDataReader_obj.Read();
                    Resultat.RefFamille = SQLiteDataReader_obj.GetInt32(0);
                }
                catch (Exception e)
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
            catch (Exception e)
            {
                MessageBox.Show("ERREUR DANS LA RECHERCHE DE LA FAMILLE : " + e.Message);
            }

            return Resultat;
        }

        public Marque Search_Marque_From_Name(String Name)
        {
            Marque Resultat = new Marque();
            Resultat.Nom = Name;

            try
            {
                My_Connection.Open();
                String SQL_String = "SELECT * FROM Marques WHERE Nom = :Name";
                SQLiteCommand SQLiteCommand_obj = new SQLiteCommand(SQL_String, My_Connection);
                SQLiteCommand_obj.Parameters.Add(new SQLiteParameter("Name", Name));
                SQLiteDataReader SQLiteDataReader_obj = SQLiteCommand_obj.ExecuteReader();
                try
                {
                    SQLiteDataReader_obj.Read();
                    Resultat.RefMarque = SQLiteDataReader_obj.GetInt32(0);
                }
                catch (Exception e)
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
            catch (Exception e)
            {
                MessageBox.Show("ERREUR DANS LA RECHERCHE DE LA MARQUE : " + e.Message);
            }

            return Resultat;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Xml;
using System.Text.Json;

namespace WPF_Probleme
{
    internal class MainClass
    {
        MySqlConnection connection;

        /// <summary>
        /// Constructeur de la classe, initialisation d'une connexion
        /// </summary>
        /// <param name="user"></param>
        public MainClass(string user = "bozo")
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=VeloMax;UID="+user+";PASSWORD="+user+";";
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        /// <summary>
        /// Permet de lire plusieurs types de requête et notamment d'afficher nos tables. 
        /// Imlémentation des résultats des requêtes dans un DataTable pour le mettre dans un datagrid WPF
        /// </summary>
        /// <param name="command"></param>
        /// <param name="table"></param>
        /// <param name="colonnes"></param>
        /// <param name="fulltable"></param>
        /// <param name="Readable"></param>
        /// <returns></returns>
        public DataTable Reader(MySqlCommand command, string table, string[] colonnes = null, bool fulltable = true,  bool Readable = true)
        {
            DataTable dt = new DataTable();
            List<string[]> list = Recuperation_Column_Type(table);
            DataColumn column;
            DataRow row;
            MySqlDataReader reader;
            try
            {
                reader = command.ExecuteReader();
            }
            catch(MySqlException e)
            {
                return null;
            }

            if (fulltable)
            {
                foreach (string[] colonne_name in list)
                {
                    column = new DataColumn();
                    column.ColumnName = colonne_name[0];
                    dt.Columns.Add(column);
                }

                while (reader.Read())
                {
                    row = dt.NewRow();
                    for (int i = 0; i < list.Count; i++)
                        row[list[i][0]] = reader.GetValue(i).ToString();
                    dt.Rows.Add(row);
                }
            }
            else
            {
                foreach (string colonne_name in colonnes)
                {
                    column = new DataColumn();
                    column.ColumnName = colonne_name;
                    dt.Columns.Add(column);
                }
                while (reader.Read())
                {
                    row = dt.NewRow();
                    for (int i = 0; i < colonnes.Length; i++)
                        row[colonnes[i]] = reader.GetValue(i).ToString();
                    dt.Rows.Add(row);
                }
            }
            reader.Close();
            return dt;
        }

        /// <summary>
        /// Permet l'affichage de n'importe quelle table de notre base.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public DataTable AffichageTable(string table)
        {
            string commandtext = "SELECT * FROM " + table + ";";
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = commandtext; 
            DataTable dt = Reader(command, table);
            command.Dispose();
            return dt;
        }

        /// <summary>
        /// On récupère le nom des colonnes et leur type pour une table donnée. 
        /// On met tout cela dans une liste de tableau de string.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<string[]> Recuperation_Column_Type(string table)
        {
            string commandtext = "SELECT COLUMN_NAME, DATA_TYPE, COLUMN_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + table + "' ORDER BY ORDINAL_POSITION ASC;";
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = commandtext;

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            command.Dispose();
            List<string[]> column = new List<string[]>();
            while (reader.Read())
                column.Add(new string[] { reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(2).ToString() });
            reader.Close();
            return column;
        }

        /// <summary>
        /// Convertisseur de type en string vers les types MySqlDbType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static MySqlDbType Conversion_Type(string type)
        {
            if (type == "int" || type == "System.Int32") { return MySqlDbType.Int32; }
            else if (type == "date") { return MySqlDbType.DateTime; }
            else if (type == "float" || type == "System.Single") { return MySqlDbType.Float; }
            else if (type == "enum") { return MySqlDbType.Enum; }
            else { return MySqlDbType.String; }
        }


        /// <summary>
        /// Création d'un élément de n'importe qu'elle table de notre base de donnée
        /// </summary>
        /// <param name="table"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Creation( string table, string[] value)
        {
            try
            {
                List<string[]> columntype = Recuperation_Column_Type(table);
                MySqlCommand command = connection.CreateCommand();

                string param = "";
                string valparam = "";
                string tempo = "";
                for (int i = 0; i < columntype.Count; i++)
                {
                    param += columntype[i][0] + ",";
                    valparam += "@" + columntype[i][0] + ",";
                    MySqlParameter temp = new MySqlParameter("@" + columntype[i][0], Conversion_Type(columntype[i][1]));
                    if (columntype[i][1] == "int") { if (value[i] != null) { temp.Value = Convert.ToInt32(value[i]); } else temp.Value = null; }
                    if (columntype[i][1] == "varchar") { temp.Value = Convert.ToString(value[i]); }
                    if (columntype[i][1] == "enum") { temp.Value = Convert.ToString(value[i]); }
                    if (columntype[i][1] == "float") { temp.Value = Convert.ToSingle(value[i]); }
                    if (columntype[i][1] == "date")
                    {
                        if (value[i] == null)
                            temp.Value = DateTime.Parse("01/01/1900");
                        else
                            temp.Value = DateTime.Parse(value[i]);
                    }
                    command.Parameters.Add(temp);
                }
                param = param.Remove(param.Length - 1);
                valparam = valparam.Remove(valparam.Length - 1);
                string commandtext = "INSERT INTO VeloMax." + table + " (" + param + ") VALUES (" + valparam + ")" +tempo;
                command.CommandText = commandtext;
                DataTable dt = Reader(command, table);
                if (dt == null)
                    return null;
                command.Dispose();

                return commandtext;
            }
            catch(Exception e)
            {
                return null;
            }        
        }


        /// <summary>
        /// Permet de mettre à jour les stocks de nos pièces. 
        /// </summary>
        /// <param name="numitem"></param>
        /// <param name="quantite"></param>
        /// <param name="ope"></param>
        /// <returns></returns>
        public string UpdateStock(object numitem, int quantite,string ope)
        {
            try
            {
                MySqlParameter id = new MySqlParameter("@value", Conversion_Type(numitem.GetType().ToString()));
                id.Value = numitem;
                string commandtext;
                commandtext = "UPDATE Piece SET stock_P = stock_P" + ope + quantite + " WHERE num_P = @value;";
                MySqlCommand command = connection.CreateCommand();
                command.Parameters.Add(id);
                command.CommandText = commandtext;
                MySqlDataReader reader = command.ExecuteReader();
                reader.Close();
                command.Dispose();
                return commandtext;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Retourne la différence entre le stock disponible de la pièce et la quantité demandée. Une différence négative doit entrainer un achat fournisseur
        /// si la commande veut être passée.
        /// </summary>
        /// <param name="numitem"></param>
        /// <param name="quantite"></param>
        /// <returns></returns>
        public int VerifStock(object numitem, int quantite)
        {
            if (numitem != null && quantite != 0)
            {
                MySqlParameter id = new MySqlParameter("@value", Conversion_Type(numitem.GetType().ToString()));
                id.Value = numitem;
                string commandtext;
                commandtext = "SELECT stock_P FROM Piece WHERE num_P = @value;";
                MySqlCommand command = connection.CreateCommand();
                command.Parameters.Add(id);
                command.CommandText = commandtext;
                MySqlDataReader reader = command.ExecuteReader();
                int quantitedispo = 0;
                while (reader.Read()) { quantitedispo = reader.GetInt32(0); }
                command.Dispose();
                reader.Close();
                return quantitedispo - quantite;
            }
            else
                return -1;
        }

        /// <summary>
        /// Nécessaire pour voir le contenu d'une commande et la liste des pièces d'un vélo. On utilise ctte fonction avec les tables Construction et commandeLigne. 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="colonne"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DataTable Contenu(string table, string colonne, object value)
        {
            if (value != null)
            {
                MySqlParameter id = new MySqlParameter("@value", Conversion_Type(value.GetType().ToString()));
                id.Value = value;
                string commandtext = "SELECT * FROM " + table + " WHERE " + colonne + " = @value;";
                MySqlCommand command = connection.CreateCommand();
                command.Parameters.Add(id);
                command.CommandText = commandtext;
                DataTable dt = Reader(command, table);
                command.Dispose();
                return dt;
            }
            else
                return null;
        }


        /// <summary>
        /// Retourne la liste d'aseemblage d'un vélo en question. 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="colonne"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DataTable Construction(string table, string colonne, object value)
        {
            if (value != null)
            {
                MySqlParameter id = new MySqlParameter("@value", Conversion_Type(value.GetType().ToString()));
                id.Value = value;
                string colonnesAffichage = "num_V,num_P,description_p,nomfourni_P,numcatafourni_P,prixU_P,delaiL_P,stock_P";
                string commandtext = "SELECT " + colonnesAffichage + "  FROM " + table + " NATURAL JOIN piece WHERE " + colonne + " = @value;";
                MySqlCommand command = connection.CreateCommand();
                command.Parameters.Add(id);
                command.CommandText = commandtext;
                DataTable dt = Reader(command, table, colonnesAffichage.Split(","), false);
                command.Dispose();
                return dt;
            }
            else
                return null;

        }


        /// <summary>
        /// Supprime n'importe qu'elle instance de n'importe quelle table
        /// </summary>
        /// <param name="table"></param>
        /// <param name="colonne"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Delete(string table,string colonne, object value)
        {
            if (value != null)
            {
                try
                {
                    MySqlParameter id = new MySqlParameter("@value", Conversion_Type(value.GetType().ToString()));
                    id.Value = value;
                    string commandtext = "DELETE FROM " + table + " WHERE " + colonne + " = @value;";
                    MySqlCommand command = connection.CreateCommand();
                    command.Parameters.Add(id);
                    command.CommandText = commandtext;
                    Reader(command, table);
                    command.Dispose();
                    return commandtext;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            else
                return null;
        }

        /// <summary>
        /// Donne la liste des pièces d'un vélo sous forme de List
        /// </summary>
        /// <param name="numV"></param>
        /// <returns></returns>
        public List<string> ListePiece(object numV)
        {
            MySqlParameter id = new MySqlParameter("@value", Conversion_Type(numV.GetType().ToString()));
            id.Value = numV;
            string commandtext = "SELECT num_P FROM Construction WHERE num_V = @value;";
            List<string> listepiece = new List<string> ();
            MySqlCommand command = connection.CreateCommand();
            command.Parameters.Add(id);
            command.CommandText = commandtext;
            MySqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                listepiece.Add(reader.GetValue(0).ToString());
            }           
            command.Dispose();
            reader.Close();
            return listepiece;
        }

        /// <summary>
        /// Permet de modifier une instance d'une des tables en fonction d'une condition.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="colonne"></param>
        /// <param name="value"></param>
        /// <param name="colonnecond"></param>
        /// <param name="valuecond"></param>
        /// <returns></returns>
        public string Update(string table, string colonne, object value, string colonnecond, object valuecond)
        {
            if (value != null && valuecond != null)
            {
                try
                {
                    MySqlParameter temp;
                    if (colonne == "type_V" || colonne == "libelle_F")
                        temp = new MySqlParameter("@value", Conversion_Type("enum"));
                    else
                        temp = new MySqlParameter("@value", Conversion_Type(value.GetType().ToString()));
                    temp.Value = value;

                    MySqlParameter tempbis;
                    if (colonnecond == "type_V")
                        tempbis = new MySqlParameter("@valuecond", Conversion_Type("enum"));
                    else
                        tempbis = new MySqlParameter("@valuecond", Conversion_Type(valuecond.GetType().ToString()));
                    tempbis.Value = valuecond;

                    string commandtext = "UPDATE " + table + " SET " + colonne + " = @value WHERE " + colonnecond + " = @valuecond;";
                    MySqlCommand command = connection.CreateCommand();
                    command.Parameters.Add(temp);
                    command.Parameters.Add(tempbis);
                    command.CommandText = commandtext;
                    Reader(command, table);
                    command.Dispose();
                    return commandtext;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
                return null;

        }


        /// <summary>
        /// Retourne la recherche d'un élément en fonction d'une condition. Permet aussi d'afficher le stock des pièces selon une condition.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="colonne"></param>
        /// <param name="value"></param>
        /// <param name="research"></param>
        /// <param name="colonnesAffichage"></param>
        /// <returns></returns>
        public DataTable Research_Stock(string table, string colonne, object value, bool research, string colonnesAffichage= null)
        {
            if (value != null)
            {
                MySqlParameter temp;
                if (colonne == "type_V" || colonne == "libelle_F")
                    temp = new MySqlParameter("@value", MySqlDbType.Enum);
                else
                    temp = new MySqlParameter("@value", Conversion_Type(value.GetType().ToString()));
                temp.Value = Convert.ToString(value);
                string commandtext;
                if (research)
                {
                    commandtext = "SELECT * FROM " + table + " WHERE " + colonne + " = @value;";
                }
                else
                    commandtext = "SELECT " + colonnesAffichage + " FROM " + table + " WHERE " + colonne + " = @value;";
                MySqlCommand command = connection.CreateCommand();
                command.Parameters.Add(temp);
                command.CommandText = commandtext;
                DataTable dt;
                if (research)
                    dt = Reader(command, table);
                else
                    dt = Reader(command, table, colonnesAffichage.Split(","), false);
                command.Dispose();
                return dt;
            }
            else
                return null;
        }


        /// <summary>
        /// Retiurne la dure du programme de fidelité en fonction du numéro de ce programme. 
        /// </summary>
        /// <param name="numfid"></param>
        /// <returns></returns>
        public int Return_DureeFid(int numfid)
        {
            MySqlParameter id = new MySqlParameter("@value", Conversion_Type(numfid.GetType().ToString()));
            id.Value = numfid;
            string commandtext;
            commandtext = "SELECT duree_fid FROM Fidelio WHERE num_fid = @value ;";
            MySqlCommand command = connection.CreateCommand();
            command.Parameters.Add(id);
            command.CommandText = commandtext;
            MySqlDataReader reader = command.ExecuteReader();
            int duree=0;
            while (reader.Read()) { duree = reader.GetInt32(0); }
            command.Dispose();
            reader.Close();
            return duree;
        }


        /// <summary>
        /// Sélectionne le meilleur client particulier selon le nombre d'item commandée.  
        /// </summary>
        /// <returns></returns>
        public List<string> Best_Client_Item()
        {
            string commandtext = "SELECT nom_I, prenom_I, sum(quantite_C) "+
                "FROM Individu NATURAL JOIN Commande NATURAL JOIN CommandeLigne GROUP BY nom_I " +
                "HAVING sum(quantite_C) >= ( SELECT quantity FROM( SELECT sum(quantite_C) as quantity "+
                "FROM Individu NATURAL JOIN Commande NATURAL JOIN CommandeLigne GROUP BY nom_I ORDER BY quantity DESC)  as X LIMIT 1 ); ";
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = commandtext;
            MySqlDataReader reader = command.ExecuteReader();
            List<string> individu = new List<string>();
            string quantite = "";
            while (reader.Read())
            {
                individu.Add(reader.GetString(0)+ " "+reader.GetString(1));
                quantite = reader.GetString(2);
            }
            command.Dispose();
            reader.Close();
            individu.Add(quantite);
            return individu;
        }


        /// <summary>
        /// Sélectionne le meilleur client particulier selon le prix total de commandes. 
        /// </summary>
        /// <returns></returns>
        public List<string> Best_Client_Prix()
        {
            string commandtext = "SELECT nom_I, prenom_I, priceT FROM ( "+
                                "SELECT nom_I, prenom_I, sum(price) as priceT FROM( "+
                                "( "+
                                "SELECT nom_I, prenom_I, sum(quantite_C * prixU_P) as price "+
                                "FROM Individu NATURAL JOIN Commande NATURAL JOIN CommandeLigne NATURAL JOIN Piece "+
                                "GROUP BY nom_I "+
                                ") UNION "+
                                "( "+
                                "SELECT nom_I, prenom_I, sum(quantite_C * prixU_V) as price "+
                                "FROM Individu NATURAL JOIN Commande NATURAL JOIN CommandeLigne NATURAL JOIN Velo "+
                                "GROUP BY nom_I "+
                                ")) as total GROUP BY nom_I, prenom_I "+
                                ") as X WHERE priceT >= "+
                                "(SELECT max(priceT) FROM "+
                                "(SELECT sum(price) as priceT FROM( "+
                                "( "+
                                "SELECT nom_I, prenom_I, sum(quantite_C * prixU_P) as price "+
                                "FROM Individu NATURAL JOIN Commande NATURAL JOIN CommandeLigne NATURAL JOIN Piece "+
                                "GROUP BY nom_I "+
                                ") UNION "+
                                "( "+
                                "SELECT nom_I, prenom_I, sum(quantite_C * prixU_V) as price "+
                                "FROM Individu NATURAL JOIN Commande NATURAL JOIN CommandeLigne NATURAL JOIN Velo "+
                                "GROUP BY nom_I "+
                                ")) as total GROUP BY nom_I, prenom_I "+
                                ") as X); ";
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = commandtext;
            MySqlDataReader reader = command.ExecuteReader();
            List<string> individu = new List<string>();
            string quantite = "";
            while (reader.Read())
            {
                individu.Add(reader.GetString(0) + " " + reader.GetString(1));
                quantite = reader.GetString(2);
            }
            command.Dispose();
            reader.Close();
            individu.Add(quantite);
            return individu;
        }


        /// <summary>
        /// Retourne le prix moyen d'une commande. 
        /// </summary>
        /// <returns></returns>
        public int MoyennePrix_Commande()
        {
            string commandtext = "SELECT avg(priceT) FROM (" +
                                "SELECT num_C, sum(price) as priceT FROM ( " +
                                "("
                                + "SELECT num_C, sum(quantite_C * prixU_P) as price " +
                                "FROM Commande NATURAL JOIN CommandeLigne NATURAL JOIN Piece " +
                                "GROUP BY num_C " +
                                ") UNION " +
                                "(" +
                                "SELECT num_C, sum(quantite_C * prixU_V) as price " +
                                "FROM Commande NATURAL JOIN CommandeLigne NATURAL JOIN Velo " +
                                "GROUP BY num_C " +
                                ") UNION " +
                                "(" +
                                "SELECT num_C, sum(quantite_C * prixU_V) as price " +
                                "FROM Commande NATURAL JOIN CommandeLigne NATURAL JOIN Velo " +
                                "GROUP BY num_C " +
                                ") UNION " +
                                "( " +
                                "SELECT num_C, sum(quantite_C * prixU_P) as price " +
                                "FROM Commande NATURAL JOIN CommandeLigne NATURAL JOIN Piece " +
                                "GROUP BY num_C " +
                                ") " +
                                ") as total GROUP BY num_C ORDER BY num_C) as averagePriceCommand; ";
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = commandtext;
            MySqlDataReader reader = command.ExecuteReader();
            int mean = 0;
            while (reader.Read())
            {
                mean = reader.GetInt32(0);
            }
            command.Dispose();
            reader.Close();
            return mean;
        }


        /// <summary>
        /// Reourne le nombre de vélo moyen par commande
        /// </summary>
        /// <returns></returns>
        public int MoyenneVelo_Commande()
        {
            string commandtext = "SELECT avg(qteC) FROM ( "+
                                "SELECT num_C, sum(qteV) as qteC FROM( "+
                                "( "+
                                "SELECT num_C, sum(quantite_C) as qteV "+ 
                                "FROM Commande NATURAL JOIN CommandeLigne NATURAL JOIN Velo "+
                                "GROUP BY num_C "+
                                ") UNION "+
                                "( "+
                                "SELECT num_C, sum(quantite_C) as qteV "+
                                "FROM Commande NATURAL JOIN CommandeLigne NATURAL JOIN Velo "+
                                "GROUP BY num_C "+
                                ") "+
                                ") as total GROUP BY num_C ORDER BY num_C "+
                                ") as averagePieceCommand; ";
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = commandtext;
            MySqlDataReader reader = command.ExecuteReader();
            int mean = 0;
            while (reader.Read())
            {
                mean = reader.GetInt32(0);
            }
            command.Dispose();
            reader.Close();
            return mean;
        }

        /// <summary>
        /// Retourne le nombre de pièce moyen par commande
        /// </summary>
        /// <returns></returns>
        public double MoyennePiece_Commande()
        {
            string commandtext = "SELECT avg(qteC) FROM ( "+
                                "SELECT num_C, sum(qteP) as qteC FROM( "+
                                "( "+
                                "SELECT num_C, sum(quantite_C) as qteP "+
                                "FROM Commande NATURAL JOIN CommandeLigne NATURAL JOIN Piece "+
                                "GROUP BY num_C "+
                                ") UNION "+
                                "( "+
                                "SELECT num_C, sum(quantite_C) as qteP "+
                                "FROM Commande NATURAL JOIN CommandeLigne NATURAL JOIN Piece "+
                                "GROUP BY num_C "+
                                ") "+
                                ") as total GROUP BY num_C ORDER BY num_C) as averagePieceCommand; ";
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = commandtext;
            MySqlDataReader reader = command.ExecuteReader();
            double mean = 0;
            while (reader.Read())
            {
                mean = reader.GetDouble(0);
            }
            command.Dispose();
            reader.Close();
            return mean;
        }



        /// <summary>
        /// Dresse la liste des pièces ou des vélos ainsi que leur nombre de vente. 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="colonnesAffichage"></param>
        /// <returns></returns>
        public DataTable item_sold(string table, string colonnesAffichage)
        {
            string commandtext = "";
            if(table =="Velo")
                commandtext = "SELECT num_V, nom_V, count(*) FROM velo NATURAL JOIN commandeLigne GROUP BY num_V;";
            else
                commandtext = "SELECT num_P, description_P, count(*) FROM Piece NATURAL JOIN commandeLigne GROUP BY num_P;";
            MySqlCommand commande = connection.CreateCommand();
            commande.CommandText = commandtext;
            DataTable dt = Reader(commande, table, colonnesAffichage.Split(","), false);
            return dt;

        }

        /// <summary>
        /// Export xml des pièces dont le stock est faible
        /// </summary>
        public void exportxmlstockfaible()
        {
            string requete = "SELECT num_P,stock_P,nomfourni_P FROM piece WHERE stock_P<6 ORDER BY num_P";
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = requete;

            XmlDocument docXml = new XmlDocument();
            XmlElement racine = docXml.CreateElement("FichierXml");
            docXml.AppendChild(racine);
            XmlDeclaration xmldecl = docXml.CreateXmlDeclaration("1.0", "UTF-8", "no");
            docXml.InsertBefore(xmldecl, racine);

            XmlElement piece = docXml.CreateElement("piece");
            XmlElement num_P = docXml.CreateElement("num_P");
            XmlElement stock_P = docXml.CreateElement("stock_P");
            XmlElement nomfourni_P = docXml.CreateElement("nomfourni_P");

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                num_P.InnerText = reader.GetValue(0).ToString();
                piece.AppendChild(num_P);
                stock_P.InnerText = reader.GetValue(1).ToString();
                piece.AppendChild(stock_P);
                nomfourni_P.InnerText = reader.GetValue(2).ToString();
                piece.AppendChild(nomfourni_P);
                racine.AppendChild(piece);
                piece = docXml.CreateElement("piece");
                num_P = docXml.CreateElement("numProduit");
                stock_P = docXml.CreateElement("stockPiece");
                nomfourni_P = docXml.CreateElement("nomFournisseur");
            }
            docXml.Save("stock_faible.xml");
        }


        /// <summary>
        /// Exportation en JSON des clients dont les adhesion Fidélio prennent fin. 
        /// </summary>
        public class ResultJson
        {
            public string nom { get; set; }
            public string prenom { get; set; }
            public DateTime date { get; set; }
            public int num_fid{ get; set; }
        }

        public void exportjsonfinadhe()
        {
            string file = "";
            string requete = "SELECT nom_I, prenom_I, DateFin, num_fid FROM Individu NATURAL JOIN Adhesion WHERE DateFin < ADDDATE(CURDATE(), INTERVAL 2 MONTH);";
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                var indiv = new ResultJson
                {
                    nom = Reader.GetString(0),
                    prenom = Reader.GetString(1),
                    date = Reader.GetDateTime(2),
                    num_fid = Reader.GetInt32(3)
                };
            var options = new JsonSerializerOptions { WriteIndented = true };
            file += JsonSerializer.Serialize(indiv, options) +",\n";
            }
            File.WriteAllText("echeance_adhesion.json", file);

        }



        /// <summary>
        /// Permet la modification des délais de livraison de la ommande en fonction des stocks. 
        /// </summary>
        /// <param name="numcommande"></param>
        /// <param name="delaisupp"></param>
        public void delaiL(int numcommande, int delaisupp)
        {
            MySqlParameter temp = new MySqlParameter("@valuecommande", Conversion_Type("int"));
            temp.Value = numcommande;
            MySqlParameter tempbis = new MySqlParameter("@delai", Conversion_Type("int"));
            tempbis.Value = delaisupp;

            string commandtext1 = "SELECT DATE_ADD(date_C, INTERVAL @delai+5 DAY) FROM commande WHERE num_C= @valuecommande;";
            MySqlCommand command1 = connection.CreateCommand();
            command1.Parameters.Add(temp);
            command1.Parameters.Add(tempbis);
            command1.CommandText = commandtext1;
            MySqlDataReader reader1 = command1.ExecuteReader();
            DateTime delaitemp =new DateTime();
            while (reader1.Read()) { delaitemp = reader1.GetDateTime(0); }
            command1.Dispose();
            reader1.Close();

            string commandtext2 = "SELECT dateL_C FROM commande WHERE num_C= @valuecommande;";
            MySqlCommand command2 = connection.CreateCommand();
            command2.Parameters.Add(temp);
            command2.CommandText = commandtext2;
            MySqlDataReader reader2 = command2.ExecuteReader();
            DateTime delaiactuelle = new DateTime();
            while (reader2.Read()) { delaiactuelle = reader2.GetDateTime(0); }
            command2.Dispose();
            reader2.Close();

            if(delaiactuelle<delaitemp)
            {
                MySqlParameter newdate = new MySqlParameter("@newdate", MySqlDbType.Date);
                newdate.Value = delaitemp;
                MySqlCommand command3 = connection.CreateCommand();
                string commandtext3 = "UPDATE Commande SET dateL_C = @newdate WHERE num_C = @valuecommande;";
                command3.Parameters.Add(temp);
                command3.Parameters.Add(newdate);
                command3.CommandText = commandtext3;
                Reader(command3, "Commande");
                command3.Dispose();
            }
        }

        /// <summary>
        /// Récupère le délai de livraison d'une commande depuis chez le fournisseur. 
        /// </summary>
        /// <param name="num_P"></param>
        /// <returns></returns>
        public int delaiLsupp(string num_P)
        {
            MySqlParameter temp = new MySqlParameter("@num_P", Conversion_Type("string"));
            temp.Value = num_P;

            string commandtext = "SELECT delaiL_P From piece WHERE num_P = @num_P;";
            MySqlCommand command1 = connection.CreateCommand();
            command1.Parameters.Add(temp);
            command1.CommandText = commandtext;
            MySqlDataReader reader = command1.ExecuteReader();
            int delai = 0;
            while (reader.Read()) { delai = reader.GetInt32(0); }
            command1.Dispose();
            reader.Close();
            return delai;
        }

    }
}

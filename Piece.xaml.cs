using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;

namespace WPF_Probleme
{
    /// <summary>
    /// Logique d'interaction pour Piece.xaml
    /// </summary>
    public partial class Piece : Window
    {
        string user;
        MainClass mainclass;
        public Piece(string user)
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            mainclass = new MainClass(user);
            this.user = user;
        }

        private void Show_Pieces_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = mainclass.AffichageTable("Piece");
            MyData.DataContext = dt;
        }

        private void SelectAction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Stock_Search_Piece_Form.Visibility == Visibility.Hidden) { Stock_Search_Piece_Form.Visibility = Visibility.Visible; }
            else if(Stock_Search_Piece_Form.Visibility==Visibility.Visible){ Stock_Search_Piece_Form.Visibility = Visibility.Collapsed; }
            if (Add_Piece_Form.Visibility == Visibility.Visible) { Add_Piece_Form.Visibility = Visibility.Collapsed; }
            if (Delete_Piece_Form.Visibility == Visibility.Visible) { Delete_Piece_Form.Visibility = Visibility.Collapsed; }
            if (Modify_Piece_Form.Visibility == Visibility.Visible) { Modify_Piece_Form.Visibility = Visibility.Collapsed; }
            if (Supply_Piece_Form.Visibility == Visibility.Visible) { Supply_Piece_Form.Visibility = Visibility.Collapsed; }
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (choice == "Add") { Add_Piece_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Ajouter une Pièce"; }
            if (choice == "Delete") { Delete_Piece_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Supprimer une Pièce"; }
            if (choice == "Modify") { Modify_Piece_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Modifier une Pièce"; }
            if (choice == "Research") { Stock_Search_Piece_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Rechercher une Pièce"; }
            if (choice == "Stock Overview") { Stock_Search_Piece_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Aperçu du Sotck"; }
            if (choice == "Supply") { Supply_Piece_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Commander des Pièces"; }
        }

        private void SelectConditions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (choice == "Numéro de Pièce" || choice == "") { TextBlockConditionP.Text = "Numéro de Pièce"; }
            if (choice == "Description") { TextBlockConditionP.Text = "Description"; }
            if (choice == "Nom Pièce Fournisseur") { TextBlockConditionP.Text = "Nom Pièce Fournisseur"; }
            if (choice == "Numéro Pièce Fournisseur") { TextBlockConditionP.Text = "Numéro Pièce Fournisseur"; }
            if (choice == "Prix") { TextBlockConditionP.Text = "Prix"; }
            if (choice == "Délais") { TextBlockConditionP.Text = "Délais"; }

        }

        private void SelectChange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (choice == "Numéro de Pièce" || choice == "") { TextBlockConditionP.Text = "Numéro de Pièce"; }
            if (choice == "Description") { TextBlockUpdateP.Text = "Description"; }
            if (choice == "Nom Pièce Fournisseur") { TextBlockUpdateP.Text = "Nom Pièce Fournisseur"; }
            if (choice == "Numéro Pièce Fournisseur") { TextBlockUpdateP.Text = "Numéro Pièce Fournisseur"; }
            if (choice == "Prix") { TextBlockUpdateP.Text = "Prix"; }
            if (choice == "Délais") { TextBlockUpdateP.Text = "Délais"; }
        }

        private void SelectStockSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (choice == "Numéro de Pièce" || choice == "") { TextBlockStockSearchP.Text = "Numéro de Pièce"; }
            else { TextBlockStockSearchP.Text = choice; }
        }
        private void Velos_Click(object sender, RoutedEventArgs e)
        {
            Velo velo = new Velo(user);
            velo.Show();
            this.Close();
        }
        private void Fournisseurs_Click(object sender, RoutedEventArgs e)
        {
            Fournisseur fourni = new Fournisseur(user);
            fourni.Show();
            this.Close();
        }
        private void Clients_Click(object sender, RoutedEventArgs e)
        {
            Client client = new Client(user);   
            client.Show();
            this.Close();
        }
        private void Commandes_Click(object sender, RoutedEventArgs e)
        {
            Commandes commande = new Commandes(user);
            commande.Show();
            this.Close();
        }


        private void Fidelio_Click(object sender, RoutedEventArgs e)
        {
            Fidelio fidelio = new Fidelio(user);
            fidelio.Show();
            this.Close();
        }
        private void Add_Click_Piece(object sender, RoutedEventArgs e)
        {
            if (user == "root")
            {
                string[] value = new string[] { numP.Text, descrP.Text, NomPF.Text, NumPF.Text, Prix.Text, Delai.Text, Stock.Text, dateintro_add.Text, datedisc_add.Text };
                string result = mainclass.Creation("Piece", value);
                if (result == null)
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                DataTable dt = mainclass.AffichageTable("Piece");
                MyData.DataContext = dt;
            }
            else
                MessageBox.Show("Vous n'avez pas les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        private void Delete_Click_Piece(object sender, RoutedEventArgs e)
        {
            if(user =="root")
            {
                string code = numP_del.Text;
                string result = mainclass.Delete("Piece", "num_P", code);
                if (result == null)
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                DataTable dt = mainclass.AffichageTable("Piece");
                MyData.DataContext = dt;
            }
            else
                MessageBox.Show("Vous n'avez pas les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        private void Update_Click_Piece(object sender, RoutedEventArgs e)
        {
            if(user =="root")
            {
                ComboBoxItem temp = SelectConditions.SelectedItem as ComboBoxItem;
                string colonnecond = Column_From_Combo(temp);
                object valuecond = Value_From_Combo(temp, TextBoxConditionP);

                temp = SelectChange.SelectedItem as ComboBoxItem;
                string colonne = Column_From_Combo(temp);
                object value = Value_From_Combo(temp, TextBoxUpdateP);

                string result = mainclass.Update("Piece", colonne, value, colonnecond, valuecond);
                if (result == null)
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                DataTable dt = mainclass.AffichageTable("Piece");
                MyData.DataContext = dt;
            }
            else
                MessageBox.Show("Vous n'avez pas les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

        }


        private void Stock_Search_Click_Piece(object sender, RoutedEventArgs e)
        {
            ComboBoxItem temp = SelectStockSearch.SelectedItem as ComboBoxItem;
            string column = Column_From_Combo(temp);
            object value = Value_From_Combo(temp, TextBoxStockSearchP);
            DataTable dt;
            if (Title_Modif.Content == "Aperçu du Stock")
                dt = mainclass.Research_Stock("Piece", column, value, false, "num_p,description_P,stock_P");
            else
                dt = mainclass.Research_Stock("Piece", column, value, true, null);
            if (dt == null)
                MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            MyData.DataContext = dt;
        }

        private string Column_From_Combo(ComboBoxItem cboxitem)
        {
            string column = "";
            string choice = "";
            if (!(cboxitem.Content is null)) { choice = cboxitem.Content.ToString(); }
            if (choice == "Numéro de Pièce" || choice == "") { column = "num_P"; }
            else if (choice == "Description") { column = "description_P"; }
            else if (choice == "Nom Pièce Fournisseur") { column = "nomfourni_P"; }
            else if (choice == "Numéro Pièce Fournisseur") { column = "numcatafourni_P"; }
            else if (choice == "Prix") { column = "prixU_P"; }
            else if (choice == "Délais") { column = "delaiL_P"; }
            else if(choice=="stockP") { column = "stock_P"; }
            return column;
        }

        private object Value_From_Combo(ComboBoxItem cboxitem, TextBox source)
        {
            try
            {
                object value;
                string choice = "";
                if (!(cboxitem.Content is null)) { choice = cboxitem.Content.ToString(); }
                if (choice == "Numéro de Pièce" || choice == "") { value = Convert.ToString(source.Text); }
                else if (choice == "Description") { value = Convert.ToString(source.Text); }
                else if (choice == "Nom Pièce Fournisseur") { value = Convert.ToString(source.Text); }
                else if (choice == "Numéro Pièce Fournisseur") { value = Convert.ToString(source.Text); }
                else if (choice == "Prix") { value = Convert.ToString(source.Text); }
                else if (choice == "Délais") { value = Convert.ToInt32(source.Text); }
                else if (choice == "stock_P") { value = Convert.ToInt32(source.Text); }
                else { value = ""; }
                return value;
            }
            catch(Exception ex)
            {
                return null;
            }

        }

        private void Search_Supplier_Click_Piece(object sender, RoutedEventArgs e)
        {
            DataTable dt;
            dt = mainclass.Research_Stock("Piece", "num_P ", TextBoxSupplyPnumP.Text, false, "num_P,description_P,nomfourni_P");
            if (dt == null)
                MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            MyData.DataContext = dt;
        }

        private void Command_Click_Piece(object sender, RoutedEventArgs e)
        {
            if(user =="root")
            {
                DataTable dt;
                string result = mainclass.UpdateStock(TextBoxSupplyPnumP.Text, Convert.ToInt32(TextBoxSupplyPquantiteP.Text), "+");
                if (result == null)
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                dt = mainclass.AffichageTable("Approvisionnement");
                MyData.DataContext = dt;
            }
            else
                MessageBox.Show("Vous n'avez pas les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void ExportPFXML_Click(object sender, RoutedEventArgs e)
        {
            mainclass.exportxmlstockfaible();
            MessageBox.Show("Création du fichier stock_faible.xml réussie !");
        }

        public void Item_Sold_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt;
            dt = mainclass.item_sold("Piece", "num_P,description_P,count(*)");
            MyData.DataContext = dt;
        }

    }
}

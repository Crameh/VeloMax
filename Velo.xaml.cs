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
using System.Data;

namespace WPF_Probleme
{
    /// <summary>
    /// Logique d'interaction pour Velo.xaml
    /// </summary>
    public partial class Velo : Window
    {
        string user;
        MainClass mainclass;
        public Velo(string user)
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            mainclass = new MainClass(user);
            this.user = user;
        }

        private void SelectAction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Stock_Search_Velo_Form.Visibility == Visibility.Hidden) { Stock_Search_Velo_Form.Visibility = Visibility.Visible; }
            else if (Stock_Search_Velo_Form.Visibility == Visibility.Visible) { Stock_Search_Velo_Form.Visibility = Visibility.Collapsed; }
            if (Delete_Velo_Form.Visibility == Visibility.Visible) { Delete_Velo_Form.Visibility = Visibility.Collapsed; }
            if (Modify_Velo_Form.Visibility == Visibility.Visible) { Modify_Velo_Form.Visibility = Visibility.Collapsed; }
            if (Add_Velo_Form.Visibility == Visibility.Visible) { Add_Velo_Form.Visibility = Visibility.Collapsed; }
            if (Contenu_Velo_Form.Visibility == Visibility.Visible) { Contenu_Velo_Form.Visibility = Visibility.Collapsed; }
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (choice == "Add") { Add_Velo_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Ajouter un Vélo"; }
            if (choice == "Delete") { Delete_Velo_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Supprimer un Vélo"; }
            if (choice == "Modify") { Modify_Velo_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Modifier un Vélo"; }
            if (choice == "Research") { Stock_Search_Velo_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Rechercher un Vélo"; }
            if (choice == "Liste d'assemblage d'un Vélo") { Contenu_Velo_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Liste d'Assemblage du Vélo"; }
            if (choice == "Stock Overview") { Stock_Search_Velo_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Aperçu du Stock"; }
        }

        private void SelectConditions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (choice == "Numéro de Vélo" || choice == "") { TextBlockConditionV.Text = "Numéro de Vélo"; }
            else { TextBlockConditionV.Text = choice; }
        }

        private void SelectChange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (choice == "Numéro de Vélo" || choice == "") { TextBlockUpdateV.Text = "Numéro de Vélo"; }
            else { TextBlockUpdateV.Text = choice; }
        }

        private void SelectStockSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (choice == "Numéro de Vélo" || choice == "") { TextBlockStockSearchV.Text = "Numéro de Vélo"; }
            else { TextBlockStockSearchV.Text = choice; }
        }

        private void Show_Velos_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = mainclass.AffichageTable("Velo");
            MyData.DataContext = dt;
        }

        private void Pieces_Click(object sender, RoutedEventArgs e)
        {
            Piece piece = new Piece(user);
            piece.Show();
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
        private void Add_Click_Velo(object sender, RoutedEventArgs e)
        {
            if (user == "root")
            { 
                string[] value = new string[] { numV_add.Text, nomV_add.Text, tailleV_add.Text, prixUV_add.Text, typeV_add.Text, dateintro_add.Text, datedisc_add.Text };
                string result = mainclass.Creation("Velo", value);
                if(result == null)
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                DataTable dt = mainclass.AffichageTable("Velo");
                MyData.DataContext = dt;
            }
            else
                MessageBox.Show("Vous n'avez les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Delete_Click_Velo(object sender, RoutedEventArgs e)
        {
            if(user =="root")
            {
                string code = numV_del.Text;
                string result = mainclass.Delete("Velo", "num_V", code);
                if (result == null)
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                DataTable dt = mainclass.AffichageTable("Velo");
                MyData.DataContext= dt;
            }
            else
                MessageBox.Show("Vous n'avez les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        private void Contenu_Click_Velo(object sender, RoutedEventArgs e)
        {
            string code = numV_cont.Text;
            DataTable dt = mainclass.Construction("Construction", "num_V", code);
            if (dt == null)
                MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MyData.DataContext = dt;
        }


        private void Update_Click_Velo(object sender, RoutedEventArgs e)
        {
            if(user=="root")
            {
                ComboBoxItem temp = SelectConditions.SelectedItem as ComboBoxItem;
                string colonnecond = Column_From_Combo(temp);
                object valuecond = Value_From_Combo(temp, TextBoxConditionV);
                temp = SelectChange.SelectedItem as ComboBoxItem;
                string colonne = Column_From_Combo(temp);
                object value = Value_From_Combo(temp, TextBoxUpdateV);
                string result = mainclass.Update("Velo", colonne, value, colonnecond, valuecond);
                if (result == null)
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                DataTable dt = mainclass.AffichageTable("Velo");
                MyData.DataContext = dt;
            }
            else
                MessageBox.Show("Vous n'avez les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        private void Stock_Search_Click_Velo(object sender, RoutedEventArgs e)
        {
            ComboBoxItem temp = SelectStockSearch.SelectedItem as ComboBoxItem;
            string column = Column_From_Combo(temp);
            object value = Value_From_Combo(temp, TextBoxStockSearchV);
            DataTable dt;
            dt = mainclass.Research_Stock("Velo", column, value, true, null);
            if (dt == null)
                MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MyData.DataContext = dt;
        }


        private string Column_From_Combo(ComboBoxItem cboxitem)
        {
            string column = "";
            string choice = "";
            if (!(cboxitem.Content is null)) { choice = cboxitem.Content.ToString(); }
            if (choice == "Numéro de Vélo" || choice == "") { column = "num_V"; }
            else if (choice == "Nom de Vélo") { column = "nom_V"; }
            else if (choice == "Taille du Vélo") { column = "taille_V"; }
            else if (choice == "Prix Unitaire") { column = "prixU_V"; }
            else if (choice == "Type du Vélo") { column = "type_V"; }
            return column;
        }

        private object Value_From_Combo(ComboBoxItem cboxitem, TextBox source)
        {
            object value;
            string choice = "";
            try
            {
                if (!(cboxitem.Content is null)) { choice = cboxitem.Content.ToString(); }
                if (choice == "Numéro de Vélo" || choice == "") { value = Convert.ToInt32(source.Text); }
                else if (choice == "Nom de Vélo") { value = Convert.ToString(source.Text); }
                else if (choice == "Taille du Vélo") { value = Convert.ToString(source.Text); }
                else if (choice == "Prix Unitaire") { value = Convert.ToSingle(source.Text); }
                else if (choice == "Type du Vélo") { value = Convert.ToString(source.Text); }
                else { value = ""; }
                return value;
            }
            catch(Exception e)
            {
                return null;
            }

        }


        public void Item_Sold_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt;
            dt = mainclass.item_sold("Velo", "num_V,nom_V,count(*)");
            MyData.DataContext = dt;
        }
    }
}

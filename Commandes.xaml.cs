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
    /// Logique d'interaction pour Commandes.xaml
    /// </summary>
    public partial class Commandes : Window
    {
        MainClass mainclass;
        string user;
        public Commandes(string user)
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            mainclass = new MainClass(user);
            this.user = user;

        }


        private void Show_Commande_Click(object sender, RoutedEventArgs e)
        {
            MyData.DataContext = mainclass.AffichageTable("Commande"); ;
        }

        private void SelectAction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Search_Commande_Form.Visibility == Visibility.Hidden) { Search_Commande_Form.Visibility = Visibility.Visible; }
            else if (Search_Commande_Form.Visibility == Visibility.Visible) { Search_Commande_Form.Visibility = Visibility.Collapsed; }
            if (Add_Commande_Particulier_Form.Visibility == Visibility.Visible) { Add_Commande_Particulier_Form.Visibility = Visibility.Collapsed; }
            if (Add_Commande_Boutique_Form.Visibility == Visibility.Visible) { Add_Commande_Boutique_Form.Visibility = Visibility.Collapsed; }
            if (Delete_Commande_Form.Visibility == Visibility.Visible) { Delete_Commande_Form.Visibility = Visibility.Collapsed; }
            if (Modify_Commande_Form.Visibility == Visibility.Visible) { Modify_Commande_Form.Visibility = Visibility.Collapsed; }
            if (Contenu_Commande_Form.Visibility == Visibility.Visible) { Contenu_Commande_Form.Visibility = Visibility.Collapsed; }
            if(WhoCommand.Visibility == Visibility.Visible) { WhoCommand.Visibility = Visibility.Collapsed; }

            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if ((choice == "Add") && Boutiques_Selected.IsChecked == true) 
            {
                Add_Commande_Boutique_Form.Visibility = Visibility.Visible; 
                Title_Modif.Content = "Créer une commande Boutique";
                WhoCommand.Visibility = Visibility.Visible; 
            }
            if ((choice == "Add") && Particuliers_Selected.IsChecked == true) 
            { Add_Commande_Particulier_Form.Visibility = Visibility.Visible; 
                Title_Modif.Content = "Créer une commande Particulier";
                WhoCommand.Visibility= Visibility.Visible;
            }
            if (choice == "Delete") { Delete_Commande_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Supprimer une commande"; }
            if (choice == "Modify") { Modify_Commande_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Modifier une commande"; }
            if (choice == "Research") { Search_Commande_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Rechercher un commande"; }
            if (choice == "Contenu d'une Commande") { Contenu_Commande_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Contenu d'un commande"; }
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
        private void Vélos_Click(object sender, RoutedEventArgs e)
        {
            Velo velo = new Velo(user);
            velo.Show();
            this.Close();
        }
        private void Clients_Click(object sender, RoutedEventArgs e)
        {
            Client client = new Client(user);
            client.Show();
            this.Close();
        }

        private void Fidelio_Click(object sender, RoutedEventArgs e)
        {
            Fidelio fidelio = new Fidelio(user);
            fidelio.Show();
            this.Close();
        }

        private void Add_Click_Commande(object sender, RoutedEventArgs e)
        {
            if(user == "root")
            {
                string[] value = null;
                int numcommande;
                if (Particuliers_Selected.IsChecked == true)
                {
                    numcommande = Convert.ToInt32(numCP_add.Text);
                    value = new string[] { numCP_add.Text, dateCP_add.Text, null, null, nomP_add.Text, prenomP_add.Text };
                    string result = mainclass.Creation("Commande", value);
                    if (result == null)
                        MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    else
                    {
                        CommandeLigne commandeLigne = new CommandeLigne(numcommande, user);
                        commandeLigne.Show();
                        DataTable dt = mainclass.AffichageTable("Commande");
                        MyData.DataContext = dt;
                    }
                }
                else
                {
                    numcommande = Convert.ToInt32(numCB_add.Text);
                    value = new string[] { numCB_add.Text, dateCB_add.Text, null, nomB_add.Text, null, null };
                    string result = mainclass.Creation("Commande", value);
                    if (result == null)
                        MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    else
                    {
                        CommandeLigne commandeLigne = new CommandeLigne(numcommande, user);
                        commandeLigne.Show();
                        DataTable dt = mainclass.AffichageTable("Commande");
                        MyData.DataContext = dt;
                    }
                }
            }
            else
            {
                MessageBox.Show("Vous n'avez les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }


        private void Delete_Click_Commande(object sender, RoutedEventArgs e)
        {
            if(user =="root")
            {
                string num = numC_del.Text;
                string result = mainclass.Delete("Commande", "num_C", num);
                if (result == null)
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                DataTable dt = mainclass.AffichageTable("Commande");
                MyData.DataContext = dt;
            }
            else
                MessageBox.Show("Vous n'avez les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Contenu_Click_Commande(object sender, RoutedEventArgs e)
        {
            string num = numC_cont.Text;
            DataTable dt = mainclass.Contenu("CommandeLigne", "num_C", num);
            if (dt == null)
                MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            MyData.DataContext = dt;
        }

        private void Update_Click_Commande(object sender, RoutedEventArgs e)
        {
            if (user != "root")
            {
                try
                {
                    ComboBoxItem temp = SelectConditionsCo.SelectedItem as ComboBoxItem;
                    string colonnecond = Column_From_Combo(temp, "all");
                    object valuecond = Value_From_Combo(temp, TextBoxConditionCo, "all");

                    temp = SelectChangeCo.SelectedItem as ComboBoxItem;
                    string colonne = Column_From_Combo(temp, "all");
                    object value = Value_From_Combo(temp, TextBoxUpdateCo, "all");

                    mainclass.Update("Commande", colonne, value, colonnecond, valuecond);
                    DataTable dt = mainclass.AffichageTable("Commande");
                    MyData.DataContext = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
                MessageBox.Show("Vous n'avez les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void SelectChangeCo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (choice == "Numéro de commande" || choice == "") { TextBlockUpdateCo.Text = "Numéro de commande"; }
            else { TextBlockUpdateCo.Text = choice; }
        }

        private void SelectConditionsCo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (choice == "Numéro de commande" || choice == "") { TextBlockConditionCo.Text = "Numéro de commande"; }
            else { TextBlockConditionCo.Text = choice; }
        }

        private void Radio_Button_Type_Commande(object sender, RoutedEventArgs e)
        {
            ComboBoxItem temp = ((SelectAction as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (Particuliers_Selected.IsChecked == true)
                { Add_Commande_Boutique_Form.Visibility = Visibility.Collapsed; Add_Commande_Particulier_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Créer une commande Particulier"; }
            else
                { Add_Commande_Particulier_Form.Visibility = Visibility.Collapsed; Add_Commande_Boutique_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Créer une commande Boutique"; }
        }

        private void Search_Click_Commande(object sender, RoutedEventArgs e)
        {
            ComboBoxItem temp;
            string column = "";
            object value;
            DataTable dt;
            temp = SelectSearchCo.SelectedItem as ComboBoxItem;
            column = Column_From_Combo(temp, "all");
            value = Value_From_Combo(temp, TextBoxSearchCo, "all");
            dt = mainclass.Research_Stock("Commande", column, value, true, null);
            if (dt == null)
                MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            MyData.DataContext = dt;
        }

        private string Column_From_Combo(ComboBoxItem cboxitem, string type)
        {
            string column = "";
            string choice = "";
            if (type == "boutique")
            {
                if (!(cboxitem.Content is null)) { choice = cboxitem.Content.ToString(); }
                if (choice == "Numéro de Commande" || choice == "") { column = "num_C"; }
                else if (choice == "Nom Boutique") { column = "nom_B"; }
            }
            else if(type == "particulier")
            {
                if (!(cboxitem.Content is null)) { choice = cboxitem.Content.ToString(); }
                if (choice == "Numéro de Commande" || choice == "") { column = "num_C"; }
                else if (choice == "Nom Particulier") { column = "nom_I"; }
                else if (choice == "Prénom Particulier") { column = "prenom_I"; }
            }
            else
            {
                if (!(cboxitem.Content is null)) { choice = cboxitem.Content.ToString(); }
                if (choice == "Numéro de Commande" || choice == "") { column = "num_C"; }
                else if (choice == "Nom Boutique") { column = "nom_B"; }
                else if (choice == "Nom Particulier") { column = "nom_I"; }
                else if (choice == "Prénom Particulier") { column = "prenom_I"; }
            }
            return column;
        }

        private object Value_From_Combo(ComboBoxItem cboxitem, TextBox source, string type)
        {
            try
            {
                object value;
                string choice = "";
                if (type == "boutique")
                {
                    if (!(cboxitem.Content is null)) { choice = cboxitem.Content.ToString(); }
                    if (choice == "Numéro de Commande" || choice == "") { value = Convert.ToInt32(source.Text); }
                    else if (choice == "Nom Boutique") { value = Convert.ToString(source.Text); }
                    else { value = ""; }
                }
                else if (type == "particulier")
                {
                    if (!(cboxitem.Content is null)) { choice = cboxitem.Content.ToString(); }
                    if (choice == "Numéro de Commande" || choice == "") { value = Convert.ToInt32(source.Text); }
                    else if (choice == "Nom Particulier") { value = Convert.ToString(source.Text); }
                    else if (choice == "Prénom Particulier") { value = Convert.ToString(source.Text); }
                    else { value = ""; }
                }
                else
                {
                    if (!(cboxitem.Content is null)) { choice = cboxitem.Content.ToString(); }
                    if (choice == "Numéro de Commande" || choice == "") { value = Convert.ToInt32(source.Text); }
                    else if (choice == "Nom Boutique") { value = Convert.ToString(source.Text); }
                    else if (choice == "Nom Particulier") { value = Convert.ToString(source.Text); }
                    else if (choice == "Prénom Particulier") { value = Convert.ToString(source.Text); }
                    else { value = ""; }
                }
                return value;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public void Moyenne_Prix_Commande_Click(object sender, RoutedEventArgs e)
        {
            int mean = mainclass.MoyennePrix_Commande();
            MessageBox.Show("Le prix moyen des commandes passées chez VeloMax est " + mean+"€", "Prix Moyen");
        }

        public void Moyenne_Velo_Commande_Click(object sender, RoutedEventArgs e)
        {
            int mean = mainclass.MoyenneVelo_Commande();
            MessageBox.Show("En moyenne, il y a "+mean+" vélos dans chaques commandes.", "Nombre de Vélos");
        }

        public void Moyenne_Piece_Commande_Click(object sender, RoutedEventArgs e)
        {
            double mean = mainclass.MoyennePiece_Commande();
            MessageBox.Show("En moyenne, il y a "+Math.Round(mean,2)+" pièces dans chaques commandes.", "Nombre de Pièces");
        }
    }
}

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
    /// Logique d'interaction pour Fournisseur.xaml
    /// </summary>
    public partial class Fournisseur : Window
    {
        MainClass mainclass;
        string user;
        public Fournisseur(string user)
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            mainclass = new MainClass(user);
            this.user = user;   
        }


        private void Show_Fournisseur_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt;
            dt = mainclass.AffichageTable("Fournisseur");
            MyData.DataContext = dt;
        }

        private void SelectAction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Add_Fournisseur_Form.Visibility == Visibility.Hidden) { Add_Fournisseur_Form.Visibility = Visibility.Visible; }
            else if (Add_Fournisseur_Form.Visibility == Visibility.Visible) { Add_Fournisseur_Form.Visibility = Visibility.Collapsed; }
            if (Delete_Fournisseur_Form.Visibility == Visibility.Visible) { Delete_Fournisseur_Form.Visibility = Visibility.Collapsed; }
            if (Modify_Fournisseur_Form.Visibility == Visibility.Visible) { Modify_Fournisseur_Form.Visibility = Visibility.Collapsed; }
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (choice == "Add") { Add_Fournisseur_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Ajouter un Fournisseur"; }
            if (choice == "Delete") { Delete_Fournisseur_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Supprimer un Fournisseur"; }
            if (choice == "Modify") { Modify_Fournisseur_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Modifier un Fournisseur"; }
        }

        private void Vélos_Click(object sender, RoutedEventArgs e)
        {
            Velo velo = new Velo(user);
            velo.Show();
            this.Close();
        }
        private void Pieces_Click(object sender, RoutedEventArgs e)
        {
            Piece piece = new Piece(user);
            piece.Show();
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

        private void Add_Click_Fournisseur(object sender, RoutedEventArgs e)
        {
            if (user == "root")
            {
                string[] value = new string[] { siretF_add.Text, nomF_add.Text, contactF_add.Text, rueF_add.Text, villeF_add.Text, CPF_add.Text, qualiteF_add.Text };
                string result = mainclass.Creation("Fournisseur", value);
                if (result == null)
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                DataTable dt = mainclass.AffichageTable("Fournisseur");
                MyData.DataContext = dt;
            }
            else
                MessageBox.Show("Vous n'avez pas les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        private void Delete_Click_Fournisseur(object sender, RoutedEventArgs e)
        {
            if(user == "root")
            {
                string siret = siretF_del.Text;
                string result = mainclass.Delete("Fournisseur", "siret_F", siret);
                if(result == null)
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                DataTable dt = mainclass.AffichageTable("Fournisseur");
                MyData.DataContext = dt;
            }
            else
                MessageBox.Show("Vous n'avez pas les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        private void Update_Click_Fournisseur(object sender, RoutedEventArgs e)
        {
            if(user=="root")
            {
                try
                {
                    string colonnecond = "";
                    string colonne = "";
                    object value;
                    object valuecond;

                    ComboBoxItem temp = SelectConditionsF.SelectedItem as ComboBoxItem;
                    string choice = "";
                    if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
                    if (choice == "Numéro de SIRET" || choice == "") { colonnecond = "siret_F"; valuecond = Convert.ToString(TextBoxConditionF.Text); }
                    else if (choice == "Nom") { colonnecond = "nom_F"; valuecond = Convert.ToString(TextBoxConditionF.Text); }
                    else if (choice == "Contact") { colonnecond = "contact_F"; valuecond = Convert.ToString(TextBoxConditionF.Text); }
                    else if (choice == "Rue") { colonnecond = "rue_F"; valuecond = Convert.ToString(TextBoxConditionF.Text); }
                    else if (choice == "Ville") { colonnecond = "ville_F"; valuecond = Convert.ToString(TextBoxConditionF.Text); }
                    else if (choice == "Code Postal") { colonnecond = "CP_F"; valuecond = Convert.ToInt32(TextBoxConditionF.Text); }
                    else if (choice == "Département") { colonnecond = "departement_F"; valuecond = Convert.ToString(TextBoxConditionF.Text); }
                    else if (choice == "Qualité") { colonnecond = "libelle_F"; valuecond = Convert.ToString(TextBoxConditionF.Text); }
                    else { valuecond = ""; }

                    temp = SelectChangeF.SelectedItem as ComboBoxItem;
                    choice = "";
                    if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
                    if (choice == "Numéro de SIRET" || choice == "") { colonne = "siret_F"; value = Convert.ToString(TextBoxUpdateF.Text); }
                    else if (choice == "Nom") { colonne = "nom_F"; value = Convert.ToString(TextBoxUpdateF.Text); }
                    else if (choice == "Contact") { colonne = "email_F"; value = Convert.ToString(TextBoxUpdateF.Text); }
                    else if (choice == "Rue") { colonne = "rue_F"; value = Convert.ToString(TextBoxUpdateF.Text); }
                    else if (choice == "Ville") { colonne = "ville_F"; value = Convert.ToString(TextBoxUpdateF.Text); }
                    else if (choice == "Code Postal") { colonne = "CP_F"; value = Convert.ToInt32(TextBoxUpdateF.Text); }
                    else if (choice == "Département") { colonne = "departement_F"; value = Convert.ToString(TextBoxUpdateF.Text); }
                    else if (choice == "Qualité") { colonne = "libelle_F"; value = Convert.ToString(TextBoxUpdateF.Text); }
                    else { value = ""; }

                    mainclass.Update("Fournisseur", colonne, value, colonnecond, valuecond);
                    DataTable dt = mainclass.AffichageTable("Fournisseur");
                    MyData.DataContext = dt;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Erreur, vérifiez vos entrées !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            else
                MessageBox.Show("Vous n'avez pas les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }



        private void SelectChangeF_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (choice == "Numéro de SIRET" || choice == "") { TextBlockUpdateF.Text = "Numéro de SIRET"; }
            else { TextBlockUpdateF.Text = choice; }
        }

        private void SelectConditionsF_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (choice == "Numéro de SIRET" || choice == "") { TextBlockConditionF.Text = "Numéro de SIRET"; }
            else { TextBlockConditionF.Text = choice; }
        }
    }
}

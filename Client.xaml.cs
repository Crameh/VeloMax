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
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Shapes;

namespace WPF_Probleme
{
    /// <summary>
    /// Logique d'interaction pour Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        MainClass mainclass;
        string user;
        public Client(string user)
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            mainclass = new MainClass(user);
            this.user = user;
        }


        private void Show_Client_Click(object sender, RoutedEventArgs e)
        {
            mainclass = new MainClass(user); 
            DataTable dt;
            if (Particuliers_Selected.IsChecked == true)
                dt = mainclass.AffichageTable("Individu");
            else
                dt = mainclass.AffichageTable("Boutique");
            MyData.DataContext = dt;
        }

        private void SelectAction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Search_Client_Form.Visibility == Visibility.Hidden) { Search_Client_Form.Visibility = Visibility.Visible; }
            else if (Search_Client_Form.Visibility == Visibility.Visible) { Search_Client_Form.Visibility = Visibility.Collapsed; }
            if (Add_Boutique_Form.Visibility == Visibility.Visible) { Add_Boutique_Form.Visibility = Visibility.Collapsed; }
            if (Add_Particulier_Form.Visibility == Visibility.Visible) { Add_Particulier_Form.Visibility = Visibility.Collapsed; }
            if (Delete_Boutique_Form.Visibility == Visibility.Visible) { Delete_Boutique_Form.Visibility = Visibility.Collapsed; }
            if (Delete_Particulier_Form.Visibility == Visibility.Visible) { Delete_Particulier_Form.Visibility = Visibility.Collapsed; }
            if (Modify_Boutique_Form.Visibility == Visibility.Visible) { Modify_Boutique_Form.Visibility = Visibility.Collapsed; }
            if (Modify_Particulier_Form.Visibility == Visibility.Visible) { Modify_Particulier_Form.Visibility = Visibility.Collapsed; }
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if ((choice == "Add") && Boutiques_Selected.IsChecked == true) { Add_Boutique_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Ajouter une Boutique"; }
            if ((choice == "Add") && Particuliers_Selected.IsChecked == true) { Add_Particulier_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Ajouter un Particulier"; }
            if (choice == "Delete" && Boutiques_Selected.IsChecked==true) { Delete_Boutique_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Supprimer une Boutique"; }
            if (choice == "Delete" && Particuliers_Selected.IsChecked == true) { Delete_Particulier_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Supprimer un Particulier"; }
            if (choice == "Modify" && Boutiques_Selected.IsChecked==true) { Modify_Boutique_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Modifier une Boutique"; }
            if (choice == "Modify" && Particuliers_Selected.IsChecked==true) { Modify_Particulier_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Modifier un Particulier"; }
            if (choice == "Research") { Search_Client_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Rechercher un Client"; }

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

        private void Add_Click_Boutique(object sender, RoutedEventArgs e)
        {
            if(user=="root")
            {
                string[] value = new string[] { nomB_add.Text, rueB_add.Text, villeB_add.Text, CPB_add.Text, telB_add.Text, emailB_add.Text, nomcontactB_add.Text, remiseB_add.Text };
                string result = mainclass.Creation("Boutique", value);
                if (result == null)
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                DataTable dt = mainclass.AffichageTable("Boutique");
                MyData.DataContext = dt;
            }
            else
                MessageBox.Show("Vous n'avez pas les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Add_Click_Particulier(object sender, RoutedEventArgs e)
        {
            if (user == "root")
            {
                string[] value = new string[] { nomI_add.Text, prenomI_add.Text, rueI_add.Text, villeI_add.Text, CPI_add.Text, telI_add.Text, emailI_add.Text };
                string result = mainclass.Creation("Individu", value);
                if (result == null)
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                DataTable dt = mainclass.AffichageTable("Individu");
                MyData.DataContext = dt;
                string choice = "";
                int numfid = 0;
                ComboBoxItem temp = FidelioChoice.SelectedItem as ComboBoxItem;
                if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
                if (choice == "Fidélio") { numfid = 1; }
                else if (choice == "Fidélio Or") { numfid = 2; }
                else if (choice == "Fidélio Platine") { numfid = 3; }
                else if (choice == "Fidélio Max") { numfid = 4; }
                DateTime start = DateTime.Now;
                string[] valuefid = new string[] { nomI_add.Text, prenomI_add.Text, numfid.ToString(), start.ToString(), start.AddYears(mainclass.Return_DureeFid(numfid)).ToString() };
                mainclass.Creation("Adhesion", valuefid);
            }
            else
                MessageBox.Show("Vous n'avez pas les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Delete_Click_Boutique(object sender, RoutedEventArgs e)
        {
            if (user == "root")
            {
                string code = nomB_del.Text;
                string result = mainclass.Delete("Boutique", "nom_B", code);
                if (result == null)
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                DataTable dt = mainclass.AffichageTable("Boutique");
                MyData.DataContext = dt;
            }
            else
                MessageBox.Show("Vous n'avez pas les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Delete_Click_Particulier(object sender, RoutedEventArgs e)
        {
            if (user == "root")
            {
                string nom = NomI_del.Text;
                string prenom = prenomI_del.Text;
                string result = mainclass.Delete("Individu", "nom_I", nom);
                if (result == null)
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                DataTable dt = mainclass.AffichageTable("Individu");
                MyData.DataContext = dt;
            }
            else
                MessageBox.Show("Vous n'avez pas les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        private void Update_Click_Particulier(object sender, RoutedEventArgs e)
        {
            if (user == "root")
            {
                ComboBoxItem temp = SelectConditionsI.SelectedItem as ComboBoxItem;
                string colonnecond = Column_From_Combo(temp, "particulier");
                object valuecond = Value_From_Combo(temp, TextBoxConditionI,"particulier");
                temp = SelectChangeI.SelectedItem as ComboBoxItem;
                string colonne = Column_From_Combo(temp, "particulier");
                object value = Value_From_Combo(temp, TextBoxUpdateI, "particulier");
                string result = mainclass.Update("Individu", colonne, value, colonnecond, valuecond);
                if (result == null)
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                DataTable dt = mainclass.AffichageTable("Individu");
                MyData.DataContext = dt;
            }
            else
                MessageBox.Show("Vous n'avez pas les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Update_Click_Boutique(object sender, RoutedEventArgs e)
        {

            if (user == "root")
            {
                ComboBoxItem temp = SelectConditionsB.SelectedItem as ComboBoxItem;
                string colonnecond = Column_From_Combo(temp, "boutique");
                object valuecond = Value_From_Combo(temp, TextBoxConditionB, "boutique");
                temp = SelectChangeB.SelectedItem as ComboBoxItem;
                string colonne = Column_From_Combo(temp, "boutique");
                object value = Value_From_Combo(temp, TextBoxUpdateB, "boutique");
                string result = mainclass.Update("Boutique", colonne, value, colonnecond, valuecond);
                if (result == null)
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                DataTable dt = mainclass.AffichageTable("Boutique");
                MyData.DataContext = dt;
            }
            else
                MessageBox.Show("Vous n'avez pas les droits requis", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }



        private void SelectChangeB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (choice == "Nom Boutique" || choice == "") { TextBlockUpdateB.Text = "Nom Boutique"; }
            else { TextBlockUpdateB.Text = choice; }    
        }

        private void SelectConditionsB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (choice == "Nom Boutique" || choice == "") { TextBlockConditionB.Text = "Nom Boutique"; }
            else { TextBlockConditionB.Text = choice; }
        }

        private void SelectConditionsI_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (choice == "Nom" || choice == "") { TextBlockConditionI.Text = "Nom"; }
            else { TextBlockConditionI.Text = choice; }
        }

        private void SelectChangeI_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (choice == "Numéro de Vélo" || choice == "") { TextBlockUpdateI.Text = "Numéro de Vélo"; }
            else { TextBlockConditionI.Text = choice; }
        }

        private void Radio_Button_Type_Client(object sender, RoutedEventArgs e)
        {
            ComboBoxItem temp = ((SelectAction as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "Add";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if (Particuliers_Selected.IsChecked == false)
            {
                Stat_Client_Particulier.Visibility = Visibility.Collapsed;
                SelectSearchI.Visibility = Visibility.Collapsed; 
                SelectSearchB.Visibility = Visibility.Visible; 
                if (choice == "Add") { Add_Particulier_Form.Visibility = Visibility.Collapsed; Add_Boutique_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Ajouter une Boutique"; }
                if (choice == "Delete") { Delete_Particulier_Form.Visibility = Visibility.Collapsed; Delete_Boutique_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Supprimer une Boutique"; }
                if (choice == "Modify") { Modify_Particulier_Form.Visibility = Visibility.Collapsed; Modify_Boutique_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Modifier une Boutique"; }
                if (choice == "Research") { Search_Client_Form.Visibility = Visibility.Collapsed; Search_Client_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Rechercher un Client"; }
            }
            else
            {
                Stat_Client_Particulier.Visibility = Visibility.Visible;
                SelectSearchB.Visibility = Visibility.Collapsed; 
                SelectSearchI.Visibility = Visibility.Visible;   
                if (choice == "Add") { Add_Boutique_Form.Visibility = Visibility.Collapsed; Add_Particulier_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Ajouter un Particulier"; }
                if (choice == "Delete") { Delete_Boutique_Form.Visibility = Visibility.Collapsed; Delete_Particulier_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Supprimer un Particulier"; }
                if (choice == "Modify") { Modify_Boutique_Form.Visibility = Visibility.Collapsed; Modify_Particulier_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Modifier un Particulier"; }
                if (choice == "Research") { Search_Client_Form.Visibility = Visibility.Collapsed; Search_Client_Form.Visibility = Visibility.Visible; Title_Modif.Content = "Rechercher un Client"; }

            }

        }

        public void Best_Client_Item_Click(object sender, RoutedEventArgs e)
        {
            List<string> bestclient = mainclass.Best_Client_Item();
            if(bestclient.Count<=2)
                MessageBox.Show("Le meilleur client particulier est " + bestclient[0]+ " et il a commandé " + bestclient[1]+ " items.");
            else
            {
                string indivtot = "";
                for(int i =0; i<bestclient.Count-1;i++)
                {
                    indivtot += bestclient[i] +" et ";
                }
                MessageBox.Show("Les meilleurs clients particuliers sont " + indivtot + " ils ont commandé " + bestclient[bestclient.Count-1]+ " items.");
            }
        }

        public void Best_Client_Price_Click(object sender, RoutedEventArgs e)
        {
            List<string> bestclient = mainclass.Best_Client_Prix();
            if (bestclient.Count <= 2)
                MessageBox.Show("Le meilleur client particulier est " + bestclient[0] + " et il a commandé pour " + bestclient[1] + " € .");
            else
            {
                string indivtot = "";
                for (int i = 0; i < bestclient.Count - 1; i++)
                {
                    indivtot += bestclient[i] + "et";
                }
                MessageBox.Show("Les meilleurs clients particuliers sont " + bestclient[0] + " ils ont commandé pour " + bestclient[bestclient.Count - 1] + " €.");
            }
        }

        private void Search_Click_Client(object sender, RoutedEventArgs e)
        {
            ComboBoxItem temp; 
            string column;
            object value;
            DataTable dt;


            if (Particuliers_Selected.IsChecked == true)
            {
                temp = SelectSearchI.SelectedItem as ComboBoxItem;
                column = Column_From_Combo(temp, "particulier");
                value = Value_From_Combo(temp, TextBoxSearchC, "particulier");
                dt = mainclass.Research_Stock("Individu", column, value, true, null);
                if (dt == null)
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                temp = SelectSearchB.SelectedItem as ComboBoxItem;
                column = Column_From_Combo(temp, "boutique");
                value = Value_From_Combo(temp, TextBoxSearchC, "boutique");
                dt = mainclass.Research_Stock("Boutique", column, value, true, null);
                if (dt == null)
                    MessageBox.Show("Erreur, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            MyData.DataContext = dt;
        }

        private string Column_From_Combo(ComboBoxItem cboxitem, string type)
        {
            string column = "";
            string choice = "";
            if (type == "particulier")
            {
                if (!(cboxitem.Content is null)) { choice = cboxitem.Content.ToString(); }
                if (choice == "Nom" || choice == "") { column = "nom_I";  }
                else if (choice == "Prénom") { column = "prenom_I";  }
                else if (choice == "Rue") { column = "rue_I";  }
                else if (choice == "Ville") { column = "ville_I";  }
                else if (choice == "Code Postal") { column = "CP_I";  }
                else if (choice == "Téléphone") { column = "telephone_I";  }
                else if (choice == "Email") { column = "email_I"; }
            }
            else
            {
                if (!(cboxitem.Content is null)) { choice = cboxitem.Content.ToString(); }
                if (choice == "Nom Boutique" || choice == "") { column = "nom_B";  }
                else if (choice == "Rue") { column = "rue_B"; }
                else if (choice == "Ville") { column = "ville_B";  }
                else if (choice == "Code Postal") { column = "CP_B";  }
                else if (choice == "Téléphone") { column = "telephone_B";  }
                else if (choice == "Email") { column = "email_B";  }
                else if (choice == "Nom du Contact") { column = "nomcontact_B";  }
                else if (choice == "Taux de Remise") { column = "tauxR_B"; }
            }
            return column;

        }

        private object Value_From_Combo(ComboBoxItem cboxitem, TextBox source, string type)
        {
            try
            {
                object value;
                string choice = "";
                if (type == "particulier")
                {
                    if (!(cboxitem.Content is null)) { choice = cboxitem.Content.ToString(); }
                    if (choice == "Nom" || choice == "") { value = Convert.ToString(source.Text); }
                    else if (choice == "Prénom") { value = Convert.ToString(source.Text); }
                    else if (choice == "Rue") { value = Convert.ToString(source.Text); }
                    else if (choice == "Ville") { value = Convert.ToString(source.Text); }
                    else if (choice == "Code Postal") { value = Convert.ToInt32(source.Text); }
                    else if (choice == "Téléphone") { value = Convert.ToString(source.Text); }
                    else if (choice == "Email") { value = Convert.ToString(source.Text); }
                    else { value = ""; }
                }
                else
                {
                    if (!(cboxitem.Content is null)) { choice = cboxitem.Content.ToString(); }
                    if (choice == "Nom Boutique" || choice == "") { value = Convert.ToString(source.Text); }
                    else if (choice == "Rue") { value = Convert.ToString(source.Text); }
                    else if (choice == "Ville") { value = Convert.ToString(source.Text); }
                    else if (choice == "Code Postal") { value = Convert.ToString(source.Text); }
                    else if (choice == "Téléphone") { value = Convert.ToString(source.Text); }
                    else if (choice == "Email") { value = Convert.ToString(source.Text); }
                    else if (choice == "Nom du Contact") { value = Convert.ToString(source.Text); }
                    else if (choice == "Taux de Remise") { value = Convert.ToSingle(source.Text); }
                    else { value = ""; }
                }
                return value;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}

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
    /// Logique d'interaction pour Fidelio.xaml
    /// </summary>
    public partial class Fidelio : Window
    {
        string user;
        MainClass mainclass;
        public Fidelio(string user)
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            this.user = user;
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

        private void Clients_Click(object sender, RoutedEventArgs e)
        {
            Client client = new Client(user);
            client.Show();
            this.Close();
        }

        private void SelectAction_SelectionChanged(object sender, RoutedEventArgs e)
        {
            mainclass = new MainClass();
            int condition =1;
            ComboBoxItem temp = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string choice = "";
            if (!(temp.Content is null)) { choice = temp.Content.ToString(); }
            if(choice== "Fidélio") { condition = 1; }
            else if(choice=="Fidélio Or") { condition = 2; }
            else if (choice == "Fidélio Platine") { condition = 3; }
            else if (choice == "Fidélio Max") { condition = 4; }
            DataTable dt = mainclass.Research_Stock("adhesion", "num_fid", condition, true);
            MyData.DataContext = dt;
        }

        public void ExportIFjson_Click(object sender, RoutedEventArgs e)
        {
            mainclass.exportjsonfinadhe();
            MessageBox.Show("Création du fichier echeance_adhesion.json réussie !");
        }

    }

}

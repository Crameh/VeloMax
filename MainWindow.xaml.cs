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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;

namespace WPF_Probleme
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        public bool VerifConnection()
        {
            if ((Identifiant.Text == "bozo" && MotdePasse.Text == "bozo") || (Identifiant.Text == "root" && MotdePasse.Text == "root"))
                return true;
            else 
                return false;

        }

        private void Connection_Click(object sender, RoutedEventArgs e)
        {
            if(VerifConnection())
            {
                Velo velo = new Velo(Identifiant.Text);
                velo.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Identifiant ou Mot de Passe Incorrect", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
    }
}

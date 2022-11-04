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
    /// Logique d'interaction pour CommandeLigne.xaml
    /// </summary>
    public partial class CommandeLigne : Window
    {
        MainClass mainclass;
        int numcommande;
        int compteur =1;
        string user;
        public CommandeLigne(int numcommande, string user)
        {
            InitializeComponent();
            mainclass = new MainClass(user);
            this.numcommande = numcommande;
            this.user = user;
        }

        public void Radio_Button_Type_CommandeLigne(object sender, RoutedEventArgs e)
        {
            if (Velo_Selected.IsChecked == true) { Add_Velo_CommandeLigne_Form.Visibility = Visibility.Visible; Add_Piece_CommandeLigne_Form.Visibility = Visibility.Collapsed; }
            else { Add_Piece_CommandeLigne_Form.Visibility = Visibility.Visible; Add_Velo_CommandeLigne_Form.Visibility = Visibility.Collapsed; }
        }

        public void Add_Click_CommandeLigne(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] value;
                bool ajoutpossible = true;
                int delaisupp = 0;
                if (Velo_Selected.IsChecked == false)
                {
                    int stockfuture = mainclass.VerifStock(numP_add.Text, Convert.ToInt32(quantiteP_add.Text));
                    if (stockfuture < 0)
                    {
                        MessageBoxResult result = MessageBox.Show("La pièce " + numP_add.Text + " n'est plus en stock. Vous devez en commander auprès de vos fournisseurs", "Achat fournisseur", MessageBoxButton.YesNo);
                        switch (result)
                        {
                            case MessageBoxResult.Yes:
                                ajoutpossible = true;
                                int temp = mainclass.delaiLsupp(numP_add.Text);
                                if (temp>delaisupp) { delaisupp = temp; }
                                mainclass.UpdateStock(numP_add.Text, -stockfuture, "+");
                                break;
                            case MessageBoxResult.No:
                                ajoutpossible = false;
                                break;
                        }
                    }
                    value = new string[] { numcommande.ToString() + compteur.ToString(), numcommande.ToString(), quantiteP_add.Text, numP_add.Text, null };
                    if (ajoutpossible)
                        mainclass.UpdateStock(numP_add.Text, Convert.ToInt32(quantiteP_add.Text), "-");
                }
                else
                {
                    List<string> listpiece = mainclass.ListePiece(numV_add.Text);
                    List<string> listpieceindispo = new List<string>();
                    string pieceindispo = "";
                    value = new string[] { numcommande.ToString() + compteur.ToString(), numcommande.ToString(), quantiteV_add.Text, null, numV_add.Text };
                    foreach (string piece in listpiece)
                    {
                        if (mainclass.VerifStock(piece, Convert.ToInt32(quantiteV_add.Text)) < 0)
                        {
                            ajoutpossible = false;
                            pieceindispo += piece + " ";
                            listpieceindispo.Add(piece);
                        }
                    }
                    if(!ajoutpossible)
                    {
                        MessageBoxResult result = MessageBox.Show("La(les) pièce(s) " + pieceindispo + " n'est/ne sont plus en stock. Vous devez en commander auprès de vos fournisseurs", "Achat fournisseur", MessageBoxButton.YesNo);
                        switch (result)
                        {
                            case MessageBoxResult.Yes:
                                foreach (string piece in listpieceindispo)
                                {
                                    int stockfuture = mainclass.VerifStock(piece, Convert.ToInt32(quantiteV_add.Text));
                                    int temp = mainclass.delaiLsupp(piece);
                                    if (temp > delaisupp) { delaisupp = temp; }
                                    mainclass.UpdateStock(piece, -stockfuture, "+");
                                }
                                ajoutpossible = true;
                                break;
                            case MessageBoxResult.No:
                                ajoutpossible = false;
                                break;
                        }
                    }

                    if (ajoutpossible)
                    {
                        foreach (string piece in listpiece)
                        {
                            mainclass.UpdateStock(piece, Convert.ToInt32(quantiteV_add.Text), "-");
                        }
                    }
                }


                if (ajoutpossible)
                {
                    mainclass.Creation("CommandeLigne", value);
                }

                mainclass.delaiL(numcommande, delaisupp);
                DataTable dt = mainclass.Research_Stock("CommandeLigne", "num_C", numcommande, true, null);
                MyData.DataContext = dt;
                compteur++;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreurrrr, veuillez vérifier vos entrées.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        public void Quit_Click_CommandeLigne(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

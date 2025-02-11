using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rezerwacja_lotow
{
   
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SprawdzPolaczenie();
        }

        private void SprawdzPolaczenie()
        {
            try
            {
                using (var db = new ObslugaBazy())
                {
                   
                }
            }
            catch (Exception blad)
            {
                MessageBox.Show("Błąd połączenia z bazą: " + blad.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Zapisz_Gosc(object sender, RoutedEventArgs e)
        {
            
            DodajPasazera noweOkno = new DodajPasazera();
            noweOkno.ShowDialog(); 
        }

        private void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string haslo = txtHaslo.Password;

            using (var db = new ObslugaBazy())
            {
                
                var pasazer = db.Pasazer.FirstOrDefault(p => p.Email == email && p.Haslo == haslo);

                if (pasazer != null)
                {
                    MessageBox.Show("Witaj: " + email);

                   
                    MenuZalogowany menuOkno = new MenuZalogowany(pasazer.ID_Pasazera);
                    menuOkno.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Błędny email lub hasło.");
                }
            }
        }


        
    }
}
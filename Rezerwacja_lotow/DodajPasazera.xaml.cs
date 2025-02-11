using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace Rezerwacja_lotow
{
    public partial class DodajPasazera : Window
    {
        public DodajPasazera()
        {
            InitializeComponent();
        }

        private void DodajPasazeraBtn_Click(object sender, RoutedEventArgs e)
        {
            string imie = ImieTextBox.Text;
            string nazwisko = NazwiskoTextBox.Text;
            string email = EmailTextBox.Text;
            string telefon = TelefonTextBox.Text;  
            string haslo = HasloPasswordBox.Password;

           
            if (string.IsNullOrWhiteSpace(imie) || string.IsNullOrWhiteSpace(nazwisko) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(telefon) ||
                string.IsNullOrWhiteSpace(haslo))
            {
                MessageBox.Show("Wszystkie pola są wymagane.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

           
            if (imie.Length < 3)
            {
                MessageBox.Show("Imię musi mieć co najmniej 3 znaki.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (nazwisko.Length < 3)
            {
                MessageBox.Show("Nazwisko musi mieć co najmniej 3 znaki.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            if (!SprawdzMail(email))
            {
                MessageBox.Show("Podano nieprawidłowy adres email.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            if (haslo.Length < 4)
            {
                MessageBox.Show("Hasło musi mieć co najmniej 4 znaki.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

           
            if (telefon.Length != 9 || !telefon.All(char.IsDigit))
            {
                MessageBox.Show("Numer telefonu musi składać się z dokładnie 9 cyfr.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

           
            using (var db = new ObslugaBazy())
            {
                var nowyPasazer = new Pasazer
                {
                    Imie = imie,
                    Nazwisko = nazwisko,
                    Email = email,
                    Telefon = telefon, 
                    Haslo = haslo
                };

                db.Pasazer.Add(nowyPasazer);
                db.SaveChanges();  
            }

            MessageBox.Show("Konto pasażera zostało utworzone.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();  
        }


        
        private bool SprawdzMail(string email)
        {
          
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}

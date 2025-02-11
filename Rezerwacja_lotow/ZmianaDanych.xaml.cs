using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Rezerwacja_lotow
{
    public partial class ZmianaDanych : Window
    {
        private int idZalogowanegoPasazera;

        public ZmianaDanych(int idPasazera)
        {
            InitializeComponent();
            idZalogowanegoPasazera = idPasazera;

            // Załaduj dane użytkownika do pól tekstowych
            ZaładujDaneUzytkownika();
        }

        private void ZaładujDaneUzytkownika()
        {
            using (var db = new ObslugaBazy())
            {
                var pasazer = db.Pasazer.FirstOrDefault(p => p.ID_Pasazera == idZalogowanegoPasazera);
                if (pasazer != null)
                {
                    // Wypełniamy pola danymi użytkownika
                    ImieTextBox.Text = pasazer.Imie;
                    NazwiskoTextBox.Text = pasazer.Nazwisko;
                    EmailTextBox.Text = pasazer.Email;
                    TelefonTextBox.Text = pasazer.Telefon;
                    HasloPasswordBox.Password = pasazer.Haslo;


                }
            }
        }
        private bool SprawdzMail(string email)
        {
            // Prosty wzorzec sprawdzający obecność znaku @ i poprawny format emaila
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private void ZapiszZmianyBtn_Click(object sender, RoutedEventArgs e)
        {
            // Pobierz zmienione dane z pól tekstowych
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

            // Walidacja długości imienia
            if (imie.Length < 3)
            {
                MessageBox.Show("Imię musi mieć co najmniej 3 znaki.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Walidacja długości nazwiska
            if (nazwisko.Length < 3)
            {
                MessageBox.Show("Nazwisko musi mieć co najmniej 3 znaki.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Walidacja adresu email
            if (!SprawdzMail(email))
            {
                MessageBox.Show("Podano nieprawidłowy adres email.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Walidacja długości hasła
            if (haslo.Length < 4)
            {
                MessageBox.Show("Hasło musi mieć co najmniej 4 znaki.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Walidacja numeru telefonu
            if (telefon.Length != 9 || !telefon.All(char.IsDigit))
            {
                MessageBox.Show("Numer telefonu musi składać się z dokładnie 9 cyfr.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var db = new ObslugaBazy())
            {
                var pasazer = db.Pasazer.FirstOrDefault(p => p.ID_Pasazera == idZalogowanegoPasazera);
                if (pasazer != null)
                {
                    // Zaktualizuj dane użytkownika
                    pasazer.Imie = imie;
                    pasazer.Nazwisko = nazwisko;
                    pasazer.Email = email;
                    pasazer.Telefon = telefon;
                    pasazer.Haslo = haslo;  

                    db.SaveChanges(); // Zapisz zmiany w bazie danych

                    MessageBox.Show("Dane zostały zaktualizowane.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close(); // Zamknij okno po zapisaniu
                }
                else
                {
                    MessageBox.Show("Nie znaleziono użytkownika.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
    }
}

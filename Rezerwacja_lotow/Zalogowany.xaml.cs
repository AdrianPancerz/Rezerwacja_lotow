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

namespace Rezerwacja_lotow
{
    public partial class Zalogowany : Window
    {
        private int idZalogowanegoPasazera;

        public Zalogowany(int idPasazera)
        {
            InitializeComponent();
            idZalogowanegoPasazera = idPasazera;
            ZaladujDostepneLoty();
        }

        private void Powrotx(object sender, RoutedEventArgs e)
        {
            MenuZalogowany powrot = new MenuZalogowany(idZalogowanegoPasazera);
            powrot.Show();
            this.Close();
        }

        private void ZaladujDostepneLoty()
        {
            using (var db = new ObslugaBazy())
            {
               
                var loty = db.DostepneLoty.Where(l => l.Dostepnosc).ToList();
                DostepneLotyGrid.ItemsSource = loty;
            }
        }

        private void ZarezerwujLot_Click(object sender, RoutedEventArgs e)
        {
            if (DostepneLotyGrid.SelectedItem is DostepneLoty wybranyLot)
            {
                using (var db = new ObslugaBazy())
                {
                    
                    var nowaRezerwacja = new Rezerwacja
                    {
                        ID_Pasazera = idZalogowanegoPasazera,
                        ID_Lotu = wybranyLot.ID_Lotu,
                        Data_Rezerwacji = DateTime.Now,
                        Status = "Oczekująca"
                    };

                    db.Rezerwacja.Add(nowaRezerwacja);
                    db.SaveChanges();
                    MessageBox.Show("Wybrałeś lot, nie zapomij go potwierdzić!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    wybranyLot.Dostepnosc = false;
                    db.Update(wybranyLot);
                    db.SaveChanges();
                    ZaladujDostepneLoty();
                }
            }
            else
            {
                MessageBox.Show("Proszę wybrać lot z listy.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

namespace Rezerwacja_lotow
{
    public partial class HistoriaRezerwacji : Window
    {
        private int idZalogowanegoPasazera1;

        public HistoriaRezerwacji(int idPasazera)
        {
            InitializeComponent();
            idZalogowanegoPasazera1 = idPasazera;
            ZaladujPotwierdzoneRezerwacje();
        }

        private void ZaladujPotwierdzoneRezerwacje()
        {
            using (var db = new ObslugaBazy())
            {
                
                var rezerwacje = db.Rezerwacja
                                   .Where(r => r.ID_Pasazera == idZalogowanegoPasazera1 && r.Status == "Potwierdzona")
                                   .Include(r => r.Lot)  
                                   .ToList();
                HistoriaGrid.ItemsSource = rezerwacje;
            }
           
        }



        private void PowrotBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();  
        }
    }
}

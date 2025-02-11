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
    public partial class MenuZalogowany : Window
    {
        private int idZalogowanegoPasazera;

        public MenuZalogowany(int idPasazera)
        {
            InitializeComponent();
            idZalogowanegoPasazera = idPasazera;
            ZaladujDaneUzytkownika();
           
        }
        private void Wyjdz_Click(object sender, RoutedEventArgs e)
        {
            MainWindow noweOkno = new MainWindow();
            noweOkno.Show();
            this.Close();
        }

        
        private void OtworzRezerwacje_Click(object sender, RoutedEventArgs e)
        {
            
            Zalogowany oknoRezerwacji = new Zalogowany(idZalogowanegoPasazera);
            oknoRezerwacji.Show();

            
            this.Close();
        }

        private void SprawdzRezerwacje_Click(object sender, RoutedEventArgs e)
        {
            
            MojeRezerwacje oknoRezerwacji = new MojeRezerwacje(idZalogowanegoPasazera);
            oknoRezerwacji.Show();
            this.Close();
        }
       

        private void HistoriaRezerwacji_Click(object sender, RoutedEventArgs e)
        {
            HistoriaRezerwacji oknoHistorii = new HistoriaRezerwacji(idZalogowanegoPasazera);
            oknoHistorii.ShowDialog();
        }


        private void ZaladujDaneUzytkownika()
        {
            using (var db = new ObslugaBazy())
            {
              
                var pasazer = db.Pasazer.FirstOrDefault(p => p.ID_Pasazera == idZalogowanegoPasazera);

                if (pasazer != null)
                {
                  
                    UserNameTextBlock.Text = $"Użytkonik: {pasazer.Imie} {pasazer.Nazwisko}";
                }
                else
                {
                    MessageBox.Show("Nie udało się załadować danych użytkownika.");
                }
            }
        }
        private void ZmianaDanych_Click(object sender, RoutedEventArgs e)
        {
         
            ZmianaDanych zmianaDanychWindow = new ZmianaDanych(idZalogowanegoPasazera);
            zmianaDanychWindow.Show();
        }

        private void PokazBilety_Click(object sender, RoutedEventArgs e)
        {
            
            BiletyWindow biletyWindow = new BiletyWindow(idZalogowanegoPasazera);
            biletyWindow.Show();
            this.Close();
        }

    }
}

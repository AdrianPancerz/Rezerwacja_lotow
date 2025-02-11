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
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Rezerwacja_lotow
{
    public partial class MojeRezerwacje : Window
    {
        private int idZalogowanegoPasazera;

        public MojeRezerwacje(int idPasazera)
        {
            InitializeComponent();
            idZalogowanegoPasazera = idPasazera;
            ZaladujRezerwacje();
        }

        private void ZaladujRezerwacje()
        {
            using (var db = new ObslugaBazy())
            {
                
                var rezerwacje = db.Rezerwacja
                                   .Where(r => r.ID_Pasazera == idZalogowanegoPasazera && r.Status == "Oczekująca")
                                   .Include(r => r.Lot)
                                   .ToList();
            
                RezerwacjeGrid.ItemsSource = rezerwacje;
            }
        }
        
        private string GenerujNumerBiletu()
        {
            Random random = new Random();
            int numer = random.Next(1000, 9999);  
            return numer.ToString();
        }

        
        private string GenerujMiejsce()
        {
            Random random = new Random();
            int numerRzedu = random.Next(1, 11); 
            char[] literyMiejsc = { 'A', 'B' };   
            char literaMiejsca = literyMiejsc[random.Next(literyMiejsc.Length)];
            return $"{numerRzedu}{literaMiejsca}";
        }


        private void PowrotDoMenu_Click(object sender, RoutedEventArgs e)
        {
            
            MenuZalogowany menuOkno = new MenuZalogowany(idZalogowanegoPasazera);
            menuOkno.Show();
            this.Close();
        }

        private void PotwierdzRezerwacje_Click(object sender, RoutedEventArgs e)
        {
            var wybranaRezerwacja = (Rezerwacja)RezerwacjeGrid.SelectedItem;

            if (wybranaRezerwacja != null)
            {
                using (var db = new ObslugaBazy())
                {
                    var rezerwacja = db.Rezerwacja.FirstOrDefault(r => r.ID_Rezerwacji == wybranaRezerwacja.ID_Rezerwacji);
                    var lot = db.Lot.FirstOrDefault(l => l.ID_Lotu == rezerwacja.ID_Lotu);

                    if (rezerwacja != null && lot != null)
                    {
                        
                        rezerwacja.Status = "Potwierdzona";
                        db.SaveChanges();
                        string numerBiletu = "B" + new Random().Next(1000, 9999).ToString(); 
                        string miejsce = lot.Miasto_Poczatkowe.Substring(0, 1) + new Random().Next(1, 30).ToString();  

                        var bilet = new Bilet
                        {
                            Numer_Biletu = numerBiletu, 
                            ID_Rezerwacji = rezerwacja.ID_Rezerwacji,
                            ID_Lotu = lot.ID_Lotu,
                            NumerLotu = lot.NumerLotu,
                            Miasto_Poczatkowe = lot.Miasto_Poczatkowe,
                            Miasto_Celowe = lot.Miasto_Celowe,
                            Data_Wylotu = lot.Data_Wylotu,
                            Data_Przylotu = lot.Data_Przylotu,
                            Cena = lot.Cena,
                            Miejsce = miejsce
                        };

                        
                        db.Bilet.Add(bilet);
                        db.SaveChanges();
                        MessageBox.Show("Rezerwacja została potwierdzona, bilet został wygenerowany.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                        ZaladujRezerwacje(); 
                    }
                    else
                    {
                        MessageBox.Show("Nie znaleziono odpowiedniej rezerwacji lub lotu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Nie wybrano rezerwacji.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void AnulujRezerwacje_Click(object sender, RoutedEventArgs e)
        {
            
            var button = sender as Button;
            int idRezerwacji = int.Parse(button.Tag.ToString());

            using (var db = new ObslugaBazy())
            {
                
                var rezerwacja = db.Rezerwacja.FirstOrDefault(r => r.ID_Rezerwacji == idRezerwacji);

                if (rezerwacja != null)
                {
                    
                    rezerwacja.Status = "Anulowana";               
                    var dostepnyLot = db.DostepneLoty.FirstOrDefault(dl => dl.ID_Lotu == rezerwacja.ID_Lotu);
                    if (dostepnyLot != null)
                    {
                        dostepnyLot.Dostepnosc = true;  
                    }
                    db.SaveChanges();
                    MessageBox.Show("Rezerwacja została anulowana.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    ZaladujRezerwacje();
                }
                else
                {
                    MessageBox.Show("Nie znaleziono rezerwacji.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rezerwacja_lotow
{

     public class DostepneLoty
    {
       
            [Key]
            public int ID_Lotu { get; set; }
            public string Miasto_Poczatkowe { get; set; }
            public string Miasto_Celowe { get; set; }
            public DateTime Data_Wylotu { get; set; }
            public DateTime Data_Przylotu { get; set; }
            public decimal Cena { get; set; }
            public bool Dostepnosc { get; set; }
        

    }
}

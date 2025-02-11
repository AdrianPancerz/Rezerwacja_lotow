using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rezerwacja_lotow
{
    public class Bilet
    {
        [Key]
        public int ID_Biletu { get; set; }

        [ForeignKey("Rezerwacja")]
        public int ID_Rezerwacji { get; set; }

        [ForeignKey("Lot")]
        public int ID_Lotu { get; set; }

        public string Numer_Biletu { get; set; }

        public string Miasto_Poczatkowe { get; set; }

        public string Miasto_Celowe { get; set; }

        public DateTime Data_Wylotu { get; set; }

        public DateTime Data_Przylotu { get; set; }

        public decimal Cena { get; set; }

        public string Miejsce { get; set; }

        
        public virtual Rezerwacja Rezerwacja { get; set; }

       
        public virtual Lot Lot { get; set; }
        public string NumerLotu { get; internal set; }
    }
}
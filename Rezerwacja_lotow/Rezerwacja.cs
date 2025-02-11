using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rezerwacja_lotow
{
    public class Rezerwacja
    {
        [Key]
        public int ID_Rezerwacji { get; set; }

        [ForeignKey("Pasazer")]
        public int ID_Pasazera { get; set; }

        [ForeignKey("Lot")]
        public int ID_Lotu { get; set; }

        public string Status { get; set; }

        public DateTime Data_Rezerwacji { get; set; }

        
        public virtual Lot Lot { get; set; }

        
        public virtual Pasazer Pasazer { get; set; }
    }

}

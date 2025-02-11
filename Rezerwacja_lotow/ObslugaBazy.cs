using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rezerwacja_lotow
{
    internal class ObslugaBazy : DbContext
    {
        public DbSet<Pasazer> Pasazer { get; set; }
        public DbSet<Lot> Lot { get; set; }
        public DbSet<Rezerwacja> Rezerwacja { get; set; }
        public DbSet<DostepneLoty> DostepneLoty { get; set; }

        public DbSet<Bilet> Bilet { get; set; }

       

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=DESKTOP-8J0KRJ9;Database=SystemRezerwacjiLotow; TrustServerCertificate=True; Trusted_Connection=True;");


        }
    }
    

    
}

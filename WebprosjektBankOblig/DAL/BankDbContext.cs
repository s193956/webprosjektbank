using System;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.ModelConfiguration.Conventions;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.DAL
{
    
    public class CustomInitializer<T> : DropCreateDatabaseAlways<BankDbContext>
    {
        public override void InitializeDatabase(BankDbContext context)
        {
            try
            {
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction
                , string.Format("ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", context.Database.Connection.Database));

            }
            catch
            {
                
            }
            
            base.InitializeDatabase(context);
        }
    }

    public class BankDbContext : DbContext
    {
        public BankDbContext()
            : base("name=DBContext")
        {
            Database.CreateIfNotExists();
            Database.SetInitializer<BankDbContext>(new CustomInitializer<BankDbContext>());
        }

        public DbSet<Kunde> Kunder { get; set; }
        public DbSet<Autentisering> Autentiseringer { get; set; }
        public DbSet<Poststed> Poststeder { get; set; }
        public DbSet<Konto> Kontoer { get; set; }
        public DbSet<Betaling> Betalinger { get; set; }
        public DbSet<AdminBruker> Administratorer { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kunde>().HasKey(p => p.Id);
            modelBuilder.Entity<Autentisering>().HasKey(p => p.Id);
            modelBuilder.Entity<Poststed>().HasKey(p => p.Id);
            modelBuilder.Entity<Konto>().HasKey(p => p.Id);
            modelBuilder.Entity<Betaling>().HasKey(p => p.Id);
            modelBuilder.Entity<AdminBruker>().HasKey(p => p.Id);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Kunde>()
                        .HasOptional(s => s.Autentisering) 
                        .WithRequired(ad => ad.Kunde);
        }
    }
}
namespace WebprosjektBankOblig.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class CustomInitializer<T> : DropCreateDatabaseAlways<DBContext>
    {
        public override void InitializeDatabase(DBContext context)
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

    public class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
        {
            Database.CreateIfNotExists();
            Database.SetInitializer<DBContext>(new CustomInitializer<DBContext>());
        }

        public DbSet<Kunde> Kunder { get; set; }
        public DbSet<Autentisering> Autentiseringer { get; set; }
        public DbSet<Poststed> Poststeder { get; set; }
        public DbSet<Konto> Kontoer { get; set; }
        public DbSet<Betaling> Betalinger { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kunde>().HasKey(p => p.Id);
            modelBuilder.Entity<Autentisering>().HasKey(p => p.Id);
            modelBuilder.Entity<Poststed>().HasKey(p => p.Id);
            modelBuilder.Entity<Konto>().HasKey(p => p.kontonr);
            modelBuilder.Entity<Betaling>().HasKey(p => p.Id);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Kunde>()
                        .HasOptional(s => s.Autentisering) 
                        .WithRequired(ad => ad.Kunde);

            /*modelBuilder.Entity<Konto>()
                .HasOptional(c => c.Betaling)
                .WithRequired(ap => ap.Konto);*/

        }
    }
}
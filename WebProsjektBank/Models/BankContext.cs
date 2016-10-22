namespace TestOppgave.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public class BankContext : DbContext
    {
        // Your context has been configured to use a 'BankContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'TestOppgave.Models.BankContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'BankContext' 
        // connection string in the application configuration file.
        public BankContext()
            : base("name=BankContext")
        {
            Database.CreateIfNotExists();
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public IDbSet<Kunde> Kunder { get; set; }
        public IDbSet<Konto> Kontoer { get; set; }
        public IDbSet<Betaling> Betalinger { get; set; }
        public IDbSet<BankIDBrikke> BankIDBrikker { get; set; }
        public IDbSet<KundeAutentisering> KundeAutentiseringer { get; set; }
        public IDbSet<LoggInn> Logging { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoggInn>()
                        .HasKey(p => p.Personnummer);
            modelBuilder.Entity<Kunde>()
                        .HasKey(p => p.Personnummer);
            modelBuilder.Entity<Konto>()
                        .HasKey(p => p.Personnummer);
            modelBuilder.Entity<Betaling>()
                        .HasKey(p => p.Personnummer);
            modelBuilder.Entity<LoggInn>()
                        .HasKey(p => p.Personnummer);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}
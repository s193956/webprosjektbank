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
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction
                , string.Format("ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", context.Database.Connection.Database));

            base.InitializeDatabase(context);
        }
    }

    public class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
        {
            Database.SetInitializer<DBContext>(new CustomInitializer<DBContext>());
            Database.CreateIfNotExists();
        }

        public DbSet<Kunde> Kunder { get; set; }
        public DbSet<Autentisering> Autentiseringer { get; set; }
        public DbSet<Poststeder> Poststed { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kunde>().HasKey(p => p.Id);
            modelBuilder.Entity<Autentisering>().HasKey(p => p.Id);
            modelBuilder.Entity<Poststeder>().HasKey(p => p.Id);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
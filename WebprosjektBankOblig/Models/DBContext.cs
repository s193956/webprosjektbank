namespace WebprosjektBankOblig.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
        {
            Database.CreateIfNotExists();
            Database.SetInitializer<DBContext>(new DropCreateDatabaseIfModelChanges<DBContext>());
        }

        public DbSet<Kunde> Kunder { get; set; }
        public DbSet<Autentisering> Autentiseringer { get; set; }
        public DbSet<Poststeder> Poststed { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kunde>().HasKey(p => p.Id);
            modelBuilder.Entity<Autentisering>().HasKey(p => p.Id);
            modelBuilder.Entity<Poststeder>().HasKey(p => p.Id);
        }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}
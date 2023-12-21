using Course.DataLayer.Entities.User;
using Course.DataLayer.Entities.Wallet;
using Microsoft.EntityFrameworkCore;

namespace Course.DataLayer.Context
{
    public class CourseDbContext : DbContext
    {

        public CourseDbContext(DbContextOptions<CourseDbContext> options) : base(options)
        {

        }


        #region user

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        #endregion


        #region Wallet

        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<WalletType> WalletType { get; set; }
        
        #endregion



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Role>()
              //  .HasQueryFilter(r => !r.IsDelete);

            base.OnModelCreating(modelBuilder);
        }



    }
}

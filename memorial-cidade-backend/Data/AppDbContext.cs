using memorial_cidade_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace memorial_cidade_backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagCategory> TagCategories { get; set; }
        public DbSet<LocationData> LocationDatas { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Photographer> Photographers { get; set; }
        public DbSet<Source> Sources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Photo>().ToTable("Photos");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Tag>().ToTable("Tags");
            modelBuilder.Entity<TagCategory>().ToTable("TagCategories");
            modelBuilder.Entity<LocationData>().ToTable("LocationDatas");
            modelBuilder.Entity<Organization>().ToTable("Organizations");
            modelBuilder.Entity<Photographer>().ToTable("Photographers");
            modelBuilder.Entity<Source>().ToTable("Sources");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Pokemon.Model;

namespace Pokemon.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pokemont> Pokemons { get; set; }
        public DbSet<PokemonCategory> PokemonCategories { get; set; }
        public DbSet<PokemonOwner> PokemonOwners { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Using Fluent Api
            //Many(Pokemon) - Many(Category)
            modelBuilder.Entity<PokemonCategory>()//========> FK
                .HasKey(pc => new { pc.PokemonId, pc.CategoryId });

            modelBuilder.Entity<PokemonCategory>()
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(c => c.PokemonId);

            modelBuilder.Entity<PokemonCategory>()
               .HasOne(p => p.Category)
               .WithMany(pc => pc.PokemonCategories)
               .HasForeignKey(c => c.CategoryId);


            //Many(Pokemon) - Many(Owner)
            modelBuilder.Entity<PokemonOwner>()//========> FK
                .HasKey(pc => new { pc.PokemonId, pc.OwnerId });

            modelBuilder.Entity<PokemonOwner>()
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokemonOwners)
                .HasForeignKey(c => c.PokemonId);

            modelBuilder.Entity<PokemonOwner>()
               .HasOne(p => p.Owner)
               .WithMany(pc => pc.PokemonOwners)
               .HasForeignKey(c => c.OwnerId);
        }

    }
}

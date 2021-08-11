using Challenge.Aceleracion.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.Aceleracion.Context
{
    public class DisneyContext : DbContext
    {
        private const string Schema = "Disney";
        
        public DisneyContext(DbContextOptions options) : base(options)
        {

        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema(Schema);

            builder.Entity<Genre>().HasMany(x => x.Movies).WithOne(x => x.Genres);
            builder.Entity<Movie>().HasOne(x => x.Genres).WithMany(x => x.Movies);
            builder.Entity<Movie>().HasMany(x => x.Characters).WithMany(x => x.Movies);
            builder.Entity<Character>().HasMany(x => x.Movies).WithMany(x => x.Characters);

            builder.Entity<Genre>().HasData(
                new Genre
                {
                    Id = 1,
                    Name = "Comedia",
                    Image = "Testimage"
                },
                new Genre
                {
                    Id = 2,
                    Name = "Fantasía",
                    Image = "Testimage"
                },
                new Genre
                {
                    Id = 3,
                    Name = "Musical",
                    Image = "Testimage"
                },
                new Genre
                {
                    Id = 4,
                    Name = "Acción/Aventura",
                    Image = "Testimage"
                });
        }

        public DbSet<Movie> Movies { get; set; } = null;
        public DbSet<Character> Characters { get; set; } = null;
        public DbSet<Genre> Genres { get; set; } = null;
        public DbSet<CharacterMovie> CharacterMovies { get; set; } = null;
    }
}

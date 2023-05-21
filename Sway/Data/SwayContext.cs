using Microsoft.EntityFrameworkCore;
using Sway.Models;

namespace Sway.Data
{
    public class SwayContext : DbContext
    {
        public SwayContext(DbContextOptions<SwayContext> options)
            : base(options)
        {
        }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Phrase> Phrases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<Document>()
                .HasMany<Phrase>(d => d.Phrases)
                .WithOne(p => p.Document)
                .HasForeignKey(p=>p.DocumentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Phrase>()
                .HasMany<Opinion>(d => d.Opinions)
                .WithOne(p => p.Phrase)
                .HasForeignKey(p => p.PhraseID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Opinion>()
                .HasMany<Assessment>(d => d.Assessments)
                .WithOne(p => p.Opinion)
                .HasForeignKey(p => p.OpinionID)
                .OnDelete(DeleteBehavior.Restrict);*/

        }

        public DbSet<Sway.Models.Opinion> Opinion { get; set; }

        public DbSet<Sway.Models.Assessment> Assessment { get; set; }

    }
}

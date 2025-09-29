using Microsoft.EntityFrameworkCore;

namespace EfMigrationsDemo;

public class AppDbContext : DbContext
{
    public DbSet<Blog> Blogs => Set<Blog>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // SQLite dosya yolu
            optionsBuilder.UseSqlite("Data Source=EfMigrationsDemo.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // BLOG: CreatedTimestamp için DB default (CURRENT_TIMESTAMP)
        modelBuilder.Entity<Blog>()
            .Property(b => b.CreatedTimestamp)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        // POST: CreatedAt için DB default (CURRENT_TIMESTAMP)
        modelBuilder.Entity<Post>()
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // COMMENT: FK(PostId) + CreatedAt default + Cascade delete
        modelBuilder.Entity<Comment>()
            .Property(c => c.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)           // Post.Comments koleksiyonuna bağlanır
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // İsteğe bağlı: Index örnekleri
        modelBuilder.Entity<Post>()
            .HasIndex(p => p.BlogId);

        modelBuilder.Entity<Comment>()
            .HasIndex(c => c.PostId);
    }
}


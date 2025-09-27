using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Blog> Blogs => Set<Blog>();
    public DbSet<Post> Posts => Set<Post>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlite("Data Source=EfMigrationsDemo.db");
    }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .Property(b => b.CreatedTimestamp)
        .HasDefaultValueSql("CURRENT_TIMESTAMP") // DB default
        .ValueGeneratedOnAdd();                  // EF INSERT sırasında bu alanı boş bıraksın
}

}


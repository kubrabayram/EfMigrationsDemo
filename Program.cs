// Program.cs
using System;
using System.Linq;
using EfMigrationsDemo;              // <-- AppDbContext, Blog, Post, Comment buradaki namespace
using Microsoft.EntityFrameworkCore; // <-- db.Database.Migrate()

using var db = new AppDbContext();

// Varsa bekleyen migration'ları uygula (tablo yoksa oluşturur)
db.Database.Migrate();

// Örnek veri: aynı blogu iki kere eklememek için kontrol
var blog = db.Blogs.FirstOrDefault(b => b.Name == "Migration Blog");
if (blog is null)
{
    blog = new Blog { Name = "Migration Blog" };
    db.Blogs.Add(blog);
    db.SaveChanges();

    db.Posts.Add(new Post
    {
        Title = "İlk Yazı",
        Content = "Migration ile tablo eklendi!",
        CreatedAt = DateTime.UtcNow,
        BlogId = blog.Id
    });
    db.SaveChanges();
}

// Verileri yazdır
Console.WriteLine("Bloglar ve yazıları:");
foreach (var b in db.Blogs.ToList())
{
    Console.WriteLine($"Blog: {b.Name}");
    var posts = db.Posts.Where(p => p.BlogId == b.Id).ToList();
    foreach (var p in posts)
    {
        Console.WriteLine($"  - {p.Title} ({p.CreatedAt}): {p.Content}");
    }
    
    using (var conn = db.Database.GetDbConnection())
{
    await conn.OpenAsync();
    using var cmd = conn.CreateCommand();
    cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY 1;";
    using var reader = await cmd.ExecuteReaderAsync();
    Console.WriteLine("\nTablolar:");
    while (await reader.ReadAsync())
        Console.WriteLine(" - " + reader.GetString(0));
}
}

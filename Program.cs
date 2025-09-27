using System;
using System.Linq;

using var db = new AppDbContext();

// Yeni blog ekle
var blog = new Blog { Name = "Migration Blog" };
db.Blogs.Add(blog);
db.SaveChanges();

// Bu blog’a post ekle
db.Posts.Add(new Post
{
    Title = "İlk Yazı",
    Content = "Migration ile tablo eklendi!",
    CreatedAt = DateTime.UtcNow,
    BlogId = blog.Id
});
db.SaveChanges();

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
}

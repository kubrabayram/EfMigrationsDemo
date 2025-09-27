using System;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    // Foreign Key (BlogId) → her post bir blog’a ait
    public int BlogId { get; set; }
    public Blog Blog { get; set; } = default!;
}

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}

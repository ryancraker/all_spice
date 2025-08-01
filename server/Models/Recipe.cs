namespace all_spice.Models;

public class Recipe : DbItem<int>
{
    public string Title { get; set; }
    public string Img { get; set; }
    public string Category { get; set; }
    public string CreatorId { get; set; }
    public Profile Creator { get; set; }
    public int FavoriteCount { get; set; }
#nullable enable
    public string? Instructions { get; set; }
}

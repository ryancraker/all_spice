namespace all_spice.Models;

public class Favorite : DbItem<int>
{
    public int RecipeId { get; set; }
    public string AccountId { get; set; }
}

public class FavoriteRecipe : Recipe
{
    public int FavoriteId { get; set; }
    public string AccountId { get; set; }
}

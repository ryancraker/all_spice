namespace all_spice.Models;

public class Ingredient : DbItem<int>
{
    public string Name { get; set; }
    public string Quantity { get; set; }
    public int RecipeId { get; set; }
}

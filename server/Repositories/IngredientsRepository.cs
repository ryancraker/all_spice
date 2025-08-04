namespace all_spice.Repositories;

public class IngredientsRepository
{
    private readonly IDbConnection _db;

    public IngredientsRepository(IDbConnection db)
    {
        _db = db;
    }

    public Ingredient AddIngredientToRecipe(Ingredient ingredientData)
    {
        string sql =
            @"
    INSERT INTO ingredients (name, quantity, recipe_id) VALUES (@name, @quantity, @recipeId);

    SELECT * FROM ingredients WHERE id = LAST_INSERT_ID()
    ;";
        Ingredient ingredient = _db.Query<Ingredient>(sql, ingredientData).FirstOrDefault();
        return ingredient;
    }

    public List<Ingredient> GetIngredientsForRecipe(int recipeId)
    {
        string sql =
            @"
        SELECT * FROM ingredients WHERE recipe_id = @recipeId
        ;";
        List<Ingredient> ingredients = _db.Query<Ingredient>(sql, new { recipeId }).ToList();
        return ingredients;
    }

    public Ingredient GetIngredientById(int ingredientId)
    {
        string sql = "SELECT * FROM ingredients WHERE id = @ingredientId LIMIT 1;";
        Ingredient ingredient = _db.Query<Ingredient>(sql, new { ingredientId }).SingleOrDefault();
        return ingredient;
    }

    public void DeleteIngredient(int ingredientId)
    {
        string sql = @"DELETE FROM ingredients WHERE id = @ingredientId LIMIT 1";
        _db.Execute(sql, new { ingredientId });
    }
}

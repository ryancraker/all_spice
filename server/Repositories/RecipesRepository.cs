namespace all_spice.Repositories;

public class RecipesRepository
{
    private readonly IDbConnection _db;

    public RecipesRepository(IDbConnection db)
    {
        _db = db;
    }

    public Recipe CreateRecipe(Recipe recipeData)
    {
        string sql =
            @"INSERT INTO recipes 
        (title, instructions, img, category, creator_id) 
        VALUES (@title,@instructions, @img, @category, @creatorId);

        SELECT * FROM recipes JOIN accounts ON recipes.creator_id = accounts.id WHERE recipes.id = LAST_INSERT_ID();";
        Recipe recipe = _db.Query(
                sql,
                (Recipe recipe, Profile profile) =>
                {
                    recipe.Creator = profile;
                    return recipe;
                },
                recipeData
            )
            .SingleOrDefault();
        return recipe;
    }

    public List<Recipe> GetAllRecipes()
    {
        string sql =
            @"SELECT recipes.*, 
            COUNT(favorites.id) AS favoriteCount, accounts.*
            FROM recipes 
            JOIN accounts ON recipes.creator_id = accounts.id 
            LEFT JOIN favorites ON recipes.id = favorites.recipe_id
            GROUP BY recipes.id
            ORDER BY recipes.id ASC";
        List<Recipe> recipes = _db.Query(
                sql,
                (Recipe recipe, Profile profile) =>
                {
                    recipe.Creator = profile;
                    return recipe;
                }
            )
            .ToList();
        return recipes;
    }

    public List<Recipe> GetAllRecipes(string category)
    {
        string sql =
            @"SELECT recipes.*, 
            COUNT(favorites.id) AS favoriteCount, accounts.*
            FROM recipes 
            JOIN accounts ON recipes.creator_id = accounts.id 
            LEFT JOIN favorites ON recipes.id = favorites.recipe_id
            WHERE recipes.category = @category
            GROUP BY recipes.id
            ORDER BY recipes.id ASC;";
        List<Recipe> recipes = _db.Query(
                sql,
                (Recipe recipe, Profile profile) =>
                {
                    recipe.Creator = profile;
                    return recipe;
                },
                new { category }
            )
            .ToList();
        return recipes;
    }

    public Recipe GetRecipeById(int recipeId)
    {
        string sql =
            "SELECT recipes.*, COUNT(favorites.id) AS favoriteCount, accounts.* FROM recipes JOIN accounts ON recipes.creator_id = accounts.id LEFT JOIN favorites ON recipes.id = favorites.recipe_id WHERE recipes.id = @recipeId GROUP BY recipes.id;";
        Recipe recipe = _db.Query(
                sql,
                (Recipe recipe, Profile profile) =>
                {
                    recipe.Creator = profile;
                    return recipe;
                },
                new { recipeId }
            )
            .FirstOrDefault();
        return recipe;
    }

    public Recipe UpdateRecipe(Recipe recipeToUpdate)
    {
        string sql =
            @"UPDATE recipes SET title = @title, instructions = @instructions, img = @img, category = @category WHERE id = @id LIMIT 1;

         SELECT * FROM recipes JOIN accounts ON recipes.creator_id = accounts.id WHERE recipes.id = @id;";
        Recipe recipe = _db.Query(
                sql,
                (Recipe recipe, Profile profile) =>
                {
                    recipe.Creator = profile;
                    return recipe;
                },
                recipeToUpdate
            )
            .SingleOrDefault();
        return recipe;
    }

    public void DeleteRecipe(int recipeId)
    {
        string sql = "DELETE FROM recipes WHERE id = @recipeId LIMIT 1;";
        _db.Execute(sql, new { recipeId });
    }
}

namespace all_spice.Repositories;

public class FavoritesRepository
{
    private readonly IDbConnection _db;

    public FavoritesRepository(IDbConnection db)
    {
        _db = db;
    }

    public FavoriteRecipe FavoriteARecipe(Favorite favoriteData)
    {
        string sql =
            @"INSERT INTO favorites (recipe_id, account_id) VALUES (@recipeId, @accountId);

            SELECT favorites.*, recipes.*, accounts.*
            FROM
            favorites
            RIGHT JOIN recipes ON favorites.recipe_id = recipes.id
            JOIN accounts ON recipes.creator_id = accounts.id
            WHERE favorites.id = LAST_INSERT_ID();";
        FavoriteRecipe favoriteRecipe = _db.Query(
                sql,
                (Favorite favorite, FavoriteRecipe favoriteRecipe, Profile profile) =>
                {
                    favoriteRecipe.FavoriteId = favorite.Id;
                    favoriteRecipe.AccountId = favorite.AccountId;
                    favoriteRecipe.Creator = profile;
                    return favoriteRecipe;
                },
                favoriteData
            )
            .SingleOrDefault();
        return favoriteRecipe;
    }

    public List<FavoriteRecipe> GetFavoriteRecipes(string userId)
    {
        string sql =
            @"
                SELECT
                recipes.*,
                favorites.*,
                accounts.*
                FROM
                favorites
                RIGHT JOIN recipes_with_fav_count AS recipes ON favorites.recipe_id = recipes.id
                JOIN accounts ON recipes.creator_id = accounts.id
                WHERE favorites.account_id = @userId;
            ";
        List<FavoriteRecipe> favoriteRecipes = _db.Query(
                sql,
                (FavoriteRecipe favoriteRecipe, Favorite favorite, Profile profile) =>
                {
                    favoriteRecipe.Creator = profile;
                    favoriteRecipe.AccountId = favorite.AccountId;
                    favoriteRecipe.FavoriteId = favorite.Id;
                    return favoriteRecipe;
                },
                new { userId }
            )
            .ToList();
        return favoriteRecipes;
    }

    public Favorite GetFavoriteById(int favoriteId)
    {
        string sql = @"SELECT * FROM favorites WHERE favorites.id = @favoriteId ;";
        Favorite favorite = _db.Query<Favorite>(sql, new { favoriteId }).SingleOrDefault();
        return favorite;
    }

    public void DeleteFavorite(int favoriteId)
    {
        string sql = @"DELETE FROM favorites WHERE favorites.id = @favoriteId LIMIT 1;";
        _db.Execute(sql, new { favoriteId });
    }
}

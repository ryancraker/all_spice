namespace all_spice.Services;

public class FavoritesService
{
    private readonly FavoritesRepository _repo;
    private readonly RecipesService _recipesService;

    public FavoritesService(FavoritesRepository repo, RecipesService recipesService)
    {
        _repo = repo;
        _recipesService = recipesService;
    }

    public FavoriteRecipe FavoriteARecipe(Favorite favoriteData)
    {
        Recipe recipe = _recipesService.GetRecipeById(favoriteData.RecipeId);
        // if (recipe.CreatorId == favoriteData.AccountId)
        //     throw new Exception("Cannot favorite your own recipe.");
        FavoriteRecipe favoriteRecipe = _repo.FavoriteARecipe(favoriteData);
        return favoriteRecipe;
    }

    public List<FavoriteRecipe> GetFavoriteRecipes(string userId)
    {
        List<FavoriteRecipe> favoriteRecipes = _repo.GetFavoriteRecipes(userId);
        return favoriteRecipes;
    }

    public string DeleteFavorite(int favoriteId, Account userInfo)
    {
        Favorite favorite = GetFavoriteById(favoriteId);
        if (favorite.AccountId != userInfo.Id)
            throw new Exception(
                $"You cannot remove another's favorite, {userInfo.Name.ToUpper()}!"
            );
        _repo.DeleteFavorite(favoriteId);
        return $"Recipe {favorite.RecipeId} has been removed from your favorites";
    }

    public Favorite GetFavoriteById(int favoriteId)
    {
        Favorite favorite = _repo.GetFavoriteById(favoriteId);
        if (favorite == null)
            throw new Exception($"No favorite found with ID: {favoriteId}");
        return favorite;
    }
}

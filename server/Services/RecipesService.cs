namespace all_spice.Services;

public class RecipesService
{
    private readonly RecipesRepository _repo;

    public RecipesService(RecipesRepository repo)
    {
        _repo = repo;
    }

    public Recipe CreateRecipe(Recipe recipeData)
    {
        Recipe recipe = _repo.CreateRecipe(recipeData);
        return recipe;
    }

    public List<Recipe> GetAllRecipes()
    {
        List<Recipe> recipes = _repo.GetAllRecipes();
        return recipes;
    }

    public Recipe GetRecipeById(int recipeId)
    {
        Recipe recipe = _repo.GetRecipeById(recipeId);
        if (recipe == null)
            throw new Exception($"Invalid id. No recipes have an id of {recipeId}");
        return recipe;
    }

    public Recipe UpdateRecipe(int recipeId, Recipe recipeData, Account userInfo)
    {
        Recipe recipeToUpdate = GetRecipeById(recipeId);
        if (recipeToUpdate.CreatorId != userInfo.Id)
            throw new Exception($"This ain't your recipe, {userInfo.Name.ToUpper()}!");
        recipeToUpdate.Title = recipeData.Title ?? recipeToUpdate.Title;
        recipeToUpdate.Instructions = recipeData.Instructions ?? recipeToUpdate.Instructions;
        recipeToUpdate.Img = recipeData.Img ?? recipeToUpdate.Img;
        recipeToUpdate.Category = recipeData.Category ?? recipeToUpdate.Category;
        Recipe recipe = _repo.UpdateRecipe(recipeToUpdate);
        return recipe;
    }

    public string DeleteRecipe(int recipeId, Account userInfo)
    {
        Recipe recipeToDelete = GetRecipeById(recipeId);
        if (recipeToDelete == null)
            throw new Exception($"Invalid id. No recipes have an id of {recipeId}");
        if (recipeToDelete.CreatorId != userInfo.Id)
            throw new Exception(
                $"You cannot delete someone else's recipe, {userInfo.Name.ToUpper()}!"
            );
        _repo.DeleteRecipe(recipeId);
        return $"Your recipe for {recipeToDelete.Title} has been deleted";
    }
}

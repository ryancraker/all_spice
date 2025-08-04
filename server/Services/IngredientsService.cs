namespace all_spice.Services;

public class IngredientsService
{
    private readonly IngredientsRepository _repo;
    private readonly RecipesService _recipesService;

    public IngredientsService(IngredientsRepository repo, RecipesService recipesService)
    {
        _repo = repo;
        _recipesService = recipesService;
    }

    public Ingredient AddIngredientToRecipe(Ingredient ingredientData, Account userInfo)
    {
        Recipe recipeToModify = _recipesService.GetRecipeById(ingredientData.RecipeId);
        if (recipeToModify.CreatorId != userInfo.Id)
            throw new Exception($"You cannot modify another's recipe, {userInfo.Name.ToUpper()}!");
        Ingredient ingredient = _repo.AddIngredientToRecipe(ingredientData);
        return ingredient;
    }

    public List<Ingredient> GetIngredientsForRecipe(int recipeId)
    {
        _recipesService.GetRecipeById(recipeId);
        List<Ingredient> ingredients = _repo.GetIngredientsForRecipe(recipeId);
        return ingredients;
    }

    public string DeleteIngredient(int ingredientId, Account userInfo)
    {
        Ingredient ingredient = GetIngredientById(ingredientId);
        Recipe recipe = _recipesService.GetRecipeById(ingredient.RecipeId);
        if (recipe.CreatorId != userInfo.Id)
            throw new Exception($"You cannot modify another's recipe, {userInfo.Name.ToUpper()}!");
        _repo.DeleteIngredient(ingredientId);
        return $"{ingredient.Name} has been removed from the recipe {recipe.Title}.";
    }

    public Ingredient GetIngredientById(int ingredientId)
    {
        Ingredient ingredient = _repo.GetIngredientById(ingredientId);
        if (ingredient == null)
            throw new Exception($"No ingredients is database with id {ingredientId}");
        return ingredient;
    }
}

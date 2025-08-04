namespace all_spice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly RecipesService _recipesService;
    private readonly Auth0Provider _auth;
    private readonly IngredientsService _ingredientsService;

    public RecipesController(
        RecipesService recipesService,
        Auth0Provider auth,
        IngredientsService ingredientsService
    )
    {
        _recipesService = recipesService;
        _auth = auth;
        _ingredientsService = ingredientsService;
    }

    [HttpPost, Authorize]
    public async Task<ActionResult<Recipe>> CreateRecipe([FromBody] Recipe recipeData)
    {
        try
        {
            Account userInfo = await _auth.GetUserInfoAsync<Account>(HttpContext);
            recipeData.CreatorId = userInfo.Id;
            Recipe recipe = _recipesService.CreateRecipe(recipeData);
            return Ok(recipe);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet]
    public ActionResult<List<Recipe>> GetAllRecipes([FromQuery] string category)
    {
        try
        {
            List<Recipe> recipes;
            if (category == null)
            {
                recipes = _recipesService.GetAllRecipes();
            }
            else
            {
                recipes = _recipesService.GetAllRecipes(category);
            }
            return Ok(recipes);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("{recipeId}")]
    public ActionResult<Recipe> GetRecipeById(int recipeId)
    {
        try
        {
            Recipe recipe = _recipesService.GetRecipeById(recipeId);
            return Ok(recipe);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("{recipeId}"), Authorize]
    public async Task<ActionResult<Recipe>> UpdateRecipe([FromBody] Recipe recipeData, int recipeId)
    {
        try
        {
            Account userInfo = await _auth.GetUserInfoAsync<Account>(HttpContext);
            Recipe recipe = _recipesService.UpdateRecipe(recipeId, recipeData, userInfo);
            return Ok(recipe);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("{recipeId}"), Authorize]
    public async Task<ActionResult<string>> DeleteRecipe(int recipeId)
    {
        try
        {
            Account userInfo = await _auth.GetUserInfoAsync<Account>(HttpContext);
            string message = _recipesService.DeleteRecipe(recipeId, userInfo);
            return Ok(message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("{recipeId}/ingredients")]
    public ActionResult<List<Ingredient>> GetIngredientsForRecipe(int recipeId)
    {
        try
        {
            List<Ingredient> ingredients = _ingredientsService.GetIngredientsForRecipe(recipeId);
            return ingredients;
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}

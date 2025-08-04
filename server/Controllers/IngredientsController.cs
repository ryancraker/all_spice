namespace all_spice.Controllers;

[ApiController]
[Route("api/ingredients")]
public class IngredientsController : ControllerBase
{
    private readonly IngredientsService _ingredientsService;
    private readonly Auth0Provider _auth;

    public IngredientsController(IngredientsService ingredientsService, Auth0Provider auth)
    {
        _ingredientsService = ingredientsService;
        _auth = auth;
    }

    [HttpPost, Authorize]
    public async Task<ActionResult<Ingredient>> AddIngredientToRecipe(
        [FromBody] Ingredient ingredientData
    )
    {
        try
        {
            Account userinfo = await _auth.GetUserInfoAsync<Account>(HttpContext);
            Ingredient ingredient = _ingredientsService.AddIngredientToRecipe(
                ingredientData,
                userinfo
            );
            return Ok(ingredient);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("{ingredientId}"), Authorize]
    public async Task<ActionResult<string>> DeleteIngredient(int ingredientId)
    {
        try
        {
            Account userinfo = await _auth.GetUserInfoAsync<Account>(HttpContext);
            string message = _ingredientsService.DeleteIngredient(ingredientId, userinfo);
            return message;
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}

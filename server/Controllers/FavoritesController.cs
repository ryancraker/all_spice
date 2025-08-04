namespace all_spice.Controllers;

[ApiController]
[Route("api/favorites")]
public class FavoritesController : ControllerBase
{
    private readonly FavoritesService _favoritesService;
    private readonly Auth0Provider _auth;

    public FavoritesController(FavoritesService favoritesService, Auth0Provider auth)
    {
        _favoritesService = favoritesService;
        _auth = auth;
    }

    [HttpPost, Authorize]
    public async Task<ActionResult<FavoriteRecipe>> FavoriteARecipe(
        [FromBody] Favorite favoriteData
    )
    {
        try
        {
            Account userInfo = await _auth.GetUserInfoAsync<Account>(HttpContext);
            favoriteData.AccountId = userInfo.Id;
            FavoriteRecipe favoriteRecipe = _favoritesService.FavoriteARecipe(favoriteData);
            return Ok(favoriteRecipe);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("{favoriteId}"), Authorize]
    public async Task<ActionResult<string>> DeleteFavorite(int favoriteId)
    {
        try
        {
            Account userInfo = await _auth.GetUserInfoAsync<Account>(HttpContext);
            string removedFavorite = _favoritesService.DeleteFavorite(favoriteId, userInfo);
            return Ok(removedFavorite);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}

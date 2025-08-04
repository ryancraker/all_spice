namespace all_spice.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;
    private readonly Auth0Provider _auth0Provider;
    private readonly FavoritesService _favoritesSerivce;

    public AccountController(
        AccountService accountService,
        Auth0Provider auth0Provider,
        FavoritesService favoritesService
    )
    {
        _accountService = accountService;
        _auth0Provider = auth0Provider;
        _favoritesSerivce = favoritesService;
    }

    [HttpGet]
    public async Task<ActionResult<Account>> Get()
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            return Ok(_accountService.GetOrCreateAccount(userInfo));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("favorites")]
    public async Task<ActionResult<List<FavoriteRecipe>>> GetFavoriteRecipes()
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            List<FavoriteRecipe> favoriteRecipes = _favoritesSerivce.GetFavoriteRecipes(
                userInfo.Id
            );
            return favoriteRecipes;
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}

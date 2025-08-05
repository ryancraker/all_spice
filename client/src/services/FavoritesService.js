import { logger } from "@/utils/Logger.js";
import { api } from "./AxiosService.js";
import { AppState } from "@/AppState.js";
import { Recipe } from "@/models/Recipe.js";

class FavoritesService {
	async unlikeRecipe(recipeId) {
		const favoriteRecipes = AppState.favoriteRecipes;
		const unfavoriteRecipe = favoriteRecipes.find(favorite => favorite.id == recipeId);
		const res = await api.delete(`api/favorites/${unfavoriteRecipe.favoriteId}`);
		const recipe = AppState.recipes.find(recipe => recipe.id == recipeId);
		logger.log(res.data);
		const unfavoriteIndex = favoriteRecipes.findIndex(
			favorite => favorite.favoriteId == unfavoriteRecipe.favoriteId
		);
		favoriteRecipes.splice(unfavoriteIndex, 1);
		recipe.favoriteCount--;
	}
	async likeRecipe(recipeId) {
		const res = await api.post("api/favorites/", { recipeId: recipeId });
		logger.log(res.data);
		const favoriteRecipe = AppState.recipes.find(recipe => recipe.id == recipeId);
		favoriteRecipe.favoriteCount++;
		AppState.favoriteRecipes.push(new Recipe(res.data));
	}
}
export const favoritesService = new FavoritesService();

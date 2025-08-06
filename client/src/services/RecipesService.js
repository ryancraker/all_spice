import { logger } from "@/utils/Logger.js";
import { api } from "./AxiosService.js";
import { AppState } from "@/AppState.js";
import { Recipe } from "@/models/Recipe.js";

class RecipesService {
	async submitRecipe(recipeData) {
		const res = await api.post("api/recipes", recipeData);
		logger.log(res.data);
		const recipe = new Recipe(res.data);
		return recipe.id;
	}
	async deleteRecipe(recipeId) {
		const res = await api.delete(`api/recipes/${recipeId}`);
		logger.log(res.data);
		AppState.selectedRecipe = null;
	}
	async getRecipeById(recipeId) {
		AppState.selectedRecipe = null;
		const res = await api.get(`api/recipes/${recipeId}`);
		logger.log(res.data);
		AppState.selectedRecipe = new Recipe(res.data);
	}
	async getAllRecipes() {
		AppState.recipes = [];
		const res = await api.get("api/recipes");
		logger.log(res.data);
		AppState.recipes = res.data.map(pojo => new Recipe(pojo));
	}
}
export const recipesService = new RecipesService();

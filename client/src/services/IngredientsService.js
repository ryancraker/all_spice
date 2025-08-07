import { logger } from "@/utils/Logger.js";
import { api } from "./AxiosService.js";
import { AppState } from "@/AppState.js";
import { Ingredient } from "@/models/Ingredient.js";

class IngredientsService {
	async deleteIngredient(ingredientId) {
		const ingredients = AppState.ingredients;
		const ingredientIndex = ingredients.findIndex(ingredient => ingredient.id == ingredientId);
		const res = await api.delete(`api/ingredients/${ingredientId}`);
		logger.log(res.data);
		ingredients.splice(ingredientIndex, 1);
	}
	async addIngredient(ingredientData) {
		const res = await api.post("api/ingredients", ingredientData);
		logger.log(res.data);
		AppState.ingredients.push(new Ingredient(res.data));
	}
	async getIngredientsByRecipeId(recipeId) {
		const res = await api.get(`api/recipes/${recipeId}/ingredients`);
		logger.log(res.data);
		AppState.ingredients = res.data.map(pojo => new Ingredient(pojo));
	}
}
export const ingredientsService = new IngredientsService();

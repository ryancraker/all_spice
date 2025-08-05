import { logger } from "@/utils/Logger.js";
import { api } from "./AxiosService.js";
import { AppState } from "@/AppState.js";
import { Ingredient } from "@/models/Ingredient.js";

class IngredientsService {
	async getIngredientsByRecipeId(recipeId) {
		const res = await api.get(`api/recipes/${recipeId}/ingredients`);
		logger.log(res.data);
		AppState.ingredients = res.data.map(pojo => new Ingredient(pojo));
	}
}
export const ingredientsService = new IngredientsService();

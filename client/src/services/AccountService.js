import { Recipe } from "@/models/Recipe.js";
import { AppState } from "../AppState.js";
import { Account } from "../models/Account.js";
import { logger } from "../utils/Logger.js";
import { api } from "./AxiosService.js";

class AccountService {
	async getMyFavorites() {
		AppState.favoriteRecipes = [];
		const res = await api.get("account/favorites");
		logger.log(res.data);
		AppState.favoriteRecipes = res.data.map(pojo => new Recipe(pojo));
	}
	async getAccount() {
		try {
			const res = await api.get("/account");
			AppState.account = new Account(res.data);
		} catch (err) {
			logger.error("HAVE YOU STARTED YOUR SERVER YET???", err);
		}
	}
}

export const accountService = new AccountService();

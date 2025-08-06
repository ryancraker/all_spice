<script setup>
	import { AppState } from "@/AppState.js";
	import { Recipe } from "@/models/Recipe.js";
	import { favoritesService } from "@/services/FavoritesService.js";
	import { logger } from "@/utils/Logger.js";
	import { Pop } from "@/utils/Pop.js";
	import { computed } from "vue";
	import { RouterLink } from "vue-router";

	const account = computed(() => AppState.account);
	const favorites = computed(() => AppState.favoriteRecipes);
	const recipe = defineProps({
		recipe: { type: Recipe, required: true }
	});

	async function likeRecipe(recipeId) {
		try {
			logger.log(recipeId);
			await favoritesService.likeRecipe(recipeId);
		} catch (error) {
			Pop.error(error);
			logger.error(error);
		}
	}
	async function unlikeRecipe(recipeId) {
		try {
			await favoritesService.unlikeRecipe(recipeId);
		} catch (error) {
			Pop.error(error);
			logger.error("could not remove favorite", error);
		}
	}
</script>

<template>
	<div class="position-relative recipe-card">
		<RouterLink :to="{ name: 'Recipe Details', params: { recipeId: recipe.recipe.id } }">
			<img class="recipe-img" :src="recipe.recipe.imgUrl" :alt="recipe.recipe.title" />
		</RouterLink>
		<span class="recipe-category text-capitalize fs-5">{{ recipe.recipe.category }}</span>
		<div v-if="account">
			<button
				v-if="favorites.find(favorite => recipe.recipe.id == favorite.id)"
				type="button"
				class="liked-recipe"
				@click="unlikeRecipe(recipe.recipe.id)">
				<i class="text-red mdi mdi-heart"></i>
			</button>
			<button v-else type="button" class="liked-recipe" @click="likeRecipe(recipe.recipe.id)">
				<i class="mdi mdi-heart-outline"></i>
			</button>
		</div>
		<RouterLink :to="{ name: 'Recipe Details', params: { recipeId: recipe.recipe.id } }">
			<div class="recipe-title d-flex justify-content-between flex-column fs-4">
				<span>{{ recipe.recipe.title }}</span>
				<span class=""> {{ recipe.recipe.favoriteCount }} <i class="mdi mdi-heart"></i> </span>
			</div>
		</RouterLink>
	</div>
</template>

<style lang="scss" scoped>
	.recipe-card {
		transition: ease 0.02s;
		text-shadow: 0px 2px 3px black;

		&:hover {
			transform: scale(1.05);
			transition: ease 0.5s;
			z-index: 9998;
		}
	}

	.recipe-img {
		width: 100%;
		height: 40dvh;
		object-fit: cover;
		object-position: center;
		border-radius: 10px;
		box-shadow: 0px 2px 7px black;
	}

	.recipe-category {
		position: absolute;
		top: 0.25rem;
		left: 0.5rem;
		background-color: rgba(128, 128, 128, 0.56);
		backdrop-filter: blur(5px);
		color: white;
		padding: 0.5rem;
		border-radius: 20px;
	}

	.recipe-title {
		position: absolute;
		bottom: 1rem;
		left: 0.5rem;
		background-color: rgba(128, 128, 128, 0.56);
		backdrop-filter: blur(5px);
		color: white;
		padding: 0.5rem;
		width: 96%;
		border-radius: 10px;
	}

	.liked-recipe {
		position: absolute;
		top: 0;
		right: 0.5rem;
		padding: 0.25rem;
		border-bottom-left-radius: 5px;
		border-bottom-right-radius: 5px;
		background-color: rgba(128, 128, 128, 0.56);
		backdrop-filter: blur(5px);
		border: none;
		font-size: 2rem;
	}
</style>

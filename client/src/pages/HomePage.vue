<script setup>
	import { AppState } from "@/AppState.js";
	import Login from "@/components/Login.vue";
	import NewRecipeModal from "@/components/NewRecipeModal.vue";
	import RecipeCard from "@/components/RecipeCard.vue";
	import { accountService } from "@/services/AccountService.js";
	import { recipesService } from "@/services/RecipesService.js";
	import { logger } from "@/utils/Logger.js";
	import { Pop } from "@/utils/Pop.js";
	import { computed, onMounted, ref, watch } from "vue";
	onMounted(() => getAllRecipes());

	const account = computed(() => AppState.account);
	watch(account, getMyFavorites);

	const recipes = computed(() => {
		if (selectedCategory.value == "") {
			if (selectedRecipes.value == "my recipes") {
				return AppState.recipes.filter(recipe => recipe.creatorId == account.value?.id);
			}
			if (selectedRecipes.value == "favorites") {
				return AppState.favoriteRecipes;
			}
			return AppState.recipes;
		}
		if (selectedCategory.value != "") {
			if (selectedRecipes.value == "my recipes") {
				return AppState.recipes.filter(
					recipe =>
						recipe.creatorId == account.value?.id && recipe.category == selectedCategory.value
				);
			}
			if (selectedRecipes.value == "favorites") {
				return AppState.favoriteRecipes.filter(recipe => recipe.category == selectedCategory.value);
			}
			return AppState.recipes.filter(recipe => recipe.category == selectedCategory.value);
		}
		return AppState.recipes;
	});
	const selectedCategory = ref("");
	const selectedRecipes = ref("home");

	async function getAllRecipes() {
		try {
			logger.log("Getting the recipes");
			await recipesService.getAllRecipes();
		} catch (error) {
			Pop.error(error);
			logger.error(error);
		}
	}

	async function getMyFavorites() {
		try {
			logger.log("getting your favorites");
			await accountService.getMyFavorites();
		} catch (error) {
			Pop.error(error);
			logger.error(error);
		}
	}
</script>

<template>
	<section class="container-fluid">
		<div class="row position-relative mb-5">
			<div class="col-12">
				<div class="login-search d-flex align-items-center gap-2">
					<div class="d-flex">
						<label for="recipe-category"></label>
						<select
							v-model="selectedCategory"
							class="form-select"
							aria-label="Category Select"
							id="recipe-category"
							required>
							<option disabled selected>Select a Category</option>
							<option value="">All</option>
							<option value="breakfast">Breakfast</option>
							<option value="lunch">Lunch</option>
							<option value="dinner">Dinner</option>
							<option value="snack">Snack</option>
						</select>
					</div>
					<Login />
				</div>
				<div class="hero-section text-center text-light d-flex flex-column justify-content-center">
					<h1>All-Spice</h1>
					<h2>Cherish Your Family</h2>
					<h2>And Their Cooking</h2>
				</div>
			</div>
			<div class="col-12 d-flex justify-content-center">
				<div class="hero-buttons">
					<button
						@click="selectedRecipes = 'home'"
						class="hero-button"
						:class="{ selectedbutton: selectedRecipes == 'home' }">
						Home
					</button>
					<button
						@click="selectedRecipes = 'my recipes'"
						v-if="account"
						class="hero-button"
						:class="{ selectedbutton: selectedRecipes == 'my recipes' }">
						My Recipes
					</button>
					<button
						@click="selectedRecipes = 'favorites'"
						v-if="account"
						class="hero-button"
						:class="{ selectedbutton: selectedRecipes == 'favorites' }">
						Favorites
					</button>
				</div>
			</div>
		</div>
		<div v-if="recipes.length > 0" class="row">
			<div class="col-lg-4 col-md-6" v-for="recipe in recipes" :key="recipe.id">
				<div class="m-5">
					<RecipeCard :recipe />
				</div>
			</div>
			<!-- ANCHOR add button -->
			<div class="add-button text-end fs-3">
				<button
					v-if="account"
					type="button"
					title="Add a recipe"
					data-bs-toggle="modal"
					data-bs-target="#recipe-modal"
					aria-label="Add a recipe"
					aria-labelledby="Add a recipe">
					<i class="mdi mdi-plus"></i>
				</button>
			</div>
		</div>
		<div v-else class="row mt-5">
			<p class="fs-1 text-center">Loading Recipes...<i class="mdi mdi-loading mdi-spin"></i></p>
		</div>
		<NewRecipeModal class="recipe-modal" />
	</section>
</template>

<style scoped lang="scss">
	a {
		text-decoration: none;
	}

	.recipe-modal {
		z-index: 9999;
	}

	.add-button {
		position: sticky;
		bottom: 5%;
		z-index: 9999;
		& button {
			margin-right: 1rem;
			border-radius: 50%;
			height: 2rem;
			aspect-ratio: 1/1;
			height: 3rem;
			color: white;
			background-color: green;
			border: thin inset;
			&:active {
				transform: scale(0.95);
				filter: brightness(0.9);
			}
		}
	}
	.hero-button {
		background-color: white;
		border: none;
		color: green;
		padding: 1rem;
		padding-inline-start: 2rem;
		padding-inline-end: 2rem;
		border-radius: 5px;

		&:hover {
			filter: brightness(0.9);
			transform: scale(2rem);
		}

		&:active {
			filter: brightness(1);
		}
	}

	.hero-buttons {
		position: absolute;
		bottom: -8%;
		box-shadow: 0px 2px 7px black;
		border-radius: 5px;
		background-color: white;
	}
	.selectedbutton {
		background-color: green;
		color: white;
		font-weight: bold;
	}
	.hero-section {
		background-image: url(../assets/img/HomeBg.jpg);
		width: 100%;
		height: 40dvh;
		background-size: cover;
		background-repeat: no-repeat;
		background-position: top;
		text-shadow: 0px 2px 2px black;
		margin-top: 1rem;
		border-radius: 5px;
		box-shadow: 0px 2px 7px black;
	}
	.login-search {
		position: absolute;
		top: 2rem;
		right: 2rem;
	}
</style>

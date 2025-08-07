<script setup>
	import { AppState } from "@/AppState.js";
	import Navbar from "@/components/Navbar.vue";
	import { ingredientsService } from "@/services/IngredientsService.js";
	import { recipesService } from "@/services/RecipesService.js";
	import { logger } from "@/utils/Logger.js";
	import { Pop } from "@/utils/Pop.js";
	import { computed, onMounted, ref } from "vue";
	import { useRoute } from "vue-router";

	onMounted(() => {
		getIngredientsByRecipeId();
		getRecipeById();
	});
	// SECTION variables
	const route = useRoute();
	const recipe = computed(() => AppState.selectedRecipe);
	const ingredients = computed(() => AppState.ingredients);
	const account = computed(() => AppState.account);
	const editMode = ref(false);
	const newIngredient = ref({
		name: "",
		quantity: "",
		recipeId: route.params.recipeId
	});
	const recipeData = ref({
		title: recipe.value?.title,
		instructions: recipe.value?.instructions
	});
	// !SECTION
	// SECTION functions
	async function getRecipeById() {
		try {
			await recipesService.getRecipeById(route.params.recipeId);
			recipeData.value.instructions = recipe.value.instructions;
			recipeData.value.title = recipe.value.title;
		} catch (error) {
			Pop.error(error);
			logger.error(error, "could not get recipe");
		}
	}

	async function getIngredientsByRecipeId() {
		try {
			await ingredientsService.getIngredientsByRecipeId(route.params.recipeId);
		} catch (error) {
			Pop.error(error);
			logger.error(error, "could not get ingredients");
		}
	}

	async function deleteRecipe() {
		const confirm = await Pop.confirm("Are you sure you want to remove your recipe?");
		if (!confirm) return;
		try {
			await recipesService.deleteRecipe(route.params.recipeId);
			location.replace("/");
		} catch (error) {
			Pop.error(error);
			logger.error(error);
		}
	}

	async function updateRecipe() {
		try {
			const updatedRecipe = recipeData.value;
			const recipeId = route.params.recipeId;
			await recipesService.updateRecipe(updatedRecipe, recipeId);
			editMode.value = false;
		} catch (error) {
			Pop.error(error);
			logger.error(error);
		}
	}

	async function addIngredient() {
		try {
			const ingredient = newIngredient.value;
			await ingredientsService.addIngredient(ingredient);
			newIngredient.value.name = "";
			newIngredient.value.quantity = "";
		} catch (error) {
			Pop.error(error);
			logger.error(error);
		}
	}

	async function deleteIngredient(ingredientId) {
		try {
			logger.log(ingredientId);
			await ingredientsService.deleteIngredient(ingredientId);
		} catch (error) {
			Pop.error(error);
			logger.error(error);
		}
	}
	// !SECTION
</script>

<template>
	<header>
		<Navbar />
	</header>
	<section v-if="recipe" class="container-fluid ps-0">
		<div v-if="!editMode" class="row">
			<div class="col-12">
				<div class="row">
					<div class="col-4 background-img position-relative">
						<span class="fav-count"><i class="mdi mdi-heart"></i> {{ recipe.favoriteCount }}</span>
						<img class="recipe-img" :src="recipe.imgUrl" :alt="recipe.title" />
					</div>
					<div class="col-8">
						<div class="row">
							<div class="col-12 text-left mt-2">
								<div class="d-flex align-items-center gap-4">
									<span class="fs-1">{{ recipe.title }}</span>
									<button
										v-if="recipe.creatorId == account?.id"
										class="btn btn-secondary fs-5 p-1 py-0"
										type="button"
										data-bs-toggle="collapse"
										data-bs-target="#creator-options"
										aria-expanded="false"
										aria-controls="creator-options"
										title="Recipe Creator Options">
										<i class="mdi mdi-dots-horizontal"></i>
									</button>
									<button v-else><i class="mdi mdi-heart-outline"></i></button>
									<div class="collapse collapse-horizontal" id="creator-options">
										<div class="card card-body d-flex gap-2">
											<button
												type="button"
												class="btn btn-secondary"
												@click="editMode = true"
												data-bs-target="#creator-options"
												data-bs-toggle="collapse">
												Edit Recipe
											</button>
											<button type="button" class="btn btn-danger" @click="deleteRecipe()">
												Delete Recipe
											</button>
										</div>
									</div>
								</div>
								<p>by: {{ recipe.creator.name }}</p>
								<span class="recipe-category text-capitalize">{{ recipe.category }}</span>
							</div>
							<div class="col-12">
								<div class="my-5">
									<h2>Ingredients</h2>
									<span v-for="ingredient in ingredients" :key="ingredient.id">
										{{ ingredient.quantity }} {{ ingredient.name }}
									</span>
								</div>
								<h3>Instructions</h3>
								<span>{{ recipe.instructions }}</span>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div v-if="editMode" class="row">
			<div class="col-12">
				<div class="row">
					<div class="col-4 background-img position-relative">
						<span class="fav-count"><i class="mdi mdi-heart"></i> {{ recipe.favoriteCount }}</span>
						<img class="recipe-img" :src="recipe.imgUrl" :alt="recipe.title" />
					</div>
					<div class="col-8">
						<div class="row">
							<form @submit.prevent="updateRecipe()">
								<div class="col-12 text-left mt-2">
									<div class="d-flex align-items-center gap-4">
										<label for="recipe-title"></label>
										<input
											v-model="recipeData.title"
											type="text"
											id="recipe-title"
											maxlength="255"
											:placeholder="recipe.title"
											required />
										<button
											type="button"
											@click="
												(editMode = false),
													(recipeData.instructions = recipe.instructions),
													(recipeData.title = recipe.title)
											">
											Cancel
										</button>
										<button type="submit">Update Recipe</button>
									</div>
									<p>by: {{ recipe.creator.name }}</p>
									<span class="recipe-category text-capitalize">{{ recipe.category }}</span>
								</div>
								<div class="col-12">
									<div class="my-5">
										<h2>Ingredients</h2>
										<span v-for="ingredient in ingredients" :key="ingredient.id">
											<button
												@click="deleteIngredient(ingredient.id)"
												class="mdi mdi-delete"
												title="Remove Ingredient"></button
											>{{ ingredient.quantity }} {{ ingredient.name }}
										</span>
									</div>
									<div class="row">
										<form @submit.prevent="addIngredient()">
											<div class="col-6">
												<label for="ingredient-quantity"></label>
												<input
													v-model="newIngredient.quantity"
													type="text"
													id="ingredient-quantity"
													maxlength="255"
													required />
											</div>
											<div class="col-6">
												<label for="ingredient-name"></label>
												<input
													v-model="newIngredient.name"
													type="text"
													id="ingredient-name"
													maxlength="255"
													required />
											</div>
											<button class="btn" type="submit" title="Add Ingredient">
												<i class="mdi mdi-plus-circle"></i>
											</button>
										</form>
									</div>
									<h3>Instructions</h3>
									<label
										for="instructions"
										title="Recipe Instructions"
										aria-label="Instructions"></label>
									<textarea
										v-model="recipeData.instructions"
										name="instructions"
										id="instructions"
										:placeholder="recipe.instructions"></textarea>
								</div>
							</form>
						</div>
					</div>
				</div>
			</div>
		</div>
	</section>
	<section v-else>
		<h1>Loading your recipe...<i class="mdi mdi-loading mdi-spin"></i></h1>
	</section>
</template>

<style lang="scss" scoped>
	header {
		position: sticky;
		top: 0;
		z-index: 2;
	}
	textarea {
		resize: none;
		width: 100%;
	}
	.card-body {
		width: 200px;
	}
	.fav-count {
		position: absolute;
		top: 0;
		left: 10%;
		font-size: 2rem;
		backdrop-filter: blur(5px);
		background-color: rgba(110, 110, 110, 0.18);
		border-bottom-left-radius: 10px;
		border-bottom-right-radius: 10px;
		padding: 0.25rem;
		color: var(--bs-success);
		font-weight: bold;
		text-shadow: 1px 1px 3px black;
	}
	.recipe-img {
		height: 100dvh;
		width: 100%;
		object-fit: cover;
	}
	.recipe-category {
		background-color: rgba(128, 128, 128, 0.56);
		backdrop-filter: blur(5px);
		color: white;
		padding: 0.5rem;
		border-radius: 20px;
		text-shadow: 2px 3px 5px black;
		font-weight: bold;
	}
</style>

<script setup>
	import { recipesService } from "@/services/RecipesService.js";
	import { logger } from "@/utils/Logger.js";
	import { Pop } from "@/utils/Pop.js";
	import { Modal } from "bootstrap";
	import { ref } from "vue";
	import { useRouter } from "vue-router";

	const router = useRouter();

	const editableRecipeData = ref({
		title: "",
		category: "",
		img: ""
	});

	async function submitRecipe() {
		try {
			logger.log(editableRecipeData.value);
			const recipeData = editableRecipeData.value;
			const recipeId = await recipesService.submitRecipe(recipeData);
			router.push({ name: "Recipe Details", params: { recipeId: recipeId } });
			Modal.getOrCreateInstance("#recipe-modal").hide();
		} catch (error) {
			Pop.error(error);
			logger.error(error);
		}
	}
</script>

<template>
	<div
		class="modal fade"
		id="recipe-modal"
		tabindex="-1"
		aria-labelledby="recipe-modal-label"
		aria-hidden="true">
		<div class="modal-dialog">
			<div class="modal-content">
				<div class="modal-header p-2">
					<h1 class="modal-title fs-5 text-light" id="recipe-modal-label">New Recipe</h1>
				</div>
				<div class="modal-body">
					<form @submit.prevent="submitRecipe()">
						<div class="container">
							<div class="row">
								<div class="col-6">
									<label for="recipe-title" class="form-label mb-2">Title</label>
									<input
										v-model="editableRecipeData.title"
										class="form-control"
										type="text"
										id="recipe-title"
										title="title"
										placeholder="Title..."
										maxlength="255"
										required />
								</div>
								<div class="col-6">
									<label for="recipe-category" class="mb-2">Category</label>
									<select
										v-model="editableRecipeData.category"
										class="form-select"
										aria-label="Category Select"
										id="recipe-category"
										required>
										<option disabled selected>Select a Category</option>
										<option value="breakfast">Breakfast</option>
										<option value="lunch">Lunch</option>
										<option value="dinner">Dinner</option>
										<option value="snack">Snack</option>
									</select>
								</div>
								<div class="col-12">
									<label for="recipe-imgUrl" class="form-label mb-2">Image</label>
									<input
										v-model="editableRecipeData.img"
										class="form-control"
										type="url"
										id="recipe-imgUrl"
										title="imgUrl"
										placeholder="Image Url..."
										maxlength="1000"
										required />
								</div>
							</div>
							<div class="gap-2 d-flex justify-content-end recipe-buttons mt-2">
								<button type="button" class="cancel" data-bs-dismiss="modal">Cancel</button>
								<button type="submit" class="submit">Submit</button>
							</div>
						</div>
					</form>
				</div>
			</div>
		</div>
	</div>
</template>

<style lang="scss" scoped>
	.modal-content {
		border-radius: 0;
		padding-bottom: 0.5rem;
	}
	.recipe-buttons {
		margin-right: 0.5rem;
		& button {
			background-color: white;
			border: none;
			margin-top: 0.75rem;
			padding-left: 0.75rem;
			padding-right: 0.75rem;
			border-radius: 10px;
			&:active {
				filter: brightness(0.9);
			}
		}
		& .submit {
			background-color: var(--bs-success);
			color: white;
			&:disabled {
				filter: brightness(0.5);
			}
		}
	}
	.modal-header {
		background-color: var(--bs-success);
		border-radius: 0;
	}
</style>

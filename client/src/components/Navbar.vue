<script setup>
	import { watch } from "vue";
	import Login from "./Login.vue";
	import { logger } from "@/utils/Logger.js";
	import { Pop } from "@/utils/Pop.js";
	import { accountService } from "@/services/AccountService.js";
	import { AppState } from "@/AppState.js";

	watch(AppState.account, getMyFavorites);

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
	<nav class="navbar navbar-expand-md bg-gray">
		<div class="container-fluid gap-2 mx-3">
			<RouterLink :to="{ name: 'Home' }" class="d-flex align-items-center text-light">
				<img class="navbar-brand" alt="logo" src="../assets/img/shaker.png" height="45" />
				<b class="fs-5">All Spice</b>
			</RouterLink>
			<!-- collapse button -->
			<button
				class="navbar-toggler"
				type="button"
				data-bs-toggle="collapse"
				data-bs-target="#navbar-links"
				aria-controls="navbarText"
				aria-expanded="false"
				aria-label="Toggle navigation">
				<span class="mdi mdi-menu text-light"></span>
			</button>
			<!-- collapsing menu -->
			<div class="collapse navbar-collapse" id="navbar-links">
				<!-- LOGIN COMPONENT HERE -->
				<div class="ms-auto bg-gray d-flex align-items-center gap-3">
					<Login />
				</div>
			</div>
		</div>
	</nav>
</template>

<style lang="scss" scoped>
	a {
		text-decoration: none;
	}

	.nav-link {
		text-transform: uppercase;
		text-decoration: none;
	}

	.navbar-nav .router-link-exact-active {
		border-bottom-left-radius: 0;
		border-bottom-right-radius: 0;
	}

	.navbar {
		position: sticky;
		top: 0;
		border-bottom: solid thin;
	}
</style>

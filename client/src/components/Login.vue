<script setup>
	import { computed } from "vue";
	import { AppState } from "../AppState.js";
	import { AuthService } from "../services/AuthService.js";

	const identity = computed(() => AppState.identity);
	const account = computed(() => AppState.account);

	function login() {
		AuthService.loginWithRedirect();
	}
	function logout() {
		AuthService.logout();
	}
</script>

<template>
	<span class="navbar-text">
		<button class="btn selectable text-green" @click="login" v-if="!identity">Login</button>
		<div v-else>
			<div class="dropdown">
				<div
					role="button"
					class="selectable no-select"
					data-bs-toggle="dropdown"
					aria-expanded="false"
					title="open account menu">
					<div v-if="account?.picture || identity?.picture">
						<img
							:src="account?.picture || identity?.picture"
							alt="account photo"
							height="50"
							class="user-img" />
					</div>
				</div>
				<div
					class="dropdown-menu dropdown-menu-sm-end dropdown-menu-start p-0"
					role="menu"
					title="account menu">
					<div class="list-group">
						<RouterLink :to="{ name: 'Account' }">
							<div class="list-group-item dropdown-item list-group-item-action">Manage Account</div>
						</RouterLink>
						<div
							class="list-group-item dropdown-item list-group-item-action text-danger selectable"
							@click="logout">
							<i class="mdi mdi-logout"></i>
							logout
						</div>
					</div>
				</div>
			</div>
		</div>
	</span>
</template>

<style lang="scss" scoped>
	.user-img {
		height: 50px;
		width: 50px;
		border-radius: 50%;
		object-fit: cover;
		object-position: center;
		box-shadow: 0px 0px 5px black;
	}
</style>

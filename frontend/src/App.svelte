<script>
	import { Events } from 'Events.svelte'
	export let name;
	const state = {
		sidebarIsOpen: false
	}
</script>

<header>
	<nav>
		<h1>CampusNow</h1>
		<button>Log In</button>
	</nav>
</header>
<main>
	{#if state.isOpen}
		<aside>
			<Events></Events>
		</aside>
	{/if}
</main>

<style>
	:root {
		font-family: sans-serif;
	}

	* {
		box-sizing: border-box;
		border: none;
		font-family: inherit;
		margin: 0;
	}

	div,
	header, main, footer, section,
	nav, aside {
		display: grid;
	}
</style>
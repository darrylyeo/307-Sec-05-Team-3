<script>
	import { API } from './api.js'
	import Map from './Map.svelte'
	import Events from './Events.svelte'
	
	const state = {
		sidebarIsOpen: true
	}

	const getEvents = API.event.getAllEvents()
</script>

<header>
	<nav>
		<h1 id="logo"><i>🏫</i> Campus<b>Now</b></h1>
		<button>Log In</button>
	</nav>
</header>
<main>
	<Map {getEvents} />
	{#if state.sidebarIsOpen}
		<aside>
			<Events {getEvents}/>
		</aside>
	{/if}
</main>

<style>
	nav {
		background-color: palegoldenrod;
		grid-auto-flow: column;
		grid-template-columns: 1fr auto;
		grid-gap: 1em;
		padding: 1em;
	}

	#logo {
		font-weight: normal;
		text-transform: uppercase;
	}

	main {
		grid-auto-flow: column;
		grid-template-columns: 1fr 25rem;
	}

	#map {
		background: rgba(0, 0, 0, 0.05);
	}
</style>
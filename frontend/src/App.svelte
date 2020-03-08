<script>
	import Map from './Map.svelte'
	import Events from './Events.svelte'
	
	import { API } from './api.js'
	
	import { writable } from 'svelte/store'

	const getEvents = API.event.getAllEvents()

	const state = {
		sidebarIsOpen: true
	}

	let currentEvent = writable()
	$: console.log('currentEvent:', $currentEvent)
</script>

<header>
	<nav>
		<h1 id="logo"><i>🏫</i> Campus<b>Now</b></h1>
		<button>Log In</button>
	</nav>
</header>
<main>
	<Map {getEvents} {currentEvent} />
	{#if state.sidebarIsOpen}
		<aside>
			<Events {getEvents} {currentEvent} />
		</aside>
	{/if}
</main>

<style>
	nav, aside {
		position: relative;
		z-index: 1;

		backdrop-filter: blur(3px) hue-rotate(80deg);
	}
	nav {
		background-color: rgba(155, 224, 142, 0.8);

		grid-auto-flow: column;
		grid-template-columns: 1fr auto;
		grid-gap: 1em;
		padding: 1em;
	}
	aside {
		background-color: rgba(220, 235, 174, 0.8);
	}

	#logo {
		font-weight: normal;
		text-transform: uppercase;
		color: #254100;
	}

	main {
		grid-auto-flow: column;
		grid-template-columns: 1fr 25rem;
	}
</style>
<script>
	import { API } from './api.js'
	import { writable } from 'svelte/store'

	import Map from './Map.svelte'
	import Events from './Events.svelte'
	import Login from './Login.svelte'

	let sidebarIsOpen = true

	let isShowingLoginRegister = false

	let currentUser = writable()
	$: console.log('currentUser:', $currentUser)
	// $currentUser = {
	// 	firstName: 'Musty',
	// 	lastName: 'Mustang'
	// }

	let events = writable([])

	let currentEvent = writable()
	$: console.log('currentEvent:', $currentEvent)

	const logOut = async function(){
		const result = await API.user.login.logout()
		console.log(result)
		$currentUser = undefined
	}
</script>

<header>
	<nav>
		<h1 id="logo"><i>🏫</i> Campus<b>Now</b></h1>

		{#if $currentUser}
			<p>
				<span>Hello, <b>{$currentUser.firstName} {$currentUser.lastName}</b></span>
				{#if $currentUser.isAdmin}<i title="You're an admin">⭐</i>{/if}
			</p>
			<button on:click={logOut}>Log Out</button>
		{:else}
			{#if isShowingLoginRegister}
				<button on:click={() => isShowingLoginRegister = false}>Cancel</button>
			{:else}
				<button on:click={() => isShowingLoginRegister = true}>Log In/Sign Up</button>
			{/if}
		{/if}
	</nav>
</header>
<main>
	<Map {events} {currentEvent} />
	{#if sidebarIsOpen}
		<aside>
			{#if isShowingLoginRegister}
				<Login {currentUser} />
			{/if}
			<Events {currentUser} {events} {currentEvent} />
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

	#logo {
		font-weight: normal;
		text-transform: uppercase;
		color: #254100;
	}

	main {
		grid-auto-flow: column;
		grid-template-columns: 1fr 25rem;
	}
	
	aside {
		background-color: rgba(220, 235, 174, 0.8);
	}
	aside > * {
		background: linear-gradient(rgba(255, 255, 255, 0.25), transparent);
	}
</style>
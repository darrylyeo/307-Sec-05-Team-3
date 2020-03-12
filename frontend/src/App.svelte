<script>
	import { API } from './api.js'
	import { writable } from 'svelte/store'

	import Map from './Map.svelte'
	import Events from './Events.svelte'
	import Login from './Login.svelte'

	let sidebarIsOpen = true

	let currentUserToken = writable(localStorage.currentUserToken)
	$: localStorage.currentUserToken = $currentUserToken
	$: if($currentUserToken){
		API.login.getCurrentUser($currentUserToken).then(user => {
			if(user)
				$currentUser = {...user, token: $currentUserToken}
		})
	}else{
		$currentUser = undefined
	}

	let currentUser = writable()
	$: console.log('currentUser:', $currentUser)
	// $currentUser = { firstName: 'Musty', lastName: 'Mustang' }

	let isShowingLoginRegister = false
	$: if($currentUser)
		isShowingLoginRegister = false


	let events = writable([])

	let currentEvent = writable()
	$: console.log('currentEvent:', $currentEvent)

	const logOut = async function(){
		const result = await API.login.logout()
		console.log(result)
		if(result)
			$currentUserToken = undefined
	}
</script>

<header>
	<nav>
		<h1 id="logo"><i>🏫</i> Campus<b>Now</b></h1>

		{#if $currentUser}
			<p>
				<span>Hello, <b>{$currentUser.firstName} {$currentUser.lastName}</b></span>
				{#if $currentUser.isAdmin}<i title="You're an admin! You can edit or delete others' events.">⭐</i>{/if}
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
				<Login {currentUserToken} />
			{/if}
			<Events {currentUser} {events} {currentEvent} />
		</aside>
	{/if}
</main>
<script>
	import { API } from './api.js'
	import Event from './Event.svelte'
	import EventForm from './EventForm.svelte'

	export let events, currentEvent, currentUser
	
	let isCreatingNewEvent = false
	let isOnlyShowingUserEvents = false
	let searchFilter = ''

	$: getEvents = $currentUser && isOnlyShowingUserEvents
		? API.event.getEventsByUser($currentUser)
		: API.event.getAllEvents()
	$: getEvents.then(_ => $events = _)

	const filterEvents = events =>
		searchFilter
			? events.filter(event =>
				event.title.toLowerCase().includes(searchFilter.toLowerCase()) ||
				event.description.toLowerCase().includes(searchFilter.toLowerCase())
			)
			: events

	import { fly } from 'svelte/transition'
</script>

<div id="events">
	<div>
		{#if isCreatingNewEvent}
			<h2>New Event</h2>
			<EventForm />
		{/if}

		<div>
			<h2>Upcoming Events</h2>
			{#if currentUser}
				<button on:click={() => isCreatingNewEvent = true}>Create Event</button>
			{/if}
		</div>

		<div>
			<input type="search" placeholder="Search events..." bind:value={searchFilter} />
			{#if currentUser}
				<button on:click={() => isOnlyShowingUserEvents = !isOnlyShowingUserEvents}>{isOnlyShowingUserEvents ? 'All Events' : 'My Events'}</button>
			{/if}
		</div>
	</div>
	{#await getEvents}
		<p transition:fly={{y: -30}}>Looking for events...</p>
	{:then events}
		{#each filterEvents(events) as event (event.listingId)}
			<Event {event} isFocused={$currentEvent === event} highlightString={searchFilter} on:click={e => $currentEvent = event}/>
		{:else}
			<p>No events found.</p>
			<p class="placeholder-icon">🏫</p>
		{/each}
	{:catch error}
		<p>We had some trouble loading the events.</p>
		<p class="error">{error.message}</p>
	{/await}
</div>

<style>
	#events {
		display: grid;
		/* grid-template-rows: auto 1fr; */
		grid-auto-rows: max-content;
		overflow-y: auto;
		grid-gap: 1px;
	}
	#events > * {
		padding: 1rem;

		display: grid;
		grid-auto-flow: row;
		grid-gap: 0.65em;
	}
	#events > * > * {
		display: grid;
		grid-auto-flow: column;
		grid-gap: 0.5em;
		align-items: center;
	}
</style>
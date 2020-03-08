<script>
	import Event from './Event.svelte'
		
	export let getEvents, currentEvent

	let searchFilter = ''

	const filterEvents = events =>
		searchFilter
			? events.filter(event => event.title.includes(searchFilter) || event.description.includes(searchFilter))
			: events

	import { fly } from 'svelte/transition'
</script>

<div id="events">
	<div>
		<h2>Upcoming Events</h2>
		<input type="search" placeholder="Search events..." bind:value={searchFilter} />
	</div>
	{#await getEvents}
		<p transition:fly={{y: -30}}>Looking for events...</p>
	{:then events}
		{#each filterEvents(events) as event (event.listingId)}
			<Event {event} isFocused={$currentEvent === event} highlightString={searchFilter} on:click={e => $currentEvent = event}/>
		{:else}
			<p>No events found.</p>
		{/each}
	{:catch error}
		<p>We couldn't find any events: {error.message}</p>
	{/await}
</div>

<style>
	div {
		display: flex;
	}

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
</style>
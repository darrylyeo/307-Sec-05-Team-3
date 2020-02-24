<script>
	import { formatDate } from './date.js'

	export let getEvents
</script>

<div id="events">
	<h2>Upcoming Events</h2>
	{#await getEvents}
		<p>Looking for events...</p>
	{:then events}
		{#if events.length}
			{#each events as event (event.listingId)}
				<div class="event">
					<h3>{event.title}</h3>
					<p>{event.description}</p>
					<date>{formatDate(event.startTime)} – {formatDate(event.endTime)}</date>
				</div>
			{/each}
		{:else}
			<p>No events found.</p>
		{/if}
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
	}
	#events > * {
		padding: 1rem;

		display: grid;
		grid-auto-flow: row;
		gap: 0.5em;
	}
</style>
<script>
	import { API } from './api.js'
	
	import Event from './Event.svelte'
	import EventForm from './EventForm.svelte'

	export let events, currentEvent, currentUser
	
	let isCreatingNewEvent = false
	let isOnlyShowingUserEvents = false
	let isOnlyShowingBookmarkedEvents = false
	let searchFilter = ''

	const refresh = () =>
		isOnlyShowingUserEvents = isOnlyShowingUserEvents

	$: getEvents = (
		/*$currentUser && isOnlyShowingUserEvents
			? API.event.getEventsByUser($currentUser)
			:*/ API.event.getAllEvents()
	)
		.then(_ => _.map(event => ({
			...event,
			startTime: new Date(event.startTime),
			endTime: new Date(event.endTime)
		})))
		.then(_ => $events = _)

	const filterEvents = events => {
		events = isOnlyShowingUserEvents ?
			events.filter(event => event.userId === $currentUser.userId)
		: isOnlyShowingBookmarkedEvents ?
			events.filter(event => event.isBookmarked)
		:
			events
		
		events = searchFilter ?
			events.filter(event =>
				event.title.toLowerCase().includes(searchFilter.toLowerCase()) ||
				event.description.toLowerCase().includes(searchFilter.toLowerCase())
			)
		:
			events
		
		return events
	}

	
	const onSubmit = async function({detail: event}){
		const result = await API.event.postNewEvent(event, $currentUser.token)
		console.log(result)

		refresh()
	}


	import { fly } from 'svelte/transition'
	import { flip } from 'svelte/animate'
</script>

{#if isCreatingNewEvent}
<div>
	<div class="sticky">
		<div>
			<h2>New Event</h2>
			<button>Cancel</button>
		</div>
	</div>
	
	<EventForm submitLabel="Post Event" on:submit={onSubmit} />
</div>
{/if}

<div>
	<div class="sticky upcoming-events">
		<div>
			<h2>Upcoming Events</h2>
			{#if $currentUser}
				<button on:click={() => isCreatingNewEvent = true}>Create Event</button>
			{/if}
		</div>

		<div>
			<input type="search" placeholder="Search events..." bind:value={searchFilter} />
		</div>
		{#if $currentUser}
		<div>
			<span>Show: </span>

			<button on:click={() => isOnlyShowingUserEvents = isOnlyShowingBookmarkedEvents = false}>All Events</button>
			<button on:click={() => isOnlyShowingUserEvents = !isOnlyShowingUserEvents}>My Events</button>
			<button on:click={() => isOnlyShowingBookmarkedEvents = !isOnlyShowingBookmarkedEvents}>Bookmarks</button>
			<!-- <button on:click={() => isOnlyShowingUserEvents = isOnlyShowingBookmarkedEvents = false} disabled={isOnlyShowingUserEvents || isOnlyShowingBookmarkedEvents}>All Events</button>
			<button on:click={() => isOnlyShowingUserEvents = !isOnlyShowingUserEvents} disabled={!isOnlyShowingUserEvents}>My Events</button>
			<button on:click={() => isOnlyShowingBookmarkedEvents = !isOnlyShowingBookmarkedEvents} disabled={!isOnlyShowingBookmarkedEvents}>Bookmarks</button> -->
		</div>
		{/if}
	</div>

	{#await getEvents}
		<p transition:fly={{y: -30}}>Loading events...</p>
	{:then events}
		{#each (isOnlyShowingUserEvents, isOnlyShowingBookmarkedEvents, filterEvents(events)) as event (event.listingId)}
			<Event
				{event}
				isFocused={$currentEvent === event}
				highlightString={searchFilter}
				on:click={e => $currentEvent = event}
				{currentUser}
				on:eventChange={refresh}
			/>
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
</style>
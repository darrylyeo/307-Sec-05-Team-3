<script>
	import { API } from './api.js'
	import { formatDate, datesAreSameDay } from './date.js'

	import EventForm from './EventForm.svelte'

	export let currentUser

	export let event = {} /*= {
		title: '',
		description: '',
		startTime: new Date(),
		endTime: new Date()
	}*/

	export let isFocused = false

	export let highlightString
	$: highlightRegex = new RegExp(highlightString, 'gi')

	const highlight = str =>
		highlightString
			? str.replace(new RegExp(highlightString, 'gi'), m => `<mark>${m}</mark>`)
			: str
		
	
	let isEditing = false

	const onSubmit = async function({detail: event}){
		const result = await API.event.updateEvent(event, $currentUser.token)
		console.log(result)
	}


	import { fly } from 'svelte/transition'
</script>

<div class="event" class:is-focused={isFocused} transition:fly={{y: 300}} on:click>
	<h3>{@html highlight(event.title)}</h3>
	<date>{formatDate(event.startTime)} – {formatDate(event.endTime, datesAreSameDay(event.startTime, event.endTime))}</date>
	<p>{@html highlight(event.description)}</p>
	<div class="actions">
		{#if $currentUser && $currentUser.userId === event.userId}
			<button on:click={() => isEditing = true}>Edit</button>
		{/if}
	</div>
</div>

{#if isEditing}
	<EventForm {event} submitLabel="Save Changes" on:submit={onSubmit} />
{/if}

<style>
	.event {
		padding: 1rem;
		position: relative;

		display: grid;
		grid-auto-flow: row;
		gap: 0.25em;

		background-color: rgba(255, 255, 255, 0.5);
		transition: 0.3s;
	}
	.event:hover, .event:focus-within {
		background-color: rgba(255, 255, 255, 0.75);
	}
	.event.is-focused {
		background-color: rgba(255, 255, 255, 0.85);
		box-shadow: inset 0 0 0 3px rgba(0, 0, 0, 0.05);
	}

	date {
		font-size: 0.9em;
	}
	p {
		font-size: 0.8em;
	}

	.event .actions {
		position: absolute;
		right: 0;
		display: grid;
		gap: 0.2rem;
		padding: 1rem;
	}
	.event:not(:hover) .actions {
		opacity: 0;
	}
</style>
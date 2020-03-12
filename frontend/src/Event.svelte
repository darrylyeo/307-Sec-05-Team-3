<script>
	import { API } from './api.js'
	import { formatDate, datesAreSameDay } from './date.js'

	import EventForm from './EventForm.svelte'

	export let currentUser = undefined

	export let event = {} /*= {
		title: '',
		description: '',
		startTime: new Date(),
		endTime: new Date()
	}*/

	export let isFocused = false

	export let highlightString = ''
	$: highlightRegex = new RegExp(highlightString, 'gi')

	const highlight = str =>
		highlightString
			? str.replace(new RegExp(highlightString, 'gi'), m => `<mark>${m}</mark>`)
			: str
		
	
	$: isEditable = $currentUser && ($currentUser.isAdmin || $currentUser.userId === event.userId)
	let isEditing = false
	let isDeleting = false

	$: if(isDeleting)
		setTimeout(() => isDeleting = false, 5000)
	

	const onUpdateSubmit = async function(){
		const result = await API.event.updateEvent(event, $currentUser.token)
		console.log(result)
		isEditing = false
	}

	const onDelete = async function(){
		const result = await API.event.deleteEvent(event, $currentUser.token)
		console.log(result)
	}


	import { fly } from 'svelte/transition'
</script>

<div class="event" class:is-focused={isFocused} transition:fly={{y: 300}} on:click>
	<h3>{@html highlight(event.title)}</h3>
	<date>{formatDate(event.startTime)} – {formatDate(event.endTime, datesAreSameDay(event.startTime, event.endTime))}</date>
	<p>{@html highlight(event.description)}</p>

	<div class="actions">
		{#if isEditable}
			<button on:click={() => isEditing = true}>Edit</button>

			<!-- <button on:click={isDeleting ? onDelete : () => isDeleting = true} class="destructive">
				{isDeleting ? 'Really Delete?' : 'Delete'}
			</button> -->
			{#if isDeleting}
				<button on:click={onDelete} class="destructive">Really Delete?</button>
			{:else}
				<button on:click={() => isDeleting = true}>Delete</button>
			{/if}
		{/if}
	</div>
</div>

{#if isEditing}
	<EventForm {event} submitLabel="Save Changes" on:submit={onUpdateSubmit} />
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
		gap: 0.3rem;
		padding: 1rem;
	}
	.event:not(:hover):not(:focus-within) .actions {
		opacity: 0;
	}
</style>
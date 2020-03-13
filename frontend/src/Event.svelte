<script>
	import { API } from './api.js'
	import { formatDate, datesAreSameDay } from './date.js'
	
	import { createEventDispatcher } from 'svelte'
	const dispatch = createEventDispatcher()

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
		try {
			const result = await API.event.updateEvent(event, $currentUser.token)
			console.log(result)
			
			if(result.ExceptionMessage){
				alert(`We couldn't update your event: ${result.ExceptionMessage}`)
			}else{
				isEditing = false
				dispatch('eventChange')
			}
		}catch(e){
			alert(`We couldn't update your event: ${e.message}`)
		}
	}

	const onDelete = async function(){
		try {
			const result = await API.event.deleteEvent(event, $currentUser.token)
			console.log(result)

			dispatch('eventChange')
		}catch(e){
			alert(`We couldn't delete your event: ${e.message}`)
		}
	}


	// import { fly } from 'svelte/transition'
	import { flip } from 'svelte/animate'
	import { quintOut } from 'svelte/easing';
	import { crossfade } from 'svelte/transition';

	const [send, receive] = crossfade({
		duration: d => Math.sqrt(d * 200),

		fallback(node, params) {
			const style = getComputedStyle(node);
			const transform = style.transform === 'none' ? '' : style.transform;

			return {
				duration: 600,
				easing: quintOut,
				css: t => `
					transform: ${transform} scale(${t});
					opacity: ${t}
				`
			};
		}
	})
</script>

<div class="event" class:is-focused={isFocused} class:is-bookmarked={event.isBookmarked} on:click in:receive={{key: event.listingId}} out:send={{key: event.listingId}}>
	<h3>{@html highlightString ? highlight(event.title) : event.title}</h3>
	<date>{formatDate(event.startTime)} – {formatDate(event.endTime, datesAreSameDay(event.startTime, event.endTime))}</date>
	<p>{@html highlightString ? highlight(event.description) : event.title}</p>

	<div class="actions">
		{#if $currentUser}
			<button class:is-bookmarked={event.isBookmarked} on:click={() => event.isBookmarked = !event.isBookmarked}>{event.isBookmarked ? 'Bookmarked' : 'Bookmark'}</button>
		{/if}
		{#if isEditing}
			<button on:click={() => isEditing = false}>Cancel Edits</button>
		{:else if isEditable}
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

		padding: 0.75em;
		font-size: 0.85em;
	}
	.event:not(:hover):not(:focus-within) .actions {
		opacity: 0;
	}

	.event.is-bookmarked {
		/* box-shadow: inset 0 0 2px #d4ff96; */
		background-color: rgba(145, 255, 0, 0.5);
	}
	button.is-bookmarked {
		opacity: 0.6;
	}
</style>
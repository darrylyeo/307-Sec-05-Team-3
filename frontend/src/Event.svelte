<script>
	import { formatDate } from './date.js'

	export let event = {
		title: '[Event Title]',
		description: '[Event description]',
		startTime: new Date(),
		endTime: new Date()
	}

	import { fly } from 'svelte/transition'
</script>

<div class="event" transition:fly={{y: 300}}>
	<h3>{event.title}</h3>
	<date>{formatDate(event.startTime)} – {formatDate(event.endTime)}</date>
	<p>{event.description}</p>
</div>

<style>
	div {
		padding: 1rem;

		display: grid;
		grid-auto-flow: row;
		gap: 0.25em;

		background-color: rgba(255, 255, 255, 0.5);
	}

	date {
		font-size: 0.9em;
	}

	p {
		font-size: 0.8em;
	}
</style>
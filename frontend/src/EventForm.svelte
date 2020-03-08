<script>
	import { eventModel } from 'event.js'

	export let newEvent = {
		name: 'Ada',
		description: 'Lovelace'
	}

	const formFields = Object.entries(eventModel).map(([fieldName, type]) => {fieldName, type})
</script>

{#each formFields as {fieldName, type}}
	<label>
		<span>{fieldName}</span>
		<input {type} bind:value={newEvent[fieldName]}>
	</label>
{/each}

<input bind:value={newEvent.description}>